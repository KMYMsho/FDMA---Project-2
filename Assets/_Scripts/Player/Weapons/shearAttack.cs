using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shearAttack : MonoBehaviour
{
    public KeyCode attack = KeyCode.Mouse0;
    bool attacking;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
}
