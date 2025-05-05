using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelUI : MonoBehaviour
{
    private Boolean isPaused = false;
    private Boolean isGameOver = false;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject winScreenUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject HUD;

    [Header("Heads Up Display elements")]
    [SerializeField] private TextMeshProUGUI Scores;
    //[SerializeField] private TextMeshProUGUI Health;
    //[SerializeField] private TextMeshProUGUI Fuel;

    private PlayerManager playerManager;

    void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        HideAllUI();
        ShowHUD();
    }

    void Update()
    {
        //UpdateHUD();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            if (!isPaused)
            {
                Debug.Log("Game is not paused, pausing now");
                pauseGame();
            }
            else
            {
                Debug.Log("Game is paused, resuming now");
                resumeGame();
            }
        }
    }

    public void UpdateHUD(int currentScores, int scoreToWin, int currentHealth, int currentFuel)
    {
        Scores.text = "Score: " + currentScores + "/" + scoreToWin;
        //Health.text = "HP: " + currentHealth;
        //Fuel.text = "Fuel: " + currentFuel;
        //scores
        //health
        // other heads up display elements
    }

    public void pauseGame()
    {
        if (isGameOver)
        {
            return;
        }
        Time.timeScale = 0f;
        isPaused = true;
        ShowPauseMenuUI();
    }

    public void resumeGame()
    {
        isPaused = false;
        if (isGameOver)
        {
            return;
        }
        Time.timeScale = 1f;
        HidePauseMenuUI();
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        Debug.Log("Reloading current level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //GameManager.Instance.ReloadCurrentScene();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevelTwo()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Level Two Loading");
        SceneManager.LoadScene("LevelTWO");
    }

    public void HideAllUI()
    {
        HideDeathScreenUI();
        HideLevelCompleteUI();
        HideWinScreenUI();
        HidePauseMenuUI();
        HideHUD(); 
    }

    public void ShowLevelCompleteUI() => levelCompleteUI.SetActive(true);

    public void HideLevelCompleteUI() => levelCompleteUI.SetActive(false);

    public void ShowPauseMenuUI() => pauseMenuUI.SetActive(true);

    public void HidePauseMenuUI() => pauseMenuUI.SetActive(false);

    public void ShowDeathScreenUI() => deathScreenUI.SetActive(true);

    public void HideDeathScreenUI() => deathScreenUI.SetActive(false);

    public void ShowWinScreenUI() => winScreenUI.SetActive(true);

    public void HideWinScreenUI() => winScreenUI.SetActive(false);

    public void ShowHUD() => HUD.SetActive(true); 

    public void HideHUD() => HUD.SetActive(false); 
}