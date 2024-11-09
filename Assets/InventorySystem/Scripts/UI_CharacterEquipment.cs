using UnityEngine;



public class UI_CharacterEquipment : MonoBehaviour
{
    [SerializeField] private Transform pfUI_Item;
    [SerializeField] private UI_CharacterEquipmentSlot helmetSlot;
    [SerializeField] private UI_CharacterEquipmentSlot armorSlot;
    [SerializeField] private UI_CharacterEquipmentSlot weaponSlot;
    private CharacterEquipment characterEquipment;
    private bool initializedSlots=false;
    public Transform player;

    private void Awake()
    {
        if (!initializedSlots) { 
            InitializeSlots();
        }
        UpdateVisual();
    }
    void InitializeSlots()
    {
        initializedSlots = true;
        if (armorSlot == null)
            armorSlot = transform.Find("armorSlot").GetComponent<UI_CharacterEquipmentSlot>();
        if (helmetSlot == null)
            helmetSlot = transform.Find("helmetSlot").GetComponent<UI_CharacterEquipmentSlot>();
        if (weaponSlot == null)
            weaponSlot = transform.Find("weaponSlot").GetComponent<UI_CharacterEquipmentSlot>();
        helmetSlot.OnItemDropped += HelmetSlot_OnItemDropped;
        armorSlot.OnItemDropped += ArmorSlot_OnItemDropped;
        weaponSlot.OnItemDropped += WeaponSlot_OnItemDropped;

        
    }
    private void OnDestroy()
    {
        weaponSlot.OnItemDropped -= WeaponSlot_OnItemDropped;
        helmetSlot.OnItemDropped -= HelmetSlot_OnItemDropped;
        armorSlot.OnItemDropped -= ArmorSlot_OnItemDropped;
    }

    
    
    private void ArmorSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in Armor slot
        if (e.item != null)
        {
         

            UI_itemTouch.Instance.touchItemDropedinEquipment = true;
            
            characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Armour, e.item);
        }
        else
        {
            GameLog.LogMessage("Item dropped is null ??");
        }
    }
    private void HelmetSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in Helmet slot
        if (e.item != null)
        {
           



            GameLog.LogMessage("Item droped" + e.item);
            UI_itemTouch.Instance.touchItemDropedinEquipment = true;
           
            characterEquipment.TryEquipItem(CharacterEquipment.EquipSlot.Helmet, e.item);
        }
        else
        {
            GameLog.LogMessage("Item dropped is null ??");
        }
    }
    private void WeaponSlot_OnItemDropped(object sender, UI_CharacterEquipmentSlot.OnItemDroppedEventArgs e)
    {
        // Item dropped in weapon slot
        if (e.item != null)
        {
            UI_itemTouch.Instance.touchItemDropedinEquipment = true;
            GameLog.LogMessage("Item droped" + e.item);
           
            characterEquipment.TryEquipItem(e.item.GetEquipSlot(), e.item);
        }
        else
        {
            GameLog.LogMessage("Item dropped is null ??");
        }
    }
    public void SetCharacterEquipment(CharacterEquipment characterEquipment)
    {
        this.characterEquipment = characterEquipment;
        characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChanged;
    }
    public void UnSetCharacterEquipment()
    {
            characterEquipment.OnEquipmentChanged -= CharacterEquipment_OnEquipmentChanged;
    }
    private void CharacterEquipment_OnEquipmentChanged(object sender, System.EventArgs e)
    {
        GameLog.LogMessage("On equipment changed !!!!!!!!!!!");
        if (!initializedSlots) { InitializeSlots(); }
        
        UpdateVisual();
    }
    private void ShowEquipmentItem(Item item)
    {
        GameLog.LogMessage("visualize weapon item weapon ID=" + item.Id);

        UI_CharacterEquipmentSlot slot= null;       
        switch (item.GetEquipSlot())
        {
            case CharacterEquipment.EquipSlot.None:
                break;
            case CharacterEquipment.EquipSlot.Letter:
                break;
            case CharacterEquipment.EquipSlot.Helmet:
                slot = helmetSlot;
                break;
            case CharacterEquipment.EquipSlot.Armour:
                slot = armorSlot;
                break;
            case CharacterEquipment.EquipSlot.Weapon:
                slot = weaponSlot;      
                break;
            case CharacterEquipment.EquipSlot.Repair:
                break;
            case CharacterEquipment.EquipSlot.Tool:
                slot = weaponSlot;
                break;
            case CharacterEquipment.EquipSlot.Default:
                slot = weaponSlot;
                break;
            default:
                slot = weaponSlot;
                break;
        }

        VisualizeSlot(slot, item);



    }

    private void VisualizeSlot(UI_CharacterEquipmentSlot _slot,Item item) {

        if (_slot == null) {

            GameLog.LogMessage("Equipment slot is null for this item :" + item);
            return;
        }
           
        if (item.Id != 0 && item.Id != -1)
        {
            GameLog.LogMessage("equipment item id from inside" + item.Id);
            GameObject uiItemObject = ObjectPoolerGeneric.Instance.SpawnFromPool("UIItems", _slot.transform.position, Quaternion.identity, transform);
            Transform uiItemTransform = uiItemObject.transform;
            uiItemTransform.SetAsLastSibling();
            uiItemTransform.GetComponent<RectTransform>().anchoredPosition = _slot.GetComponent<RectTransform>().anchoredPosition;
            uiItemTransform.localScale = Vector3.one * 1.5f;
            uiItemTransform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            UI_ItemMain uiItem = uiItemTransform.GetComponent<UI_ItemMain>();
            uiItem.SetItem(item);
            uiItem.SetLocation(ItemLocation.Equipment);
            if (_slot)
            {
                _slot.transform.Find("emptyImage").gameObject.SetActive(false);
               
            }
        }

    }
    private void UpdateVisual()
    {
        GameLog.LogMessage("Update equipment visual");
        foreach (Transform child in transform)
        {
            if (weaponSlot && armorSlot && helmetSlot)
                if (child != weaponSlot.transform && child != armorSlot.transform && child != helmetSlot.transform)
                    if (child.gameObject.activeSelf)
                        ObjectPoolerGeneric.Instance.ReleaseToPool("UIItems", child.gameObject);
        }
        weaponSlot.transform.Find("emptyImage").gameObject.SetActive(true);
        Item weaponItem = characterEquipment.GetWeaponItem();
        if (weaponItem != null)
        {
            ShowEquipmentItem(weaponItem);
        }

        
        if (armorSlot)
        {
            armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            Item armorItem = characterEquipment.GetArmorItem();

            if (armorItem != null)
            {
                ShowEquipmentItem(armorItem);
            }
        }
        if (helmetSlot)
        {
            helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            Item helmetItem = characterEquipment.GetHelmetItem();
            if (helmetItem != null)
            {
                ShowEquipmentItem(helmetItem);
            }
        }
    }
}
