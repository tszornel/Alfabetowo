using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SkyLetterManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void disableLetterLights() {

       Light2D[] lights = gameObject.GetComponentsInChildren<Light2D>();
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = 0;
        }
    }
}
