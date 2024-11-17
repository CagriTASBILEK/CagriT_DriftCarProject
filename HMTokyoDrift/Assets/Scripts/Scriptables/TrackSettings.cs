using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(fileName = "TrackSettings", menuName = "Game/Settings/TrackSettings")]
    public class TrackSettings : ScriptableObject
    {
        [Header("Track Configuration")]
        public GameObject trackSegmentPrefab;
        public int activeSegmentCount = 3;
        public float segmentLength = 50f;
    
        [Header("Lanes")]
        public int laneCount = 3;
        public float laneWidth = 4f;
    
        [Header("Pool Settings")]
        public int initialPoolSize = 5;
        public int maxPoolSize = 8;
    }
}