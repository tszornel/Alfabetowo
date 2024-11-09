using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
using System.Linq;
using UnityEngine.Pool;
public class ShopDisplay : MonoBehaviour
{
    [SerializeField] private GameObject itemInventoryPrefab;
   // public static Action<Item> UseItemAction;
    public InventoryObject inventoryObject;
    public DescriptionDisplay displayItem;
    public Transform itemSlotContainer;
   // List<Transform> clickedItems = new List<Transform>();
    Transform clickedItem;
    public float smoothTime = 0.3F;
    List<GameObject> itemsToRelease;
    private ObjectPoolerGeneric pooler;
    public UnitAudioBehaviour audio;

    public static ShopDisplay Instance { get; private set; }

  

    public void HideShop()
    {

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        displayItem?.Hide();

    }

    public void DisplayShop() { 
    
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
          
    
    }

    private void Awake()
    {
        DescriptionDisplay.ItemBoughtAction += ItemBought;
        SetInventory(inventoryObject);
        if (itemsToRelease==null)
            itemsToRelease = new List<GameObject>();
        pooler = ObjectPoolerGeneric.Instance;
        audio = GetComponent<UnitAudioBehaviour>();
    }

    private void ItemBought()
    {
        audio?.PlaySuccess();
        HideShop(); 
    }

    public void SetInventory(InventoryObject inventory)
    {
        GameLog.LogMessage("Set inventory OnItemListChanged");
        this.inventoryObject = inventory;
        if(!pooler)
            pooler = ObjectPoolerGeneric.Instance;
      
        UpdateShopDisplay();
    }
    private void Start()
    {
        GameLog.LogMessage("SHOW DISPLAY");
        
    }
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        GameLog.LogMessage("Inventory_OnItemListChanged");
        UpdateShopDisplay();
    }


    void ClearShopDisplay() {

        foreach (Transform child in itemSlotContainer)
        {
            if (child.name.Contains("Item"))
                if (itemsToRelease != null)
                    itemsToRelease.Add(child.gameObject);
           
            else
                Destroy(child.gameObject);
        }

        if (itemsToRelease != null)
            if (itemsToRelease.Count > 0)
            {
                foreach (GameObject _item in itemsToRelease)
                {
                    pooler.ReleaseToPool("ShopItemsScene", _item);
                }
                itemsToRelease.Clear();
            }
            else {
                 itemsToRelease = new List<GameObject>();
            }


    }
    private void UpdateShopDisplay()
    {
        GameLog.LogMessage("Update Shop Display");
        ClearShopDisplay();
        if (inventoryObject)
        {
            
            for (int i = 0; i < inventoryObject.inventory.GetInventorySlotArray().Length; i++)
            {
                InventorySlot slot = inventoryObject.inventory.GetInventorySlotArray()[i];
                //var obj = Instantiate(itemInventoryPrefab, itemSlotContainer).GetComponent<RectTransform>();
                var obj = pooler.SpawnFromPool("ShopItemsScene", itemSlotContainer.position, itemSlotContainer.rotation, itemSlotContainer);
                obj.transform.name = "Item";
                obj.transform.SetAsLastSibling();
                RectTransform itemShopRectTransform = obj.GetComponent<RectTransform>();
               // itemShopRectTransform.gameObject.SetActive(true);
                Item item = slot.GetItem();
                if (item.Id != -1 && item.Type != ItemObjectType.None)
                {
                    item.SetLocation(ItemLocation.Shop);
                    int amount = slot.amount;
                    UI_ItemMain uiItem = obj.GetComponent<UI_ItemMain>();
                    uiItem.SetAmountText(amount);
                    uiItem.SetItem(item);
                    uiItem.SetLocation(ItemLocation.Shop);
                    uiItem.Show();
                    uiItem.GetComponent<Button_UI>().ClickFunc = () =>
                    {
                        // Use item
                        GameLog.LogMessage("Show Item Details Panel");
                        LeanTween.cancelAll();
                        //make preovious items smaller
                        if (clickedItem != null)
                            LeanTween.scale(clickedItem.GetComponent<RectTransform>(), Vector3.one, 0.5f).setEaseInBounce();

                        LeanTween.scale(itemShopRectTransform, Vector3.one * 1.2f, 2f).setEaseInBounce().setLoopPingPong();
                        clickedItem = itemShopRectTransform;
                        displayItem.gameObject.SetActive(true);
                        displayItem.ShowItemDescription(item);

                    };
                }
            }
        }
        }
            void OnDestroy()
            {
                 DescriptionDisplay.ItemBoughtAction -= ItemBought;
                if (inventoryObject)
                    inventoryObject.OnItemListChanged -= Inventory_OnItemListChanged;
            }
    }
