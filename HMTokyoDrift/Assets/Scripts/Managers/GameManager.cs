using System.Collections;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameSettings settings;
    
    private float currentGameSpeed;
    private float currentDifficultyMultiplier;
    private GameState currentGameState;
    private bool isGameInitialized;
    private float lastSpeedUpdateTime;
    private float lastDifficultyUpdateTime;
    private const float UPDATE_INTERVAL = 0.5f;

    public float CurrentGameSpeed => currentGameSpeed;
    public float DifficultyMultiplier => currentDifficultyMultiplier;
    public GameState CurrentGameState => currentGameState;

    protected override void Awake()
    {
        base.Awake();
        InitializeGame();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnGameStart += HandleGameStart;
    }

    private void UnsubscribeFromEvents()
    {
        GameEvents.OnGameStart -= HandleGameStart;
    }

    private void InitializeGame()
    {
        if (isGameInitialized) return;
        
        currentGameSpeed = settings.initialGameSpeed;
        currentDifficultyMultiplier = settings.difficultyMultiplier;
        currentGameState = GameState.MainMenu;
        lastSpeedUpdateTime = 0f;
        lastDifficultyUpdateTime = 0f;
        isGameInitialized = true;
        
        GameEvents.TriggerGameInitialize();
    }

    private void HandleGameStart()
    {
        currentGameState = GameState.Playing;
        StartCoroutine(GameUpdateRoutine());
    }
    
    private IEnumerator GameUpdateRoutine()
    {
        WaitForSeconds updateWait = new(UPDATE_INTERVAL);

        while (currentGameState == GameState.Playing)
        {
            UpdateGameSpeed();
            UpdateDifficulty();
            yield return updateWait;
        }
    }
    private void UpdateGameSpeed()
    {
        if (currentGameSpeed >= settings.maxGameSpeed) return;

        float timeSinceLastUpdate = Time.time - lastSpeedUpdateTime;
        float speedIncrease = settings.speedIncreaseRate * timeSinceLastUpdate;
        
        currentGameSpeed = Mathf.Min(currentGameSpeed + speedIncrease, settings.maxGameSpeed);
        
        if (currentGameSpeed != settings.maxGameSpeed)
        {
            GameEvents.TriggerSpeedChange(currentGameSpeed);
        }

        lastSpeedUpdateTime = Time.time;
    }

    private void UpdateDifficulty()
    {
        if (currentDifficultyMultiplier >= settings.maxDifficultyMultiplier) return;

        float timeSinceLastUpdate = Time.time - lastDifficultyUpdateTime;
        float difficultyIncrease = settings.difficultyIncreaseRate * timeSinceLastUpdate;
        
        currentDifficultyMultiplier = Mathf.Min(
            currentDifficultyMultiplier + difficultyIncrease, 
            settings.maxDifficultyMultiplier
        );

        lastDifficultyUpdateTime = Time.time;
    }
}

public enum GameState
{
    MainMenu,
    Playing
}