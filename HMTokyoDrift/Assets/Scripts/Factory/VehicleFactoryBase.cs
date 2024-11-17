using Core.Pool;
using Interfaces;
using UnityEngine;
using Vehicles;

namespace Factory
{
    /// <summary>
    /// Base class for all vehicle factories providing common pool functionality
    /// </summary>
    public abstract class VehicleFactoryBase : MonoBehaviour, IVehicleFactory
    {
        protected ObjectPool<VehicleBase> vehiclePool;
    
        protected virtual void Awake()
        {
            InitializePool();
        }
        protected abstract void InitializePool();
    
        /// <summary>
        /// Gets a vehicle from the pool and initializes it
        /// </summary>
        /// <param name="position">Spawn position</param>
        /// <param name="rotation">Initial rotation</param>
        /// <param name="lane">Lane number for the vehicle</param>
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
}