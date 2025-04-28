using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int health = 50;
    public float moveSpeed = 2f; // Speed at which the enemy moves toward the player
    public float detectionRange = 10f; // Distance at which the enemy detects the player
    public float attackCooldown = 2f; // Time between attacks
    public int damage = 20;
    private bool attacking = false;
    private Transform playerTransform;

    public ParticleSystem deathParticle;

    [SerializeField] private HealthBar healthBar;

    private float attackTimer = 0f; // Timer to track attack cooldown

    private void Start()
    {
        // Initialize health bar
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }

        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // If the player is within detection range, move toward the player
            if (distanceToPlayer <= detectionRange)
            {
                MoveTowardPlayer();
            }
        }

        // Update the attack timer
        attackTimer += Time.deltaTime;
    }

    private void MoveTowardPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;

        // Ignore the y-axis to prevent tilting
        direction.y = 0;

        // Rotate the enemy to face the player
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth rotation
        }

        // Move the enemy toward the player
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Player is in the AttackHitBox. Attacking set to true.");
        attacking = true;
        // Check if the player is in the AttackHitBox
        if (other.CompareTag("Player") && attackTimer >= attackCooldown)
        {
            Attack(other);
            attackTimer = 0f; // Reset the attack timer
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attacking = false;
            Debug.Log("Player left the AttackHitBox. Attacking set to false.");
        }
    }

    public void Attack(Collider player)
    {
        // Deal damage to the player
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.OnHit(damage); // Apply damage to the player
            Debug.Log("Player attacked by enemy!");
        }
    }

    public void OnHit(int damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }
        Debug.Log("Enemy health: " + health);

        if (health <= 0)
        {
            // Call OnKill from PlayerManager
            if (playerTransform != null)
            {
                PlayerManager playerManager = playerTransform.GetComponent<PlayerManager>();
                if (playerManager != null)
                {
                    playerManager.OnKill(); // Call the OnKill method
                }
            }

            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
