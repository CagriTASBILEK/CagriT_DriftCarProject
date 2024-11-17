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
    
    if (settings == null)
    {
        Debug.LogError("Game Settings is missing! Please assign it in the inspector.");
        return;
    }
    
    currentGameState = GameState.MainMenu;
    currentGameSpeed = 0f;
    currentDifficultyMultiplier = settings.difficultyMultiplier;
    lastSpeedUpdateTime = 0f;
    lastDifficultyUpdateTime = 0f;
    }
    
    private void Start()
    {
        InitializeGame();
    }
    
    public void StartGame()
    {
        if (currentGameState == GameState.Playing) return;
        
        ResetGameState();
        currentGameState = GameState.Playing;
        currentGameSpeed = settings.initialGameSpeed;
        
        GameEvents.TriggerSpeedChange(currentGameSpeed);
        
        GameEvents.TriggerGameStart();
        
        StartCoroutine(GameUpdateRoutine());
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
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void UnsubscribeFromEvents()
    {
        GameEvents.OnGameStart -= HandleGameStart;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void InitializeGame()
    {
        if (isGameInitialized) return;
    
        if (TrackManager.Instance == null)
        {
            Debug.LogError("TrackManager instance is missing!");
            return;
        }
    
        TrackManager.Instance.Initialize();
        GameEvents.TriggerGameInitialize();
    
        isGameInitialized = true;
    }

    private void HandleGameStart()
    {
       
        if (currentGameState == GameState.Playing) return;
        
        ResetGameState();
        currentGameState = GameState.Playing;
        currentGameSpeed = settings.initialGameSpeed;
        
        
        GameEvents.TriggerSpeedChange(currentGameSpeed);
        
        StartCoroutine(GameUpdateRoutine());
    }
    
    private void HandleGameOver()
    {
        if (currentGameState != GameState.Playing) return;
        
        StopAllCoroutines();
        currentGameState = GameState.Defeat;
        currentGameSpeed = 0f;
        GameEvents.TriggerSpeedChange(currentGameSpeed);
    }
    
    private void ResetGameState()
    {
        StopAllCoroutines();
        currentGameSpeed = settings.initialGameSpeed;
        currentDifficultyMultiplier = settings.difficultyMultiplier;
        lastSpeedUpdateTime = Time.time;
        lastDifficultyUpdateTime = Time.time;
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
    Playing,
    Defeat
}