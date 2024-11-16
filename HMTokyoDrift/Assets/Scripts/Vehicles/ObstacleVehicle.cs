using UnityEngine;

public class ObstacleVehicle : MonoBehaviour, IPoolable
{
    private int currentLane;
    public int CurrentLane => currentLane;
    public bool IsActive => gameObject.activeSelf;

    public void Initialize(int lane)
    {
        currentLane = lane;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!IsActive) return;

        if (other.CompareTag("Player"))
        {
            OnDespawn();
        }
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
}
