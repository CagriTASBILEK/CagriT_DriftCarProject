using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : Singleton<TrackManager>
{
    [SerializeField] private TrackSettings settings;

    private ObjectPool<TrackSegment> trackPool;
    private readonly List<TrackSegment> activeSegments = new();
    private float[] lanePositions;
    private float moveSpeed;
    private const float UPDATE_INTERVAL = 0.02f;

    public TrackSettings Settings => settings;
    public IReadOnlyList<TrackSegment> GetActiveSegments() => activeSegments;

    protected override void Awake()
    {
        base.Awake();
    
        if (settings == null)
        {
            Debug.LogError("Track Settings is missing! Please assign it in the inspector.");
            return;
        }
    
        lanePositions = new float[settings.laneCount];
    }

    public void Initialize()
    {
        if (settings == null)
        {
            Debug.LogError("Cannot initialize TrackManager: Track Settings is missing!");
            return;
        }

        InitializePool();
        InitializeLanes();
        SubscribeToEvents();
    }
    
    private void SubscribeToEvents()
    {
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnSpeedChange += HandleSpeedChange;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnSpeedChange -= HandleSpeedChange;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void InitializePool()
    {
        if (settings.trackSegmentPrefab == null)
        {
            Debug.LogError("Track Segment Prefab is missing!");
            return;
        }

        trackPool = new ObjectPool<TrackSegment>(
            settings.trackSegmentPrefab.GetComponent<TrackSegment>(),
            transform,
            settings.initialPoolSize,
            settings.maxPoolSize
        );
    }

    private void InitializeLanes()
    {
        float startX = -(settings.laneCount - 1) * settings.laneWidth / 2f;

        for (int i = 0; i < settings.laneCount; i++)
        {
            lanePositions[i] = startX + (i * settings.laneWidth);
        }
    }

    private void SpawnInitialTrack()
    {
        Vector3 firstPosition = Vector3.zero;
        TrackSegment firstSegment = trackPool.Get(firstPosition, Quaternion.identity);
        if (firstSegment != null)
        {
            activeSegments.Add(firstSegment);
        }

        for (int i = 1; i < settings.activeSegmentCount; i++)
        {
            Vector3 position = new Vector3(0f, 0f, settings.segmentLength * i);
            TrackSegment segment = trackPool.Get(position, Quaternion.identity);
            if (segment != null)
            {
                activeSegments.Add(segment);
            }
        }
    }

    private IEnumerator TrackUpdateRoutine()
    {
        WaitForSeconds updateWait = new(UPDATE_INTERVAL);

        while (GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            MoveTrack();
            yield return updateWait;
        }
    }

    private void MoveTrack()
    {
        if (moveSpeed <= 0) return;
        
        float movement = moveSpeed * UPDATE_INTERVAL;
        Vector3 moveVector = Vector3.back * movement;

        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            TrackSegment segment = activeSegments[i];
            segment.transform.Translate(moveVector, Space.World);

            if (segment.transform.position.z < -settings.segmentLength * 1.5f)
            {
                TrackSegment lastSegment = activeSegments[activeSegments.Count - 1];
                float newZ = lastSegment.transform.position.z + settings.segmentLength;

                activeSegments.Remove(segment);

                segment.OnDespawn();


                segment.transform.position = new Vector3(0f, 0f, newZ);
                segment.OnSpawn();
                activeSegments.Add(segment);


                if (SpawnManager.Instance != null)
                {
                    SpawnManager.Instance.SpawnObstaclesForSegment(segment);
                }
            }
        }
    }

    public Vector3 GetSpawnPositionForLane(int laneIndex)
    {
        if (laneIndex < 0 || laneIndex >= settings.laneCount)
            return Vector3.zero;

        return new Vector3(lanePositions[laneIndex], 0f, settings.segmentLength);
    }
    
    private void HandleGameStart()
    {
        foreach (var segment in activeSegments)
        {
            if (segment != null)
            {
                segment.OnDespawn();
                trackPool.Return(segment);
            }
        }
        activeSegments.Clear();
        
        SpawnInitialTrack();
        moveSpeed = GameManager.Instance.CurrentGameSpeed;
        StartCoroutine(TrackUpdateRoutine());
    }
    private void HandleGameOver()
    {
        StopAllCoroutines();
        moveSpeed = 0f;
    }
    private void HandleSpeedChange(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}