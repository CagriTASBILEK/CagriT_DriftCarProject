using Managers;
using UnityEngine;

namespace Model
{
    public class GameUIModel
    { 
        public float Score { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.MainMenu;
        

        public void UpdateScore(float points)
        {
            if (CurrentState != GameState.Playing) return;
            Score += points;
        }
        
        public void SetGameState(GameState newState)
        {
            CurrentState = newState;
        }
        public void ResetGame()
        {
            Score = 0;
            CurrentState = GameState.MainMenu;
        }

    }
}