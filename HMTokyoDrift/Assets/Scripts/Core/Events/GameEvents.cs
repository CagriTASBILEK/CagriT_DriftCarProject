using System;

public static class GameEvents
{
    public static event Action OnGameInitialize;
    public static event Action OnGameStart;
    public static event Action OnGamePause;
    public static event Action OnGameResume;
    public static event Action OnGameOver;
    
    public static event Action<float> OnSpeedChange;
    
    public static event Action<int> OnScoreChange;
    
    public static event Action<IVehicleState> OnVehicleStateChanged;
    public static event Action<float> OnInputReceived;
    
    public static void TriggerGameInitialize() => OnGameInitialize?.Invoke();
    public static void TriggerGameStart() => OnGameStart?.Invoke();
    public static void TriggerGamePause() => OnGamePause?.Invoke();
    public static void TriggerGameResume() => OnGameResume?.Invoke();
    public static void TriggerGameOver() => OnGameOver?.Invoke();
    public static void TriggerSpeedChange(float speed) => OnSpeedChange?.Invoke(speed);
    public static void TriggerScoreChange(int score) => OnScoreChange?.Invoke(score);
    public static void TriggerVehicleStateChanged(IVehicleState newState) => OnVehicleStateChanged?.Invoke(newState);
    public static void TriggerInputReceived(float value) => OnInputReceived?.Invoke(value);
    
}