using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject flameParts;
    public Transform flamePos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject flame = Instantiate(flameParts, flamePos.position, Quaternion.identity);
        flame.transform.SetParent(flamePos);
        flame.transform.rotation = flamePos.rotation;
    }
}
