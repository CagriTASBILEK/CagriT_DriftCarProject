using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : VehicleBase
{
    private bool isDead;

    public override void Initialize(int lane)
    {
        base.Initialize(lane);
        isDead = false;
    }
    public override void HandleCollision(IVehicle other)
    {
        if (isDead) return;
        
        if (other is ObstacleVehicle)
        {
            isDead = true;
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        isDead = false;
    }
}
