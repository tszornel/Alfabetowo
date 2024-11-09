using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLighterVisibleInCamera : MonoBehaviour
{
    LighterController controller;


    private void Awake()
    {
        transform.parent.parent.GetComponent<LighterController>();
    }
    // Start is called before the first frame update
    private void OnBecameVisible()
    {
        //controller.SetLighterVisible();
    }


    private void OnBecameInvisible()
    {
    /*    if (controller.GetState() == State.None) {
            GameLog.LogMessage("Set lighter invisible");
            controller.UnSetLighterVisible();
        }*/
            
    }
    
}
