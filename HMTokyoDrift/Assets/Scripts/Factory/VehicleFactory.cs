using System.Collections.Generic;
using UnityEngine;

public class VehicleFactory : MonoBehaviour
{
    [SerializeField] private ObstacleVehicleSettings settings;
    private ObjectPool<ObstacleVehicle>[] obstaclePools;
    private List<int> validPoolIndices = new();

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        obstaclePools = new ObjectPool<ObstacleVehicle>[settings.obstaclePrefabs.Length];
        validPoolIndices.Clear();

        int minPoolSize = Mathf.Max(5, settings.initialPoolSize / settings.obstaclePrefabs.Length);

        for (int i = 0; i < settings.obstaclePrefabs.Length; i++)
        {
            if (settings.obstaclePrefabs[i] == null) continue;

            var prefab = settings.obstaclePrefabs[i].GetComponent<ObstacleVehicle>();
            if (prefab == null) continue;

            obstaclePools[i] = new ObjectPool<ObstacleVehicle>(
                prefab,
                transform,
                minPoolSize,
                settings.maxPoolSize / settings.obstaclePrefabs.Length,
                settings.expandSize
            );

            validPoolIndices.Add(i);
        }

        if (validPoolIndices.Count == 0)
        {
            Debug.LogError("No valid pools were created!");
        }
    }

    public ObstacleVehicle GetObstacleFromPool(Vector3 position, Quaternion rotation, int lane)
    {
        if (validPoolIndices.Count == 0)
        {
            Debug.LogError("No valid pools available!");
            return null;
        }

        int randomIndex = validPoolIndices[Random.Range(0, validPoolIndices.Count)];
        var selectedPool = obstaclePools[randomIndex];

        var obstacle = selectedPool.Get(position, rotation);
        if (obstacle != null)
        {
            obstacle.Initialize(lane);
        }

        return obstacle;
    }

    public void ReturnObstacleToPool(ObstacleVehicle obstacle)
    {
        if (obstacle == null) return;
        
        for (int i = 0; i < settings.obstaclePrefabs.Length; i++)
        {
            if (settings.obstaclePrefabs[i] != null &&
                obstacle.gameObject.name.Contains(settings.obstaclePrefabs[i].name))
            {
                obstaclePools[i]?.Return(obstacle);
                break;
            }
        }
    }
    public void ReturnAllObstacles()
    {
        foreach (var poolIndex in validPoolIndices)
        {
            obstaclePools[poolIndex]?.ReturnAll();
        }
    }
}