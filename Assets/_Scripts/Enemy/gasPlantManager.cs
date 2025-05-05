using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasPlantManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 40;
    [SerializeField] private int health = 40;
    public float moveSpeed = 1f; // Speed at which the enemy moves toward the player
    public float detectionRange = 20f; // Distance at which the enemy detects the player
    public float attackRange = 6f; // Distance at which the enemy can attack the player
    private Transform playerTransform;
    public float damagePerSecond = 10f; // Damage dealt per second to the player
    private float accumulatedDamage = 0f;
    private float damageInterval = 0.1f; // Interval in seconds to apply damage
    private float damageTimer = 0f;

    public ParticleSystem deathParticle;

    public Animator animator;

    [SerializeField] private HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player in the scene
        //healthBar = GetComponentInChildren<HealthBar>();
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(health, maxHealth);
        }
        

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the distance between the enemy and the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // If the player is within detection range, move toward the player
            if (distanceToPlayer <= detectionRange)
            {
                MoveTowardPlayer();
            }

            if(distanceToPlayer <= attackRange)
            {
                Attack();
            }
        }
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

    public void Attack()
    {
        animator.SetTrigger("Attack");
        // Accumulate damage over time
        accumulatedDamage += damagePerSecond * Time.deltaTime;
        damageTimer += Time.deltaTime;

        // Apply damage at regular intervals
        if (damageTimer >= damageInterval)
        {
            
            PlayerManager playerManager = playerTransform.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                int damageToApply = Mathf.FloorToInt(accumulatedDamage);
                if (damageToApply > 0)
                {
                    playerManager.OnHit(damageToApply);
                    accumulatedDamage -= damageToApply;
                }
            }
            damageTimer = 0f;
        }
    }
    public void OnHit(int damage)
    {
        
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
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
            //display death animation and gameover screen
            Destroy(gameObject);
        }
    }
}
