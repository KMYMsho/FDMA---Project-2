using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.OnHit(damage);
            }

            // Destroy the projectile after hitting the player
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Tool") && !other.CompareTag("Weapon"))
        {
            // Destroy the projectile if it hits any other object
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the projectile on collision with any surface
        Destroy(gameObject);
    }
}
