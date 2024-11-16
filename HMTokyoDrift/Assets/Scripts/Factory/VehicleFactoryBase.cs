using UnityEngine;

public abstract class VehicleFactoryBase : MonoBehaviour, IVehicleFactory
{
    protected ObjectPool<VehicleBase> vehiclePool;
    
    protected virtual void Awake()
    {
        InitializePool();
    }
    protected abstract void InitializePool();
    
    public virtual VehicleBase GetVehicle(Vector3 position, Quaternion rotation, int lane)
    {
        if (vehiclePool == null) return null;
        
        var vehicle = vehiclePool.Get(position, rotation);
        if (vehicle != null)
        {
            vehicle.Initialize(lane);
        }
        return vehicle;
    }
    
    public virtual void ReturnVehicle(VehicleBase vehicle)
    {
        if (vehicle == null) return;
        vehiclePool?.Return(vehicle);
    }
    
    public virtual void ReturnAllVehicles()
    {
        vehiclePool?.ReturnAll();
    }
}