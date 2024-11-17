using UnityEngine;
using System.Collections;

public class PoliceVehicle : VehicleBase
{
    private PoliceVehicleSettings settings;
    private PlayerVehicle targetPlayer;
    private float currentDistance;
    private bool isRecovering;
    private bool isPlayerDrifting;
    private Coroutine recoveryCoroutine;
    private bool isGameOver = false;

    private void OnEnable()
    {
        GameEvents.OnVehicleStateChanged += HandlePlayerStateChanged;
        GameEvents.OnGameStart += HandleGameStart;
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnVehicleStateChanged -= HandlePlayerStateChanged;
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnGameOver -= HandleGameOver;
        
        if (recoveryCoroutine != null)
        {
            StopCoroutine(recoveryCoroutine);
            recoveryCoroutine = null;
        }
        
    }
    private void HandlePlayerStateChanged(IVehicleState state)
    {
        isPlayerDrifting = state is DriftState;
    }

    private void HandleGameStart()
    {
        isGameOver = false;
        targetPlayer = PlayerManager.Instance.CurrentPlayer;
    }

    private void HandleGameOver()
    {
        isGameOver = true;
    }
    
    public void Initialize(int lane, PoliceVehicleSettings settings)
    {
        base.Initialize(lane);
        this.settings = settings;
        currentDistance = Mathf.Abs(transform.position.z);
        targetPlayer = PlayerManager.Instance.CurrentPlayer;
        isRecovering = false;
        isPlayerDrifting = false;
        isGameOver = false;
    }

    public override void UpdateMovement()
    {
        if (!IsActive || targetPlayer == null || isGameOver || GameManager.Instance.CurrentGameState != GameState.Playing) return;
        
        float targetDistance = isPlayerDrifting ? settings.escapeDistance : 
            isRecovering ? settings.collisionPushDistance : settings.catchDistance-.1f;
        
        currentDistance = Mathf.Lerp(currentDistance, targetDistance, Time.deltaTime * settings.lerpSpeed);
        
        float targetX = Mathf.Lerp(transform.position.x, targetPlayer.transform.position.x, 
            Time.deltaTime * settings.horizontalSpeed);
        
        transform.position = new Vector3(targetX, transform.position.y, -currentDistance);
        
        if (!isRecovering && !isPlayerDrifting && currentDistance <= settings.catchDistance && !isGameOver)
        {
            isGameOver = true;
            OnDespawn();
            GameEvents.TriggerGameOver();
        }
    }

    public override void HandleCollision(IVehicle other)
    {
        if (!IsActive || isGameOver || GameManager.Instance.CurrentGameState != GameState.Playing) return;

        if (other is ObstacleVehicle)
        {
            if (recoveryCoroutine != null)
                StopCoroutine(recoveryCoroutine);
                
            recoveryCoroutine = StartCoroutine(CollisionRecoveryRoutine());
        }
    }

    private IEnumerator CollisionRecoveryRoutine()
    {
        isRecovering = true;
        yield return new WaitForSeconds(settings.collisionRecoveryTime);
        isRecovering = false;
        recoveryCoroutine = null;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        if (settings != null) 
        {
            currentDistance = settings.defaultDistance;
        }
        isRecovering = false;
        isGameOver = false;
        targetPlayer = PlayerManager.Instance.CurrentPlayer;
        
        if (recoveryCoroutine != null)
        {
            StopCoroutine(recoveryCoroutine);
            recoveryCoroutine = null;
        }
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        if (recoveryCoroutine != null)
        {
            StopCoroutine(recoveryCoroutine);
            recoveryCoroutine = null;
        }
    }
}