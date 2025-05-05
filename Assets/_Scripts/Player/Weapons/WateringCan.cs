using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public KeyCode attack = KeyCode.Mouse0;
    public bool watering;

    public Animator animator;

    // Update is called once per frame
    void Update()
    {
        // Start watering when the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            watering = true;
            animator.SetBool("IsWatering", true); // Set the boolean parameter to true
        }

        // Stop watering when the left mouse button is released
        else if (Input.GetMouseButtonUp(0))
        {
            watering = false;
            animator.SetBool("IsWatering", false); // Set the boolean parameter to false
        }
    }

    // Heal friendly objects on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friendly") && watering)
        {
            FriendlyManager friendlyManager = other.GetComponent<FriendlyManager>();
            if (friendlyManager != null)
            {
                friendlyManager.HealPlayer(50);
            }
        }
    }

    // Heal friendly objects while staying in the trigger collider
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Friendly") && watering)
        {
            FriendlyManager friendlyManager = other.GetComponent<FriendlyManager>();
            if (friendlyManager != null)
            {
                friendlyManager.HealPlayer(50);
            }
        }
    }
}

