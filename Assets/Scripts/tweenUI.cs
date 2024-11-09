using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class tweenUI : MonoBehaviour
{
    [SerializeField]
    AbecadlowoHightScore highScore;
    [SerializeField]
    GameObject backPanel, star1, star2, star3, score, coins, colorWheel, mapPanel, replayButton, quitBtn;
    public Material wavingMaterial;
    public Transform pfComingSoon;
    [SerializeField]
   
  
    TMP_Text amountText;
    public WalletObject wallet;
    public float tweenTime;
    [SerializeField]
    TMP_Text timeText;
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    TMP_Text wordsText;
   // private bool showHighScore = true;
    private AbecadlowoScore currentScore;
    private bool successPanelShown = false;
   // private bool windroseClicked;
    RectTransform mapBackgroundRect;
    public float MapSize;
    [SerializeField]
    private ClockUI clock;

    public MapPanelController mapController;

    public static tweenUI Instance { get; private set; }

    private void Awake()
    {
        PlayerPlatformerController.OnGameOver += OnGameOver;
    }
    private void OnGUI()
    {
        if (GameHandler.Instance && !GameHandler.Instance.GameLogEnabled)
            return;

        if (GUI.Button(new Rect(750, (Screen.height) - 25, 150, 20), "SHOW PANEL"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");
            ShowSuccesPanel();
        }
        if (GUI.Button(new Rect(750, (Screen.height) - 50, 150, 20), "Shovel"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            Item clockKey = new Item(9, "Shovel", ItemObjectType.Equipment);

            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(clockKey);
            //  print("You clicked the button!");
            //  CloseSuccesPanel(false);
        }
        if (GUI.Button(new Rect(50, (Screen.height) - 25, 150, 20), "CLOCKKEY"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

            Item clockKey = new Item(24, "ClockKey", ItemObjectType.Equipment);

            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(clockKey);


        }
        if (GUI.Button(new Rect(50, (Screen.height) - 45, 150, 20), "KEY"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

            Item key = new Item(14, "Key", ItemObjectType.Default);

            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(key);


        }
        if (GUI.Button(new Rect(200, (Screen.height) - 25, 30, 20), "A"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

            Item letterA = new Item(0, "A", ItemObjectType.Letter);
           
            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterA);
          

        }
        if (GUI.Button(new Rect(230, (Screen.height) - 25, 30, 20), "O"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

           
            Item letterO = new Item(17, "O", ItemObjectType.Letter);

            //  PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterA);
    
            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterO);

        }

        if (GUI.Button(new Rect(400, (Screen.height) - 25, 150, 20), "Blue Key"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

            Item blueKey = new Item(29, "BlueKey", ItemObjectType.Equipment);
            blueKey.damaged = true;
            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(blueKey);
        }
        if (GUI.Button(new Rect(900, (Screen.height) - 25, 150, 20), "Brown Key"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

            Item brownKey = new Item(30, "BrownKey", ItemObjectType.Equipment);
            brownKey.damaged = true;
            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(brownKey);
        }

        if (GUI.Button(new Rect(260, (Screen.height) - 25, 30, 20), "I"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

           
            Item letterI = new Item(10, "I", ItemObjectType.Letter);
          
            //  PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterA);
            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterI);
   

        }
        if (GUI.Button(new Rect(290, (Screen.height) - 25, 30, 20), "U"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

           
            Item letterU = new Item(22, "U", ItemObjectType.Letter);
       
            //  PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterA);
            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterU);
    

        }
        if (GUI.Button(new Rect(320, (Screen.height) - 25, 30, 20), "E"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");


            Item letterE = new Item(5, "E", ItemObjectType.Letter);
     
            PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterE);
    

        }
        if (GUI.Button(new Rect(350, (Screen.height) - 25, 30, 20), "”"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            //  print("You clicked the button!");

           // Item letterU = new Item(22, "letter”", ItemObjectType.Letter);
         
            //  PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition(letterA);
        
           // PickupItemAssets.Instance._spawner.SpawnItemAtPlayerPosition();

        }
    }


    
    private void OnDisable()
    {
        PlayerPlatformerController.OnGameOver -= OnGameOver;
    }
    public void OnGameOver(object sender, EventArgs args)
    {
        ShowSuccesPanel();
    }
   
    void CountScoreValue() {
        int hoursAmount = Int32.Parse(currentScore.Time_Amount.Split(":".ToCharArray())[0]);
        int minutesAmount = Int32.Parse(currentScore.Time_Amount.Split(":".ToCharArray())[1]);
        int timeScore = 1440 - hoursAmount * 60 + minutesAmount;
        GameLog.LogMessage("hours:" + hoursAmount + " minutesAmount:" + minutesAmount + " TimeScore:" + timeScore);
        currentScore.value = currentScore.Words_Amount * 100 + currentScore.Coins_Amount * 10 + timeScore;
    }
    
    
   
    public void ShowSuccesPanel()
    {
        successPanelShown = true;
        backPanel.SetActive(true);
        mapPanel.SetActive(true);
        // LeanTween.cancelAll();
        
        DialogManager.Instance?.StopDialog();

        SteeringPanel.Instance?.HideSteeringPanel();
        //GameHandler.Instance.steeringPanel.SetActive(false);    
        LeanTween.scale(colorWheel, new Vector3(1f, 1f, 1f), 0.3f);
        LeanTween.rotateAround(colorWheel, Vector3.forward, -360, 10f).setLoopClamp();
        LeanTween.scale(backPanel, new Vector3(0.5f, 0.5f, 0.5f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
        LeanTween.moveLocal(backPanel, new Vector3(-30f, 300f, 2f), .7f).setDelay(.3f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(PlaySoundForPanel);
        LeanTween.scale(backPanel, new Vector3(1f, 1f, 1f), 2f).setDelay(.4f).setEase(LeanTweenType.easeInOutCubic);
        
    }

    void PlaySoundForPanel() {

        GetComponent<UnitAudioBehaviour>().PlayUIPanelBounce();
     
    }
    
    
    void LevelComplete()
    {
        mapController.ShowMap();
        

        LeanTween.moveLocal(backPanel, new Vector3(0f, 0f, 0f), 0.7f).setEaseOutBounce().setOnComplete(StarsAnim);
        LeanTween.rotateAround(replayButton, Vector3.forward, -360, 10f).setLoopClamp();
        LeanTween.scale(replayButton, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.9f).setEase(LeanTweenType.easeOutElastic).setLoopPingPong();
        LeanTween.alpha(coins.GetComponent<RectTransform>(), 1f, .5f).setDelay(1.1f);
        
    }
    void StarsAnim()
    {
        LeanTween.scale(mapPanel, new Vector3(1f, 1f, 1f), 0.3f).setDelay(.1f);
        LeanTween.scale(star1, new Vector3(1.5f, 1.5f, 1.5f), 2f).setEase(LeanTweenType.easeOutElastic).setLoopPingPong();
        LeanTween.scale(star3, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setLoopPingPong();
        LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic).setOnComplete(SetCoins);
    }
    /* public void YesButton()
     {
         LeanTween.scale(gameText, new Vector3(0f, 0f, 0f), .5f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.scale(yesBtn, new Vector3(0f, 0f, 0f), .5f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.scale(noBtn, new Vector3(0f, 0f, 0f), .5f).setDelay(.1f).setEase(LeanTweenType.easeOutCirc);
         //LeanTween.moveLocal(yesOrNoPanel, new Vector3(0f, -615f, 0f), 0.5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
         LeanTween.scale(yesOrNoPanel, new Vector3(0f, 0f, 0f), .5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart)
         .setOnComplete(LoadMainMenu);
     }
     public void NoButton()
     {
         LeanTween.scale(yesBtn, new Vector3(0f, 0f, 0f), .5f).setDelay(.1f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.scale(noBtn, new Vector3(0f, 0f, 0f), .5f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.moveLocal(yesOrNoPanel, new Vector3(0f, -615f, 0f), 0.5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
         LeanTween.scale(yesOrNoPanel, new Vector3(0f, 0f, 0f), .5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
         LeanTween.scale(quitBtn, new Vector3(1f, 1f, 1f), .5f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc);
     }
     void PanelEnable()
     {
         LeanTween.scale(quitBtn, new Vector3(0f, 0f, 0f), .5f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.moveLocal(yesOrNoPanel, new Vector3(0f, 0f, 0f), 0.5f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.scale(yesOrNoPanel, new Vector3(2f, 2f, 1f), .5f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.scale(yesBtn, new Vector3(1.5f, 1.5f, 1.5f), .5f).setDelay(.3f).setEase(LeanTweenType.easeOutCirc);
         LeanTween.scale(noBtn, new Vector3(1.5f, 1.5f, 1.5f), .5f).setDelay(.4f).setEase(LeanTweenType.easeOutCirc);
     }*/
    void SetCoins() {
        currentScore = new AbecadlowoScore();
        currentScore.Coins_Amount = wallet.GetAmount();
        amountText.transform.localScale = Vector3.one;
        LeanTween.scale(amountText.gameObject, Vector3.one * 2, tweenTime).setEasePunch();
        LeanTween.value(amountText.gameObject, wallet.GetAmount(), wallet.GetAmount(), tweenTime).setEasePunch().setOnUpdate(setCoinAmount).setOnComplete(SetTime);
    }
    void SetTime()
    {
        currentScore.Time_Amount = ClockUI.GetTime();
        timeText.transform.localScale = Vector3.one;
        setTimeText(currentScore.Time_Amount);
        LeanTween.scale(timeText.gameObject, Vector3.one * 2, tweenTime).setEasePunch().setOnComplete(SetWords);
    }
    void SetWords() 
    {
        currentScore.Words_Amount = PickupItemAssets.Instance.namesDatabase.GetAllDoneNames().Count();
        LeanTween.scale(wordsText.gameObject, Vector3.one * 2, tweenTime).setEasePunch();
        LeanTween.value(wordsText.gameObject, currentScore.Words_Amount, currentScore.Words_Amount, tweenTime).setEaseLinear().setOnUpdate(SetWordsAmount).setOnComplete(SetScore);
    }
    void SetScore() {
        //policz wynik
        CountScoreValue();
        highScore.AddHighScore(currentScore);
        LeanTween.scale(scoreText.gameObject, Vector3.one * 2, tweenTime).setEasePunch();
        LeanTween.value(scoreText.gameObject, 0, currentScore.value, tweenTime).setEaseLinear().setOnUpdate(SetScoreValue).setDelay(1).setOnComplete(CLickRoseOnMap); ;
    }


    public void CLickRoseOnMap() {

       // clock.StopTime();
        mapController.ClickWindRose();
        
    }
    void SetScoreValue(float value) {
        scoreText.text = value.ToString();  
    }
    public void setCoinAmount(float value)
    {
        amountText.text = value.ToString();
    }
    public void SetWordsAmount(float value)
    {
        wordsText.text = value.ToString();
    }
    public void setTimeText(string value)
    {
        timeText.text = value;
    }

   
    
    
    public void SetMapSize(float to)
    {

        var mapBackground = mapPanel.transform.Find("mapBackground");
        mapBackgroundRect = mapBackground.transform.GetComponent<RectTransform>();



        LeanTween.value(mapBackground.gameObject, mapBackgroundRect.sizeDelta.x, to, tweenTime).setEaseLinear().setOnUpdate(setMapSize).setOnComplete(MapResizeCompleted);

        


    }

    private void MapResizeCompleted()
    {
        GameLog.LogMessage("Map Resie completed");
    }

    public void setMapSize(float value)
    {
        mapBackgroundRect.sizeDelta = new Vector2(value, mapBackgroundRect.sizeDelta.y);
    }


    
   
    

    public void TweenSuccessPanel()
    {
        if (!successPanelShown)
        {
            ShowSuccesPanel();
        }
        else
        {
            CloseSuccesPanel(false);
        }
    }

    public void CloseSuccesPanel(bool quitGame)
    {
        successPanelShown = false;
        LeanTween.cancelAll();
        // mapPanel.GetComponent<Animator>().SetInteger("mapState",1);
        if (quitGame)
            LeanTween.moveLocal(backPanel, new Vector3(-30f, 1000f, 2f), .5f).setDelay(0.3f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(QuitGame);
        else
            LeanTween.moveLocal(backPanel, new Vector3(-30f, 1000f, 2f), .5f).setDelay(0.3f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(star1, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(star2, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(star3, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(colorWheel, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(mapPanel, new Vector3(0f, 0f, 0f), 0.3f).setDelay(.1f);
        LeanTween.scale(replayButton, new Vector3(0f, 0f, 0f), 0.3f).setDelay(.1f);
        LeanTween.scale(colorWheel, new Vector3(0f, 0f, 0f), 0.3f).setDelay(.2f);
        LeanTween.scale(backPanel, new Vector3(0f, 0f, 0f), 0.3f).setDelay(.3f);
        SteeringPanel.Instance.ShowSteeringPanel(.5f);
        backPanel.SetActive(false);
        clock.StartTime();

    }
    public void LoadGame()
    {
        clock.StartTime();
        GameHandler.Instance.GameOverClearing();
        SceneManager.LoadScene("NowaPrzygoda");
    }

    public void LoadShopScene()
    {
        SceneManager.LoadScene("NowaPrzygoda.InventoryRoom");
    }
    public void QuitGame()
    {
       // Application.Quit();
        Debug.Log("Game Quit");
        GameHandler.Instance.GameOverClearing();
        SceneManager.LoadScene("NowaPrzygoda.GameOver");
    }
}
