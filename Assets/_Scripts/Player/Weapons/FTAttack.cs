using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FTAttack : MonoBehaviour
{
    public int fuel = 50;
    public int maxFuel = 100;
    private float fuelConsumptionRate = 10f; // Fuel consumed per second
    private float fuelConsumptionAccumulator = 0f; // Tracks accumulated fuel consumption
    public KeyCode attack = KeyCode.Mouse0;
    public bool attacking;
    public float damagePerSecond = 10f;
    private float accumulatedDamage = 0f;
    private float damageInterval = 0.2f; // Interval in seconds to apply damage
    private float damageTimer = 0f;

    public GameObject flameParts;
    public Transform flamePos;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private HealthBar healthBar2;

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

        // Decrease fuel while attacking
        if (attacking && fuel > 0)
        {
            // Particle Emitter
            GameObject flame = Instantiate(flameParts, flamePos.position, Quaternion.identity);
            flame.transform.SetParent(flamePos);
            flame.transform.rotation = flamePos.rotation;

            // Accumulate fuel consumption over time
            fuelConsumptionAccumulator += fuelConsumptionRate * Time.deltaTime;

            // Deduct whole units of fuel when the accumulator reaches or exceeds 1
            if (fuelConsumptionAccumulator >= 1f)
            {
                int fuelToDeduct = Mathf.FloorToInt(fuelConsumptionAccumulator);
                fuel -= fuelToDeduct;
                fuelConsumptionAccumulator -= fuelToDeduct;

                // Ensure fuel doesn't go below 0
                fuel = Mathf.Max(fuel, 0);

                Debug.Log("Fuel: " + fuel);
            }
        }
        healthBar.UpdateHealthBar(fuel, maxFuel);
        healthBar2.UpdateHealthBar(fuel, maxFuel);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (attacking)
            {
                enemyManager enemyManager = other.GetComponent<enemyManager>();
                if (enemyManager != null)
                {
                    enemyManager.OnHit(0);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (attacking && fuel > 0)
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
    public void Refuel(int amount)
    {
        
        fuel +=amount;
        Debug.Log("Refueled: " + amount);
        healthBar.UpdateHealthBar(fuel, maxFuel);
        healthBar2.UpdateHealthBar(fuel, maxFuel);
    }
}

