using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    public KeyCode attack = KeyCode.Mouse0;
    public bool watering;

    public Animator animator;



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
            watering = true;
            animator.SetTrigger("Water");
        }

        else if (Input.GetMouseButtonUp(0))
        {
            watering = false;
            
        }
    }

    //Destroy enemy on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        //print("Touched something");
        if (other.CompareTag("Friendly"))
        {
            print("enemy touched");
            if (watering == true)
            {
                FriendlyManager friendlyManager = other.GetComponent<FriendlyManager>();
                friendlyManager.HealPlayer(50);
            }
        }
    }
    //Ensures that enemies are still damaged if they are in the trigger collider
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Friendly"))
        {
            if (watering == true)
            {
                FriendlyManager friendlyManager = other.GetComponent<FriendlyManager>();
                friendlyManager.HealPlayer(50);


            }
        }
    }
}

