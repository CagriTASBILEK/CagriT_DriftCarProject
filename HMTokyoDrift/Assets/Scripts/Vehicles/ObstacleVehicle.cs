using UnityEngine;

public class ObstacleVehicle : VehicleBase
{
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