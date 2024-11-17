using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "SpawnSettings", menuName = "Game/Settings/SpawnSettings")]
    public class SpawnSettings : ScriptableObject
    {   
        [Header("Spawn Settings")]
        [Range(1, 5)] public int minObstaclesPerSegment = 2;     
        [Range(3, 8)] public int maxObstaclesPerSegment = 5;   
    
        [Header("Spacing")]
        public float minDistanceBetweenVehicles = 20f; 
    }
}