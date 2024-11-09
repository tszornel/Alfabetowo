using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExtendedInventorySlotTests
{
    [Test]
    public void RemoveItemDecreasesAmountTest()
    {
        // Arrange
        InventorySlot inventorySlot = new InventorySlot(1, new Item(1, "A", ItemObjectType.Letter), 20);

        // Act
        inventorySlot.RemoveItem(); // Assuming a method that removes a specific amount of items

        // Assert
        Assert.AreEqual(0, inventorySlot.amount, "Removing items should decrease the amount correctly.");
    }




  
    

}
