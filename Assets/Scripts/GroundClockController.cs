using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundClockController : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            GameLog.LogMessage("Collide with player so switch cam entered ");

           //CameraSwitcher.SwitchCamera
        }
    }
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player")) 
        {

            GameLog.LogMessage("Collide with player so switch cam");
         
        
        }


    }
}
