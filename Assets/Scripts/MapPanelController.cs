using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MapPanelController : MonoBehaviour
{
   // public static MapPanelController Instance { get; private set; }
    public Transform mapToggleButton;
    public Transform mapBackground;
    public GameObject mapBackgroundImage;
    public Transform mapToggleButtonOpen;
    private MapObject map;
    public float mapSize;
    [SerializeField]
    GameObject windRose, mapComponents;
    [SerializeField]
    GameObject sarsenbaiMountainGrowEffect, fireDesertGrowEffect, abcLandGrowEffect, EagleForestGrowEffect;
    [SerializeField]
    GameObject sarsenbaiMountain, fireDesert, abcLand, EagleForest;
    Transform currentLand;
    public float tweenTime;
    public bool windroseClicked;
    public Material wavingMaterial;
    public Transform pfComingSoon;
    RectTransform mapBackgroundRect;
    private bool showHighScore = false;
    public GameObject HightScorePanel;

    public Transform cameraTransform;
    public Transform followTransform;
    

    [SerializeField]
    AbecadlowoHightScore highScore;

    public Transform CheckPoints;
    public Transform MapCharacterPortrait;
    private bool checkPointsOpen = false;



    private void OnDisable()
    {
        if(CheckPoints)
         HideCheckPointsOnMap();
        
        //checkPointsOpen = false;

    }
    // private bool toggleClicked;

    // Start is called before the first frame update
    public void SetMap(MapObject mapObject)
    {
        if(mapObject != null)
        {
            this.map = mapObject;
            map.OnMapused += Map_OnMapUsed;
        }
      
    }
    void PlaySoundForPanel()
    {

        GetComponent<UnitAudioBehaviour>()?.PlayUIPanelBounce();
     
    }

    public void UnSetMap()
    {
        if(map)
            map.OnMapused -= Map_OnMapUsed;
    }
    private void Map_OnMapUsed(object sender, System.EventArgs e)
    {
        ShowMap();
    }
    public void Awake()
    {
       
        windroseClicked = false;
        if (!mapToggleButtonOpen)
            mapToggleButtonOpen = mapToggleButton.Find("mapaOpen");
    }
    public void Start()
    {
        transform.gameObject.SetActive(false);
    }
    public void SwitchLandOnMap(int landNumber)
    {
        GameObject growEffect;
        switch (landNumber)
        {
            case 1:
                currentLand = abcLand.transform;
                growEffect = abcLandGrowEffect;
                break;
            case 2:
                currentLand = fireDesert.transform;
                growEffect = fireDesertGrowEffect;
                break;
            case 3:
                currentLand = EagleForest.transform;
                growEffect = EagleForestGrowEffect;
                break;
            case 4:
                currentLand = sarsenbaiMountain.transform;
                growEffect = sarsenbaiMountainGrowEffect;
                break;
            default:
                currentLand = abcLand.transform;
                growEffect = abcLandGrowEffect;
                break;
        }
        SetMaterialOnLand(currentLand);
        SetGrowEffectOnLand(growEffect);
        SetCoverOnLand(currentLand);
    }
    public void ToogleMap()
    {
          SetMapSize(200,false);
    }


    public void SetMapSize(float to,bool close)
    {
        if(!mapBackground)
            mapBackground = transform.Find("MapBackground");
        if(!mapBackgroundRect)
            mapBackgroundRect = mapBackground.transform.GetComponent<RectTransform>();
        if (!close)
        {
            LeanTween.value(mapBackground.gameObject, mapBackgroundRect.sizeDelta.x, to, tweenTime).setEaseInBack().setOnUpdate(setMapSize).setOnComplete(MapResizeCompleted);
        }
        else
        {
            LeanTween.value(mapBackground.gameObject, mapBackgroundRect.sizeDelta.x, to, tweenTime).setEaseInBack().setOnUpdate(setMapSize).setOnComplete(FinishedClosing);
            
        }
    }
    private void FinishedClosing()
    {
        //showCheckpoints and arrow

    }


    private void ShowComponentsOnMap()
    {
        ShowComponents();
        EnableMapComponents();
        if (windroseClicked && !checkPointsOpen)
        {
            ShowCheckPointsOnMap();
           
        } else if (windroseClicked && checkPointsOpen)
        {
            HideCheckPointsOnMap();
        }
        windroseClicked = false;
    }

    private void ShowCheckPointsOnMap() {
        checkPointsOpen = true;
        CheckPoints?.gameObject.SetActive(true);
        MapCharacterPortrait.gameObject.SetActive(true);

    }

    private void HideCheckPointsOnMap()
    {
        checkPointsOpen = false;
        CheckPoints?.gameObject.SetActive(false);
        MapCharacterPortrait.gameObject.SetActive(false);

    }


    private void MapResizeCompleted()
    {
        
        mapBackgroundRect = mapBackground.transform.GetComponent<RectTransform>();
        LeanTween.value(mapBackground.gameObject, mapBackgroundRect.sizeDelta.x, mapSize, tweenTime).setEaseInBack().setOnUpdate(setMapSize).setOnComplete(ShowComponentsOnMap); 

        GameLog.LogMessage("Map Resie completed");
    }

    public void setMapSize(float value)
    {
        mapBackgroundRect.sizeDelta = new Vector2(value, mapBackgroundRect.sizeDelta.y);
        //mapBackgroundRect.ForceUpdateRectTransforms();
    }


    public void ReorganizeMap()
    {
        HideComponents();
        HightScorePanel.SetActive(false);
        ToogleMap();
  

    }

    public void ShowCheckpointsOnMap()
    {
        ShowComponentsOnMap();
       // HightScorePanel.SetActive(false);
       // ToogleMap();


    }
    public void ClickWindRoseOnMap()
    {
        if (windroseClicked)
            return;
        windroseClicked = true;
        LeanTween.rotateAround(windRose, Vector3.forward, -3600, 2f).setEase(LeanTweenType.easeOutElastic).setLoopOnce().setOnComplete(ShowCheckpointsOnMap);
        //HideMap

        // Score score = new Score();
    }

    public void ClickWindRose()
    {
        if (windroseClicked)
            return;
        windroseClicked = true;
        LeanTween.rotateAround(windRose, Vector3.forward, -3600, 2f).setEase(LeanTweenType.easeOutElastic).setLoopOnce().setOnComplete(ShowHighScoreExecute);
        //HideMap
        
        // Score score = new Score();
    }
    public void LevelComplete() 
    {
        GetComponent<Animator>().SetInteger("mapState", 1);
        EnableMapComponents();
        LeanTween.scale(windRose, new Vector3(1f, 1f, 1f), 0.3f).setDelay(1.5f).setEaseInOutElastic();
        LeanTween.scale(mapComponents, new Vector3(1f, 1f, 1f), 0.3f);
    }
    
    private void SetGrowEffectOnLand(GameObject growEffect)
    {
        DisableGrowEffect();
        growEffect.SetActive(true);
    }

    void SetMaterialOnLand(Transform land)
    {
        DisableWaving();
        Image landLabel = land.Find("LandLabel").GetComponent<Image>();
        landLabel.material = wavingMaterial;
    }
    void DisableGrowEffect()
    {
        abcLandGrowEffect.SetActive(false);
        fireDesertGrowEffect.SetActive(false);
        EagleForestGrowEffect.SetActive(false);
        sarsenbaiMountainGrowEffect.SetActive(false);
    }
    void DisableWaving()
    {
        abcLand.transform.Find("LandLabel").GetComponent<Image>().material = null;
        fireDesert.transform.Find("LandLabel").GetComponent<Image>().material = null;
        EagleForest.transform.Find("LandLabel").GetComponent<Image>().material = null;
        sarsenbaiMountain.transform.Find("LandLabel").GetComponent<Image>().material = null;
    }
    void EnableAllCovers()
    {
        sarsenbaiMountain.transform.Find("Cover").gameObject.SetActive(true);
        fireDesert.transform.Find("Cover").gameObject.SetActive(true);
        abcLand.transform.Find("Cover").gameObject.SetActive(true);
        EagleForest.transform.Find("Cover").gameObject.SetActive(true);
    }
    private void SetCoverOnLand(Transform _currentLand)
    {
        EnableAllCovers();
        DisableUnderConstructions();
        if (!_currentLand.Equals(abcLand.transform))
        {
            _currentLand.Find("Coming_soon").gameObject.SetActive(true);
        }
        _currentLand.Find("Cover").gameObject.SetActive(false);
    }
    private void DisableUnderConstructions()
    {
        fireDesert.transform.Find("Coming_soon").gameObject.SetActive(false);
        EagleForest.transform.Find("Coming_soon").gameObject.SetActive(false);
        sarsenbaiMountain.transform.Find("Coming_soon").gameObject.SetActive(false);
    }
    private void SetupClearMap() 
    {
        if (!mapBackgroundRect)
            mapBackgroundRect = mapBackground.transform.GetComponent<RectTransform>();
        mapBackgroundRect.sizeDelta = new Vector2(10, mapBackgroundRect.sizeDelta.y);
        HightScorePanel.SetActive(false);
        DisableMapComponenets();
        HideComponents();
        showHighScore = false;
    }
    
    public void ShowMap() {

        SetupClearMap();
        //mapToggleButton.gameObject.SetActive(tr);
        mapToggleButtonOpen.gameObject.SetActive(true);
        
        transform.gameObject.SetActive(true);
        
        SetMapSize(mapSize,false);
       
        //GetComponent<Animator>().SetBool("showMap",true);
    }
    public void ToggleMap()
    {
        //if (toggleClicked)
      //      return;
       // toggleClicked = true;
        LeanTween.cancelAll();

        GameHandler.Instance.DisableArrow();
        PlaySoundForPanel();
       
        if (!transform.gameObject.activeSelf) 
        {
            ShowMap();
        }
        else
        {
            HideMap();       
        }
    }


    public void HideComponents() {

        ChangeComponents(0f);

    }


    private void ShowHighScoreExecute()
    {
        GameLog.LogMessage("Show HighScores enteres" + showHighScore);
        if (showHighScore) 
        {
            GameLog.LogMessage("Show HighScores");
            DisableMapComponenets();
            mapComponents.SetActive(true);
            ShowComponents();
            HightScorePanel.SetActive(true);
        }
        else
        {
            ShowComponents();
            EnableMapComponents();
            HightScorePanel.SetActive(false);

        }
            
        showHighScore = !showHighScore;
        windroseClicked = false;
        GameLog.LogMessage("Show HighScores left");

    }
    private void DisableMapComponenets()
    {
        GameLog.LogMessage("DisableMapComponenets entered");

        // mapBackground.gameObject.SetActive(false);
        //  
        // mapComponents.SetActive(true);
        sarsenbaiMountain.SetActive(false);
       fireDesert.SetActive(false);
       abcLand.SetActive(false);
       EagleForest.SetActive(false);
       mapBackgroundImage.SetActive(false);
        GameLog.LogMessage("DisableMapComponenets left");
    }
    private void EnableMapComponents()
    {
        GameLog.LogMessage("EnableMapComponents entered");
        // mapBackground.gameObject.SetActive(true);
        // mapComponents.SetActive(true);
        sarsenbaiMountain.SetActive(true);
        fireDesert.SetActive(true);
        abcLand.SetActive(true);
        EagleForest.SetActive(true);
        mapBackgroundImage.SetActive(true);
        // ShowComponents();
        GameLog.LogMessage("EnableMapComponents left");
    }
    private void ChangeComponents(float value) {

        LeanTween.scale(mapComponents, new Vector3(value, value, value), 0.3f);
       // LeanTween.scale(windRose, new Vector3(value, value, value), 0.1f).setEaseInOutElastic();
    }




    public void ShowComponents() {
        GameLog.LogMessage("ShowComponents entered");
        //LeanTween.scale(windRose, new Vector3(1f, 1f, 1f), 0.3f).setDelay(1.5f).setEaseInOutElastic().setOnComplete(ShowHighScoreExecute);
        LeanTween.scale(mapComponents, new Vector3(1f, 1f, 1f), 0.3f);
        
    }
    public void FreezeWhenMapOpened() {
        GameLog.LogMessage("FreezeWhenMapOpened entered");
        // Time.timeScale = 0;

        mapToggleButton.gameObject.SetActive(true);

       // mapToggleButton.gameObject.SetActive(false);
       
    }

    public void HideMap()
    {
        GameLog.LogMessage("HideMap entered");
        HideComponents();
        DisableMapComponenets();
        HightScorePanel.SetActive(false);
        showHighScore = false;
       // mapToggleButtonOpen.gameObject.SetActive(false);
        transform.gameObject.SetActive(false);
        SetMapSize(10,true);
        //GetComponent<Animator>().SetBool("showMap",false);
        GameLog.LogMessage("HideMap left");
    }
    void OnDestroy()
    {
        GameLog.LogMessage("OnDestroy NaPPanelController entered");
        if (map)
            map.OnMapused -= Map_OnMapUsed;
    }

   /* private void FixedUpdate()
    {
        if (!followTransform)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                followTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        // GameLog.LogMessage("Player: " + followTransform.ToString() + " camera:" + cameraTransform.ToString());
        if (cameraTransform && followTransform)
            cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, cameraTransform.position.z);
    }

    private void LateUpdate()
    {
        if (!followTransform)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                followTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (cameraTransform && followTransform)
            cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, cameraTransform.position.z);
    }*/

}
