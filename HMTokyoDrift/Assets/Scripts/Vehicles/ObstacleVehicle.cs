using Interfaces;

namespace Vehicles
{
    public class ObstacleVehicle : VehicleBase
    {
        public override void UpdateMovement()
        {
        }
        public override void HandleCollision(IVehicle other)
        {
            if (other is ObstacleVehicle)
            {
                OnDespawn();
            }
        }
        public override void Initialize(int lane)
        {
            base.Initialize(lane);
        }
    }
}
