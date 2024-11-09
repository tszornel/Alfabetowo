using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LetterInventoryDisplay : MonoBehaviour
{
    // Inventory inventory;
    [SerializeField] private Transform pfUI_LetterItem;
    public InventoryObject newInventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
   // public event EventHandler OnItemListChanged;
    // private Player player;
    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        itemSlotTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        SetInventory(newInventory);
    }
    public void SetInventory(InventoryObject inventory)
    {
       // this.newInventory = inventory;
        newInventory.OnItemListChanged += Inventory_OnItemListChanged;
        UpdateInventory();
    }
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        UpdateInventory();
    }
    private void Update()
    {
       // UpdateInventory();
    }
    private void UpdateInventory()
    {
       /* foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 100f;
        foreach (InventorySlot inventorySlot in newInventory.inventory.items)
        {
            GameLog.LogMessage("test update");
            ItemObject item = inventorySlot.item;
            int amount = inventorySlot.amount;
            if (item.type == ItemObjectType.Letter)
            {
                LetterObject letterItem = (LetterObject)item;
                string letterText = letterItem.GetText();
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
                if (!inventorySlot.IsEmpty())
                {
                    // Not Empty, has Item
                    Transform uiItemTransform = Instantiate(pfUI_LetterItem, itemSlotTemplate);
                    uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                    UI_letterDrag uiItem = uiItemTransform.GetComponent<UI_letterDrag>();
                    //uiItem.SetItem(letterItem);
                    uiItem.Show(letterItem);
                    //uiItem.SetLetterText(((LetterObject)item).GetText());
                }
            }
        }*/
    }
    private void DisplayInventory()
    {
        /*foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 54f;
        foreach (InventorySlot inventorySlot in newInventory.inventory.items)
        {
            GameLog.LogMessage("test display");
            ItemObject item = inventorySlot.item;
            int amount = inventorySlot.amount;
            if (item.type == ItemObjectType.Letter)
            {
                LetterObject letterItem = (LetterObject)item;
                string letterText = letterItem.GetText();
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);
                if (!inventorySlot.IsEmpty())
                {
                    // Not Empty, has Item
                    Transform uiItemTransform = Instantiate(pfUI_LetterItem, itemSlotTemplate);
                    uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                    UI_letterDrag uiItem = uiItemTransform.GetComponent<UI_letterDrag>();
                    //uiItem.SetItem((LetterObject)item);
                    uiItem.Show(letterItem);
                }
            }
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        */
    }
}