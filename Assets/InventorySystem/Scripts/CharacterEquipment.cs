using System;
using UnityEngine;


public class CharacterEquipment : MonoBehaviour, IItemHolderNew
{
 
    public delegate void EquipmentUpdated(Item updatedItem);
    public event EventHandler OnEquipmentChanged;
    public EquipmentUpdated OnEquipmentAdded;
    public EquipmentUpdated OnEquipmentRemoved;
    private Player player;
    public PlayerEquipmentData playerData;
    
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string  dialogName;
    private Dialog dialog;
    private bool dialogManager;

    public enum EquipSlot
    {
        None,
        Letter,
        Helmet,
        Armour,
        Weapon,
        Repair,
        Tool,
        Default
    }
    private void Awake()
    {
        player = GetComponent<Player>();
        SetWeaponItem(playerData.weaponItem,true);
        SetArmorItem(playerData.armorItem);
        SetHelmetItem(playerData.helmetItem);
        player.OnEquipmentUpdatePlayerStats(playerData);
        if (playerData.armorItem!=null || playerData.helmetItem!=null || playerData.weaponItem != null) { 
            OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        }

        dialogManager = DialogManager.Instance;
        if(dialogManager)
            dialog = DialogManager.Instance.GetDialog(dialogName);
    }
    public Item GetWeaponItem()
    {
        return playerData.weaponItem;
    }
    public Item GetHelmetItem()
    {
        return playerData.helmetItem;
    }
    public Item GetArmorItem()
    {
        return playerData.armorItem;
    }
    public void RemoveItem(Item item) {
        GameLog.LogMessage("Item Equip slot" + item.GetEquipSlot());
        item.SetItemHolder(null);
        switch (item.GetEquipSlot())
        {
            default:
                break;
            case EquipSlot.Armour: removeArmourItem(item); break;
            case EquipSlot.Helmet: removeHelmetItem(item); break;
            case EquipSlot.Weapon: removeWeaponItem(item); break;
            case EquipSlot.Tool: removeWeaponItem(item); break;
        }
    }
    public void removeWeaponItem(Item item) {
        GameLog.LogMessage("Remove weapon item");
        Item removedItem = item;//new Item (playerData.weaponItem);
        this.playerData.weaponItem = null;
        player.EquipWeapon_Punch();
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        if (OnEquipmentRemoved != null) { 
            GameLog.LogMessage("Invoke OnEquipmentRemoved !!!!");
            OnEquipmentRemoved?.Invoke(removedItem);
        }
    }


    public void removeWeaponItem() {

        removeWeaponItem(GetWeaponItem());


    }
    public void removeArmourItem(Item item)
    {
        Item removedItem = item;//new Item(playerData.armorItem);
        this.playerData.armorItem = null;
        player.EquipArmorNone();
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        if (OnEquipmentRemoved != null)
            OnEquipmentRemoved?.Invoke(removedItem);
    }


    public void removeArmourItem()
    {
        removeArmourItem(GetArmorItem());   
    }
    public void removeHelmetItem()
    {
        removeHelmetItem(GetHelmetItem());
    }

    public void removeHelmetItem(Item item)
    {
        Item removedItem = item;// new Item(playerData.helmetItem);
        this.playerData.helmetItem = null;
        player.EquipHelmetNone();
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        if (OnEquipmentRemoved != null)
            OnEquipmentRemoved?.Invoke(removedItem);
    }
    private void SetWeaponItem(Item weaponItem,bool initializing)
    {
        if(UI_itemTouch.Instance)
            UI_itemTouch.Instance.touchItemDropedinEquipment = true;
        GameLog.LogMessage("SetWeaponItem " + weaponItem + " initializng=" + initializing);
        if (weaponItem==null || weaponItem.Id == -1 || weaponItem.Id == 0)
            this.playerData.weaponItem = null;
        else {
            this.playerData.weaponItem = weaponItem;
            GameLog.LogMessage("set weapon item view on player"+ weaponItem.Id);
            if (!initializing) {
                GameLog.LogMessage("To nie jest inicjalizacja wiec skasuj item z inventory");
                
                GetComponent<PlayerPlatformerController>().GetInventory().RemoveItem(weaponItem);
            }
            else {


                GameLog.LogMessage("INICJALIZACJA wiec nie kasuj item z inventory");

            }
            player.SetEquipment(weaponItem.Id);
            if (weaponItem.GetItemHolder() != null)
                GameLog.LogMessage("Previous Item Holder" + weaponItem.GetItemHolder());
            weaponItem.Location = ItemLocation.Equipment;
            weaponItem.SetItemHolder(this);
            GameLog.LogMessage("Item Holder in Equipment:" + weaponItem.GetItemHolder());
            
            OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
            if (OnEquipmentAdded != null) {
                GameLog.LogMessage("On Equipment added!!!!!!!!!!!!!!!!!");
                OnEquipmentAdded?.Invoke(weaponItem);
            }
            if (UI_itemTouch.Instance)
                    UI_itemTouch.Instance.Hide();
        }
    }
    private void SetHelmetItem(Item helmetItem)
    {
        if (helmetItem==null||helmetItem.Id == -1 || helmetItem.Id == 0)
            this.playerData.helmetItem = null;
        else
        {
            this.playerData.helmetItem = helmetItem;
            player.SetEquipment(helmetItem.Id);
            player.transform.GetComponent<PlayerPlatformerController>().GetInventory().RemoveItem(helmetItem);
            OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
            if (OnEquipmentAdded != null)
                OnEquipmentAdded?.Invoke(helmetItem);
            if (UI_itemTouch.Instance)
                UI_itemTouch.Instance.Hide();
        }
        //
    }
    private void SetArmorItem(Item armorItem)
    {
        if (armorItem == null || armorItem.Id == -1 || armorItem.Id==0)
            this.playerData.armorItem = null;
        else { 
            this.playerData.armorItem = armorItem;
            player.SetEquipment(armorItem.Id);
            player.transform.GetComponent<PlayerPlatformerController>().GetInventory().RemoveItem(armorItem);
            OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
            if (OnEquipmentAdded != null)
                OnEquipmentAdded?.Invoke(armorItem);
            if (UI_itemTouch.Instance)
                UI_itemTouch.Instance.Hide();
        }
        //   
    }
    public void TryEquipItem(EquipSlot equipSlot, Item item)
    {

        if ((equipSlot == item.GetEquipSlot()) && !CheckSlotIsEmpty(equipSlot))
        {

            GetComponent<UnitDialogBehaviour>()?.PlayDoesNotWork();
            return;
        }
           
       

        GameLog.LogMessage("Try to equip slot" + item.Name + " id" + item.Id);
        if (equipSlot == item.GetEquipSlot())
        {
            // Item matches this EquipSlot
            switch (equipSlot)
            {
                default:
                    break;
                case EquipSlot.Armour: item.SetLocation(ItemLocation.Equipment); SetArmorItem(item); break;
                case EquipSlot.Helmet: item.SetLocation(ItemLocation.Equipment); SetHelmetItem(item); break;
                case EquipSlot.Weapon: item.SetLocation(ItemLocation.Equipment); SetWeaponItem(item, false); break;
                case EquipSlot.Tool: item.SetLocation(ItemLocation.Equipment); SetWeaponItem(item, false); break;

            }
        } else {

            GetComponent<UnitDialogBehaviour>()?.PlayDoesNotFitDialog();

        }
    }


    private bool CheckSlotIsEmpty(EquipSlot equipSlot)
    {

        switch (equipSlot)
        {
            default:
                return true;
             case EquipSlot.Armour:
                return GetArmorItem() != null ? false : true;
            case EquipSlot.Helmet:
                return GetHelmetItem() != null ? false : true;
            case EquipSlot.Weapon:
                return GetWeaponItem() != null ? false : true;
            case EquipSlot.Tool:
                return GetWeaponItem() != null ? false : true;
            case EquipSlot.Repair: PlayItemToRepaire(); return true;

        }
    }

        private void PlayItemToRepaire()
    {

        if(!dialog)
            dialog = DialogManager.Instance.GetDialog("ScampItDoesntWork");
        DialogManager.Instance.StartDialog(dialog);
    }

    void IItemHolderNew.RemoveItem(Item _item)
    {
        RemoveItem(_item);
      
    }

    void IItemHolderNew.AddItem(Item item)
    {
        throw new NotImplementedException();
    }

    bool IItemHolderNew.CanAddItem(Item item)
    {
        throw new NotImplementedException();
    }


    public void ResetEquipment() {
       
        
        playerData.Reset(this); 
        // removeArmourItem();
       // removeHelmetItem();
       // removeWeaponItem();

    }
}
