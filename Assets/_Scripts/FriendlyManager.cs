using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyManager : MonoBehaviour
{
    public int maxWater = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void HealPlayer(int healingAmount)
    {
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.Heal(healingAmount);
                Destroy(gameObject);
            }
        }
    }
}
