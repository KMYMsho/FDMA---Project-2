using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState {
        MainMenu,
        LevelSelection,
        Controls, 
        Intro, 
        Playing, 
        Outro, 
        GameOver, 
        LevelCompleted
    }
    [SerializeField] public GameState CurrentState;
    public void SetState(GameState newState)
    {
        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Controls:
                break;
            case GameState.LevelSelection:
                break;
            case GameState.Intro:
                break;
            case GameState.Outro:
                break;
            case GameState.Playing:
                break;
            case GameState.GameOver:
                break;
            case GameState.LevelCompleted:
                break;
            default:
                Debug.LogError("Invalid game state");
                break;
        }
    }

    public GameState getState() => CurrentState;
}
