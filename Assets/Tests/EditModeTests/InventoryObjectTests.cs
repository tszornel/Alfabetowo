using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class InventoryObjectTests
{

    Item newItem1;
    Item newItem2;
    Item existedItem1;
    Item existedItem2;
    InventorySlot newItemSlot1;
    InventorySlot existedItemSlot1;
    InventorySlot newItemSlot2;
    InventorySlot existedItemSlot2;

    private InventoryObject inventoryObject;
    //Create an inheritant class of ItemObject
    public class TestItemObject : ItemObject
    {
        public TestItemObject(int ID, ItemObjectType type, Sprite itemSprite,
            int amount, string description, int price, ItemBuff[] buffs)
        {
            this.ID = ID;
            this.type = type;
            this.itemSprite = itemSprite;
            this.amount = amount;
            this.description = description;
            this.price = price;
            this.buffs = buffs;
        }
        public new Item createItem()
        {
            Item item = new Item(this);
            return item;
        }
    }

    public InventoryObjectTests()
    {
        OneTimeSetUp();
    }
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        ItemObject io1 = PickupItemAssets.Instance.itemsDatabase.GetItem[7];
        ItemObject io2 = PickupItemAssets.Instance.itemsDatabase.GetItem[10];
        ItemObject itemObject1 = new TestItemObject(40, ItemObjectType.Coin, io2.itemSprite,
            1, "no description", 11, new ItemBuff[] { });
        ItemObject itemObject2 = new TestItemObject(50, ItemObjectType.Key, io1.itemSprite,
            1, "no description", 11, new ItemBuff[] { });
        this.newItem1 = itemObject1.createItem();
        this.newItem2 = itemObject2.createItem();
        this.existedItem1 = io1.createItem();
        this.existedItem2 = io2.createItem();
        newItemSlot1 = new InventorySlot(1, newItem1, 1);
        existedItemSlot1 = new InventorySlot(2, existedItem1, 1);
        newItemSlot2 = new InventorySlot(3, newItem1, 1);
        existedItemSlot2 = new InventorySlot(4, existedItem2, 1);
    }

  
    [SetUp]
    public void SetUp()
    {
        inventoryObject = ScriptableObject.CreateInstance<InventoryObject>();
        inventoryObject.inventory = new Inventory();
        // Initialize the slots array with non-null InventorySlot instances
        inventoryObject.inventory.slots = new InventorySlot[10]; // Adjust the size as necessary

        // Properly initialize each slot in the array
        for (int i = 0; i < inventoryObject.inventory.slots.Length; i++)
        {
            inventoryObject.inventory.slots[i] = new InventorySlot(-1, null, 0); // Assuming InventorySlot has a constructor
        }
    }

  

    [Test]
    public void AddItem_SuccessfullyAddsItemToInventory()
    {
        // Mock or create a test Item instance
        var testItem = new Item(1, "Coin", ItemObjectType.Coin); // Adjust parameters as needed

        // Act
        var success = inventoryObject.AddItem(testItem, 1); // Use a valid amount that makes sense for your test

        // Assert
        Assert.IsTrue(success, "The item was not successfully added to the inventory.");
        // Further assertions to verify that the item was added correctly...
    }



    [TearDown]
    public void TearDown()
    {
        // Clean up
        if (inventoryObject != null)
        {
            ScriptableObject.DestroyImmediate(inventoryObject);
        }
    }


    // A Test behaves as an ordinary method
    [Test]
    public void AddOneItemExistedInDatabaseAndOneNewItem()
    {
        // Use the Assert class to test conditions
        // 2 ways
        // create a new item from the scratch and add it into database
        // add this item to inventory and test whether it is there
        // or add an existed item to invetory to test
        InventorySlot[] inventorySlots = new InventorySlot[]{existedItemSlot1, newItemSlot1};
        Inventory inventory = new Inventory();
        //Array.Copy(inventorySlots, inventory.slots, 2);
        inventory.slots = inventorySlots;
        InventoryObject inventoryObj = new InventoryObject();
        inventoryObj.inventory = inventory;
        inventoryObj.AddItem(existedItem1, existedItemSlot1, 1);
        inventoryObj.AddItem(newItem1, newItemSlot1, 1);
        Assert.AreEqual(inventoryObj.inventory.slots, inventorySlots);
    }

    [Test]
    public void AddingSecondItemWiththeSameIdToInventory()
    {
        InventorySlot[] inventorySlots = new InventorySlot[] { newItemSlot1, newItemSlot1 };
        Inventory inventory = new Inventory();
        inventory.slots = inventorySlots;
        InventoryObject inventoryObj = new InventoryObject();
        inventoryObj.inventory = inventory;
        inventoryObj.AddItem(existedItem1, existedItemSlot1, 1);
        inventoryObj.AddItem(existedItem1, existedItemSlot1, 2);
        Assert.AreEqual(inventoryObj.inventory.slots, inventorySlots);
    }

    [Test]
    public void AddTwoItemsExistedInDatabase()
    {

    }
    [Test]
    public void RemoveItem_NonStackableOrSingleQuantity_ClearsSlot()
    {
        // Adjust the item instantiation to match the available constructors or property setters
        Item testItem = new Item(1,"Coin",  ItemObjectType.Coin);
        
        var slot = new InventorySlot(); // Assuming InventorySlot has a parameterless constructor or is set up differently
        slot.UpdateSlot(testItem.Id, testItem, 1);

        slot.RemoveItem();

        Assert.IsNull(slot.item, "Slot item should be null after removal.");
        Assert.AreEqual(-1, slot.ID, "Slot ID should be reset after item removal.");
        Assert.AreEqual(0, slot.amount, "Slot amount should be 0 after item removal.");
    }


}
