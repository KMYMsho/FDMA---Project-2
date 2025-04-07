using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shearAttack : MonoBehaviour
{
    public KeyCode attack = KeyCode.Mouse0;
    public bool attacking;
    

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
            print("attacking");
        }

        else if (Input.GetMouseButtonUp(0))
        {
            attacking = false;
            print("not attacking");
        }
    }

    //Destroy enemy on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        print("Touched something");
        if (other.CompareTag("Enemy"))
        {
            print("enemy touched");
            if (attacking == true)
            {
                print("enemy hit by weapon");
                Destroy(other.gameObject);
            }
        }
    }
    //Ensures that enemies are still damaged if they are in the trigger collider
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            print("enemy touched");
            if (attacking == true)
            {
                print("enemy hit by weapon");
                Destroy(other.gameObject);
            }
        }
    }
}

