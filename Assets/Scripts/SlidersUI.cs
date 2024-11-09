using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class SlidersUI : MonoBehaviour
{

    public List<SliderObject> sliderList = new List<SliderObject>();
    public Dictionary<String, SliderObject> sliderDictionary = new Dictionary<String, SliderObject>();
    public Slider[] slidersBehaviour;
    public Button[] slidersMuteButtons;
    public Sprite[] muteIcons;
    public Transform slidersTransform;

    static SliderObject allSlider;
    static SliderObject musicSlider;
    static SliderObject playerSFXSlider;
    static SliderObject environmentSoundsSlider;
    static SliderObject dialogSlider;

    [HideInInspector]
    public bool initialized;


    public static SlidersUI Instance { get; private set; }


    public void InitializeSliders()
    {
        
        initialized = true;


        if (allSlider == null) { 
           allSlider = new SliderObject();
            allSlider.SliderBehaviour = slidersBehaviour[0];
           allSlider.key = "allVolume";
            allSlider.sliderMuteButton = slidersMuteButtons[0];
            //sliderList.Add(allSlider);
        }


        //-------------------------------------/
        if (musicSlider == null)
        {
            musicSlider = new SliderObject();
            musicSlider.SliderBehaviour = slidersBehaviour[1];
            musicSlider.key = "musicVolume";
            musicSlider.sliderMuteButton = slidersMuteButtons[1];
           //sliderList.Add(musicSlider);
        }
        //-------------------------------------/
        if(playerSFXSlider == null) { 
            playerSFXSlider = new SliderObject();
            playerSFXSlider.SliderBehaviour = slidersBehaviour[2];
            playerSFXSlider.key = "playerSFXVolume";
            playerSFXSlider.sliderMuteButton = slidersMuteButtons[2];
            //sliderList.Add(playerSFXSlider);
        }
        //-------------------------------------/
        if (environmentSoundsSlider == null)
        {
            environmentSoundsSlider = new SliderObject();
            environmentSoundsSlider.SliderBehaviour = slidersBehaviour[3];
            environmentSoundsSlider.key = "environmentSoundsVolume";
            environmentSoundsSlider.sliderMuteButton = slidersMuteButtons[3];
           // sliderList.Add(environmentSoundsSlider);
        }
        //-------------------------------------/
        if (dialogSlider == null)
        {
            dialogSlider = new SliderObject();
            dialogSlider.SliderBehaviour = slidersBehaviour[4];
            dialogSlider.key = "dialogVolume";
            dialogSlider.sliderMuteButton = slidersMuteButtons[4];
            //sliderList.Add(dialogSlider);
        }
        //--------------------------------------------



        if (sliderList.Count == 0)
        {
            sliderList.Add(allSlider);
            sliderList.Add(musicSlider);
            sliderList.Add(playerSFXSlider);
            sliderList.Add(environmentSoundsSlider);
            sliderList.Add(dialogSlider);

        }


        foreach (SliderObject s in sliderList)
        {
            s.AddSliderListener();
            s.muteIcons = muteIcons;
            sliderDictionary.Add(s.key, s);

            
        }
        SetStoredValuesFromPrefs();
        slidersTransform.gameObject.SetActive(false);

    }



    void Awake()
    {
        
        Instance = this;
      //  sliderList = new List<SliderObject>();
        SetStoredValuesFromPrefs();
       

    }

    private void SetStoredValuesFromPrefs() {

        foreach (SliderObject s in sliderList)
        {
            GameLog.LogMessage("Set default value" + s.key + " to " + "0.5f");

            PlayerPrefs.SetFloat(s.key, PlayerPrefs.GetFloat(s.key, 0.5f));

        }
    }


}