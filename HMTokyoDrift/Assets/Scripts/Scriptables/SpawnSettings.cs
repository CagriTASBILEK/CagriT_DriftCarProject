using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSettings", menuName = "Game/Settings/SpawnSettings")]
public class SpawnSettings : ScriptableObject
{
    [Header("Spawn Timing")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;
    
    [Header("Pool Settings")]
    public int initialPoolSize = 10;
    public int maxPoolSize = 20;
    public int expandSize = 5;

    [Header("Spawn Distance")]
    public float spawnDistance = 100f;
    public float despawnDistance = -10f;
}