using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int health = 50;
    private GameOverManager gameOverManager;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameOverManager in the scene
        gameOverManager = FindObjectOfType<GameOverManager>();

        // Get references to the PlayerMovement and PlayerCamera scripts
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponentInChildren<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit(int damage)
    {
        //print("Hit");
        health -= damage;
        print("Health: " + health);

        if (health <= 0)
        {
            die();
        }

    }
    public void die()
    {
        Time.timeScale = 0;

        // Disable player movement and camera control
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }
        // Show the game over screen
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver();
        }
    }
}
