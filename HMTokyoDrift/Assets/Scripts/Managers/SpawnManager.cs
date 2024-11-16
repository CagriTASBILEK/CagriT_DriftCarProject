using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private SpawnSettings settings;
    
    private float nextSpawnTime;
    private bool canSpawn;
    private readonly System.Random random = new();
    private const float MIN_DISTANCE_BETWEEN_OBSTACLES = 10f;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void OnEnable()
    {
        GameEvents.OnGameStart += HandleGameStart;
     
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= HandleGameStart;
    }

    private void Initialize()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        //
    }

    private void HandleGameStart()
    {
        canSpawn = true;
        nextSpawnTime = Time.time;
        StartCoroutine(SpawnRoutine());
    }
    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds minWait = new(settings.minSpawnInterval);

        while (canSpawn)
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnObstacle();
                float interval = GetRandomSpawnInterval();
                nextSpawnTime = Time.time + interval;
                yield return minWait;
            }
            yield return null;
        }
    }

    private void SpawnObstacle()
    {
        int randomLane = TrackManager.Instance.GetRandomLane();
        Vector3 spawnPosition = TrackManager.Instance.GetSpawnPositionForLane(randomLane);
        spawnPosition.z = settings.spawnDistance;

        if (IsSpawnPositionClear(spawnPosition))
        {
           //
        }
    }
    private bool IsSpawnPositionClear(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, MIN_DISTANCE_BETWEEN_OBSTACLES);
        return colliders.Length == 0;
    }
    private float GetRandomSpawnInterval()
    {
        return Mathf.Lerp(
            settings.maxSpawnInterval,
            settings.minSpawnInterval,
            GameManager.Instance.DifficultyMultiplier / 2f
        );
    }
}
