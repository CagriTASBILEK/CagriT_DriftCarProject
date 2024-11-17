using Interfaces;
using Managers;
using UnityEngine;

namespace Track
{
    public class TrackSegment : MonoBehaviour, IPoolable
    {
        public bool IsActive => gameObject.activeSelf;

        public void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public void OnDespawn()
        {
            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.ClearObstaclesFromSegment(this);
            }
            gameObject.SetActive(false);
        }
    }
}