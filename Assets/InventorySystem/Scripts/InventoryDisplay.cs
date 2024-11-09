using Webaby.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject letterInventoryPrefab;
    [SerializeField] private GameObject itemInventoryPrefab;
    [SerializeField] private WindowCharacter_Portrait windowCharacterPortrait;
    public static Func<Item, Boolean> UseItemAction;
    // public static Action<Item> TryToBuyItemAction;
    private InventoryObject inventoryObject;
    public Transform player;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public bool shop;
    public bool displayOnStart;
    public DescriptionDisplay displayItem;
    public static event EventHandler<OnInventoryItemDroppedInSlotEventArgs> OnInventoryItemDroppedInSlot;
    public class OnInventoryItemDroppedInSlotEventArgs : EventArgs
    {
        public Item _item;
    }
  

    [Header("Pools")]
    private static ObjectPoolerGeneric pooler;
    List<GameObject> slotsToRelease;
    List<GameObject> itemsToRelease;
    private void Awake()
    {
        AwakeSlots();
        AwakePools();
        UpdateDisplay();
    }
    private void AwakePools()
    {
        pooler = ObjectPoolerGeneric.Instance;

    }

    public void SetInventory(InventoryObject inventory)
    {
        GameLog.LogMessage("Set inventory OnItemListChanged");
        this.inventoryObject = inventory;
        inventoryObject.OnItemListChanged += Inventory_OnItemListChanged;
    }


    public void DeactivateInventory(InventoryObject inventory)
    {
        GameLog.LogMessage("DeSet inventory OnItemListChanged");
        inventory.OnItemListChanged -= Inventory_OnItemListChanged;
    }

    public void SetInventory(InventoryObject inventory, bool updateDisplay)
    {
        SetInventory(inventory);
        if (updateDisplay)
        {
            if (displayOnStart && windowCharacterPortrait)
            {
                windowCharacterPortrait.Show();
            }
            UpdateDisplay();
            if (windowCharacterPortrait)
            {
                windowCharacterPortrait.Hide(5);
            }
        }
    }
    private void UseItem(Item item)
    {
        UseItemAction(item);
    }

    private void AwakeSlots()
    {
        if (!itemSlotContainer)
            itemSlotContainer = transform.Find("itemSlotContainer");
        if (!itemSlotTemplate)
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        //ukrycie obiektu slot template
        itemSlotTemplate.gameObject.SetActive(false);
        slotsToRelease = new List<GameObject>();
        itemsToRelease = new List<GameObject>();
    }
    private void Start()
    {


        //AwakeSlots();
        if (displayOnStart)
        {
            UpdateDisplay();
            if (windowCharacterPortrait)
                windowCharacterPortrait.Show();
        }

        if (windowCharacterPortrait)
            windowCharacterPortrait.Hide(5);
    }
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        GameLog.LogMessage("Inventory_OnItemListChanged");
        if (windowCharacterPortrait)
            windowCharacterPortrait.Show();
        UpdateDisplay();
        if (windowCharacterPortrait)
            windowCharacterPortrait.Hide(5);
    }



    void ClearInventoryDisplay()
    {
        GameLog.LogMessage(" Inventory Object present");
        int x = 0;
        if (slotsToRelease != null && itemsToRelease != null)
        {
            // GameLog.LogMessage(" slotsToRelease!=null");
            foreach (Transform child in itemSlotContainer)
            {
                // GameLog.LogMessage("Widze " + x + " child" + child.name);
                x++;
                if (child.name.StartsWith("Template")) Destroy(child.gameObject);
                if (child == itemSlotTemplate) continue;
                if (child.name.Contains("Slot"))
                {
                    if (child != null)
                    {
                        //  GameLog.LogMessage("Relesing child=" + child.name, child);
                        slotsToRelease.Add(child.gameObject);

                        continue;

                    }
                }
                else if (child.name.Contains("Item"))
                {
                    if (child != null)
                    {
                        //  GameLog.LogMessage("Relesing child=" + child.name);
                        itemsToRelease.Add(child.gameObject);
                        continue;

                    }
                }
            }
            //GameLog.LogMessage("Slots to Release Count" + slotsToRelease.Count);
            if (slotsToRelease != null && slotsToRelease.Count > 0)
            {
                foreach (GameObject slot in slotsToRelease)
                {
                    pooler.ReleaseToPool("InventorySlots", slot);
                }
                slotsToRelease.Clear();
            }
            if (itemsToRelease != null && itemsToRelease.Count > 0)
            {
                foreach (GameObject item in itemsToRelease)
                {
                    pooler.ReleaseToPool("UIItems", item);
                }
                itemsToRelease.Clear();
            }
        }
        else
        {

            GameLog.LogMessage("Slots to release null!!!!!!!!!!!!!!!!!!!!!!!");


        }
    }
    private void UpdateDisplay()
    {
        ClearInventoryDisplay();
        GameLog.LogMessage("Update Inventory Display");
        if (inventoryObject)
        {

            for (int i = 0; i < inventoryObject.inventory.GetInventorySlotArray().Length; i++)
            {
                GameLog.LogMessage("inside forinventory " + i);
                InventorySlot slot = inventoryObject.inventory.GetInventorySlotArray()[i];
                GameLog.LogMessage("Slot " + slot?.ToString());
                if (!pooler)
                    return;
                Transform displayedSlot = pooler.SpawnFromPool("InventorySlots", GetPosition(i), Quaternion.identity, itemSlotContainer).transform;
                displayedSlot.SetAsLastSibling();
                RectTransform itemSlotRectTransform = displayedSlot.GetComponent<RectTransform>();
                itemSlotRectTransform.name = "Slot";
                itemSlotRectTransform.anchoredPosition = GetPosition(i);
                UI_ItemSlot uiItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
                InventorySlot tmpInventorySlot = slot;

                if (!shop)
                    uiItemSlot.SetOnDropAction(() =>
                    {
                        GameLog.LogMessage("on drop inventory action entered");
                        Item draggedItem = UI_itemTouch.Instance.GetItem();
                        GameLog.LogMessage("on drop inventory action entered item location" + draggedItem.Location);
                        UI_itemTouch.Instance.touchItemDropedinInventory = true;
                        if (draggedItem.Location == ItemLocation.Inventory)
                        {

                            GameLog.LogMessage("Przesuniecie z Inventarza do innego slot jest zabronione nic nie rob !!!!");
                            return;

                        }


                        GameLog.LogMessage("Remove item from previous Holder");

                        if (draggedItem.Location == ItemLocation.Equipment)
                            player.GetComponent<PlayerPlatformerController>().GetEquipment().RemoveItem(draggedItem);
                        else if (draggedItem.GetItemHolder() != null)
                        {
                            GameLog.LogMessage("Show holder " + draggedItem.GetItemHolder());

                            draggedItem.GetItemHolder()?.RemoveItem(draggedItem);

                        }
                        else if (draggedItem.Location == ItemLocation.FurnaceSlot) {

                            OnInventoryItemDroppedInSlot.Invoke(this, new OnInventoryItemDroppedInSlotEventArgs { _item = draggedItem });


                        }


                        GameLog.LogMessage("Item id that is already in slot " + tmpInventorySlot.GetItem()?.Id);
                        if (tmpInventorySlot.IsEmpty())
                        {



                            if (draggedItem.Location == ItemLocation.FurnaceSlot)
                            {
                                GameLog.LogMessage("Item moved to inventory from  Furnace ");

                                Item newItem = new Item(draggedItem);

                                inventoryObject.AddItem(newItem, tmpInventorySlot, 1);


                            }
                            else
                            if (draggedItem.Location == ItemLocation.Equipment)
                            {
                                GameLog.LogMessage("Item moved to inventory from  Equipment ");
                                // player.GetComponent<PlayerPlatformerController>().GetEquipment().RemoveItem(draggedItem);
                                Item newItem = new Item(draggedItem);
                                newItem.Location = ItemLocation.Inventory;
                                inventoryObject.AddItem(newItem, tmpInventorySlot, 1);
                            }
                            else
                            if (draggedItem.Location == ItemLocation.Shop)
                            {

                                Item newItem = new Item(draggedItem);
                                // inventoryObject.RemoveItem(draggedItem);
                                //TryToBuyItem(newItem);
                            }
                            else
                            {
                                GameLog.LogMessage("Slot pusty wiec dodajemy item");
                                //inventoryObject.RemoveItem(slot, draggedItem);
                                Item newItem = new Item(draggedItem);
                                inventoryObject.AddItem(newItem, tmpInventorySlot, 1);
                            }
                        }
                        else if (tmpInventorySlot.GetItem().Id == draggedItem.Id && tmpInventorySlot.GetItem().stackable)
                        {
                            GameLog.LogMessage("W slot jest ten same item  wiec dodajemy ");
                            //inventoryObject.RemoveItem(slot, draggedItem);
                            Item newItem = new Item(draggedItem);
                            newItem.Location = ItemLocation.Inventory;
                            inventoryObject.AddItem(newItem, tmpInventorySlot, 1);
                        }
                    });


                if (slot != null && !slot.IsEmpty())
                {
                    GameLog.LogMessage("Dlaczego tu wpada ?");
                    Item item = slot.GetItem();
                    item.SetLocation(ItemLocation.Inventory);
                    int amount = slot.amount;
                    GameLog.LogMessage("Amount ze slota" + amount);

                    var obj = pooler.SpawnFromPool("UIItems", itemSlotRectTransform.position, Quaternion.identity, itemSlotContainer);
                    obj.transform.SetParent(itemSlotContainer, false);
                    obj.transform.SetAsLastSibling();
                    obj.name = "Item";
                    obj.transform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                    obj.transform.GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
                    obj.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    UI_ItemMain uiItem = obj.GetComponent<UI_ItemMain>();

                    uiItem.SetParentSlot(itemSlotRectTransform);
                    uiItem.SetAmountText(amount);
                    uiItem.SetItem(item);
                    uiItem.SetLocation(ItemLocation.Inventory);
                    uiItem.Show();

                    uiItem.GetComponent<Button_UI>().ClickFunc = () =>
                    {
                        // Use item
                        GameLog.LogMessage("Set Use or Try to Buy item on click !!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        if (!shop)
                        // TryToBuyItem(slot.item);
                        // else 
                        {
                            UseItem(slot.item);

                        }

                    };

                    //GameLog.LogMessage("Set click func" + itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc + " slot:" + slot.GetItem());

                    uiItem.GetComponent<Button_UI>().LongTouchFunc = () =>
                    {
                        // Use item
                        GameLog.LogMessage("SUPER !!! Pokaz item ");
                        //  displayItem.gameObject.SetActive(true);
                        // displayItem.ShowItemDescription(item);
                    };

                    uiItem.GetComponent<Button_UI>().MouseRightClickFunc = () =>
                    {
                        // Drop item
                        GameLog.LogMessage("Double touch Try drop item  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        // GameLog.LogMessage("Droping item" + slot.GetItem().ToString() + "Position " + player.position.ToString());

                        if (player)
                            PickupItem.DropItem(player.position, slot.item, player.GetComponent<PlayerPlatformerController>().faceRightCheck());
                        inventoryObject.RemoveItem(slot, slot.item);
                    };
                }
            }
        }
    }
    private Vector2 GetPosition(int i)
    {
        return new Vector2(X_START + (X_SPACE_BETWEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)));
    }

    private void OnDisable()
    {
        // if (inventoryObject)
        //   DeactivateInventory(inventoryObject);
    }

    void OnDestroy()
    {
        if (inventoryObject)
            DeactivateInventory(inventoryObject);
    }
}