using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "PlayerVehicleSettings", menuName = "Game/Vehicles/PlayerVehicleSettings")]
    public class PlayerVehicleSettings : ScriptableObject
    {
        [Header("Vehicle Settings")]
        public GameObject playerPrefab;
    
        [Header("Movement Settings")]
        public float normalSteerSpeed = 5f;
        public float driftSpeed = 7f;
        public float returnSpeed = 2f;
        public float maxSteerDistance = 4f;
        public float maxDriftDistance = 5f;
        public float driftThreshold = 0.7f;
    
        [Header("Rotation Settings")]
        public float tiltAngle = 15f;
        public float rotationAngle = 30f;
        public float wheelTurnAngle = 20f;

    
        [Header("Pool Settings")]
        public int initialPoolSize = 1;
        public int maxPoolSize = 1;
    }
}
