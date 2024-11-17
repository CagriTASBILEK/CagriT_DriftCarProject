using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "ObstacleVehicleSettings", menuName = "Game/Vehicles/ObstacleVehicleSettings")]
    public class ObstacleVehicleSettings : ScriptableObject
    {
        [Header("Obstacle Prefabs")]
        public GameObject[] obstaclePrefabs;
    
        [Header("Pool Settings")]
        public int initialPoolSize = 15;
        public int maxPoolSize = 20;
        public int expandSize = 5;
    }
}
