using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventorySlotTests
{
   
    [Test]
    public void InventoryAddItemTest()
    {
        //creating player inventory 
        InventoryObject playerInventory = ScriptableObject.CreateInstance<InventoryObject>();
        Inventory inventoryNew = new Inventory();
        GameLog.LogMessage("invenoty" + inventoryNew);
        playerInventory.inventory = inventoryNew;
        InventorySlot slot1 = new InventorySlot();
        InventorySlot[] slotsNew = new InventorySlot[12] { slot1 , slot1 , slot1 , slot1 , slot1 , slot1 , slot1 , slot1 , slot1 , slot1 , slot1 , slot1 };
        playerInventory.inventory.slots = slotsNew;
        Item item = new Item(1, "sword", ItemObjectType.Equipment);
        playerInventory.AddItem(item); 
        InventorySlot[] slots = playerInventory.inventory.GetInventorySlotArray();
        string addedItemString  = slots[0].GetItem().ToString();
        Assert.AreEqual(item.ToString(), addedItemString);

    }
    [Test]
    public void ToStringOverriden()
    {
        InventorySlot inventoryObj = new InventorySlot(1, new Item(1, "sword", ItemObjectType.Equipment), 10);
        string output = inventoryObj.ToString();
        Assert.AreEqual("1ID=1,Name=sword,Type=Equipment damaged=False10", output);

    }
    [Test]
    public void UpdateSlotTest()
    {
        InventorySlot inventoryObj = new InventorySlot(1, new Item(1, "sword", ItemObjectType.Equipment), 10);
        inventoryObj.UpdateSlot(2, new Item(2, "chest", ItemObjectType.Equipment), 20);
        string output = inventoryObj.ToString();
        Assert.AreEqual("2ID=2,Name=chest,Type=Equipment damaged=False20", output);
    }
    [Test]
    public void AddAmountTest()
    {
        InventorySlot inventoryObj = new InventorySlot(1, new Item(1, "sword", ItemObjectType.Equipment), 10);
        inventoryObj.AddAmount(10);
        Assert.AreEqual(20, inventoryObj.amount);
    }
    [Test]
    public void SetAmountTest()
    {
        InventorySlot inventoryObj = new InventorySlot();
        inventoryObj.SetAmount(15);
        int output = inventoryObj.amount;
        Assert.AreEqual(15, output);
    }
    [Test]
    public void IsEmptyReturnTrueTest()
    {
        InventorySlot inventoryObj = new InventorySlot();
        bool output = inventoryObj.IsEmpty();
        Assert.IsTrue(output);
    }
    [Test]
    public void IsEmptyReturnFalseTest()
    {
        InventorySlot inventoryObj = new InventorySlot(1, new Item(1, "sword", ItemObjectType.Equipment), 10);
        bool output = inventoryObj.IsEmpty();
        Assert.IsFalse(output);
    }
    [Test]
    public void GetItemReturnNullTest()
    {
        InventorySlot inventoryObj = new InventorySlot();
        Assert.IsNull(inventoryObj.GetItem());
    }
    [Test]
    public void GetItemNotReturnNullTest()
    {
        InventorySlot inventoryObj = new InventorySlot();
        inventoryObj.item = new Item(1, "chest", ItemObjectType.Equipment);
        string output = inventoryObj.GetItem().ToString();
        Assert.AreEqual("ID=1,Name=chest,Type=Equipment damaged=False", output);
    }
    [Test]
    public void GetAmount()
    {
        InventorySlot inventoryObj = new InventorySlot();
        inventoryObj.amount = 15;
        Assert.AreEqual(inventoryObj.GetAmount(), 15);
    }
    [Test]
    public void SetItemTest()
    {
        InventorySlot inventoryObj = new InventorySlot();
        Item tempItem = new Item(1, "padding", ItemObjectType.Equipment);
        inventoryObj.SetItem(tempItem);
        Assert.AreEqual(inventoryObj.item, tempItem);
    }
    [Test]
    public void RemoveItemTest()
    {
        InventorySlot inventoryObj = new InventorySlot();
        Item tempItem = new Item(1, "padding", ItemObjectType.Equipment);
        inventoryObj.RemoveItem();
        Assert.IsNull(inventoryObj.item);
    }

}
