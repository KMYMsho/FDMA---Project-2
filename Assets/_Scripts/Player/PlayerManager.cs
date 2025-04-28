using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum EquipmentState
    {
        Shears,
        Flamethrower,
        WateringCan,
        Mower
    }
    [SerializeField] private HealthBar healthBar;

    public int health = 100;
    private int maxHealth = 100;
    private int kills = 0;
    private GameOverManager gameOverManager;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private GameLevelUI ui;

    public GameObject ShearsParent; 
    public GameObject FlamethrowerParent;
    public GameObject WateringCanParent;
    private EquipmentState currentEquipment = EquipmentState.Shears;

    private FTAttack ftAttack;

    // Start is called before the first frame update
    void Start()
    {
        UpdateEquipmentState();

        ui = FindObjectOfType<GameLevelUI>();
        gameOverManager = FindObjectOfType<GameOverManager>();

        //healthBar = GetComponentInChildren<HealthBar>();
        healthBar.UpdateHealthBar(health, maxHealth);

        playerMovement = GetComponent<PlayerMovement>();
        playerCamera = GetComponentInChildren<PlayerCamera>();

        ftAttack = FlamethrowerParent.GetComponentInChildren<FTAttack>();

        if (ftAttack == null)
        {
            Debug.LogError("FTAttack reference is null in PlayerManager!");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        int currentFuel = ftAttack != null ? ftAttack.fuel : 0;
        ui.UpdateHUD(kills, health, currentFuel);

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
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("Watering Can selected");
            currentEquipment = EquipmentState.WateringCan;
            UpdateEquipmentState();
        }
    }

    public void OnHit(int damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        //print("Health: " + health);

        if (health <= 0)
        {
            Die();
        }

    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        health = Mathf.Min(health, maxHealth); // Ensure health doesn't exceed maxHealth
        healthBar.UpdateHealthBar(health, maxHealth);
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

        if (WateringCanParent != null)
        {
            WateringCanParent.SetActive(currentEquipment == EquipmentState.WateringCan);
        }

        // Add logic for Mower if needed in the future
    }
    public void OnKill()
    {
        kills++;
        Debug.Log("Kills: " + kills);

        if (kills >= 9)
        {
            Debug.Log("You win!");
            Win();
        }

        if (currentEquipment == EquipmentState.Shears && ftAttack != null)
        {
            ftAttack.Refuel(25); // Add fuel to the flamethrower
        }
    }

    public void Win()
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
      
        ui.ShowWinScreenUI();
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
        //if (gameOverManager != null)
        //{
        //    gameOverManager.ShowGameOver();
        //}
        ui.ShowDeathScreenUI();
    }
}
