using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



[CreateAssetMenu(fileName = "New Inventory ", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, IItemHolderNew
{
    public event EventHandler OnItemListChanged;
    public string savePath;
    public ItemObjectDatabase database;
    public Inventory inventory;
 


    private void OnDestroy()
    {
         PickupItem.OnCollect -= OnPickupItemCollected;
    }

    public void SetListenerOnCollection() {

        PickupItem.OnCollect += this.OnPickupItemCollected;
    }

    public void SetListenerOnRemove()
    {
        PickupItem.OnCollect += this.OnPickupItemCollected;
    }


    public void SetListenerOnFurnaceCollected()
    {
        FurnaceController.OnDamagedItemCollected += this.OnDamagedItemCollected;
    }

    public void UnSetListenerOnFurnaceCollected()
    {

        FurnaceController.OnDamagedItemCollected -= this.OnDamagedItemCollected;
    }

    public void UnsetListenerOnCollection()
    {
        PickupItem.OnCollect -= this.OnPickupItemCollected;
    }

    bool OnPickupItemCollected(Item _item)
    {
        GameLog.LogMessage("OnPickupItemCollected:"+_item);
        bool success = AddItem(_item,1);
        return success;
    }


    bool OnDamagedItemCollected(Item _item)
    {
        GameLog.LogMessage("OnDamageItemCollectedbyFurnace:" + _item);
        RemoveItem(_item);
        return true;
    }

    public void ShowInventory() 
    {

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    public bool CheckItemInInventory(Item _item)
    {
        if(_item == null)
            return false;
        InventorySlot[] slots = inventory.GetInventorySlotArray();
        if (slots == null || slots.Length <= 0)
            return false;
        for (int i = 0; i < slots.Length; i++)
        {

            if(slots[i].GetItem() == null || (slots[i].GetItem()!=null && slots[i].GetItem().Id == -1))
                continue;
            if (slots[i].GetItem().Name.Equals(_item.Name))
                return true;
        }
        return false;
    }

    public void PrintInventory()
    {
        GameLog.LogMessage("Count:" + inventory.GetInventorySlotArray().Length);
        for (int i = 0; i < inventory.GetInventorySlotArray().Length; i++)
        {
            if (inventory.GetInventorySlotArray()[i].GetItem() != null)
              GameLog.LogMessage(inventory.GetInventorySlotArray()[i].GetItem().ToString());
        }
    }
    public void AddItem(Item _item, InventorySlot inventorySlot,int amount)
    {
        // Add Item to a specific Inventory slot
        //itemList.Add(item);
       
        //GameLog.LogMessage("Add item to droped slot");
        //inventorySlot.AddAmount(amount);
        int slotAmount = inventorySlot.GetAmount();
        slotAmount = amount;
        inventorySlot.UpdateSlot(_item.Id,_item, slotAmount);

        _item.SetItemHolder(this);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public void AddItem(Item _item, InventorySlot inventorySlot)
    {
        // Add Item to a specific Inventory slot
        //itemList.Add(item);
        //item.SetItemHolder(this);
        //GameLog.LogMessage("Add item to droped slot");
        //inventorySlot.AddAmount(amount);

        int slotAmount = inventorySlot.GetAmount();
        GameLog.LogMessage("Slot Amount from AddItem" + slotAmount);
       // if (slotAmount == 0)
        //    slotAmount = 1;
        inventorySlot.UpdateSlot(_item.Id, _item, slotAmount);
        _item.SetItemHolder(this);
        _item.SetSlotId(inventorySlot.ID);   
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public void RemoveItem(Item _item)
    {
        GameLog.LogMessage("removed item entered" + _item);

        _item?.SetItemHolder(null);
        if (_item.Location==ItemLocation.Inventory && _item.GetSlotId() != -1)
        {
            GameLog.LogMessage("Remove item from slot" + _item.GetSlotId() + " item" + _item);
            RemoveItem(inventory.GetInventorySlotArray()[_item.GetSlotId()], _item);
            return;
        }
        GameLog.LogMessage("removed item entered for loop" + _item);
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                GameLog.LogMessage("Slot id in for loop "+ inventory.slots[i].ID+" item.id="+_item.Id);
            if (inventory.slots[i].item !=null && inventory.slots[i].item.Equals(_item))
            {

                GameLog.LogMessage("Slot item ="+ inventory.slots[i].item);
                GameLog.LogMessage("_item to remove " + _item);
                int amount = inventory.slots[i].amount;
               
                if (amount > 1) 
                {
                    GameLog.LogMessage("Amount > 1"+ inventory.slots[i].amount);
                    inventory.slots[i].UpdateSlot(_item.Id, _item, --amount);
                }
                else 
                {
                    GameLog.LogMessage("Remove Item " + _item+" from inventory"+this.name,this);
                    inventory.slots[i].RemoveItem(); ;
                    _item.SetSlotId(-1);
        
                }
                break;
                
            }
        }
       ///}
        GameLog.LogMessage("removed item in slot"+_item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }


    public void RemoveItem(InventorySlot slot,Item item)
    {
        item.SetItemHolder(null);
        item.SetSlotId(-1);

        if (slot.item != null &&  slot.item.Name.Equals(item.Name))
            {
                int amount = slot.amount;
                GameLog.LogMessage("amount=" + amount);
                if (amount > 1)
                    slot.UpdateSlot(item.Id, item, --amount);
                else
                {
                    GameLog.LogMessage("Remove Item " + item);
                    slot.RemoveItem();
                    // inventory.slots[i].amount = 0;
                }
            
        }
        GameLog.LogMessage("removed item in slot" + item);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool AddItem(Item _item, int _amount)
    {
        bool success = false;
       
        if (_item.stackable)
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.slots[i].item!=null && inventory.slots[i].item.Name.Equals(_item.Name))
                {
                    
                    inventory.slots[i].AddAmount(_amount);
                    _item.SetItemHolder(this);
                    OnItemListChanged?.Invoke(this, EventArgs.Empty);
                    success = true;
                    return success;
                }
            }
        InventorySlot freeSlot = inventory.GetEmptyInventorySlot();
        if (freeSlot != null)
        {
           _item.SetItemHolder(this);
            _item.SetSlotId(freeSlot.ID);    
            freeSlot.UpdateSlot(_item.Id, _item, _amount);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            success = true;
            return success;
        }
        else
        {
            GameLog.LogMessage("Brak miejsca w plecaku !!!");
            return success;
        }
    }
    
    [ContextMenu("Save")]
    public void Save()
    {
        //GameLog.LogMessage("Execute Save Inventory");
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        GameLog.LogMessage("Saved to "+ Application.persistentDataPath+savePath);
        bf.Serialize(file, saveData);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        //GameLog.LogMessage("Execute Load Inventory");
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    [ContextMenu("Clear")]
    public void Clear() {
         GameLog.LogMessage("Execute Clear Inventory");


        for (int i = 0; i < inventory?.GetInventorySlotArray()?.Length; i++)
        {
            if (inventory?.GetInventorySlotArray()[i]?.GetItem() != null)
               inventory?.GetInventorySlotArray()[i].RemoveItem();   
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        // var previousSize = inventory.slots.Length;
        // inventory = new Inventory();
        // inventory.slots = new InventorySlot[previousSize];
        //   OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public void AddItem(Item _item)
    {
      
        if (_item.stackable)
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.slots[i].ID == _item.Id)
                {
                    _item.SetItemHolder(this);
                    _item.Location = ItemLocation.Inventory;
                    inventory.slots[i].AddAmount(1);
                    OnItemListChanged?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        InventorySlot freeSlot = inventory.GetEmptyInventorySlot();
        if (freeSlot != null)
        {
            _item.SetItemHolder(this);
            _item.Location = ItemLocation.Inventory;
            freeSlot.UpdateSlot(_item.Id, _item, 1);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

       
    }

    public bool CanAddItem(Item _item)
    {

        if (_item.stackable)
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.slots[i].ID == _item.Id)
                {
                    _item.SetItemHolder(this);
                    _item.Location = ItemLocation.Inventory;
                    inventory.slots[i].AddAmount(1);
                    OnItemListChanged?.Invoke(this, EventArgs.Empty);
                    return true;
                }
            }
        InventorySlot freeSlot = inventory.GetEmptyInventorySlot();
        if (freeSlot != null)
        {
            _item.SetItemHolder(this);
            _item.Location = ItemLocation.Inventory;
            freeSlot.UpdateSlot(_item.Id, _item, 1);
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;   
    }
}
[System.Serializable]
public class Inventory  {
    int currentIndex;
    
    
    public InventorySlot[] slots; //= new InventorySlot[12];
    
    public InventorySlot[] GetInventorySlotArray()
    {
        return slots ;
    }

    public InventorySlot GetFirstSlotFromArray()
    {
        currentIndex = 0;
        GameLog.LogMessage("current index:" + currentIndex);
        return slots[currentIndex];
    }

    public InventorySlot GetNextSlotFromArray()
    {
        currentIndex = currentIndex + 1;
        if (currentIndex > (slots.Length - 1)) 
        {
            currentIndex = 0;
           
        }
        
        return slots[currentIndex];
    }

    public InventorySlot GetPreviousSlotFromArray()
    {
        currentIndex = currentIndex - 1;
        if (currentIndex <  0) 
        {
            currentIndex = slots.Length - 1;
            
        }
       
        return slots[currentIndex];
    }

    public InventorySlot GetEmptyInventorySlot()
    {
        foreach (InventorySlot inventorySlot in slots)
        {
            if (inventorySlot.IsEmpty())
            {
                return inventorySlot;
            }
        }
        //GameLog.LogMessage("Cannot find an empty InventorySlot!");
        return null;
    }

    public bool CheckAllInventorySlotsEmpty()
    {
        foreach (InventorySlot inventorySlot in slots)
        {
            if (!inventorySlot.IsEmpty())
            {
                return false;
            }
        }
        //GameLog.LogMessage("Cannot find an empty InventorySlot!");
        return true;
    }

}
[System.Serializable]
public class InventorySlot 
{
    
    public int ID = -1;
    public Item item;
    public int amount=0;
    public override string ToString()
    {
        string obj = "" + ID + "" + item + "" + amount;
        return obj;
    }
    public InventorySlot()
    {
        //GameLog.LogMessage("Create new slot id " + this.ID + "with new Id -1");
        ID = -1;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id,Item _item, int _amount) {
         ID = _id;
         item = _item;
         amount = _amount;
     }
    public void UpdateSlot(int _id, Item _item, int _amount)
    {
       // GameLog.LogMessage("Update slot id " +this.ID+ "with new Id"+  _id);
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value) {
        GameLog.LogMessage("Previous amount:" + amount + " value" + value);
        amount += value;
    }
    public void SetAmount(int _amount) {
        amount = _amount;
    }
    public bool IsEmpty()
    {

        return ID == -1;
    }
    public Item GetItem()
    {
        return item;
    }
    public int GetAmount()
    {
        return amount;
    }
    public void SetItem(Item item)
    {
        this.item = item;
    }
    public void RemoveItem()
    {
        GameLog.LogMessage("Remove item directly in slot" + this);
        item?.SetItemHolder(null);
        ID = -1;
        item = null;
        amount = 0;
        
    }

    public void RemoveItem(Item item)
    {
       
        // GameLog.LogMessage("amount=" + amount);
        if (amount > 1)
            UpdateSlot(item.Id, item, --amount);
        else
        {
            GameLog.LogMessage("Remove Item " + item);
            RemoveItem();
            // inventory.slots[i].amount = 0;
        }

       
    }

    public void AddItem(Item item)
    {
        SetItem(item);
    }

    public bool CanAddItem(Item item)
    {
        if (IsEmpty()) {
            return true;
        }
        return false;
    }
}
