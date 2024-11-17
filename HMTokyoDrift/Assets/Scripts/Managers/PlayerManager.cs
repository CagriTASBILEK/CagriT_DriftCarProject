using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerVehicleFactory playerFactory;
    [SerializeField] private PoliceVehicleFactory policeFactory;
    private PlayerVehicle currentPlayer;
    private PoliceVehicle policeVehicle;
    
    public PlayerVehicle CurrentPlayer => currentPlayer;
    private void OnEnable()
    {
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnGameOver += HandleGameOver;
        GameEvents.OnInputReceived += HandleMovement;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnGameOver -= HandleGameOver;
        GameEvents.OnInputReceived -= HandleMovement;
        
        if (currentPlayer != null)
        {
            playerFactory.ReturnVehicle(currentPlayer);
        }
        if (policeVehicle != null)
        {
            policeFactory.ReturnVehicle(policeVehicle);
        }
    }
    
    private void HandleMovement(float input)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;
        currentPlayer?.UpdateMovement();
        policeVehicle?.UpdateMovement();
    }
    
    public void SetCurrentPlayer(PlayerVehicle player)
    {
        currentPlayer = player;
    }
    public void ClearCurrentPlayer()
    {
        currentPlayer = null;
    }

    private void HandleGameStart()
    {
        SpawnPlayer();
        SpawnPolice();
    }

    private void SpawnPlayer()
    {
        int middleLane = TrackManager.Instance.Settings.laneCount / 2;
        Vector3 spawnPosition = TrackManager.Instance.GetSpawnPositionForLane(middleLane);
        spawnPosition.z = 10f;

        currentPlayer = playerFactory.GetVehicle(spawnPosition, Quaternion.identity, middleLane) as PlayerVehicle;
        if (currentPlayer == null)
        {
            Debug.LogError("Failed to spawn player!");
        }
    }
    private void SpawnPolice()
    {
        int middleLane = TrackManager.Instance.Settings.laneCount / 2;
        Vector3 spawnPosition = TrackManager.Instance.GetSpawnPositionForLane(middleLane);
        spawnPosition.z = -30;

        policeVehicle = policeFactory.GetVehicle(spawnPosition, Quaternion.identity, middleLane) as PoliceVehicle;
        if (policeVehicle == null)
        {
            Debug.LogError("Failed to spawn police!");
        }
    }
    
    private void HandleGameOver()
    {
        if (currentPlayer != null)
        {
            playerFactory.ReturnVehicle(currentPlayer);
            currentPlayer = null;
        }
        if (policeVehicle != null)
        {
            policeFactory.ReturnVehicle(policeVehicle);
            policeVehicle = null;
        }
    }
    
}