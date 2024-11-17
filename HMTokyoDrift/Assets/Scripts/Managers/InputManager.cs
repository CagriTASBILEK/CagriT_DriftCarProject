using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] public Joystick joystick;

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;
        
        float horizontalInput = joystick != null ? joystick.Horizontal : 0f;
        GameEvents.TriggerInputReceived(horizontalInput);
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        GameEvents.TriggerInputReceived(0f);
    }
}
