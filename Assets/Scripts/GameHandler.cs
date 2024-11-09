using Webaby.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Inspector;
using System.Linq;
using System;
using UnityEngine.UI;

public enum CutsceneMode
{
    Play,
    Down,
    None
}
[ExecuteInEditMode]
public class GameHandler : MonoBehaviour
{

    [Header("Lighter Settings")]
    [SerializeField]
    private Transform lighterTransform;
    [SerializeField] private FurnaceController furnace;
    [SerializeField] private GameObject furnaceUIPanel;
    public bool GameLogEnabled;
    public DialogLanguage language;

    [Header("Intro Frame 1")]
    public CutsceneMode introFrame1Cutscene;
    public CutsceneTimelineBehaviour introFrame1CutsceneBehaviour;
    [Header("Intro Frame 2")]
    public CutsceneMode introFrame2Cutscene;
    public CutsceneTimelineBehaviour introFrame2CutsceneBehaviour;
    [Header("Intro Frame 3")]
    public CutsceneMode introFrame3Cutscene;
    public CutsceneTimelineBehaviour introFrame3CutsceneBehaviour;
    [Header("Zwyciestwo -  Victory")]
    public CutsceneTimelineBehaviour victoryCutsceneBehaviour;
    public SceneField victoryNextScene;
    [Header("Przegrana - Defeat")]
    public CutsceneTimelineBehaviour defeatCutsceneBehaviour;
    public SceneField defeatNextScene;
    //  private SceneField selectedNextScene;
    [Header("Player Settings")]
    [SerializeField] private GameObject playerTransform;
    [SerializeField] private WindowCharacter_Portrait windowCharacterPortrait;
    [SerializeField] private InventoryDisplay uiInventory;
    [SerializeField] private WalletDisplay walletUI;
    [SerializeField] private UI_CharacterEquipment uiCharacterEquipment;
    [SerializeField] private CharacterEquipment characterEquipment;
    [SerializeField] private DisplayName displayName;
    private WalletController wallet;
    private PlayerPlatformerController playerController;
    private Player player;
    private Rigidbody2D rb;
    private Animator heroAnim;
    [Header("Map")]
    [SerializeField] private MapPanelController mapPanel;

    //
    [StringInList(typeof(PropertyDrawerHelper), "AllItems")]
    [SerializeField] private string mapObjectName;
    private MapObject map;
    [Header("Sterowanie")]
    public GameObject steeringPanelGO;
    [Header("Friends Inventory objects")]
    [SerializeField] private List<GameObject> frames;
    // private static bool steeringPanelHiden = true;
    [Header("Name Settings")]
    public static NamesDatabase nameDatabase;
    public static DialogDatabase dialogDatabase;
    [Header("Inventory settings")]
    InventoryDatabase id;
    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    public string InventoryName;
    private InventoryObject inventory;

    // [SerializeField] private SlidersUI sliders;
    public Transform vcam;
    public Transform buttonTransform;
    //public static event EventHandler ShowArrow;
    public GameObject arrow;
    private Image arrowImage;
    private Action arrowTriggerAction;
    private Sprite genericArrowSprite;
    PickupItemAssets assetInstanceInGameHandler;
    
    public bool setup;
    public string SetupFrame;
    [SerializeField]
    private Health heroHealth;
    [SerializeField]
    private UIHeartController heroHealthPanel;
    private bool onDrawningSubscribed;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string dialogName = "IntroScamp";


    private CheckpointBehaviour GetActiveCheckPoint()
    {

        return CheckpointBehaviour.GetActiveCheckPoint();

    }
    public void GameOver() { 
    
    
        GameOverClearing();
    
    }




    private void RespawnPlayerToCheckPoint()
    {


        //activate Frame Object 
        SetupFrame = GetActiveCheckPoint().parentFrame.name;
        Setup();
        SetupGame();
        playerTransform.transform.position = CheckpointBehaviour.GetActiveCheckPointPosition();
        player.GetComponent<PlayerPlatformerController>().PlayTeleportEffect();


    }

    private void SetFurnace()
    {


        furnaceUIPanel.SetActive(true);
        furnace.FurnaceSlotSetUp();
        furnaceUIPanel.SetActive(false);
    }
    public static GameHandler Instance { get; private set; }

    // Start is called before the first frame update

    private void OnFurnaceInteract(object sender, System.EventArgs e)
    {
       // ShowInventory();
        // ToggleNameBox();
        //   ToggleInventory();
    }

    private void OnFurnaceNameEntered(object sender, System.EventArgs e)
    {
        // ToggleNameBox();
        GameLog.LogMessage("Show Name display entered");
        if (!displayName)
            displayName.gameObject.SetActive(true);
        DisplayName.Instance.ShowNameBox();
        ShowInventory();
    }

    private void OnFurnaceNameExit(object sender, System.EventArgs e)
    {
        DisplayName.Instance?.HideNameBox();
        HideInventoryImmediate();
    }

    private void OnPlayerDrowning(object sender, System.EventArgs e)
    {
        GameLog.LogMessage("OnPlayerDrowning Routine entered sender:" + sender);
        if (!River.drawning)
            StartCoroutine(Drown(player.transform));
    }

    private void ToggleNameBox()
    {
        if (displayName.isOpen)
            displayName?.HideNameBox();
        else
            displayName?.ShowNameBox();
    }
    public void ToggleInventory()
    {

        // WindowCharacter_Portrait.Instance?.ToggleInventory();
        windowCharacterPortrait.ToggleInventory();
    }
    public void ShowInventory()
    {


        windowCharacterPortrait.Show();




    }


    public bool CheckInventoryOpen()
    {

        return (windowCharacterPortrait && windowCharacterPortrait.gameObject.activeSelf) ? true : false;
    }
    public void HideInventory()
    {
        if (windowCharacterPortrait && windowCharacterPortrait.gameObject.activeSelf)
            windowCharacterPortrait.Hide(5);

    }

    public void HideInventoryImmediate()
    {
        GameLog.LogMessage("HideInventory Immediate");

        
        windowCharacterPortrait?.Hide(0);

    }


    public void OnArrowClick()
    {

        arrowTriggerAction?.Invoke();

    }



    private void TweenArrow()
    {

        float tweenTime = 1f;
        LeanTween.cancel(arrow.gameObject);
        arrow.transform.localScale = Vector3.one;
        LeanTween.scale(arrow.gameObject, Vector3.one * 2, tweenTime).setEaseOutElastic().setLoopPingPong();


    }

    private void StopTweenArrow()
    {

        LeanTween.cancel(arrow.gameObject);

    }


    public void ShowArrow(Action triggerActionWhenClick, Sprite arrowSprite)
    {
        GameLog.LogMessage("ShowArrow entered");
        if (arrowImage & arrowSprite)
        {
            GameLog.LogMessage(arrowImage.sprite.name + " : " + arrowSprite.name);
            arrowImage.sprite = arrowSprite;
            // arrowImage.heu

        }
        else
        {
            GameLog.LogMessage("Brak spritea");
            arrowImage.sprite = genericArrowSprite;
        }

        arrowTriggerAction = triggerActionWhenClick;
        
        arrow.SetActive(true);
        //Lerp Arrow to show it nice
        TweenArrow();
        HideInventoryImmediate();
    }

    public void DisableArrow()
    {

        // arrowTriggerAction = triggerActionWhenClick;
        if (arrow && arrow.active)
        {
            StopTweenArrow();
            arrow.SetActive(false);
        }
        //Lerp Arrow to show it nice
    }


    void Start()
    {
        StartGameLogic();
    }

    private void Setup()
    {
        if (SetupFrame == null || SetupFrame.Equals(""))
            SetupFrame = "Frame1";

        // var foundCanvasObjects = SetupFrame.Find("Cinemachine");//FindObjectOfType<CinemachineVirtualCamera>();
        //  CameraSwitcher.SwitchCamera(foundCanvasObjects);
        GameLog.LogMessage("Setup frames");
        foreach (GameObject frame in frames)
        {

            //deactivate
            if (frame.name != SetupFrame)
            {

                frame.SetActive(false);

            }
            else
            {

                frame.SetActive(true);
                if (playerTransform)
                    playerTransform.transform.position = frame.transform.position;
            }

        }
    }
    //activate SetupFrame frame


    private void ShowAllFramesSetup()
    {
        GameLog.LogMessage("Setup frames");
        foreach (GameObject frame in frames)
        {

            frame.SetActive(true);

        }


    }


    private void OnEnable()
    {

      //  GameLog.LogMessage("Enable executed in GameHandler", transform);


        FurnaceController.OnItemInteracted += OnFurnaceInteract;
        FurnaceController.OnNameEntered += OnFurnaceNameEntered;
        FurnaceController.OnNameExit += OnFurnaceNameExit;
        RegisterOnDrawning(true);

    }



    void RegisterOnDrawning(bool enabled)
    {

        if (!onDrawningSubscribed)
            River.OnPlayerDrowning += OnPlayerDrowning;
        onDrawningSubscribed = enabled;

    }


    void DeRegisterOnDrawning(bool enabled)
    {

        if (onDrawningSubscribed)
            River.OnPlayerDrowning -= OnPlayerDrowning;
        onDrawningSubscribed = enabled;

    }

    private void Awake()
    {
        // SetFurnace();

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
        LoadEnum();
        if (!windowCharacterPortrait)
            windowCharacterPortrait = GameObject.FindObjectOfType<WindowCharacter_Portrait>(true);
        //windowCharacterPortrait = WindowCharacter_Portrait.Instance;

        steeringPanelGO = GameObject.FindObjectOfType<SteeringPanel>(true).gameObject;
        PlayerPlatformerController.OnGameOver += OnGameOver;
        id = Resources.Load<InventoryDatabase>("InventoryDatabase");
        //nameDatabase = Resources.Load<NamesDatabase>("NamesDatabase");
        //  dialogDatabase = Resources.Load<DialogDatabase>("DialogDatabase");
        if (InventoryName != null && InventoryName != "None")
        {

            inventory = id.GetInventoryObjectFromName(InventoryName);
        }
        GameLog.LogMessage("Inventory for player" + inventory);
        PlayerPrefs.SetString("language", language.ToString());
        GameLog.LogMessage("language" + PlayerPrefs.GetString("language"));
        assetInstanceInGameHandler = FindObjectOfType<PickupItemAssets>();
        if (assetInstanceInGameHandler)
        {
            dialogDatabase = assetInstanceInGameHandler.dialogDatabase;
            nameDatabase = assetInstanceInGameHandler.namesDatabase;
        }
        //  if (nameDatabase)
        //     nameDatabase.loadDataToDict(language);

        RegisterOnDrawning(true);
        //River.OnPlayerDrowning += OnPlayerDrowning;
        /* FurnaceController.OnItemInteracted += OnFurnaceInteract;
         FurnaceController.OnNameEntered += OnFurnaceNameEntered;
         FurnaceController.OnNameExit += OnFurnaceNameExit;
         River.OnPlayerDrowning += OnPlayerDrowning;*/

        arrowImage = arrow.GetComponent<Image>();
        genericArrowSprite = arrowImage.sprite;
        //  DisplayName.OnNameChanged += OnNameChangeUpdate;
        InteractableBase.showArrowDelegate += ShowArrow;
        InteractableBase.disableArrowDelegate += DisableArrow;
        if (setup)
            Setup();

        Debug.unityLogger.logEnabled = Debug.isDebugBuild;
        //Logging eanbled or not 
        if (!GameLogEnabled)
            Debug.unityLogger.logEnabled = GameLogEnabled;


        // Debug.unityLogger.logEnabled = false;
        GameLog.LogMessage("LOGING ENABLED " + Debug.unityLogger.logEnabled);
        // steeringPanel.SetActive(false);
        wallet = playerTransform.GetComponent<WalletController>();
        player = playerTransform.GetComponent<Player>();
        playerController = playerTransform.GetComponent<PlayerPlatformerController>();
        rb = playerTransform.GetComponent<Rigidbody2D>();
        heroAnim = playerTransform.GetComponent<Animator>();
        //steeringPanel.GetComponent<Animator>().SetTrigger("Hide");
        if (characterEquipment)
        {
            // 
            uiCharacterEquipment?.SetCharacterEquipment(characterEquipment);
            /*if (windowCharacterPortrait)
                windowCharacterPortrait.Show();*/
            player?.SetCharacterEquipment(characterEquipment);
        }
        else
        {
            characterEquipment = playerTransform.GetComponent<CharacterEquipment>();
            uiCharacterEquipment?.SetCharacterEquipment(characterEquipment);
           // windowCharacterPortrait.Show();
            player?.SetCharacterEquipment(characterEquipment);
        }
        if (heroHealth)
        {
            heroHealthPanel?.SetHealth(heroHealth);


        }

    }

    private void OnGameOver(object sender, EventArgs e)
    {
        GameOverClearing();
    }

    IEnumerator Drown(Transform _player)
    {
        River.drawning = true;
        if (playerController.targetCamera)
            CameraSwitcher.SwitchCamera(playerController.targetCamera);
        GameLog.LogMessage("Drawn routine entered");

        int corpseSortingOrder = 9;
        int leftLegSortingOrder = 3;
        int rightLegSortingOrder = 4;
        string sortingLayerName = "Default";
        string currentSortingLayerName = "Player";
        GameLog.LogMessage("Drawn routine started");
        SteeringPanel.Instance.TweenSteeringPanel();
        playerController.PlayDrowning();
        ObjectPoolerGeneric.Instance.SpawnFromPool("DrawningEffect", _player.transform.position, Quaternion.identity, null);
        playerController.DisableKinematic();
        // rb.gravityScale = 1f;
        int index = heroAnim.GetLayerIndex("HeroDrawning");
        int index2 = heroAnim.GetLayerIndex("isTalkingLayer");
        heroAnim.SetLayerWeight(index, 1);
        heroAnim.SetLayerWeight(index2, 1);
        player.GetComponent<Player>().PlayerDrawning(0, 0, 0, sortingLayerName);
        yield return new WaitForSeconds(3f);

        heroAnim.SetLayerWeight(index, 0);
        heroAnim.SetLayerWeight(index2, 0);
        GameLog.LogMessage("Play effect for water dumpl" + rb.isKinematic);
        playerController.EnableKinematic();
        player.PlayerDrawning(corpseSortingOrder, leftLegSortingOrder, rightLegSortingOrder, currentSortingLayerName);
        GameLog.LogMessage("Drawn routine stopped");
        SteeringPanel.Instance.TweenSteeringPanel();
        heroHealth.LostLife();
        if (!heroHealth.isAlive)
        {
            playerController.GameOver();
        }
        else
        {
            Setup();
            playerController.ReSpawnPlayer(_player);
        }


        River.drawning = false;
    }

    void StartGameLogic()
    {
        GameLog.LogMessage("Start GameLogic entered");
        // CheckpointBehaviour.PrintCheckPoints();
        windowCharacterPortrait.Hide(0);
        if (GetActiveCheckPoint() != null)
        {
            RespawnPlayerToCheckPoint();
            return;
        }

        if (introFrame1CutsceneBehaviour & !introFrame1CutsceneBehaviour.isActiveAndEnabled)
            introFrame1CutsceneBehaviour.gameObject.SetActive(true);
        if (introFrame3CutsceneBehaviour & !introFrame3CutsceneBehaviour.isActiveAndEnabled)
            introFrame3CutsceneBehaviour.gameObject.SetActive(true);

        switch (introFrame1Cutscene)
        {
            case CutsceneMode.Play:
                StartIntroFrame1Cutscene();
                introFrame1Cutscene =CutsceneMode.None;
                //  HideInventoryImmediate(); = 

                break;
            case CutsceneMode.None:
                // StartBattle();
                break;
        }
    }
    private void OnDestroy()
    {
        //introFrame1Cutscene = CutsceneMode.Play;
        player.UnSetCharacterEquipment();
        uiCharacterEquipment.UnSetCharacterEquipment();
        if (inventory)
        {

            uiInventory.DeactivateInventory(inventory);
        }


        heroHealthPanel?.UnSetHealth();
      
    }
    void PlayCutsceneFrame3()
    {
        switch (introFrame3Cutscene)
        {
            case CutsceneMode.Play:
                StartIntroFrame3Cutscene();
                break;
            case CutsceneMode.None:
                // StartBattle();
                break;
        }
    }
    void StartIntroFrame1Cutscene()
    {
        //  
        if (introFrame1CutsceneBehaviour)
            introFrame1CutsceneBehaviour.StartTimeline();
    }
    void StartIntroFrame3Cutscene()
    {
        GameLog.LogMessage("StartIntroFrame3Cutscene");
        introFrame3CutsceneBehaviour.StartTimeline();
    }
    public void StartBattle()
    {
        //SteeringPanel.Instance.ShowSteeringPanel(.5f);

    }

    public void StartingTimeLineFinished()
    {
        // introFrame1CutsceneBehaviour.StopAllCoroutines();
        // introFrame1CutsceneBehaviour.transform.gameObject.SetActive(false);
        GameLog.LogMessage("Starting timeline finished entered");
        //Wiecej nie pokazuj timeline z frame1 dbo juz odegrany 
        introFrame1Cutscene = CutsceneMode.None;
        SetupGame();
        //  HideInventoryImmediate();
        // playerController.TriggerDialogName(dialogName);
        if (heroAnim)
            DialogManager.Instance.SetPlayerAnimator(heroAnim);


        player?.GetComponent<UnitDialogBehaviour>()?.GetNextDialogOrShortifAlreadyPlayed()?.PlayDialog();

    }

    private void SetupGame()
    {
        if (!steeringPanelGO)
        {
            steeringPanelGO = GameObject.FindObjectOfType<SteeringPanel>(true).gameObject;
        }
        steeringPanelGO.SetActive(true);
        SteeringPanel.Instance.ShowSteeringPanel(0.5f);
        // ShowInventory();

        //windowCharacterPortrait.Show();
        if (buttonTransform)
        {
            GameLog.LogMessage("set click fun on HERO !!!!!!!!!!!!!!!!!!!");
            buttonTransform.GetComponent<Button_Sprite>().ClickFunc = () =>
            {
                GameLog.LogMessage("Clicked Sprite Button !!!!");
                // ShowInventory();

                playerController.DisplayInventoryAndStats();
                // windowCharacterPortrait.Show();
            };
        }
        if (playerTransform)
        {
            if (!buttonTransform)
            {
                Button_Sprite playerButton = playerTransform.GetComponent<Button_Sprite>();
                if (playerButton)
                    playerButton.ClickFunc = () =>
                    {
                        GameLog.LogMessage("Clicked Sprite Button !!!!");
                        ShowInventory();
                        // windowCharacterPortrait.Show();
                    };
            }
            //playerTransform.GetComponent<PlayerPlatformerController>();
            SetupInvenoty();

            // if (playerTransform)
            // uiInventory.SetInventory(playerTransform.GetComponent<PlayerPlatformerController>().GetInventory());
            walletUI.SetWallet(wallet);
            if (!map)
                map = (MapObject)PickupItemAssets.Instance.allItemsDatabase.GetItemObjectFromName("Map");
            mapPanel.SetMap(map);
            mapPanel.gameObject.SetActive(false);
        }
    }



    private void SetupInvenoty() {

        if (!inventory)
        {


            GameLog.LogMessage("Missing inventory !!!!!!!!!!!!!");

        }
        uiInventory.SetInventory(inventory, true);
       // EditorUtility.SetDirty(inventory);

    }

    private void CreateNewInventory()
    {
        playerController.GetInventory().Clear();
        
    }

    public void GameOverClearing()
    {
        ResetPlayingCutSceneFrame1();
        //EditorUtility.SetDirty(gameObject);

        GameLog.LogMessage("OnApplicationQuit entered:" + introFrame1Cutscene);
        if (steeringPanelGO)
        {
            steeringPanelGO.SetActive(true);
            SteeringPanel.Instance?.HideSteeringPanel();
        }
        CreateNewInventory();

        ClearNames();
        ClearDialogs();
      //  StartingTimeLineFinished();
        CleanItemsData();
        ClearWallet();
       // ShowAllFramesSetup();
        ResetHealth();
        ResetCheckPoints();
        ClearEquipment();
        CleanTasks();

        HideInventoryImmediate();

        //playerController.gameObject.SetActive(false);
        //bplayerController.gameObject.SetActive(true);
    }





    private void CleanTasks() {

     
            TasksDatabase tasksDatabase = PickupItemAssets.Instance.tasksDatabase;
            tasksDatabase.ResetTasks();

     

    }
    private void ClearEquipment() { 
        characterEquipment.ResetEquipment();    
    
    
    }

    private void ResetCheckPoints()
    {
        CheckpointBehaviour.ResetAllCheckPoints();
    }

    private void OnApplicationQuit()
    {
       
        GameOverClearing();




    }

    void LoadEnum()
    {
        string loadString = PlayerPrefs.GetString("introFrame1Cutscene");

        System.Enum.TryParse(loadString, out CutsceneMode loadState);
        introFrame1Cutscene = loadState;
    }

    public void ResetPlayingCutSceneFrame1()
    {
        this.introFrame1Cutscene = CutsceneMode.Play;
        string saveString = introFrame1Cutscene.ToString();

        PlayerPrefs.SetString("introFrame1Cutscene", saveString);
        PlayerPrefs.Save();
    }

    /*private void OnApplicationFocus(bool focus)
    {
       *//* if(!focus)
        {
            OnApplicationQuit();

        }*//*
    }*/

    private void ResetHealth()
    {

        playerController.GetPlayerHealth().resetHealth();

    }

    private void ClearDialogs()
    {
        if (dialogDatabase)
            dialogDatabase.ResetDialogs();
    }
    private void CleanItemsData()
    {
        ItemsToDamageDatabase database = PickupItemAssets.Instance.itemsToDamageDatabase;
        database.ResetItemsToDamage();
        heroHealth.resetHealth();
    }

    private void ClearWallet()
    {
        wallet.ClearWallet();
    }

    private void OnDisable()
    {
        walletUI.UnSetWallet(wallet);
        mapPanel.UnSetMap();
        FurnaceController.OnItemInteracted -= OnFurnaceInteract;
        FurnaceController.OnNameEntered -= OnFurnaceNameEntered;
        FurnaceController.OnNameExit -= OnFurnaceNameExit;
        DeRegisterOnDrawning(false);
        //River.OnPlayerDrowning -= OnPlayerDrowning;
        PlayerPlatformerController.OnGameOver -= OnGameOver;

        InteractableBase.showArrowDelegate -= ShowArrow;
        InteractableBase.disableArrowDelegate -= DisableArrow;
        FurnaceController.OnItemInteracted -= OnFurnaceInteract;
        River.OnPlayerDrowning -= OnPlayerDrowning;
        if (introFrame1CutsceneBehaviour)
            introFrame1CutsceneBehaviour.transform.gameObject.SetActive(false);
        if (introFrame3CutsceneBehaviour)
            introFrame3CutsceneBehaviour.transform.gameObject.SetActive(false);

#if UNITY_EDITOR
        foreach (GameObject frame in frames)
        {
            if(frame != null)
                frame.SetActive(true);


        }
#endif
    }
    private void ClearNames()
    {
        if (nameDatabase)
            ResetNames();
        else
        {
            nameDatabase = Resources.Load<NamesDatabase>("NamesDatabase");
            ResetNames();
        }

    }

    private void ResetNames()
    {
        foreach (NameObject name in nameDatabase.namesArray.ToList())
        {

            name.ResetNameObject();



        }

    }
}
