using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
        // Ensure the game over panel is initially disabled
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        // Enable the game over panel
        gameOverPanel.SetActive(true);
    }
}
