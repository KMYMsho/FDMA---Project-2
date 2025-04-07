using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameLevelUI : MonoBehaviour
{
    private Boolean isPaused = false;
    private Boolean isGameOver = false;
    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject pauseMenuUI;
    [Header("Heads Up Display (HUD)")]
    [SerializeField] private GameObject HUD;
    [SerializeField] private TextMeshProUGUI Scores;
    [SerializeField] private TextMeshProUGUI Health;

    void Awake()
    {
        HideAllUI();
        ShowHUD();
    }

    void Update()
    {
        UpdateHUD();
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

    public void UpdateHUD()
    {
        Scores.text = "updated score";
        Health.text = "updated health";
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

    public void HideAllUI()
    {
        HideDeathScreenUI();
        HideLevelCompleteUI();
        HidePauseMenuUI();
        HideHUD(); 
    }

    public void ShowLevelCompleteUI() => levelCompleteUI.SetActive(true);

    public void HideLevelCompleteUI() => levelCompleteUI.SetActive(false);

    public void ShowPauseMenuUI() => pauseMenuUI.SetActive(true);

    public void HidePauseMenuUI() => pauseMenuUI.SetActive(false);

    public void ShowDeathScreenUI() => deathScreenUI.SetActive(true);

    public void HideDeathScreenUI() => deathScreenUI.SetActive(false);

    public void ShowHUD() => HUD.SetActive(true); 

    public void HideHUD() => HUD.SetActive(false); 
}