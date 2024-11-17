using UnityEngine;

[CreateAssetMenu(fileName = "PoliceVehicleSettings", menuName = "Game/Vehicles/PoliceVehicleSettings")]
public class PoliceVehicleSettings : ScriptableObject
{
    [Header("Vehicle Settings")]
    public GameObject policePrefab;
    
    [Header("Chase Settings")]
    public float defaultDistance = 10f;     
    public float catchDistance = 3f;        
    public float escapeDistance = 20f;     
    public float horizontalSpeed = 5f;     
    public float lerpSpeed = 2f;          
    
    [Header("Collision Settings")]
    public float collisionRecoveryTime = 2f;  
    public float collisionPushDistance = 15f;
    
    [Header("Pool Settings")]
    public int initialPoolSize = 1;
    public int maxPoolSize = 1;
}
