using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class SliderObject
{
    public String key;
    public Slider SliderBehaviour;
    public Button sliderMuteButton;
    public bool muted;
    public Sprite[] muteIcons;
    private float previousValue;
   // public MusicSlidersData sliderData;

    public void AddSliderListener()
    {

        SliderBehaviour.onValueChanged.AddListener(HandleSliderValueChanged);
        
    }

    void HandleSliderValueChanged(float value)
    {
       // GameLog.LogMessage("Set volume to default values mixerGroup=" + key + " value=" + value);
        PlayerPrefs.SetFloat(key, value);
        Music.Instance.ChangeVolume(key, SliderBehaviour.value);

        if (value == 0)
        {
            muted = true;
            SwitchIcon();
           
        }
        else {
            muted = false;
            SwitchIcon();
           
        }



        }
    public void HandleMuteToggle()
    {
        GameLog.LogMessage("HandleMuteToggle Entered");
        if (!muted) {
          //  GameLog.LogMessage("Nie jest muted wiec zrob mute");

            previousValue = SliderBehaviour.value;
            SliderBehaviour.value = SliderBehaviour.minValue;
            SliderBehaviour.onValueChanged?.Invoke(SliderBehaviour.minValue);
            muted = true;
        }
        else {
            
            if (previousValue != 0)
                SliderBehaviour.value = previousValue;
            else
                SliderBehaviour.value = SliderBehaviour.maxValue;
            muted = false;
        }
           

        SwitchIcon();
        GameLog.LogMessage("HandleMuteToggle Left");
    }


    public void SwitchIcon()
    {

        if (muted && sliderMuteButton)
            sliderMuteButton.GetComponent<Image>().sprite = muteIcons[1];
        else if(sliderMuteButton)
            sliderMuteButton.GetComponent<Image>().sprite = muteIcons[0];

    }

}