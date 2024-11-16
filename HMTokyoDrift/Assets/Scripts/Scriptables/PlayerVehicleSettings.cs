using UnityEngine;

[CreateAssetMenu(fileName = "PlayerVehicleSettings", menuName = "Game/Vehicles/PlayerVehicleSettings")]
public class PlayerVehicleSettings : ScriptableObject
{
    [Header("Vehicle Settings")]
    public GameObject playerPrefab;
    
    [Header("Pool Settings")]
    public int initialPoolSize = 1;
    public int maxPoolSize = 1;
}
