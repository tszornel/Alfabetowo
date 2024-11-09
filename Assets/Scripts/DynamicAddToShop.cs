using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DynamicAddToShop : MonoBehaviour
{
         #region Fields
         [SerializeField] private GameObject panelPrefab;
         [SerializeField] private GameObject itemPrefab;
        //[SerializeField] private ToggleGroup toggleGroup;
         [SerializeField] private SimpleScrollSnap scrollSnap;
         [SerializeField] private Toggle togglePrefab;
         [SerializeField] private ToggleGroup toggleGroup;
    public static Action<Item> UseItemAction;
        public InventoryObject inventoryObject;
    private float toggleWidth;
    #endregion
    #region Methods
    private void Awake()
        {
            toggleWidth = (itemPrefab.transform as RectTransform).sizeDelta.x * (Screen.width / 2048f); ;
        }
    private void Start()
    {
        UpdateShopDisplay();
    }
    private void UpdateShopDisplay()
    {
        for (int i = 0; i < inventoryObject.inventory.GetInventorySlotArray().Length; i++)
        {
            InventorySlot slot = inventoryObject.inventory.GetInventorySlotArray()[i];
            Item item = slot.GetItem();
            int amount = slot.amount;
           // UI_ItemMain uiItem = itemPrefab.GetComponent<UI_ItemMain>();
          //  uiItem.SetAmountText(amount);
          //  uiItem.SetItem(item);
          //  uiItem.SetLocation(ItemLocation.Shop);
            AddToFront(panelPrefab);
            // uiItem.Show();
        }
    }
    public void Add(GameObject itemPrefab,int index)
        {
        // Pagination
        //  var obj = Instantiate(itemPrefab, scrollSnap.Pagination.transform.position + new Vector3(toggleWidth * (scrollSnap.NumberOfPanels + 1), 0, 0), Quaternion.identity, scrollSnap.Pagination.transform);
        // toggle.group = toggleGroup;
        // Pagination
        Toggle toggle = Instantiate(togglePrefab, scrollSnap.Pagination.transform.position + new Vector3(toggleWidth * (scrollSnap.NumberOfPanels + 1), 0, 0), Quaternion.identity, scrollSnap.Pagination.transform);
        toggle.group = toggleGroup;
        scrollSnap.Pagination.transform.position -= new Vector3(toggleWidth / 2f, 0, 0);
        //scrollSnap.Pagination.transform.position -= new Vector3(toggleWidth / 2f, 0, 0);
        // Panel
        // panelPrefab.GetComponent<Image>().color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        // itemPrefab.GetComponent<Image>().color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        scrollSnap.Add(itemPrefab, index);
        }
        public void  AddToFront(GameObject itemPrefab)
        {
            Add(itemPrefab, 0);
        }
      /*  public void AddToBack()
        {
            Add(scrollSnap.NumberOfPanels);
        }*/
        public void Remove(int index)
        {
            if (scrollSnap.NumberOfPanels > 0)
            {
                // Pagination
                DestroyImmediate(scrollSnap.Pagination.transform.GetChild(scrollSnap.NumberOfPanels - 1).gameObject);
                scrollSnap.Pagination.transform.position += new Vector3(toggleWidth / 2f, 0, 0);
                // Panel
                scrollSnap.Remove(index);
            }
        }
        public void RemoveFromFront()
        {
            Remove(0);
        }
        public void RemoveFromBack()
        {
            if (scrollSnap.NumberOfPanels > 0)
            {
                Remove(scrollSnap.NumberOfPanels - 1);
            }
            else
            {
                Remove(0);
            }
        }
        #endregion
}