using UnityEngine;

public class PlayerVehicleFactory : VehicleFactoryBase
{
    [SerializeField] private PlayerVehicleSettings settings;
    private ObjectPool<PlayerVehicle> playerPool;

    protected override void InitializePool()
    {
        if (!ValidateSettings()) return;

        var prefab = settings.playerPrefab.GetComponent<PlayerVehicle>();
        if (prefab == null)
        {
            Debug.LogError("PlayerVehicle component missing on prefab!");
            return;
        }

        playerPool = new ObjectPool<PlayerVehicle>(
            prefab,
            transform,
            settings.initialPoolSize,
            settings.maxPoolSize
        );
    }

    private bool ValidateSettings()
    {
        if (settings == null || settings.playerPrefab == null)
        {
            Debug.LogError("PlayerVehicleSettings is missing or invalid!");
            return false;
        }
        return true;
    }

    public override VehicleBase GetVehicle(Vector3 position, Quaternion rotation, int lane)
    {
        var player = playerPool.Get(position, rotation);
        if (player != null)
        {
            player.Initialize(lane);
        }
        return player;
    }

    public override void ReturnVehicle(VehicleBase vehicle)
    {
        if (vehicle is PlayerVehicle player)
        {
            playerPool.Return(player);
        }
    }

    public override void ReturnAllVehicles()
    {
        playerPool?.ReturnAll();
    }
}
