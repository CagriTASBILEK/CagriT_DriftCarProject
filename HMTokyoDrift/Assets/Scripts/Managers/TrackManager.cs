using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : Singleton<TrackManager>
{
    [SerializeField] private TrackSettings settings;
    
    private ObjectPool<TrackSegment> trackPool;
    private readonly List<TrackSegment> activeSegments = new();
    private readonly float[] lanePositions;
    private float moveSpeed;
    private float totalDistance;
    private const float UPDATE_INTERVAL = 0.02f;

    public TrackManager()
    {
        lanePositions = new float[settings.laneCount];
    }

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void OnEnable()
    {
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnSpeedChange += HandleSpeedChange;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnSpeedChange -= HandleSpeedChange;
    }

    private void Initialize()
    {
        InitializePool();
        InitializeLanes();
        SpawnInitialTrack();
    }

    private void InitializePool()
    {
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
        for (int i = 0; i < settings.activeSegmentCount; i++)
        {
            SpawnNewSegment();
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
        float movement = moveSpeed * UPDATE_INTERVAL;
        Vector3 moveVector = Vector3.back * movement;

        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            TrackSegment segment = activeSegments[i];
            segment.transform.Translate(moveVector, Space.World);

            if (segment.transform.position.z < -settings.segmentLength)
            {
                RecycleSegment(segment);
                SpawnNewSegment();
            }
        }
    }

    private void SpawnNewSegment()
    {
        Vector3 spawnPosition = Vector3.forward * totalDistance;
        TrackSegment segment = trackPool.Get(spawnPosition, Quaternion.identity);
        
        if (segment != null)
        {
            activeSegments.Add(segment);
            totalDistance += settings.segmentLength;
        }
    }

    private void RecycleSegment(TrackSegment segment)
    {
        activeSegments.Remove(segment);
        trackPool.Return(segment);
    }

    public Vector3 GetSpawnPositionForLane(int laneIndex)
    {
        if (laneIndex < 0 || laneIndex >= settings.laneCount)
            return Vector3.zero;

        return new Vector3(lanePositions[laneIndex], 0f, settings.segmentLength);
    }

    public int GetRandomLane() => Random.Range(0, settings.laneCount);
    private void HandleGameStart() => StartCoroutine(TrackUpdateRoutine());
    private void HandleSpeedChange(float newSpeed) => moveSpeed = newSpeed;
}