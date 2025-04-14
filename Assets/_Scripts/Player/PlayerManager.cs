using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum EquipmentState
    {
        Shears,
        Flamethrower,
        Mower
    }
    public int health = 50;
    private int maxHealth = 50;
    private GameOverManager gameOverManager;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private GameLevelUI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<GameLevelUI>();
        // Find the GameOverManager in the scene
        gameOverManager = FindObjectOfType<GameOverManager>();

        // Get references to the PlayerMovement and PlayerCamera scripts
        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponentInChildren<PlayerCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        ui.UpdateHUD(0, health);
    }

    public void OnHit(int damage)
    {
        //print("Hit");
        health -= damage;
        print("Health: " + health);

        if (health <= 0)
        {
            Die();
        }

    }
    public void Die()
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
