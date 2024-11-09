using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventoryRoomInventoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject itemInventoryPrefab;
    [SerializeField] private GameObject itemShown;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private Transform itemSlot;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text parameters;
    private Item newItem;
    private InventorySlot slot;
    [SerializeField] private InventoryObject wardrobeInventory;
    [SerializeField] private InventoryObject playerInventory;

    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
   // private int currentIndex;
    public static Action<Item> UseItemAction;
    private InventoryObject inventoryObject;
    InventorySlot currentSlot;
    public GameObject effect;
    UnitAudioBehaviour audioBehaviour;
    UnitDialogBehaviour dialog;
    public Transform player;
    private StringBuilder sb;
    Dictionary<string, string> atributesNameToIcon = new Dictionary<string, string>();
    public Image wardrobeItemImage;
    public TMP_Text wardrobeLetterText;
    //public ParticleSystem pickupEffect;

    [SerializeField] private GameObject buttons;

    [SerializeField] private GameObject TakeOffButton;
    [SerializeField] private GameObject TryButton;

    [SerializeField] private CharacterEquipment equipment;
    private void Awake()
    {
        sb = new StringBuilder();
        dialog = GetComponent<UnitDialogBehaviour>();
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
        InventoryPanelController.OnPanelClicked += Inventory_OnPanelClicked;
       // currentIndex = 0;
        HideAll();
        if (!wardrobeInventory)
            wardrobeInventory = PickupItemAssets.Instance.inventoryDatabase.GetInventoryObjectFromName("Wardrobe");
        if (!playerInventory)
            playerInventory = PickupItemAssets.Instance.inventoryDatabase.GetInventoryObjectFromName("ScampInventory");

        atributesNameToIcon.Add("Power", "<sprite name=Sword>");
        atributesNameToIcon.Add("Health", "<sprite name=Heart>");
        atributesNameToIcon.Add("Intelect", "<sprite name=Intelect>");
        atributesNameToIcon.Add("Agility", "<sprite name=Jump>");
        atributesNameToIcon.Add("Speed", "<sprite name=Run>");
        atributesNameToIcon.Add("Protection", "<sprite name=Shield>");
    }
    private void HideAll()
    {
        SetItemSlotsArrows(false);
        SetButtons(false);
        SetParameters(false);
        itemName.gameObject.SetActive(false);
    }
    private void SetItemSlotsArrows(bool set)
    {
        leftArrow.SetActive(set);
        rightArrow.SetActive(set);
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
    private void Inventory_OnPanelClicked(object sender, System.EventArgs e)
    {
        SetInventory((InventoryObject)sender);
    }
    public void SetInventory(InventoryObject inventory)
    {
        GameLog.LogMessage("Set inventory OnItemListChanged");
        this.inventoryObject = inventory;
        UpdateDisplay();
    }
    private void OnDisable()
    {
        InventoryPanelController.OnPanelClicked -= Inventory_OnPanelClicked;
    }
    private void UpdateDisplay()
    {
        ClearInventoryDisplay();

        slot = inventoryObject.inventory.GetFirstSlotFromArray();
        HideAll();
        DisplaySlot(slot);
        tweenItem();
    }
    public void TakeNextSlot()
    {
        ClearInventoryDisplay();
        slot = inventoryObject.inventory.GetNextSlotFromArray();
        DisplaySlot(slot);
        tweenItem();
    }
    public void TakePreviousSlot()
    {
        ClearInventoryDisplay();
        //currentIndex = currentIndex - 1;
        slot = inventoryObject.inventory.GetPreviousSlotFromArray();
        DisplaySlot(slot);
        tweenItem();
    }




    void DisplaySlot(InventorySlot slot)
    {
        GameLog.LogMessage("Update Inventory Display");

        if (inventoryObject)
        {
            currentSlot = slot;
            newItem = slot.GetItem();
            GameLog.LogMessage("Item in SLOT " + newItem);
            if (newItem != null && !newItem.Name.Equals("None") && newItem.Id != -1 && !newItem.Name.Equals("") && !(newItem.Id == 0 && newItem.Type == ItemObjectType.Coin))
            {
                ShowSlotSteerings();

                var newItemObject = newItem.GetItemObject();
                if (newItemObject != null)
                {
                    GameLog.LogMessage("New item object from item:" + newItemObject);
                    SetupItemSlot(newItemObject);
                    GameObject itemToinstantiate;
                    if (newItemObject.itemPrefab)
                    {
                        itemToinstantiate = newItemObject.itemPrefab;
                        GameObject obj = Instantiate(itemToinstantiate, itemContainer);
                        obj.GetComponent<SpriteRenderer>().size = new Vector2(2, 2);
                        itemShown = obj;
                        tweenItem();
                    }
                    else
                    {
                        itemToinstantiate = itemInventoryPrefab;
                        if (itemToinstantiate)
                        {
                            GameLog.LogMessage("Instantiate:" + itemToinstantiate);
                            var obj = Instantiate(itemToinstantiate, itemContainer);
                            obj.transform.SetParent(itemContainer, false);
                            obj.transform.SetAsLastSibling();
                            obj.name = "Item";
                            RectTransform rt = obj.transform.GetComponent<RectTransform>();
                            rt.sizeDelta = new Vector2(1, 1);
                            obj.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                            itemShown = obj;
                            UI_ItemMain uiItem = obj.GetComponent<UI_ItemMain>();
                            uiItem.SetParentSlot(itemContainer);
                            uiItem.SetItem(newItem);
                            uiItem.Show();
                        }
                    }
                }
            }
            else
            {

                AllSlotsEmptyCheck();
            }
        }
    }
    public void TryToBuy()
    {
        bool success = DescriptionDisplay.TryToBuyItem(newItem);
        if (success)
            PlayEffect();
    }


    public void TryToEquip()
    {
        if (newItem != null)
        {
            ItemObject _itemObject = newItem.GetItemObject();


            switch (newItem.GetEquipSlot())
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
            equipment.TryEquipItem(newItem.GetEquipSlot(), newItem);
            TryButton.SetActive(false);
            TakeOffButton.SetActive(true);



        }
    }

    public void TakeOff()
    {
        equipment.removeHelmetItem();
        equipment.removeArmourItem();
        equipment.removeWeaponItem();
        TakeOffButton.SetActive(false);
        TryButton.SetActive(true);

    }


    void AllSlotsEmptyCheck()
    {
        ClearInventoryDisplay();

        bool isAllSlotsEmpty = inventoryObject.inventory.CheckAllInventorySlotsEmpty();
        GameLog.LogMessage("All Slots empty ?" + isAllSlotsEmpty);
        if (!isAllSlotsEmpty)
        {
            TakeNextSlot();
        }
        else
        {
            HideAll();
        }
    }


    private void OnApplicationQuit()
    {
        //reset wardrobe

        wardrobeInventory.Clear();
    }
    public void MoveToInventory()
    {
        bool success = playerInventory.AddItem(newItem, 1);

        if (success)
        {
            ObjectPoolerGeneric.Instance.SpawnFromPool("PickUpEffect", player.position, Quaternion.identity, null);
            Item itemToRemove = newItem;
            InventorySlot slotToremove = currentSlot;
            TakeNextSlot();
            inventoryObject.RemoveItem(slotToremove, itemToRemove);
            AllSlotsEmptyCheck();

        }
        else
        {
            if (dialog)
                dialog.PlayFullBag();
            GameLog.LogMessage("Hide to inventory  failed!!!! no more space left in inventory");
        }

    }

    IEnumerator PlayEffectRoutine()
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        effect.SetActive(false);

    }


    void PlayEffect()
    {
        audioBehaviour.PlaySuccess();
        StartCoroutine(PlayEffectRoutine());


    }


    void PlayNotAllowed()
    {
        audioBehaviour.PlayRandomSound();
       // StartCoroutine(PlayEffectRoutine());


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

        bool success = wardrobeInventory.AddItem(newItem, 1);

        if (success)
        {
            StartCoroutine(OpenWardrobeRoutine());
            ObjectPoolerGeneric.Instance.SpawnFromPool("PickUpEffect", transform.position, Quaternion.identity, null);
            Item itemToRemove = newItem;
            InventorySlot slotToremove = currentSlot;
            TakeNextSlot();
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
            else {
                wardrobeLetterText.gameObject.SetActive(false);
                wardrobeItemImage.sprite = _itemObject.itemSprite;
                wardrobeItemImage.gameObject.SetActive(true);

            }
            
        }
    }
    void SetupItemSlot(ItemObject _itemObject)
    {
        GameLog.LogMessage("SetupItemSlot:" + _itemObject.name);
        itemName.text = _itemObject.displayName;
        StartCoroutine(ShowWardrobeItemRoutine(_itemObject));

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
        sb.Append("<sprite name=Coin> ");
        sb.Append(_itemObject.price);
        //descriptionText += "<sprite name=Coin> " + itemObject.price;
        parameters.text = sb.ToString();

    }
    private void ClearInventoryDisplay()
    {
        HideAll();
        wardrobeItemImage.gameObject.SetActive(false);

        GameLog.LogMessage("ClearInventoryDisplay entered");
        if (itemContainer)
            foreach (Transform _item in itemContainer)
            {
                GameLog.LogMessage("element name" + _item.name);
                Destroy(_item.gameObject);
            }
    }
    private void tweenItem()
    {
        if (itemShown)
        {
            LeanTween.cancel(itemShown);
            LeanTween.moveY(itemShown, itemShown.transform.position.y + 1f, 1f).setEaseInOutSine().setLoopPingPong();
        }
    }
}
