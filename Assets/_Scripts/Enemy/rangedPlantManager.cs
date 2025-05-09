using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedPlantManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int health = 20;
    public float detectionRange = 10f; // Distance at which the enemy detects the player
    public float attackRange = 20f; // Distance at which the enemy can attack the player
    public float attackCooldown = 2f; // Time between attacks
    public GameObject projectilePrefab; // The projectile to launch
    public Transform projectileSpawnPoint; // Where the projectile spawns
    public float projectileSpeed = 10f; // Speed of the projectile

    private Transform playerTransform;
    private float attackTimer = 0f;
    private GameObject currentProjectile; // Reference to the currently active projectile

    public ParticleSystem deathParticle;

    public Animator animator;

    [SerializeField] private HealthBar healthBar;

    [Header("Audio")]
    [SerializeField] private AudioSource attackAudioSource; //  Reference to the AudioSource for attack sound
    [SerializeField] private AudioClip attackClip; //  Audio clip for the attack sound


    // Start is called before the first frame update
    void Start()
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

        if (attackAudioSource == null)
        {
            Debug.LogError("rangedPlantManager: Missing AudioSource component for attack sound. Please assign it in the inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the distance between the enemy and the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // If the player is within detection range, rotate to face the player
            if (distanceToPlayer <= detectionRange)
            {
                RotateTowardPlayer();
            }

            // If the player is within attack range, attack
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
        }
    }

    private void RotateTowardPlayer()
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
    }

    public void Attack()
    {
        
        // Check if the attack cooldown has elapsed
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            animator.SetTrigger("Attack");
            // Reset the attack timer
            attackTimer = 0f;

            // **NEW** Play the attack sound
            if (attackAudioSource != null && attackClip != null)
            {
                attackAudioSource.PlayOneShot(attackClip);
            }

            //// Destroy the current projectile if it exists
            //if (currentProjectile != null)
            //{
            //    Destroy(currentProjectile);
            //}

            // Instantiate the new projectile
            if (projectilePrefab != null && projectileSpawnPoint != null)
            {
                currentProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

                // Calculate the direction to the player
                Vector3 direction = (playerTransform.position - projectileSpawnPoint.position).normalized;

                // Set the projectile's velocity
                Rigidbody rb = currentProjectile.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = direction * projectileSpeed;
                }
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
        print("Health: " + health);

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
