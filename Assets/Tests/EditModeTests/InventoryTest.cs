using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void GetArrayOfInventorySlots()
    {
        var inventoryObj = new Inventory();
        InventorySlot slot1st = new InventorySlot(1, new Item(1, "sword", ItemObjectType.Equipment), 1);
        InventorySlot slot2nd = new InventorySlot(1, new Item(1, "A", ItemObjectType.Letter), 5);
        InventorySlot slot3rd= new InventorySlot(1, new Item(1, "coins", ItemObjectType.Coin), 15);
        //ACTION
        inventoryObj.slots = new InventorySlot[] { slot1st, slot2nd, slot3rd};
        InventorySlot[] output = new InventorySlot[] { slot1st, slot2nd, slot3rd };
        //ASSERT
        Assert.AreEqual(output, inventoryObj.GetInventorySlotArray());
    }
    [Test]
    public void NonEmptyInventorySlot()
    {
        // Use the Assert class to test conditions

        var inventoryObj = new Inventory();
        inventoryObj.slots = new InventorySlot[] { new InventorySlot(1, new Item(1, "sword", ItemObjectType.Equipment), 10)};
        //ACTION

        //ASSERT
        Assert.IsNull(inventoryObj.GetEmptyInventorySlot());
    }
    
    [Test]
    public void CanAddItemTest()
    {
     //   var inventoryObj = new Inventory();
    //    Assert.Throws<NotImplementedException>(() => inventoryObj.CanAddItem());
    }
}
