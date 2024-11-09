using DanielLochner.Assets.SimpleScrollSnap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
using System.Linq;
using UnityEngine.Pool;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class InventoryDisplayShop : MonoBehaviour
{
    [SerializeField] private GameObject itemInventoryPrefab;
    [SerializeField] private InventoryObject wardrobeInventory;
    [SerializeField] private InventoryObject playerInventory;
    // public static Action<Item> UseItemAction;
    public InventoryObject inventoryObject;
    public DescriptionDisplay displayItem;
    public Transform itemSlotContainer;
    // List<Transform> clickedItems = new List<Transform>();
    Transform clickedItem;
    public float smoothTime = 0.3F;
    List<GameObject> itemsToRelease;
    private ObjectPoolerGeneric pooler;
    
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text parameters;
    [SerializeField] private TMP_Text price;
    private StringBuilder sb;
    Dictionary<string, string> atributesNameToIcon = new Dictionary<string, string>();
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject buttons;

    [SerializeField] private GameObject itemShown;

    private Item itemToBuy;
    [SerializeField] private GameObject SwipeControllerGO;
    public GameObject effect;
    UnitAudioBehaviour audioBehaviour;

    public static InventoryDisplayShop Instance { get; private set; }
    public GameObject scrollView;
    public bool showTween = false;

    [SerializeField] private GameObject TakeOffButton;
    [SerializeField] private GameObject TryButton;
    [SerializeField] private GameObject BuyButton;

    [SerializeField] private CharacterEquipment equipment;
    public Image wardrobeItemImage;
    public TMP_Text wardrobeLetterText;
    InventorySlot currentSlot;

    void SetupItemSlot(ItemObject _itemObject)
    {
      
        if (!_itemObject)
            return;

        SetButtons(true);
        StartCoroutine(ShowWardrobeItemRoutine(_itemObject));

        if (inventoryObject.Equals(playerInventory))
        {
            if (_itemObject.type == ItemObjectType.Equipment)
            {

                // SetButtons(false);
                BuyButton.SetActive(false);
                TryButton.SetActive(true);
            }
        }
        else {
            BuyButton.SetActive(true);
            TryButton.SetActive(false);
            //SetButtons(true);


        }

            GameLog.LogMessage("SetupItemSlot:" + _itemObject.name);
        itemName.text = _itemObject.displayName;
        //StartCoroutine(ShowWardrobeItemRoutine(_itemObject));
        price.text= "<sprite name=Coin> "+ _itemObject.price;
        SetPrice(true);
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

                    sb.Append(atributesNameToIcon[_itemObject?.buffs[i]?.attribute.ToString()]);
                    sb.Append(" ");
                    sb.Append(value);
                    sb.Append("\n");

                    // descriptionText += atributesNameToIcon[item.buffs[i].attribute.ToString()] + " " + value + "\n";
                }
            }
        }
       
        //descriptionText += "<sprite name=Coin> " + itemObject.price;
        parameters.text = sb.ToString();
       

    }
    IEnumerator OpenWardrobeRoutine()
    {
        InventoryPanelController.OpenWardrobe();
        yield return new WaitForSeconds(0.5f);
        InventoryPanelController.CloseWardrobe();
    }

    public void HideToWardrobe()
    {

        //added for test
        StopAllCoroutines();

        bool success = wardrobeInventory.AddItem(itemToBuy, 1);

        if (success)
        {
            StartCoroutine(OpenWardrobeRoutine());
            ObjectPoolerGeneric.Instance.SpawnFromPool("PickUpEffect", transform.position, Quaternion.identity, null);
            Item itemToRemove = itemToBuy;
            InventorySlot slotToremove = currentSlot;
            UpdateShopDisplay();    
            inventoryObject.RemoveItem(slotToremove, itemToRemove);
            AllSlotsEmptyCheck();


        }
        else
        {
            GameLog.LogMessage("HideTowardrobe failed!!!! no more space left in wardrobe");
        }
    }


    IEnumerator ShowWardrobeItemRoutine(ItemObject _itemObject)
    {
        if (inventoryObject.name.Equals("Wardrobe"))
        {
            yield return new WaitForSeconds(0.5f);

            if (_itemObject.type == ItemObjectType.Letter)
            {

                wardrobeItemImage.gameObject.SetActive(false);
                wardrobeLetterText.text = ((LetterObject)_itemObject).text;
                wardrobeLetterText.gameObject.SetActive(true);

            }
            else
            {
                wardrobeLetterText.gameObject.SetActive(false);
                wardrobeItemImage.sprite = _itemObject.itemSprite;
                wardrobeItemImage.gameObject.SetActive(true);

            }

        }
    }
    public void HideShop()
    {

        if (gameObject.activeSelf)
            gameObject.SetActive(false);
        displayItem?.Hide();

    }

    public void DisplayShop()
    {

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);


    }
    private void HideAll()
    {
        GameLog.LogMessage("hide all enetered");
        SetItemSlotsArrows(false);
        SetButtons(false);
        SetParameters(false);
        SetPrice(false);
        itemName.gameObject.SetActive(false);
        
    }
    private void SetItemSlotsArrows(bool set)
    {
        leftArrow.SetActive(set);
        rightArrow.SetActive(set);
    }

    private void SetPrice(bool set)
    {
        price.gameObject.SetActive(set);
        
    }
    private void SetButtons(bool set)
    {
        buttons.SetActive(set);
    }
    private void SetParameters(bool set)
    {
        parameters.gameObject.SetActive(set);
    }
    void ShowSlotSteerings()
    {
        GameLog.LogMessage("ShowSlotSteerings entered");
        SetParameters(true);
        SetItemSlotsArrows(true);
        SetButtons(true);
        itemName.gameObject.SetActive(true);


    }


    private void OnEnable()
    {
        SwipeController.OnSwipeNext += OnSwipeUpdateNext;
        SwipeController.OnSwipePrevious += OnSwipeUpdatePrevious;
    }



    private void UpdateSlot(int currentPage) 
    {
        // int currentPage = controller.GetCurrentPage();

       if(!inventoryObject)
            return;
        currentSlot = inventoryObject?.inventory.GetInventorySlotArray()[currentPage - 1];
        Item item = currentSlot.GetItem();

        itemToBuy = item;

        var newItemObject = item.GetItemObject();
        if (newItemObject != null)
        {
            SetupItemSlot(newItemObject);
        }
        
       /* if(showTween)
            tweenItem(SwipeControllerGO.GetComponent<SwipeController>().GetLevelPages().GetChild(currentPage - 1).gameObject);
*/
    }

    public void TryToBuy()
    {
        bool success = DescriptionDisplay.TryToBuyItem(itemToBuy);
        if (success)
            PlayEffect();
    }

    IEnumerator PlayEffectRoutine()
    {
        effect?.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        effect?.SetActive(false);

    }


    void PlayEffect()
    {
        audioBehaviour?.PlaySuccess();
        //StartCoroutine(PlayEffectRoutine());


    }


    private void OnSwipeUpdatePrevious(object sender, EventArgs e)
    {
        UpdateSlot(((SwipeController)sender).GetCurrentPage());
       
    }

    private void OnSwipeUpdateNext(object sender, EventArgs e)
    {
        UpdateSlot(((SwipeController)sender).GetCurrentPage());
    }

    private void OnDisable()
    {
        SwipeController.OnSwipeNext -= OnSwipeUpdateNext;
        SwipeController.OnSwipePrevious -= OnSwipeUpdatePrevious;
    }
    private void Awake()
    {
        sb = new StringBuilder();
       // DescriptionDisplay.ItemBoughtAction += ItemBought;
        SetInventory(inventoryObject);
        if (itemsToRelease == null)
            itemsToRelease = new List<GameObject>();
        pooler = ObjectPoolerGeneric.Instance;
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
        atributesNameToIcon.Add("Power", "<sprite name=Sword>");
        atributesNameToIcon.Add("Health", "<sprite name=Heart>");
        atributesNameToIcon.Add("Intelect", "<sprite name=Intelect>");
        atributesNameToIcon.Add("Agility", "<sprite name=Jump>");
        atributesNameToIcon.Add("Speed", "<sprite name=Run>");
        atributesNameToIcon.Add("Protection", "<sprite name=Shield>");
    }


    public void TryToEquip()
    {
        if (itemToBuy != null)
        {
            ItemObject _itemObject = itemToBuy.GetItemObject();


            switch (itemToBuy.GetEquipSlot())
            {

                case CharacterEquipment.EquipSlot.Helmet:
                    equipment.removeHelmetItem();
                    break;

                case CharacterEquipment.EquipSlot.Armour:
                    equipment.removeArmourItem();
                    break;
                case CharacterEquipment.EquipSlot.Weapon:


                    equipment.removeWeaponItem();
                    break;
                case CharacterEquipment.EquipSlot.Tool:

                    PlayNotAllowed();
                    return;


                case CharacterEquipment.EquipSlot.Default:
                    break;
                default:

                    break;
            }
            equipment.TryEquipItem(itemToBuy.GetEquipSlot(), itemToBuy);
            TryButton.SetActive(false);
            TakeOffButton.SetActive(true);



        }
    }


    void PlayNotAllowed()
    {
        audioBehaviour.PlayRandomSound();
        // StartCoroutine(PlayEffectRoutine());


    }
    public void TakeOff()
    {
        equipment.removeHelmetItem();
        equipment.removeArmourItem();
        equipment.removeWeaponItem();
        TakeOffButton.SetActive(false);
        TryButton.SetActive(true);

    }
    private void ClearInventoryDisplay()
    {
       wardrobeItemImage?.gameObject.SetActive(false);

        GameLog.LogMessage("ClearInventoryDisplay entered");
        if (itemSlotContainer)
            foreach (Transform _item in itemSlotContainer)
            {
                GameLog.LogMessage("element name" + _item.name);
                DestroyImmediate(_item.gameObject);
            }
    }

    void AllSlotsEmptyCheck()
    {
      if(!inventoryObject)
            return;

        bool isAllSlotsEmpty = inventoryObject.inventory.CheckAllInventorySlotsEmpty();
        GameLog.LogMessage("All Slots empty ?" + isAllSlotsEmpty);
        if (!isAllSlotsEmpty)
        {
            HideAll();
            ClearInventoryDisplay();
        }
    }

    private void ItemBought()
    {
        audioBehaviour?.PlaySuccess();
        HideShop();
    }

    private void tweenItem(GameObject item)
    {
        if (item)
        {
            LeanTween.cancel(item);
            float from = item.transform.position.y;
            float to = item.transform.position.y+25f;

            LeanTween.moveY(item, to, 1f).setEaseInOutSine().setLoopPingPong();
            /*LeanTween.value(item, from, to, 1f).setEaseInOutSine().setLoopPingPong().setOnUpdate((to) => {
                
                var pos = transform.position;
                var newY = item.transform.position.y + to;
                transform.position = new Vector3(pos.x, newY, pos.z);
            });
*/

        }
    }
    public void SetInventory(InventoryObject inventory)
    {
        GameLog.LogMessage("Set inventory OnItemListChanged");
        this.inventoryObject = inventory;
        //ClearInventoryDisplay();
        HideAll();  
        if (!pooler)
            pooler = ObjectPoolerGeneric.Instance;
        
        
       // SwipeControllerGO.SetActive(false);
      //  SwipeControllerGO.SetActive(true);

        UpdateShopDisplay();
        
        UpdateSlot(1);
        SwipeControllerGO.GetComponent<SwipeController>().ResetCurrentPage();
       // if (itemSlotContainer.childCount>0)
          

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


    void ClearShopDisplay()
    {

        foreach (Transform child in itemSlotContainer)
        {
           /* if (child.name.Contains("Item"))
                if (itemsToRelease != null)
                    itemsToRelease.Add(child.gameObject);

                else*/
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
            else
            {
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


                
                if(slot.ID==-1)
                    continue;

                var newItemObject = slot.GetItem().GetItemObject();

                GameObject obj = null;

               // obj = pooler.SpawnFromPool("ShopItemsScene", itemSlotContainer.position, itemSlotContainer.rotation, itemSlotContainer);
                obj = Instantiate(itemInventoryPrefab, itemSlotContainer);

               // SetupItemSlot(newItemObject);
                //  var obj = pooler.SpawnFromPool("ShopItemsScene", itemSlotContainer.position, itemSlotContainer.rotation, itemSlotContainer);
              //  itemShown = obj;
               if(showTween)
                 tweenItem(obj);
                // GameObject obj = Instantiate(itemInventoryPrefab, itemSlotContainer).GetComponent<RectTransform>();

                obj.transform.name = "Item";
                obj.transform.SetAsLastSibling();
                RectTransform itemShopRectTransform = obj.GetComponent<RectTransform>();
                // itemShopRectTransform.gameObject.SetActive(true);
                Item item = slot.GetItem();
                if (item.Id != -1 && item.Type != ItemObjectType.None)
                {
                    ShowSlotSteerings();
                   
                    item.SetLocation(ItemLocation.Shop);
                    int amount = slot.amount;
                    UI_InventoryItem uiItem = obj.GetComponent<UI_InventoryItem>();
                    
                    if (uiItem != null) { 
                        uiItem.SetAmountText(amount);
                        uiItem.SetItem(item);
                        uiItem.SetLocation(ItemLocation.Shop);
                        uiItem.Show();
                        /*uiItem.GetComponent<Button_UI>().ClickFunc = () =>
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

                        };*/
                    }
                }
            }
        }
    }
    void OnDestroy()
    {
       // DescriptionDisplay.ItemBoughtAction -= ItemBought;
      /*  if (inventoryObject)
            inventoryObject.OnItemListChanged -= Inventory_OnItemListChanged;*/
    }
}
