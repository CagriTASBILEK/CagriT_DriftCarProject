using Core.Pool;
using Scriptables;
using UnityEngine;
using Vehicles;

namespace Factory
{
    /// <summary>
    /// Factory responsible for creating and managing police vehicle pool
    /// </summary>
    public class PoliceVehicleFactory : VehicleFactoryBase
    {
        [SerializeField] private PoliceVehicleSettings settings;
        private ObjectPool<PoliceVehicle> policePool;

        protected override void InitializePool()
        {
            if (!ValidateSettings()) return;

            var prefab = settings.policePrefab.GetComponent<PoliceVehicle>();
            if (prefab == null)
            {
                Debug.LogError("PoliceVehicle component missing on prefab!");
                return;
            }

            policePool = new ObjectPool<PoliceVehicle>(
                prefab,
                transform,
                settings.initialPoolSize,
                settings.maxPoolSize
            );
        }

        private bool ValidateSettings()
        {
            if (settings == null || settings.policePrefab == null)
            {
                Debug.LogError("PoliceVehicleSettings is missing or invalid!");
                return false;
            }
            return true;
        }

        public override VehicleBase GetVehicle(Vector3 position, Quaternion rotation, int lane)
        {
            var police = policePool.Get(position, rotation);
            if (police != null)
            {
                police.Initialize(lane,settings);
            }
            return police;
        }

        public override void ReturnVehicle(VehicleBase vehicle)
        {
            if (vehicle is PoliceVehicle police)
            {
                policePool.Return(police);
            }
        }

        public override void ReturnAllVehicles()
        {
            policePool?.ReturnAll();
        }
    }
}