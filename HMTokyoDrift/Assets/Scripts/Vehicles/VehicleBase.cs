using UnityEngine;

public abstract class VehicleBase : MonoBehaviour, IVehicle, IPoolable
{
    protected int currentLane;
    
    public int CurrentLane => currentLane;
    public bool IsActive => gameObject.activeSelf;
    
    public virtual void Initialize(int lane)
    {
        currentLane = lane;
    }
    public abstract void UpdateMovement(); 
    public abstract void HandleCollision(IVehicle other);
    public virtual void OnSpawn()
    {
        gameObject.SetActive(true);
    }
    
    public virtual void OnDespawn()
    {
        gameObject.SetActive(false);
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!IsActive) return;
        
        if (other.TryGetComponent<IVehicle>(out var vehicle))
        {
            HandleCollision(vehicle);
        }
    }
}