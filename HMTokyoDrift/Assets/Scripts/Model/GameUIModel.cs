namespace Model
{
    public class GameUIModel
    { 
        public float Score { get; private set; }
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        public void UpdateScore(float newScore)
        {
            Score = newScore;
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