using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTAttack : MonoBehaviour
{
    public KeyCode attack = KeyCode.Mouse0;
    public bool attacking;
    public float damagePerSecond = 10f;
    private float accumulatedDamage = 0f;
    private float damageInterval = 0.5f; // Interval in seconds to apply damage
    private float damageTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attacking = true;
            
        }
            

        
        else if (Input.GetMouseButtonUp(0))
        {
            attacking = false;
            
        }
            
        
    }
    //Should consider adding delay to damage ticks to player can't spam damage procs
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //print("enemy touched");
            if (attacking == true)
            {
                //print("enemy hit by weapon");
                enemyManager enemyManager = other.GetComponent<enemyManager>();
                if (enemyManager != null)
                {
                    enemyManager.OnHit(2);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (attacking == true)
            {
                enemyManager enemyManager = other.GetComponent<enemyManager>();
                if (enemyManager != null)
                {
                    // Accumulate damage over time
                    accumulatedDamage += damagePerSecond * Time.deltaTime;
                    damageTimer += Time.deltaTime;

                    // Apply damage at regular intervals
                    if (damageTimer >= damageInterval)
                    {
                        int damageToApply = Mathf.FloorToInt(accumulatedDamage);
                        if (damageToApply > 0)
                        {
                            enemyManager.OnHit(damageToApply);
                            accumulatedDamage -= damageToApply;
                        }
                        damageTimer = 0f;
                    }
                }
            }
        }
    }
}

