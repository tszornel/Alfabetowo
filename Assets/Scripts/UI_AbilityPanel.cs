using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_AbilityPanel : MonoBehaviour
{
    public GameObject UIPanel;
    public GameObject SliderTransform;
    public GameObject sliderBackground;
    private Slider slider;
    public GameObject buttonAbility;
    public TextMeshProUGUI portraitImages;
    public bool HideLighterPanel=false; 
    public string spriteName;
    private bool active;
    [SerializeField] SteeringPanel steeringController;
    private UnitAudioBehaviour audioSounds;
    public static UI_AbilityPanel instance;
    private bool heroTouched;
    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    public string inventoryName;
    [SerializeField] InventoryObject inventoryObject;
    public UISliderBehaviour sliderBehaviour;
    public Image ringImage;
    public Health health;
    private Color LighterBackgroundPanelColor =  new Color(7, 2, 19, 1);
    private Color HeroColor = new Color(137, 67, 41, 255);


    void SignUpToHealthChange()
    {
        health.UpdateHealthAction += DisplayHealhPanel;
    }


    void UnSignUpToHealthChange()
    {
        health.UpdateHealthAction -= DisplayHealhPanel;
    }
    void SetupSliderDisplay(Health _health)
    {
        GameLog.LogMessage("SetupSliderDisplay entered max healt"+_health.GetMaxHealth());
        if (!sliderBehaviour)
            sliderBehaviour = GetComponent<UISliderBehaviour>();
        //health = _healthBehaviour;
        int totalHealth = _health.GetCurrentHealth();
        int maxHealth = _health.GetMaxHealth();
        GameLog.LogMessage("Enemy" + gameObject.name + " health=" + totalHealth);
        SignUpToHealthChange();
        sliderBehaviour.SetupDisplay((float)maxHealth);
        UpdateHealthDisplay(totalHealth);
    }

    void UpdateHealthDisplay(int newHealthAmount)
    {
        sliderBehaviour.SetCurrentValue((float)newHealthAmount);
        
    }


    private void DisplayHealhPanel(Health newHealth)
    {
        UpdateHealthDisplay(newHealth.GetCurrentHealth());
    }


    public InventoryObject GetInventoryObject() { 
    
        return inventoryObject; 
    }
    
   
    private void Awake()
    {
        LighterController.LighterControl += OnStateChanged;
        PlayerPlatformerController.HeroController += OnHeroTouched;
      //  InventoryPanelController.HeroPanel += OnHeroTouchedinInventoryRoom
        active = false;
        slider = SliderTransform?.GetComponent<Slider>();
        audioSounds = GetComponent<UnitAudioBehaviour>();
        // slider = 
        heroTouched = false;
        if(!inventoryObject)
            inventoryObject = PickupItemAssets.Instance.inventoryDatabase.GetInventoryObjectFromName(inventoryName);  
        if (ringImage == null)
            ringImage = GameObject.Find("Image_Ring_Fill")?.GetComponent<Image>();
        if(!steeringController)
            steeringController = SteeringPanel.Instance;
        if (health!=null)
            SetupSliderDisplay(health);
        else if(sliderBehaviour)
            sliderBehaviour.SetupDisplay(1f);


    }



    private void OnDisable()
    {
        LighterController.LighterControl -= OnStateChanged;
        PlayerPlatformerController.HeroController -= OnHeroTouched;
    }

    public bool isPanelActive()
    {
        return active;
    }

    public void TweenRing()
    {
       
        if (!active)
        {
            FillRing();
        }
        else
        {
            UnFillRing(0.5f);
        }
    }

    public void FillRing()
    {
        active = true;
      
        GameLog.LogMessage("Charge Hero");
      

        if(health)
            TweenRing(ringImage, 0, health.GetCurrentHealth(), 0.3f);
        else
            TweenRing(ringImage, 0, 1, 0.3f);
        
        // buttonAbility.SetActive(true);
        // heroTouched = true;
        ActivatePortraitForSeconds(2);
    }
    public void UnFillRing(float tweenTime)
    {
        active = false;
        // heroTouched = false;
        if(health==null)    
            TweenRing(ringImage, 1, 0, 0.3f);
        else
            TweenRing(ringImage, health.GetCurrentHealth(), 0, 0.3f);
        // buttonAbility.SetActive(false);
        DeactivatePortraitAnim();
    }

    void TweenRing(Image ring, float from, float to, float tweenTime)
    {
        
        GameLog.LogMessage("SetRing entered " + to);
       
        LeanTween.value(ringImage.gameObject, from, to, tweenTime).setOnUpdate((to) =>
        {
            // ringImage.fillAmount = to;
            sliderBehaviour.SetCurrentValue(to);

        }
        );
    }


    private void OnHeroTouched(object sender, PlayerPlatformerController.OnHeroStateChangedEventArgs e)
    {
        GameLog.LogMessage("OnHeroTouched entered sender:"+ sender+" e"+e.isInventoryOpen+" heroTouched: "+heroTouched);
        if (sender.ToString().Contains("Hero") && this.name.Equals("UI_Unit_Display_Lighter"))
            return;

        if (sender.ToString().Contains("Hero") && this.name.Equals("UI_Unit_Display_Enemy"))
            return;
        
        HeroTouched();
    }
    public void HeroTouched() {
        if (!heroTouched)
        {
           heroTouched = true;
            if (!active)
            {
                GameHandler.Instance?.ShowInventory();
                WindowCharacter_Portrait.SetDoNotHide();
                DisplayPanelHero();
                active = true;
            }
            else
            {
                WindowCharacter_Portrait.UnSetDoNotHide();
                GameHandler.Instance?.HideInventoryImmediate();
                UnChargeHero(0.5f);
                active = false;
            }
        }
    }
    private void SetPanelWindow(GameObject gameObject, float from, float to, float tweenTime)
    {
        gameObject.SetActive(true);
        GameLog.LogMessage("SetDescriptionWindow Entered " + to);
        
        gameObject.GetComponent<RectTransform>().localScale = new Vector2(from, from);
        LeanTween.value(gameObject, from, to, tweenTime).setOnUpdate((to) =>
        {
            // GameLog.LogMessage("POSITION x: to " + to);
            gameObject.GetComponent<RectTransform>().localScale = new Vector2(to, to);
            //  GameLog.LogMessage("POSITION x:" + gameObject.GetComponent<RectTransform>().anchoredPosition.x);
        }
        ).setEaseOutElastic().setOnComplete(ChargeHero);
    }
    private void ActivatePortraitForSeconds(int sec) 
    {
        StartCoroutine(ActivatePortraitAnimCouroutine(sec));
    }
    private void ActivatePortraitAnim() 
    {
        if (portraitImages)
        {
            portraitImages.text = "<sprite anim=\"0,3,10\">";
        }
    }


    public void SwitchPortraitImage(string image,TMP_SpriteAsset asset)
    {

        if (asset) 
        { 
            portraitImages.spriteAsset = asset;
            portraitImages.UpdateFontAsset();
        }

        portraitImages.text = "<sprite name=\""+image+"\">";
        
    }
    private void SetLighterPanelColor() 
    {
        TweenSliderColor(sliderBackground, Color.red, 0.5f);
    }


    private void SetHeroColor()
    {
        TweenImageColor(ringImage, new Color(255,255,255), 0.3f);
    }

    private void UnSetHeroColor() 
    {
        TweenImageColor(ringImage, Color.blue, 0.3f);
     
    }
    
    private void UnSetLighterPanelColor()
    {
        TweenSliderColor(sliderBackground, LighterBackgroundPanelColor, 0.5f);
    }
    IEnumerator ActivatePortraitAnimCouroutine(int sec)
    {
        ActivatePortraitAnim();
        buttonAbility.SetActive(true);
        yield return new WaitForSeconds(sec);
        buttonAbility.SetActive(false);
        DeactivatePortraitAnim();
    }
    private void DeactivatePortraitAnim()
    {
        if (portraitImages)
        {
            portraitImages.text = "<sprite name=\""+spriteName+"\">";
        }
    }
    void ChargeHero() {
        GameLog.LogMessage("Charge Hero");
        SetHeroColor();
        if (!slider)
            slider = SliderTransform?.GetComponent<Slider>();
        if(health!=null)
            TweenSlider(SliderTransform.GetComponent<Slider>(), 0, health.GetCurrentHealth(),0.3f);
        else
            TweenSlider(SliderTransform.GetComponent<Slider>(), 0, 1, 0.3f);
        // buttonAbility.SetActive(true);
        heroTouched = false;
        ActivatePortraitForSeconds(2);
    }
    void UnChargeHero(float tweenTime)
    {
        if (!slider)
            slider = SliderTransform?.GetComponent<Slider>();
        heroTouched = false;
        UnSetHeroColor();
       // TweenSlider(slider, 1, 0,tweenTime);
       // buttonAbility.SetActive(false);
        DeactivatePortraitAnim();
    }
    void TweenSliderColor(GameObject sliderBackground, Color toColor, float tweenTime)
    {
        SliderTransform.SetActive(true);
        GameLog.LogMessage("SetSlider color entered change color="+ toColor, sliderBackground);
        LeanTween.color(sliderBackground, toColor, tweenTime);

    }

    void TweenImageColor(Image sliderBackground, Color toColor, float tweenTime)
    {
        SliderTransform.SetActive(true);
        GameLog.LogMessage("SetSlider color entered change color=" + toColor, sliderBackground);
        LeanTween.value(sliderBackground.gameObject, sliderBackground.color, toColor, tweenTime).setOnUpdate((toColor) =>
        {
            sliderBackground.color = toColor;

        }
        ); ;

    }

    void TweenSlider(Slider _slider, float from, float to, float tweenTime) {

        LeanTween.value(gameObject, from, to, tweenTime).setOnUpdate((to) =>
        {
              sliderBehaviour.SetCurrentValue(to);
           
        }
        );
    }
    void DisplayPanelHero() {
        DisplayPanel(0.2f);
      
    }
    private void DisplayPanel(float tweenTime) 
    {
        GameLog.LogMessage(this + " panel owner");
        if(!gameObject.activeSelf)
            this.gameObject.SetActive(true);
        
        SetPanelWindow(UIPanel, 0f, 1f, tweenTime);
        //ChargeLighter();
        //UIHeroPanel.SetActive(true);    
    }
    private void HidePanel(float tweenTime)
    {
        SetPanelWindow(UIPanel, 1f, 0f, tweenTime);
       // UIPanel.SetActive(false);
    }


    public void ShowLighterPanel(State _state) 
    {
        GameLog.LogMessage("ShowLighterPanel entered"+_state);
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);    
        audioSounds.PlayLighterPanelSound();
        if (_state == State.ControlledByTouch)
        {
            DisplayPanel(0.5f);
            SteeringPanel.Instance.HideSteeringPanel(0.2f);
            ChargeHero();
        }
        else if (_state == State.Follow && HideLighterPanel)
        {
            HidePanel(0.5f);
            UnSetLighterPanelColor();
        }
        else if (_state == State.Stay)
        {
            steeringController.ShowSteeringPanel(0.2f);
            SetLighterPanelStay();
        }
        else
            UnChargeHero(0.5f);

    }

    private void OnStateChanged(object sender, LighterController.OnStateChangedEventArgs e)
    {
        GameLog.LogMessage("OnStateChanged sender" + sender + " state=" + e.currentState+" UI_DISPLAY"+this.name);
        if (sender.ToString().Contains("Swietlik") && !this.name.Equals("UI_Unit_Display_Lighter"))
            return;
        ShowLighterPanel(e.currentState);
    }
    private void SetLighterPanelStay()
    {
        SetLighterPanelColor();
        TweenSlider(slider, 1, .5f, 0.5f);
        ActivatePortraitForSeconds(2);
    }


    private void OnDestroy()
    {
        if(health != null)
            UnSignUpToHealthChange();   
    }
}