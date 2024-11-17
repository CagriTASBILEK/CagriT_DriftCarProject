using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerVehicleFactory playerFactory;
    private PlayerVehicle currentPlayer;
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
    }
    
    private void HandleMovement(float input)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;
        currentPlayer?.UpdateMovement();
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
    }

    private void SpawnPlayer()
    {
        int middleLane = TrackManager.Instance.Settings.laneCount / 2;
        Vector3 spawnPosition = TrackManager.Instance.GetSpawnPositionForLane(middleLane);
        spawnPosition.z = 5f;

        currentPlayer = playerFactory.GetVehicle(spawnPosition, Quaternion.identity, middleLane) as PlayerVehicle;
        if (currentPlayer == null)
        {
            Debug.LogError("Failed to spawn player!");
        }
    }
    private void HandleGameOver()
    {
        if (currentPlayer != null)
        {
            playerFactory.ReturnVehicle(currentPlayer);
            currentPlayer = null;
        }
    }
    public PlayerVehicle CurrentPlayer => currentPlayer;
}