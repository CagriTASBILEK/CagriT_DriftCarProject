using System;
using System.Collections;
using Model;
using UnityEngine;

namespace ViewModel
{
    public class GameUIViewModel : MonoBehaviour
    {
        private GameUIModel model;
        
        public event Action<float> OnScoreChanged;
        public event Action<GameState> OnGameStateChanged;
    
        private void Awake()
        {
            model = new GameUIModel();
        }

        private void OnEnable()
        {
            GameEvents.OnGameStart += HandleGameStart;
            GameEvents.OnGameOver += HandleGameOver;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStart -= HandleGameStart;
            GameEvents.OnGameOver -= HandleGameOver;
        }
        
        public void OnPlayButtonClicked()
        {
            GameManager.Instance.StartGame();
            model.SetGameState(GameState.Playing);
            OnGameStateChanged?.Invoke(GameState.Playing);
        }

        public void OnRestartButtonClicked()
        {
            model.ResetGame();
            OnGameStateChanged?.Invoke(GameState.MainMenu);
            OnScoreChanged?.Invoke(0);
        }

       
        private void HandleGameStart()
        {
            model.SetGameState(GameState.Playing);
            model.UpdateScore(0);
            OnGameStateChanged?.Invoke(GameState.Playing);
            OnScoreChanged?.Invoke(0);
        }

        private void HandleGameOver()
        {
            model.SetGameState(GameState.Defeat);
            OnGameStateChanged?.Invoke(GameState.Defeat);
        }

       
        public void UpdateScore(float points)
        {
            model.UpdateScore(points);
            OnScoreChanged?.Invoke(model.Score);
        }
    }
}