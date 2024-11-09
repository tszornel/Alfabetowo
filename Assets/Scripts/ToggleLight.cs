using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ToggleLight : MonoBehaviour
{
    private Light2D intactableLight;
    public float lightIntensity = 2f;
    public float tweenTime = 0.8f;

    private void Awake()
    {
        intactableLight = GetComponent<Light2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (intactableLight)
        {
            intactableLight.enabled = true;
            LeanTween.value(gameObject, 0, lightIntensity, tweenTime).setEaseLinear().setOnUpdate(setLight).setLoopPingPong();

        }
    }
    private void setLight(float value)
    {
        intactableLight.intensity = value;
    }

}
