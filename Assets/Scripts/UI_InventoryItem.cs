using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_InventoryItem : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private CanvasGroup canvasGroup;
    public UnityEngine.UI.Image image;
    private Item item;
    public TextMeshProUGUI amountText;
    public TextMeshProUGUI letterText;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemAttributes;

    private GameObject player;
    private PlayerPlatformerController playerPlatformerController;
    private int amount;
    private ItemLocation location;
    private Transform parentSlot;
    private InventoryObject inventoryObject;
    bool blocked = false;
    private StringBuilder sb;
    Dictionary<string, string> atributesNameToIcon = new Dictionary<string, string>();

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
        atributesNameToIcon.Add("Power", "<sprite name=Sword>");
        atributesNameToIcon.Add("Health", "<sprite name=Heart>");
        atributesNameToIcon.Add("Intelect", "<sprite name=Intelect>");
        atributesNameToIcon.Add("Agility", "<sprite name=Jump>");
        atributesNameToIcon.Add("Speed", "<sprite name=Run>");
        atributesNameToIcon.Add("Protection", "<sprite name=Shield>");
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
        else
        {
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
          GameLog.LogMessage("On Drag ItemMain");
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
        else
        {
            // item.GetItemHolder()?.RemoveItem(item);
        }
        UI_itemTouch.Instance.touchItemDropedinInventory = false;
        UI_itemTouch.Instance.touchItemDropedinEquipment = false;
        UI_itemTouch.Instance.touchItemDropedinName = false;
        UI_itemTouch.Instance.touchItemDropedinFurnace = false;
        SetLocation(ItemLocation.None);
        GameLog.LogMessage("OnEndDrag left");
    }

    public void RemoveItemInSlot(Item item)
    {
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


    private void SetParameters(ItemObject _itemObject) {

        // string descriptionText = "";
        if (sb != null)
            sb.Clear();

        if (_itemObject.buffs != null)
        {
            if (_itemObject.buffs.Length > 0)
            {
                for (int i = 0; i < _itemObject.buffs.Length; i++)
                {
                    int value = _itemObject.buffs[i].value;
                    string name = _itemObject.buffs[i].attribute.ToString();
                    GameLog.LogMessage("attribute value" + value + " i=" + i);
                    GameLog.LogMessage("attribute name" + name);

                    sb.Append(atributesNameToIcon[_itemObject.buffs[i].attribute.ToString()]);
                    sb.Append(" ");
                    sb.Append(value);
                    sb.Append("\n");

                    // descriptionText += atributesNameToIcon[item.buffs[i].attribute.ToString()] + " " + value + "\n";
                }
            }
        }

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


       /* if (itemName)
            itemName.text = itemObject.displayName;*/

      



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
        GameLog.LogMessage("ODDrop from Item !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + this.GetParentSlot()?.name + " transform" + transform.name + " parent:" + transform.parent.name);


        

    }
}


