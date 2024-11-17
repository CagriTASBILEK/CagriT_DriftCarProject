using UnityEngine;
using Vehicles;

namespace Interfaces
{
    public interface IVehicleFactory
    {
        VehicleBase GetVehicle(Vector3 position, Quaternion rotation, int lane);
        void ReturnVehicle(VehicleBase vehicle);
        void ReturnAllVehicles();
    }
}