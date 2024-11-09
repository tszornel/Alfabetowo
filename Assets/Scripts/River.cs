using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class River : MonoBehaviour
{
    public GameObject drawningEffect;
    public Transform respawnPoint; 
    public static bool drawning = false;
    public static event EventHandler OnPlayerDrowning;
    


    private void OnTriggerEnter2D(Collider2D other)
    {
       // GameLog.LogMessage("DRAWNING!!!!" + other.GetType());
        if (other.tag == "Player" && !drawning)
        {

            GameLog.LogMessage(transform.name+" I!!!NVOKE OnPlayerDrawning!" + other.GetType());
            if(!other.GetComponent<PlayerPlatformerController>().disableCollisions)
                OnPlayerDrowning?.Invoke(this, EventArgs.Empty);


        }
         
    }

   
}
