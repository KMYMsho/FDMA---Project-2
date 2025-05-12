using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shearAttack : MonoBehaviour
{
    public KeyCode attack = KeyCode.Mouse0;
    public bool attacking;
    public float damagePerSecond = 15f;
    public float attackDelay = .5f; // Delay between attacks
    public ParticleSystem clippingParts;
    public Transform snips;

    public Animator animator;

    public bool canAttack = true; // Cooldown flag

    [Header("Audio")]
    [SerializeField] private AudioSource attackAudioSource; // **NEW** AudioSource for attack sound
    [SerializeField] private AudioClip attackClip;

    private void Update()
    {
        // Check for left mouse button click and if the player can attack
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            attacking = true;
            animator.SetTrigger("Attack");

            if (attackAudioSource != null && attackClip != null)
            {
                attackAudioSource.PlayOneShot(attackClip);
            }
            ApplyDamageToEnemiesInHitbox();
            StartCoroutine(AttackCooldown());
        }

        // Stop attacking when the left mouse button is released
        else if (Input.GetMouseButtonUp(0))
        {
            attacking = false;
        }
    }

    private IEnumerator AttackCooldown()
    {
        // Disable attacking during cooldown
        canAttack = false;

        // Wait for the specified delay
        yield return new WaitForSeconds(attackDelay);

        // Re-enable attacking
        canAttack = true;
    }

    private void ApplyDamageToEnemiesInHitbox()
    {
        // Find all colliders in the hitbox
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f); // Adjust radius as needed
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                enemyManager enemy = collider.GetComponent<enemyManager>();
                if (enemy != null)
                {
                    enemy.OnHit((int)damagePerSecond);
                }

                // Play particle effect
                if (clippingParts != null && snips != null)
                {
                    Instantiate(clippingParts, snips.position, Quaternion.identity);
                }

                rangedPlantManager rangedEnemy = collider.GetComponent<rangedPlantManager>();
                if (rangedEnemy != null)
                {
                    rangedEnemy.OnHit((int)damagePerSecond);
                }

                // Play particle effect
                if (clippingParts != null && snips != null)
                {
                    Instantiate(clippingParts, snips.position, Quaternion.identity);
                }

                gasPlantManager gasEnemy = collider.GetComponent<gasPlantManager>();
                if (gasEnemy != null)
                {
                    gasEnemy.OnHit((int)damagePerSecond);
                }

                // Play particle effect
                if (clippingParts != null && snips != null)
                {
                    Instantiate(clippingParts, snips.position, Quaternion.identity);
                }
            }
        }
    }
}

