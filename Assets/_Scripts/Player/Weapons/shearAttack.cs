using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shearAttack : MonoBehaviour
{
    public KeyCode attack = KeyCode.Mouse0;
    public bool attacking;
    public float damagePerSecond = 25f;
    private float accumulatedDamage = 0f;
    private float damageInterval = 1f; // Interval in seconds to apply damage
    private float damageTimer = 0f;

    public ParticleSystem clippingParts;
    public Transform snips;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Sets attacking when lmb is pressed
        if (Input.GetMouseButtonDown(0))
        {
            attacking = true;
            
        }

        else if (Input.GetMouseButtonUp(0))
        {
            attacking = false;
            
        }
    }

    //Destroy enemy on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        //print("Touched something");
        if (other.CompareTag("Enemy"))
        {
            print("enemy touched");
            if (attacking == true)
            {
                enemyManager enemyManager = other.GetComponent<enemyManager>();
                if (enemyManager != null)
                {
                    enemyManager.OnHit(0);
                }
            }
        }
    }
    //Ensures that enemies are still damaged if they are in the trigger collider
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
                    Instantiate(clippingParts, snips.position, Quaternion.identity);

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

