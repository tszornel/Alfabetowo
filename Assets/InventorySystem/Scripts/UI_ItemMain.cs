/* 
    ------------------- Code Monkey -------------------
    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!
               unitycodemonkey.com
    --------------------------------------------------
 */

using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;


public enum ItemLocation
{
    Inventory,
    Equipment,
    Shop,
    Name,
    FurnaceSlot,
    None
}
public class UI_ItemMain : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private CanvasGroup canvasGroup;
    private UnityEngine.UI.Image image;
    private Item item;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI letterText;
    private GameObject player;
    private PlayerPlatformerController playerPlatformerController;  
    private int amount;
    private ItemLocation location;
    private Transform parentSlot;
    private InventoryObject inventoryObject;
    bool blocked = false;   



    public event EventHandler<OnInventoryItemDroppedEventArgs> OnInventoryItemDropped;
    public class OnInventoryItemDroppedEventArgs : EventArgs
    {
        public Item _item;
    }
    public void SetParentSlot(Transform _parentSlot)
    {
        parentSlot = _parentSlot;
    }
    public Transform GetParentSlot()
    {
        return parentSlot;
    }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        image = transform.Find("image").GetComponent<UnityEngine.UI.Image>();
        letterText = transform.Find("letterText")?.GetComponent<TextMeshProUGUI>();
        amountText = transform.Find("amountText")?.GetComponent<TextMeshProUGUI>();
        // parentRectTransform = transform.parent.GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerPlatformerController = player?.GetComponent<PlayerPlatformerController>(); 
        location = ItemLocation.None;
        inventoryObject = player.GetComponent<PlayerPlatformerController>().GetInventory();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (item.GetItemHolder() != null)
        {
            GameLog.LogMessage("Item Holder" + item.GetItemHolder());
           // GameLog.LogBreak(" Holder present");
        }
        else {


            GameLog.LogMessage("Missing Holder");
        
        }

        if (item != null)
        {
            
            GameLog.LogMessage("On Begin Drag ItemMain:" + location + " item location" + item.Location);
            GameLog.LogMessage("On Begin Drag Item" + item + " location" + location + " droped in name?" + UI_itemTouch.Instance.touchItemDropedinName + " droped in furnace?" + UI_itemTouch.Instance.touchItemDropedinFurnace + " droped in inventory?" + UI_itemTouch.Instance.touchItemDropedinInventory + " droped in equipment?" + UI_itemTouch.Instance.touchItemDropedinEquipment);
        }
        // canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;
        GameLog.LogMessage("UI Item main show item:" + item);
        UI_itemTouch.Instance.Show(item);

    }
    public void OnDrag(PointerEventData eventData)
    {
        //  GameLog.LogMessage("On Drag ItemMain");
        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        UI_itemTouch.Instance.Hide();
        if (blocked) 
        {
            GameLog.LogMessage("OnEndDrag BLocked !!!!!!!!!!!!!!!!!!!!");
            blocked = false;
            return;
        }
            
        GameLog.LogMessage("On End Drag ItemMain");
        if (UI_itemTouch.Instance.touchItemMoving)
            UI_itemTouch.Instance.touchItemMoving = false;
        //  canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        UI_itemTouch.Instance.Hide();
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
      

        GameLog.LogMessage("Item" + item + " location" + item?.Location + " droped in name?" + UI_itemTouch.Instance?.touchItemDropedinName + " droped in furnace?" + UI_itemTouch.Instance?.touchItemDropedinFurnace + " droped in inventory?" + UI_itemTouch.Instance?.touchItemDropedinInventory);
        if (UI_itemTouch.Instance.touchItemDropedinEquipment == false && UI_itemTouch.Instance.touchItemDropedinFurnace == false && UI_itemTouch.Instance.touchItemDropedinInventory == false && UI_itemTouch.Instance.touchItemDropedinName == false)
        {


            PickupItem droppedItem = null;
            GameLog.LogMessage("Item location =" + item.Location);
            if (item.Location != ItemLocation.Shop && item.Location != ItemLocation.FurnaceSlot)
            {
                droppedItem = PickupItem.DropItem(player.transform.position, item, player.GetComponent<PlayerPlatformerController>().faceRightCheck());
                

                if (droppedItem != null)
                {
                    GameLog.LogMessage("Removing item in inventory");
                    RemoveItemInSlot(item);
                    item?.SetLocation(ItemLocation.None);
                }

            }

            

        }
        else {

           // item.GetItemHolder()?.RemoveItem(item);

        }
/*

        GameLog.LogMessage("Item location after drop:" + item?.Location);
        if (item != null)
            if (item.Location == ItemLocation.Equipment)
            {
                GameLog.LogMessage("Item droped outside Equipment ");
               // item.GetItemHolder()?.RemoveItem(item);
                //player.GetComponent<PlayerPlatformerController>().GetEquipment().RemoveItem(item);
            }
            else if (item.Location == ItemLocation.Shop)
            {
                GameLog.LogMessage("OnEndDrag Location Shop");
                //nic na razie nie rob
                return;
            }
            else if (item.Location == ItemLocation.Name)
            {
                GameLog.LogMessage("OnEndDrag Location Name");
                // nie rob nic 
                //return;
            }
            else if (item.Location == ItemLocation.FurnaceSlot)
            {
                GameLog.LogMessage("OnEndDrag from FURNADCE");
                OnInventoryItemDropped.Invoke(this, new OnInventoryItemDroppedEventArgs { _item = item });


                // return;
            }*/


        UI_itemTouch.Instance.touchItemDropedinInventory = false;
        UI_itemTouch.Instance.touchItemDropedinEquipment = false;
        UI_itemTouch.Instance.touchItemDropedinName = false;
        UI_itemTouch.Instance.touchItemDropedinFurnace = false;
        SetLocation(ItemLocation.None);
        GameLog.LogMessage("OnEndDrag left");
    }

    public void RemoveItemInSlot(Item item) {
        GameLog.LogMessage("Removing Item in slot" + item.Location);
        if (item.Location == ItemLocation.Inventory || item.Location == ItemLocation.None)
        {
            playerPlatformerController.GetInventory().RemoveItem(item);
          //  item.GetItemHolder()?.RemoveItem(item);
        }
        else if (item != null && item.Location == ItemLocation.FurnaceSlot)
        {
            GameLog.LogMessage("OnEndDrag from FURNADCE");
            OnInventoryItemDropped.Invoke(this, new OnInventoryItemDroppedEventArgs { _item = item });

           // item.GetItemHolder()?.RemoveItem(item);
            // return;
        }
        else if (item.Location == ItemLocation.Equipment)
        {


            player.GetComponent<PlayerPlatformerController>().GetEquipment().RemoveItem(item);
        }

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // GameLog.LogMessage("On Pointer Down");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Right click, split
            if (item != null)
            {
                // Has item
                //item.ID
                //if (item.IsStackable()) {
                //    // Is Stackable
                //    if (item.amount > 1) {
                //        // More than 1
                //        if (item.GetItemHolder().CanAddItem()) {
                //            // Can split
                //            int splitAmount = Mathf.FloorToInt(item.amount / 2f);
                //            item.amount -= splitAmount;
                //                   ItemGround duplicateItem = new ItemGround { itemScriptableObject = item.itemScriptableObject, amount = splitAmount };
                //            item.GetItemHolder().AddItem(duplicateItem);
                //        }
                //    }
                //}
            }
        }
    }
    public void SetSprite(Sprite sprite)
    {
        if (image != null && sprite != null)
        {
            image.sprite = sprite;
            image.enabled = true;
        }
        else if (image != null)
        {
            image.sprite = null;
        }
    }
    public void SetLocation(ItemLocation newLocation)
    {
       this.location = newLocation;
    }
    public ItemLocation GetLocation()
    {
        return location;
    }
    public void SetAmountText(int value)
    {
        //zerowanie amount
        amountText.text = "";
        //  GameLog.LogMessage("ustawiam text new value" + value);
        amount = value;
        // GameLog.LogMessage("ustawiam text amount+ value " + amount);
        if (amount <= 1)
        {
            amountText.text = "";
        }
        else
        {
            // More than 1
            amountText.text = amount.ToString();
        }
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void SetItem(Item _item)
    {
        if (_item == null || _item.Id == -1)
        {
            this.item = null;
            if (amountText)
                amountText.text = "";
            if (letterText)
                letterText.text = "";

            SetSprite(null);
            return;
        }
        if (_item.GetItemType() == ItemObjectType.None)
        {
            return;
        }
        this.item = _item;
        GameLog.LogMessage("Item" + item);
        ItemObject itemObject = item.GetItemObject();
        // GameLog.LogMessage("Item Type" + item.GetType());
        if (item.GetItemType() == ItemObjectType.Letter)
        {
            GameLog.LogMessage("ItemObject" + itemObject);
            SetLetterText(((LetterObject)itemObject).GetText());
            letterText.gameObject.SetActive(true);
            image.gameObject.SetActive(false);
        }
        else
        {
            if (itemObject && itemObject.itemSprite)
            {
                if (item.damaged)
                    SetSprite(itemObject.itemSpriteDamaged);
                else
                    SetSprite(itemObject.itemSprite);
                if (image)
                {
                    image.gameObject.SetActive(true);
                }
                else
                {
                    image = transform.Find("image").GetComponent<UnityEngine.UI.Image>();
                    if (image)
                    {
                        image.gameObject.SetActive(true);
                    }
                }
            }
            letterText?.gameObject.SetActive(false);
        }
        // SetAmountText(itemObject.amount);
    }
    public void SetLetterText(string _text)
    {
        // GameLog.LogMessage("Text to set" + _text);
        if (!letterText)
            letterText = transform.Find("letterText").GetComponent<TextMeshProUGUI>();
        letterText.text = _text;
        letterText.enabled = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameLog.LogMessage("ODDrop from Item !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!"+this.GetParentSlot()?.name+" transform"+transform.name+" parent:"+transform.parent.name);
        
        
       /* Item draggedItem = UI_itemTouch.Instance.GetItem();
        if (draggedItem != null && draggedItem.Id == item.Id && draggedItem.Location != ItemLocation.FurnaceSlot && draggedItem.Location != ItemLocation.Equipment)
        {
            Item itemNew= new Item(draggedItem);
            UI_itemTouch.Instance.touchItemDropedinInventory = true;
            UI_itemTouch.Instance.Hide();
            draggedItem.GetItemHolder()?.RemoveItem(draggedItem);
            inventoryObject.AddItem(itemNew);
           
        }
        else {
            blocked = true;
            UI_itemTouch.Instance.Hide();
            //Nie rob nic bo opoczony item na item

        }*/
        
    }
}
