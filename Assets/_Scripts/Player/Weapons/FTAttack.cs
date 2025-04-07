using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTAttack : MonoBehaviour
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

