using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int health = 50;
    public float moveSpeed = 2f; // Speed at which the enemy moves toward the player
    public float detectionRange = 10f; // Distance at which the enemy detects the player
    private float attackCooldown = 2f; // Time between attacks
    public int damage = 20;
    private bool attacking = false;
    private Transform playerTransform;

    public Animator animator;

    public ParticleSystem deathParticle;

    [SerializeField] private HealthBar healthBar;

    private float attackTimer = 0f; // Timer to track attack cooldown
    private Coroutine damageCoroutine; // Reference to the DelayedDamage coroutine

    [Header("Audio")]
    [SerializeField] private AudioSource walkingAudioSource; // **NEW** AudioSource for walking sound
    [SerializeField] private AudioClip walkingClip;

    private bool isWalking = false;

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

        if (walkingAudioSource == null)
        {
            Debug.LogError("enemyManager: Missing AudioSource component for walking sound. Please assign it in the inspector.");
        }
        else
        {
            walkingAudioSource.loop = true; // Ensure the walking sound loops
            walkingAudioSource.clip = walkingClip;
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
            else
            {
                StopWalking(); // **NEW** Stop walking sound if the enemy is not moving
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
        StartWalking();
    }

    private void StartWalking() 
    {
        if (!isWalking)
        {
            isWalking = true;
            if (walkingAudioSource != null && !walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Play();
            }
        }
    }

    private void StopWalking() 
    {
        if (isWalking)
        {
            isWalking = false;
            if (walkingAudioSource != null && walkingAudioSource.isPlaying)
            {
                walkingAudioSource.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Start attacking the player
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the player is in the AttackHitBox
        if (other.CompareTag("Player"))
        {
            
            attacking = true;
            if (attackTimer >= attackCooldown)
            {
                Attack(other);
                attackTimer = 0f; // Reset the attack timer
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            attacking = false;
            Debug.Log("Player left the AttackHitBox. Attacking set to false.");

            // Stop the DelayedDamage coroutine if it's running
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    public void Attack(Collider player)
    {
        // Only start the coroutine if the player is still in the hitbox
        if (attacking)
        {
            animator.SetTrigger("Attack");
            // Start a coroutine to delay the damage application
            damageCoroutine = StartCoroutine(DelayedDamage(player, 0.5f)); // Delay damage by 0.5 seconds
        }
    }

    private IEnumerator DelayedDamage(Collider player, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Check if the player is still in the hitbox before applying damage
        if (attacking)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.OnHit(damage); // Apply damage to the player
                Debug.Log("Player attacked by enemy after delay!");
            }
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
