using System;
using System.Collections.Generic;
using UnityEngine;
using static PickupItem;

public class FurnaceController : InteractableBase, IItemHolderNew
{
    [SerializeField] private UI_CharacterEquipmentSlot furnaceSlot;
    private InventoryObject characterInventory;
    private AudioSource playerAudioSource;
    public Dictionary<Item, Item> GetRepairedItemDictionary = new Dictionary<Item, Item>();
    private GameObject displayName;
    // public GameHandler gameHandler;
    public static event EventHandler OnItemInteracted;
    public static event EventHandler OnNameEntered;
    public static event EventHandler OnNameExit;
    public static event EventHandler OnFurnaceSlotSuccess;
    public delegate bool OnDamagedCollected(Item item);
    public static event OnDamagedCollected OnDamagedItemCollected;
    [SerializeField] private AudioClip cantDoItAC;
    //public static Action<NameObject> OnShowName;
    public Name nameInFurnace;
    protected static Item itemInFurnace;
    public FurnaceSlotData furnaceSlotData;
    ItemObject repairObject;
    public CutsceneTimelineBehaviour cutScene;
    public CutsceneTimelineBehaviour succesTimeline;
    [SerializeField] private UI_ItemMain itemInSlot;
    UI_FurnaceSlot furnaceSlotInstance;
    public InventoryDisplay inventoryDisplay;

    private void InventorySlot_OnItemDroppedFromFurnace(object sender, UI_ItemMain.OnInventoryItemDroppedEventArgs e)
    {
        GameLog.LogMessage("InventorySlot_OnItemDroppedFromFurnace entered item:" + e._item + "droped");
        furnaceSlotInstance.RemoveItemInFurnaceSlot();
        furnaceSlotInstance.SetItemInFurnaceSlot(null);
        furnaceSlotData.furnaceSlotItem =null;  
        furnaceSlotData.furnaceName.NameIn = null;
        Name.HideNameBox();
    }

    private void InventorySlot_OnItemDroppedFromFurnaceInSlot(object sender, InventoryDisplay.OnInventoryItemDroppedInSlotEventArgs e)
    {
        GameLog.LogMessage("InventorySlot_OnItemDroppedFromFurnace entered item:" + e._item + "droped");
        furnaceSlotInstance.RemoveItemInFurnaceSlot();
        furnaceSlotInstance.SetItemInFurnaceSlot(null);
        furnaceSlotData.furnaceSlotItem = null;
        furnaceSlotData.furnaceName.NameIn = null;
        Name.HideNameBox();
    }

    private void Slot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        GameLog.LogMessage("Slot_OnItemDropped Entered location:"+ e.item.GetEquipSlot());
         if (e.item.GetEquipSlot() == CharacterEquipment.EquipSlot.Repair && itemInFurnace == null) 
            {
            GameLog.LogMessage("SET FURNACE ITEMS IN SLOT" + furnaceSlotInstance+" item:"+e.item+" from "+ e.item.GetItemHolder().ToString());
           
            GameLog.LogMessage("Remove item droped from inventory when dropped in Furnace");
            e.item.GetItemHolder().RemoveItem(e.item);
            //characterInventory.RemoveItem(furnaceSlotData.furnaceSlotItem);
            e.item.SetItemHolder(this);
            e.item.Location = ItemLocation.FurnaceSlot;
            furnaceSlotInstance.SetItemInFurnaceSlot(e.item);
            furnaceSlotInstance.GetItemInSlot().OnInventoryItemDropped += InventorySlot_OnItemDroppedFromFurnace;
            InventoryDisplay.OnInventoryItemDroppedInSlot += InventorySlot_OnItemDroppedFromFurnaceInSlot;
            UI_itemTouch.Instance.touchItemDropedinFurnace = true;
            UI_itemTouch.Instance.Hide();
           
            
           
           

            //  itemInSlot?.SetItem(e.item);
            OnDamagedItemCollected?.Invoke(e.item);
                furnaceSlotData.furnaceSlotItem = new Item(e.item);
                furnaceSlotData.furnaceSlotItem.Location = ItemLocation.FurnaceSlot;
                player.GetComponent<PlayerPlatformerController>().GetInventory().RemoveItem(e.item);
                GameLog.LogMessage(" item in furnace" + furnaceSlotData.furnaceSlotItem.ToString());
                repairObject = furnaceSlotData.furnaceSlotItem.GetItemObject();
                GameLog.LogMessage(" item name Obejct" + repairObject.nameObject.ToString());
                furnaceSlotData.furnaceName.NameIn = repairObject.nameObject;
            if(!furnaceSlotData.furnaceName.NameIn.CheckCorrect())    
                furnaceSlotData.furnaceName.UncheckSuccess();
                furnaceSlotData.furnaceName.OnPlaySuccess += Name_OnPlaySuccess;
                furnaceSlotData.furnaceName.ItemIn = e.item;//new Item(repairObject.itemAfterFix);
                furnaceSlotData.furnaceName.ItemPrefab = repairObject.itemPrefab;
                displayName.SetActive(true);
                furnaceSlotData.furnaceName.ShowName();
               // DisplayName.Instance.ShowNameBox();
                // nameIn.repairedObjectIn = true;
                // characterInventory.RemoveItem(e.item);
                //Odpalic event On dropSlot do wyswietlenia nameIn
                //Name.ShowName(nameInFurnace.nameIn);
                if (cutScene)
                    cutScene.StartTimeline();
            }
            else
            {
                //Odebraj nie moge tego 
                if (cantDoItAC && playerAudioSource)
                    playerAudioSource.PlayOneShot(cantDoItAC);
                else
                    GameLog.LogMessage("NIE MOGE TEGO ZROBIC equip slot"+ e.item.GetEquipSlot());
                GameLog.LogMessage(" item in furnace" + itemInFurnace?.ToString());
            }
        
    }
    private void Name_OnPlaySuccess(object sender, EventArgs e)
    {
        GameLog.LogMessage("Name_OnPlaySuccess succes entered:"+ sender);
        OnFurnaceSlotSuccess.Invoke(this, e);   
          succesTimeline?.StartTimeline();
        if(furnaceSlotData.furnaceName)
            furnaceSlotData.furnaceName.OnPlaySuccess -= Name_OnPlaySuccess;
        //Remove item from inventory
      
        furnaceSlot.GetComponent<UI_ItemMain>().SetItem(null);
        furnaceSlotData.furnaceName.NameIn = null;  
        furnaceSlotData.furnaceSlotItem = null;
        furnaceSlotInstance.RemoveItemInFurnaceSlot();
      
        Name.HideNameBox();
       // itemInSlot?.SetItem(null);
    }
    public void FurnaceSlotSetUp() {
        if (furnaceSlot)
        {
            GameLog.LogMessage("Furnace Slot subscribed");
            furnaceSlot.OnItemDropped += Slot_OnItemDropped;
        }
    }
    public void UnactivateFurnaceSlotSetUp()
    {
        if (furnaceSlot)
        {
            GameLog.LogMessage("Furnace Slot subscribed");
            furnaceSlot.OnItemDropped -= Slot_OnItemDropped;
        }
    }
    protected override void OnInteractableStart()
    {
        GameLog.LogMessage("OnInteractable Start");
        furnaceSlotInstance = UI_FurnaceSlot.Instance;


        //if (inventoryDisplay)
           // inventoryDisplay


        if (!furnaceSlotInstance)
            furnaceSlotInstance = GameObject.FindObjectOfType<UI_FurnaceSlot>(true).GetComponent<UI_FurnaceSlot>();
        //portrait = GetComponent<WindowCharacter_Portrait>();
        if (!displayName)
            displayName = GameObject.FindObjectOfType<DisplayName>(true)?.gameObject;
        if (!furnaceSlotData.furnaceName)
            furnaceSlotData.furnaceName = GetComponent<Name>();
        if (!cutScene)
            cutScene = GetComponent<CutsceneTimelineBehaviour>();
        else {
            GameLog.LogMessage("Furnace Slot not present");
        }
        if (player)
        {
            characterInventory = player.GetInventory();
            playerAudioSource = player.gameObject.GetComponent<AudioSource>();
        }
    }


  
    protected override void StartCollideWithPlayer()
    {
        //WindowCharacter_Portrait.SetDoNotHide();
        //ustawienie on drop slot todo
        if (furnaceSlotData.furnaceSlotItem != null && furnaceSlotData.furnaceSlotItem.Id != 0)
        {
            showArrow = false;
            furnaceSlotInstance.ActivateUISlot();
            if(furnaceSlotData.furnaceSlotItem != null )
                OnNameEntered?.Invoke(this, EventArgs.Empty);
            // nameInFurnace?.ShowName();
            GameLog.LogMessage("Furnace item" + furnaceSlotData.furnaceSlotItem.ToString());
            // GameLog.LogMessage("nameIn" + nameIn.ToString());
            //nameInFurnace.ShowName();
        }
        else {
            OnItemInteracted?.Invoke(this, EventArgs.Empty);
        }
    }
    protected override void StopCollideWithPlayer()
    {
        GameLog.LogMessage("StopCollideWithPlayer Furnace item");
       // if(WindowCharacter_Portrait.Instance)
         //   WindowCharacter_Portrait.UnSetDoNotHide();
        // displayName.GetComponent<DisplayName>().HideNameBox();
        //wylaczenie obiektu ();
        UnactivateFurnaceSlotSetUp();
        furnaceSlotInstance.DeactivateUISlot();
        // SlotUIGO?.SetActive(false);
        if (furnaceSlotData.furnaceSlotItem != null && furnaceSlotData.furnaceSlotItem.Id != 0)
        {
            GameLog.LogMessage("Furnace item" + furnaceSlotData.furnaceSlotItem.ToString());
            OnNameExit?.Invoke(this, EventArgs.Empty);
            //  GameLog.LogMessage("nameIn" + nameInFurnace.ToString());
        }
        else 
        {
            showArrow = true;
        }
         OnItemInteracted?.Invoke(this, EventArgs.Empty);
    }
    protected override void InteractControllerAction()
    {
        ActivateFurnace();
    }
    public void ActivateFurnace() 
    {
        GameLog.LogMessage("ActivateFurnace entered",this);
        //DiasbleArrow
        GameHandler.Instance.DisableArrow();
        GameHandler.Instance?.ShowInventory();
        FurnaceSlotSetUp();
        furnaceSlotInstance.ActivateUISlot();
        //Tween();
        if (furnaceSlotData.furnaceSlotItem != null)
        {
          DisplayName.Instance?.HideNameBox();
        }else 
        {
            GameLog.LogMessage("Brak itema w furnace");
        }
        /* if (gameHandler)
        * 
            gameHandler.ShowInventory();*/
        //Jezeli item inside to wyzucamy przedmiot (musi byc ciagle zniszczony bo jak bedzie naprawiony to sam wyleci)
    }
    private void Tween() {
        LeanTween.cancel(gameObject);
       // previousRotation = gameObject.transform.rotation.eulerAngles;
       // LeanTween.rotateZ(gameObject, -45, 1f).setEaseOutBounce();
        LeanTween.rotateZ(gameObject, -45, 1f).setEasePunch().setOnComplete(() =>
            {
                Debug.Log("Animation Complete!");
                LeanTween.rotateZ(gameObject, 45, 1f).setEasePunch().setOnComplete(TweenBack);
          });
    }
    private void TweenBack() {
        LeanTween.rotateZ(gameObject, 0, 1f).setEasePunch();
    }


    private void OnApplicationQuit()
    {
        furnaceSlotInstance?.DeactivateUISlot();
        furnaceSlotData.Reset();
        if (furnaceSlotInstance != null && furnaceSlotInstance.GetItemInSlot())
            furnaceSlotInstance.GetItemInSlot().OnInventoryItemDropped -= InventorySlot_OnItemDroppedFromFurnace;
        // inventoryDisplay.OnInventoryItemDropped -= InventorySlot_OnItemDroppedFromFurnace;

    }


    private void OnDisable()
    {
        if(furnaceSlotInstance != null && furnaceSlotInstance.GetItemInSlot()) 
            furnaceSlotInstance.GetItemInSlot().OnInventoryItemDropped -= InventorySlot_OnItemDroppedFromFurnace;
    }

    public void RemoveItem(Item item)
    {
        throw new NotImplementedException();
    }

    public void AddItem(Item item)
    {
        throw new NotImplementedException();
    }

    public bool CanAddItem(Item item)
    {
        throw new NotImplementedException();
    }
}
