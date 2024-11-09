using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using Webaby.Utils;
public class InteractableBase : MonoBehaviour
{
    public bool showArrow = false;
    public static Action<Action, Sprite> showArrowDelegate;
    public static Action disableArrowDelegate;
    public Action interactAction;
    public PlayerPlatformerController player;
    public Transform lighterTransform;
    public Light2D interactableLight;
    protected Animator playerAnimator;
    public float tweenTime = 1f;
    public Button_Sprite button;
    public float lightIntensity = 1f;
    private SpriteRenderer itemSpriteRenderer;
    private Sprite itemSprite;
    protected bool isInRange;
    public bool changeArrowSprite = true;
    public bool clickableSprite = true;
    private GameObject playerTransform;
    public UnityEvent interactActionEvent;

    
    public bool checkEquipmentTool = false;
    public bool UnblockPortalWhenEquipmentToolUsed= false;
    public bool dontStartOnEnter = false;
    public bool showLightBeforeArrow = false;
    [StringInList(typeof(PropertyDrawerHelper), "AllItems")]
    public string interactableItemName;
    public Item interactableItem;
    public bool removeInteractableItem;

    public void SetupInteractableObject(string _itemName, string taskName)
    {
        GameHandler.Instance?.DisableArrow();
        GameLog.LogMessage("SetupInteractableObject entered");
        if (_itemName != null && !_itemName.Equals("") && !_itemName.Contains("Coin") && !_itemName.Contains("Armour")&& !_itemName.Equals("None"))
        {
            showArrow = false;
            //checkEquipmentTool = true;
            GameLog.LogMessage("SetupInteractableObject =" + _itemName + "taskName:" + taskName);
            GameLog.LogMessage("transform name" + transform.name, transform);
            this.interactableItemName = _itemName;
            interactableItem = PickupItemAssets.Instance.allItemsDatabase.GetItemObjectFromName(_itemName).createItem();
            
            SetCheckEquipmentTool();

        }
        /*else if(_itemName == null && taskName == null){
            showArrow = false;

        }*/
        
        if (taskName != null && !taskName.Equals("") && !taskName.Equals("Coin") && !taskName.Equals("None"))
            SetupTimelineAction(taskName);
    }



    protected void SetCheckEquipmentTool() {

       
            checkEquipmentTool = true;
    

    }
    private void Awake()
    {
        
        itemSpriteRenderer = GetComponent<SpriteRenderer>();

        SetupInteractableObject(interactableItemName, null);



       /* if (!interactableItemName.Equals("") && !interactableItemName.Equals("None") && !interactableItemName.Equals("Coin") && !interactableItemName.Equals("Armour"))
            interactableItem = PickupItemAssets.Instance.GetItem(interactableItemName.ToString());
*/
       // SetCheckEquipmentTool();
        if (itemSpriteRenderer)
            itemSprite = itemSpriteRenderer.sprite;
        interactAction = InteractControllerAction;
        if(!button)
            button = GetComponent<Button_Sprite>();
        if (button && clickableSprite)
        {
            button.ClickFunc = () =>
            {
                if (isInRange)
                {
                    if (disableArrowDelegate != null)
                        disableArrowDelegate();
                    interactAction();
                }
            };
        }
        if (!player)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player");
            if (playerTransform)
            {
                player = playerTransform.GetComponent<PlayerPlatformerController>();
                playerAnimator = playerTransform.GetComponent<Animator>();
            }
        }

        if (!lighterTransform) { 
        
            lighterTransform = GameObject.FindGameObjectWithTag("Lighter").transform;
        }
        SetupInteractableObject(interactableItemName, null);
         if (!interactableLight)
            interactableLight = GetComponent<Light2D>();
        if (!interactableLight) 
        {
            var testLight = transform.Find("Light");
                if(testLight)
                    interactableLight = testLight.GetComponent<Light2D>();  
        }
        AwakeControllerAction();
    }
    void Start()
    {
        // interactableItem = transform.parent.GetComponent<Item>();
        OnInteractableStart();
    }

    public void RemoveInteractableItemCheck()
    {
        if (removeInteractableItem)
        {
            player.GetEquipment().removeWeaponItem();
            checkEquipmentTool = false;
        }
    }


    protected virtual void AwakeControllerAction()
    {
    }

    protected virtual void InteractControllerAction()
    {
    }
    protected virtual void SetupTimelineAction(string TaskName)
    {
    }
    protected virtual void StartCollideWithPlayer()
    {
    }
    protected virtual void OnInteractableStart()
    {
    }
    protected virtual void StopCollideWithPlayer()
    {
    }


    protected virtual void DoWhenItemNotMatch()
    {
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // GameLog.LogMessage("Interactable object collision entered");
        if (collision.tag == "Player")
        {
            if (showLightBeforeArrow)
                ShowLight();
            if (!dontStartOnEnter)
            PerformCheckAndAction();
            GameLog.LogMessage("interact with player" + transform.name);
        }
    }
    private void ShowLight() {
        ShowLight(0, lightIntensity);
    }
    private void ShowLight(float from, float to) {
       // GameLog.LogMessage("Show Light Intensity amount:" + interactableLight?.intensity);
        if(interactableLight) { 
            if(to!=0)
                interactableLight.gameObject.SetActive(true);
            interactableLight.enabled = true;
            var seq = LeanTween.sequence();
            seq.append(LeanTween.value(gameObject, from, to, tweenTime).setEase(LeanTweenType.easeInOutQuad).setDelay(0f).setLoopPingPong().setOnUpdate((float val) =>
            {
                interactableLight.intensity = val;
            }));
        }
    }
    private void DisableLight()
    {
        if (interactableLight) 
        {
            ShowLight(lightIntensity, 0);
            LeanTween.cancel(interactableLight.transform.gameObject);
            // interactableLight.gameObject.SetActive(false);
            interactableLight.enabled = false;
        }
    }
    public void PerformCheckAndAction()
    {
        GameLog.LogMessage("Perform and Check Action entered");
        if (checkEquipmentTool)
        {
            showArrow = false;
            if (interactableItem != null && player.GetEquipment() && player.GetEquipment().GetWeaponItem() != null && interactableItemName == player.GetEquipment().GetWeaponItem().Name)
            {
                GameLog.LogMessage("Interactable obieks sie zgadza weic puszczaj ");
                showArrow = true;
                removeInteractableItem = true;


            }
            else if (interactableItem != null && player.GetEquipment() && player.GetEquipment().GetWeaponItem() != null && interactableItemName != player.GetEquipment().GetWeaponItem().Name)
            {
                DoWhenItemNotMatch();
                GameLog.LogMessage("Item w equipment nie zgadza sie z itemem z interactable object" + interactableItemName + " itam from equipment" + player.GetEquipment().GetWeaponItem().Name);
              //  return;
            }
            else
            {
                DoWhenItemNotMatch();
                GameLog.LogMessage("Brakuje obkeitow w rece lub konfiguracji interactable obiektu");
                //return;
            }
        }
        
        StartCollideWithPlayer();
        this.isInRange = true;
        ShowArrorAndLight();
    }
    private void ShowArrorAndLight()
    {
        if (showArrow & showArrowDelegate != null)
        {
            if (itemSprite)
                GameLog.LogMessage("Show arrow sprite " + itemSprite);
            else
                GameLog.LogMessage("Nie ma spritea" + itemSprite);
            if (changeArrowSprite)
                showArrowDelegate(interactAction, itemSprite);
            else
                showArrowDelegate(interactAction, null);
        }
        else {
            GameHandler.Instance.DisableArrow();
            return;
        }
        ShowLight();
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        GameLog.LogMessage("Interactoable object collision entered");
        if (collision.tag == "Player")
        {
            GameLog.LogMessage("Collide with player left");
            DisableLight();
            StopCollideWithPlayer();
            if (disableArrowDelegate != null)
                disableArrowDelegate();
            //showArrow = false;
            this.isInRange = false;
            LeanTween.cancel(gameObject);
            GameLog.LogMessage("Interact with player" + transform.name);
        }
    }
}
