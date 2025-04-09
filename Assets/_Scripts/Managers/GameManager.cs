using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState {
        MainMenu,
        LevelSelection,
        Controls, 
        Playing, 
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
                //handleMainMenu();
                break;
            case GameState.Controls:
                break;
            case GameState.LevelSelection:
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

    //private void handleMainMenu()
    //{
    //    SceneManager.LoadScene("MainMenu");
    //}

    //public void ReloadCurrentScene()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}
