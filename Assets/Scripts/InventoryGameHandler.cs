using System;
using UnityEngine;
using UnityEngine.UI;
using Webaby.Utils;

public class InventoryGameHandler : MonoBehaviour
{
    [Header("Wallet settings")]
    [SerializeField] private WalletDisplay walletUI;
    private WalletController wallet;
    [Header("Player settings")]
    [SerializeField] private GameObject playerTransform;
    private PlayerPlatformerController playerController;
  
    [Header("Inventory settings")]
    InventoryDatabase id;

    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    [SerializeField] public string squirelInventoryObjectName;
    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    [SerializeField] public string grutekInventoryObjectName;
    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    [SerializeField] public string frogInventoryObjectName;


    [Header("Inventory Sprite Images")]
    [SerializeField] private Image HeroSpriteImage;
    [SerializeField] private Image SquirelSpriteImage;
    [SerializeField] private Image FrogSpriteImage;
    [SerializeField] private Image GrutekSpriteImage;
    private Transform activeInventoryTransform;
    private Transform activeInventorySpriteTransform;
    private Image activeInventorySprite;
    [Header("Character Equipment Settings")]
    [SerializeField] private UI_CharacterEquipment uiCharacterEquipment;
    [SerializeField] private CharacterEquipment characterEquipment;
    [Header("Timeline JumpintoGame")]
    public CutsceneMode jumpIntoCutscene;
    public CutsceneTimelineBehaviour jumpIntoCutsceneBehaviour;
    private int layerIndex;
    private Animator playerAnim;
    public void StartGameLogic()
    {
        if (!jumpIntoCutsceneBehaviour.gameObject.activeSelf)
            jumpIntoCutsceneBehaviour.gameObject.SetActive(true);
        switch (jumpIntoCutscene)
        {
            case CutsceneMode.Play:
                StartJumpCutscene();
                break;
            case CutsceneMode.None:
                break;
        }
    }
    private void StartJumpCutscene()
    {
        SetJumpLayer(1);
        jumpIntoCutsceneBehaviour.StartTimeline();
       
    }
    public void SetJumpLayer(int weight)
    {
        GameLog.LogMessage("Set jump layer:"+weight);
        layerIndex = playerAnim.GetLayerIndex("JumpIntoMirror");
        playerAnim.SetLayerWeight(layerIndex, weight);
    }
        
    private void Awake()
    {
        wallet = playerTransform.GetComponent<WalletController>();
        playerController = playerTransform.GetComponent<PlayerPlatformerController>();
        playerAnim = playerTransform.GetComponent<Animator>();
        id = Resources.Load<InventoryDatabase>("InventoryDatabase");

        playerController.GetComponent<Button_Sprite>().ClickFunc = () =>
            {
                GameLog.LogMessage("Clicked Sprite Button !!!!");
                    playerController.AttackEnemy(); 
                // windowCharacterPortrait.Show();
            };

    }
    // Start is called before the first frame update
    void Start()
    {
        if (walletUI)
            walletUI.SetWallet(wallet);
       
    }
    public void SwitchInventory(Transform inventoryButton)
    {
        activeInventorySpriteTransform = inventoryButton.Find("SlotImage/Sprite");
        GameLog.LogMessage(activeInventorySpriteTransform.ToString());
        activeInventorySprite = activeInventorySpriteTransform.GetComponent<Image>();
        activeInventoryTransform = inventoryButton.Find("Inventory");
        deactivateAllInventoriesObjects();
        activeInventoryTransform.gameObject.SetActive(true);
        activeInventorySprite.color = new Color(activeInventorySprite.color.r, activeInventorySprite.color.g, activeInventorySprite.color.b, 1f);
    }
    private void deactivateAllInventoriesObjects()
    {
       
        HeroSpriteImage.color = new Color(HeroSpriteImage.color.r, HeroSpriteImage.color.g, HeroSpriteImage.color.b, 0.2f);
        Image faceCoverImage = HeroSpriteImage.transform.Find("HeroSprite (1)").GetComponent<Image>();
        faceCoverImage.color = new Color(faceCoverImage.color.r, faceCoverImage.color.g, faceCoverImage.color.b, 0.2f);
        FrogSpriteImage.color = new Color(FrogSpriteImage.color.r, FrogSpriteImage.color.g, FrogSpriteImage.color.b, 0.2f);
        GrutekSpriteImage.color = new Color(GrutekSpriteImage.color.r, GrutekSpriteImage.color.g, GrutekSpriteImage.color.b, 0.2f);
        SquirelSpriteImage.color = new Color(SquirelSpriteImage.color.r, SquirelSpriteImage.color.g, SquirelSpriteImage.color.b, 0.2f);
    }
    private void OnDisable()
    {
        if (walletUI)
            walletUI.UnSetWallet(wallet);
    }
}
