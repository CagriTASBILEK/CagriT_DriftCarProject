using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Settings/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Game Speed")]
    public float initialGameSpeed = 10f;
    public float maxGameSpeed = 25f;
    public float speedIncreaseRate = 0.1f;

    [Header("Difficulty")]
    public float difficultyMultiplier = 1f;
    public float maxDifficultyMultiplier = 2f;
    public float difficultyIncreaseRate = 0.05f;
}