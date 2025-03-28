using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLevelUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    void Start()
    {
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    void LoadMainMenu()
    {
        Debug.Log("Loading Main Menu");
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
