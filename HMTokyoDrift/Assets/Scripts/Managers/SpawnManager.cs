using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private SpawnSettings settings;
    [SerializeField] private ObstacleVehicleFactory obstacleFactory;

    private void OnEnable()
    {
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleGameStart()
    {
        var segments = TrackManager.Instance.GetActiveSegments();
        foreach (var segment in segments)
        {
            ClearObstaclesFromSegment(segment);
        }
        SpawnInitialObstacles();
    }
    
    private void HandleGameOver()
    {
        Debug.Log("SpawnManager: Game Over");
    }

    public void SpawnObstaclesForSegment(TrackSegment segment)
    {
        int obstacleCount = Mathf.RoundToInt(Mathf.Lerp(
            settings.minObstaclesPerSegment,
            settings.maxObstaclesPerSegment,
            GameManager.Instance.DifficultyMultiplier
        ));

        List<(int lane, float zPos)> spawnPoints = new();
        int attempts = 0;
        int maxAttempts = obstacleCount * 3;

        while (spawnPoints.Count < obstacleCount && attempts < maxAttempts)
        {
            attempts++;

            int lane = Random.Range(0, TrackManager.Instance.Settings.laneCount);
            float zOffset = Random.Range(0f, TrackManager.Instance.Settings.segmentLength);
            
            bool isValidPosition = true;
            foreach (var point in spawnPoints)
            {
                if (point.lane == lane)
                {
                    float distance = Mathf.Abs(point.zPos - zOffset);
                    if (distance < settings.minDistanceBetweenVehicles)
                    {
                        isValidPosition = false;
                        break;
                    }
                }
            }

            if (isValidPosition)
            {
                spawnPoints.Add((lane, zOffset));
            }
        }
        
        foreach (var point in spawnPoints)
        {
            Vector3 spawnPosition = TrackManager.Instance.GetSpawnPositionForLane(point.lane);
            spawnPosition.z = segment.transform.position.z + point.zPos;

            var obstacle = obstacleFactory.GetVehicle(spawnPosition, Quaternion.identity, point.lane) as ObstacleVehicle;
            if (obstacle != null)
            {
                obstacle.transform.SetParent(segment.transform);
            }
        }
    }

    private void SpawnInitialObstacles()
    {
        var segments = TrackManager.Instance.GetActiveSegments();
        for (int i = 1; i < segments.Count; i++)
        {
            SpawnObstaclesForSegment(segments[i]);
        }
    }

    public void ClearObstaclesFromSegment(TrackSegment segment)
    {
        if (segment == null) return;

        foreach (Transform child in segment.transform)
        {
            if (child.TryGetComponent<ObstacleVehicle>(out var obstacle))
            {
                obstacle.OnDespawn();
                obstacle.transform.SetParent(null);
                obstacleFactory.ReturnVehicle(obstacle);
            }
        }
    }
}