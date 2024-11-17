using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

namespace View
{
    public class GameUIView : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private GameObject defeatPanel;

        [Header("UI Elements")] 
        [SerializeField] private Button playButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI finalScoreText;

        [Header("References")] [SerializeField]
        private GameUIViewModel viewModel;

        
        private void Start()
        {
            HandleGameStateChanged(GameState.MainMenu);
        }

        
        private void OnEnable()
        {
            if (viewModel != null)
            {
                viewModel.OnGameStateChanged += HandleGameStateChanged;
                viewModel.OnScoreChanged += HandleScoreChanged;
            }

            SetupButtons();
        }

        private void OnDisable()
        {
            if (viewModel != null)
            {
                viewModel.OnGameStateChanged -= HandleGameStateChanged;
                viewModel.OnScoreChanged -= HandleScoreChanged;
            }

            CleanupButtons();
        }

        private void SetupButtons()
        {
            if (playButton != null)
                playButton.onClick.AddListener(viewModel.OnPlayButtonClicked);

            if (restartButton != null)
                restartButton.onClick.AddListener(viewModel.OnRestartButtonClicked);
        }

        private void CleanupButtons()
        {
            if (playButton != null)
                playButton.onClick.RemoveListener(viewModel.OnPlayButtonClicked);

            if (restartButton != null)
                restartButton.onClick.RemoveListener(viewModel.OnRestartButtonClicked);
        }

        private void HandleGameStateChanged(GameState newState)
        {
            mainMenuPanel?.SetActive(newState == GameState.MainMenu);
            gameplayPanel?.SetActive(newState == GameState.Playing);
            defeatPanel?.SetActive(newState == GameState.Defeat);
            
            if (newState == GameState.Defeat)
            {
                if (finalScoreText != null && scoreText != null)
                {
                    finalScoreText.text = scoreText.text;
                }
            }
        }

        private void HandleScoreChanged(float newScore)
        {
            if (scoreText != null)
                scoreText.text = $"Score: {newScore:N0}";

            if (finalScoreText != null)
                finalScoreText.text = $"Final Score: {newScore:N0}";
        }
    }
}