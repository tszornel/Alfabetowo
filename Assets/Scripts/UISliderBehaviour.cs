using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISliderBehaviour : MonoBehaviour
{

    [Header("UI References")]
    public Slider sliderForBehaviour;
   
    public UITextBehaviour textSliderDisplay;
    // private bool turnRight = true;


    private void Awake()
    {
        if (sliderForBehaviour == null)
            sliderForBehaviour = GetComponent<Slider>();
        if (textSliderDisplay == null)
            textSliderDisplay = GetComponent<UITextBehaviour>();
    }

    private void Start()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);


        if (sliderForBehaviour == null)
            sliderForBehaviour = GetComponent<Slider>();
        if (textSliderDisplay == null)
            textSliderDisplay = GetComponent<UITextBehaviour>();

        if (!sliderForBehaviour.isActiveAndEnabled)
            sliderForBehaviour.enabled = true;
        // transform.Find("");
       
    }

    public void SetupDisplay(float totalValue)
    {
        if(!gameObject.activeSelf)   
            gameObject.SetActive(true);
        if(sliderForBehaviour == null)
            sliderForBehaviour = GetComponent<Slider>();
        if (!sliderForBehaviour.isActiveAndEnabled)
            sliderForBehaviour.enabled = true;
        SetMaxValue(totalValue);
    }

    void SetMaxValue(float newValue)
    {
        if(sliderForBehaviour)
            sliderForBehaviour.maxValue = newValue;
    }

    public float GetSliderValue()
    {
        if (sliderForBehaviour)
            return sliderForBehaviour.value;
        else return 0;
    }

    public void RotateText(bool rotateRight) 
    {
        if(rotateRight)
            textSliderDisplay.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 180, 0);   
        else
            textSliderDisplay.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0,0);  

    }

    public void SetCurrentValue(float newValue)
    {
        //GameLog.LogMessage("SetCurrentValue on Slider" + newValue);
        if(sliderForBehaviour)
            sliderForBehaviour.value = newValue;
        SetTextDisplay();
    }

    void SetTextDisplay()
    {
        if (textSliderDisplay != null)
        {
            textSliderDisplay.SetText(sliderForBehaviour.value + "/" + sliderForBehaviour.maxValue);
        }
    }

   /* private void OnApplicationQuit()
    {
        slider.value = 0;
    }
    private void OnDestroy()
    {
        slider.value = 0;
    }*/

}
