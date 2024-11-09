using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShopDisplay : MonoBehaviour
{
    [SerializeField] private GameObject letterInventoryPrefab;
    [SerializeField] private GameObject itemInventoryPrefab;
    [SerializeField] private WindowCharacter_Portrait windowCharacterPortrait;
    public static Action<Item> UseItemAction;
    public static Action<Item> TryToBuyItemAction;
    private InventoryObject inventoryObject;
    // public event EventHandler OnItemListChanged;
    //private ItemObject itemObject;
    public Transform player;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;

    [Header("Pools")]
    private static ObjectPoolerGeneric pooler;
    List<GameObject> slotsToRelease;
    List<GameObject> itemsToRelease;
    private void Awake()
    {
      //  AwakeSlots();
        AwakePools();
    }
    private void Start()
    {

       // ShopDisplay();

    }
    private void AwakePools()
    {
        pooler = ObjectPoolerGeneric.Instance;

    }
    private void TryToBuyItem(Item item)
    {
        TryToBuyItemAction(item);
    }


   /* private void Start()
    {
         //  ShopDisplay();
    }*/


    void ClearInventoryDisplay()
    {
        GameLog.LogMessage(" Inventory Object present");
        int x = 0;

        foreach (Transform child in itemSlotContainer)
        {
            //  GameLog.LogMessage("Widze " + x + " child" + child.name);
            x++;
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
        foreach (GameObject slot in slotsToRelease)
        {
            pooler.ReleaseToPool("InventorySlots", slot);
        }
        slotsToRelease.Clear();
        foreach (GameObject item in itemsToRelease)
        {
            pooler.ReleaseToPool("UIItems", item);
        }
        itemsToRelease.Clear();
    }
}
