using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button startButton;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        startButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        Debug.Log("Loading GameLevel");
        SceneManager.LoadSceneAsync("TutorialScene");
    }
}
