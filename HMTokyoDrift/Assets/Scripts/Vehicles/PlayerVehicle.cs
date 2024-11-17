using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Input değerini dışarıdan set etmek için
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
    }
    
    private void OnEnable()
    {
        GameEvents.OnInputReceived += HandleInput;
    }

    private void OnDisable()
    {
        GameEvents.OnInputReceived -= HandleInput;
    }

    private void HandleInput(float input)
    {
        if (!IsActive) return;
        horizontalInput = input;
    }
    
    
    
}
