using Controller;
using Core.Events;
using Interfaces;
using Managers;
using Scriptables;
using State;
using UnityEngine;

namespace Vehicles
{
    /// <summary>
    /// Represents the player-controlled vehicle with state management and input handling
    /// </summary>
    public class PlayerVehicle : VehicleBase
    {
        [SerializeField] private PlayerVehicleSettings settings;
        [SerializeField] private WheelController wheelController;
    
        private IVehicleState currentState;
        private float horizontalInput;

        public override void Initialize(int lane)
        {
            base.Initialize(lane);
        
            if (wheelController != null)
            {
                ChangeState(new NormalState(this, settings, wheelController));
            }
            else
            {
                Debug.LogError("WheelController is missing on PlayerVehicle!");
            }
        }

        public override void UpdateMovement()
        {
            if (!IsActive || currentState == null) return;
        
            currentState.HandleInput(horizontalInput);
        }

        private void FixedUpdate()
        {
            if (!IsActive || currentState == null) return;
        
            currentState.HandlePhysics(Time.fixedDeltaTime);
        }

        public void ChangeState(IVehicleState newState)
        {
            currentState?.ExitState();
            currentState = newState;
            currentState.EnterState();
        }
    
        public void SetInput(float input)
        {
            horizontalInput = input;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (wheelController != null)
            {
                ChangeState(new NormalState(this, settings, wheelController));
            }
            PlayerManager.Instance.SetCurrentPlayer(this);
        }

        public override void OnDespawn()
        {
            base.OnDespawn();
            currentState?.ExitState();
            currentState = null;
            horizontalInput = 0f;
            PlayerManager.Instance.ClearCurrentPlayer();
        }

        public override void HandleCollision(IVehicle other)
        {
            if (other is ObstacleVehicle)
            {
                GameEvents.TriggerGameOver();
            }
        }
    
        private void OnEnable()
        {
            GameEvents.OnInputReceived += HandleInput;
            GameEvents.OnGameStart += HandleGameStart;
        }

        private void OnDisable()
        {
            GameEvents.OnInputReceived -= HandleInput;
            GameEvents.OnGameStart -= HandleGameStart;
        }

        private void HandleInput(float input)
        {
            if (!IsActive) return;
            horizontalInput = input;
        }
    
        private void HandleGameStart()
        {
            Debug.Log("PlayerVehicle: Starting new game");
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            ChangeState(new NormalState(this, settings, wheelController));
        }
    
    }
}
