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

    public GameObject ShearsParent; 
    public GameObject FlamethrowerParent; 
    private EquipmentState currentEquipment = EquipmentState.Shears;

    // Start is called before the first frame update
    void Start()
    {
        UpdateEquipmentState();

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

        // Check for key presses to switch equipment
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("Shears selected");
            currentEquipment = EquipmentState.Shears;
            UpdateEquipmentState();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("Flamethrower selected");
            currentEquipment = EquipmentState.Flamethrower;
            UpdateEquipmentState();
        }
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

    private void UpdateEquipmentState()
    {
        // Enable/Disable GameObjects based on the current equipment state
        if (ShearsParent != null)
        {
            ShearsParent.SetActive(currentEquipment == EquipmentState.Shears);
        }

        if (FlamethrowerParent != null)
        {
            FlamethrowerParent.SetActive(currentEquipment == EquipmentState.Flamethrower);
        }

        // Add logic for Mower if needed in the future
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
