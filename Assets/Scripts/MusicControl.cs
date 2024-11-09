using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Webaby.Utils;

public class MusicControl : MonoBehaviour
{
    //float _multiplier = 20f;

    // Start is called before the first frame update
    private Music music;
    private Image soundIcon;
    public Sprite[] volumeSprites;
    bool slidersShown=false;
    public SlidersUI sliders;
    private bool initialized;
    private float previousValue;
    SliderObject musicSlider;
    SliderObject dialogSlider;

    public static MusicControl Instance { get; private set; }
    public bool Initialized { get => initialized; private set => initialized = value; }

    private void OnDestroy()
    {
        SafeSliderValues();
        Initialized = false;
    }

    private void Awake()
    {
        music = Music.Instance;
        sliders = GetComponent<SlidersUI>();
        
        if(!sliders.initialized)
            sliders.InitializeSliders();

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
       
        soundIcon = GetComponent<Image>();
        GetComponent<Button_UI>().LongTouchFunc = () =>
        {
            // Use item
            GameLog.LogMessage("SUPER !!! Pokaz slidery ");
            ToggleSliders();
        };

        GetComponent<Button_UI>().ShortTouchFunc = () =>
        {
           // Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
            // Use item
            if (slidersShown)
            {
                //Hide Sliders
                ToggleSliders();
            }
            else
            {
                GameLog.LogMessage("SUPER !!! Zrob mute ");
                ToggleMuteVolume("allVolume");
            }

        };

        GetComponent<Button_UI>().ClickFunc = () =>
        {
            // Use item
            if (slidersShown)
            {
                //Hide Sliders
                ToggleSliders();
            }
            else
            {
                GameLog.LogMessage("SUPER !!! Zrob mute ");
                ToggleMuteVolume("allVolume");
            }

        };
        SafeSliderValues();



    }

    private void Start()
    {

        InitializeSoundVolume();

    }

    void InitializeSoundVolume() {
        Initialized = true;
        GameLog.LogMessage("InitializeSoundVolume Entered");
        foreach (SliderObject s in SlidersUI.Instance.sliderList) {

            float storedValue = PlayerPrefs.GetFloat(s.key, 0.5f);
            if (storedValue == 0)
            {
                GameLog.LogMessage("Default value 0.5 . Not found in player prefs");
                s.SliderBehaviour.value = 1f;
                PlayerPrefs.SetFloat(s.key, s.SliderBehaviour.value);
            }
            else 
            {
                s.SliderBehaviour.value = storedValue;
            }
 
            
            s.SliderBehaviour.onValueChanged?.Invoke(s.SliderBehaviour.value);
            GameLog.LogMessage("InitializeSoundVolume Left");

        }
    
    
    }

    /* This method is for fading sliders for volume tap */
    private void ToggleSliders()
    {
       
        GameLog.LogMessage("Show Sloders Entered");
        if (!slidersShown)
        {
            slidersShown = true;
            StartCoroutine(FadeSlidersRoutine(1));
        }
        else
        {
            slidersShown = false;
            StartCoroutine(FadeSlidersRoutine(0));

        }

    }
    /* This method is Routine for fading sliders for volume tap */
    IEnumerator FadeSlidersRoutine(int v)
    {
        if (v == 0)
        {
            LeanTween.alphaCanvas(sliders.slidersTransform.gameObject.GetComponent<CanvasGroup>(), v, 0.5f);
            yield return new WaitForSeconds(0.5f);
            SafeSliderValues();
            sliders.slidersTransform.gameObject.SetActive(false);
        }
        else
        {
            /*if (!initialized)
                InitializeSoundVolume();*/
            sliders.slidersTransform.gameObject.SetActive(true);
            LeanTween.alphaCanvas(sliders.slidersTransform.GetComponent<CanvasGroup>(), v, 0.5f);
        }
    }
    /* Toggle music volume when tap valume icon */
    public void ToggleMuteVolume(String volumeParameterName)
    {
        GameLog.LogMessage("Toggle Music Volume/UNMUTE"+ volumeParameterName);

        SwitchMute(volumeParameterName);
    }


    public void MuteMusicWhenDialog() {

        musicSlider = sliders.sliderDictionary["musicVolume"];
        dialogSlider = sliders.sliderDictionary["dialogVolume"];
        if (!musicSlider.muted) { 
            previousValue = musicSlider.SliderBehaviour.value;
            float dialogVolumeValue = dialogSlider.SliderBehaviour.value;

            musicSlider.SliderBehaviour.value = dialogVolumeValue - 0.6f;
            Music.Instance.ChangeVolume(musicSlider.key, musicSlider.SliderBehaviour.value);
        }



    }

    public void UnMuteMusicWhenDialog()
    {
        SliderObject musicSlider = sliders.sliderDictionary["musicVolume"];
        if (!musicSlider.muted)
        {
            musicSlider.SliderBehaviour.value = previousValue;
            Music.Instance.ChangeVolume(musicSlider.key, musicSlider.SliderBehaviour.value);
        }

    }
    public void SwitchMute(String volumeParameterName) 
    {
        
       
            SliderObject s;
            sliders.sliderDictionary.TryGetValue(volumeParameterName, out s);
            if (s!=null)
            {
                s.HandleMuteToggle();
            if (volumeParameterName == "allVolume")
            {
                SwitchMainSoundIcon();
            }

        }
            else
            {
            
           // sliders.InitializeSliders();
            
                GameLog.LogMessage("Mising slider in dictionary for key " + volumeParameterName);

            }
        
     


    }


    private void SwitchMainSoundIcon() {

        if (soundIcon.sprite == volumeSprites[0])
            soundIcon.sprite = volumeSprites[1];
        else
            soundIcon.sprite = volumeSprites[0];
        
    }


    void SafeSliderValues() {

        foreach (SliderObject s in sliders.sliderList)
        {

            PlayerPrefs.SetFloat(s.key, s.SliderBehaviour.value);
         }
        PlayerPrefs.Save();

    }
    private void OnDisable()
    {
        SafeSliderValues();
    }

    private void OnAplicationQuit()
    {
        SafeSliderValues();
    }
}
