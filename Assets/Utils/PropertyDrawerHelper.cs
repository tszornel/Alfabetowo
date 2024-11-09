using System.Linq;
using UnityEngine;

public static class PropertyDrawerHelper
{
    private static NamesDatabase nameDatabase = Resources.Load<NamesDatabase>("NamesDatabase");
    private static ItemObjectDatabase letterDatabase = Resources.Load<ItemObjectDatabase>("LettersDatabase");
    private static ItemObjectDatabase itemsDatabase = Resources.Load<ItemObjectDatabase>("ItemsDatabase");
    private static InventoryDatabase inventoryDatabase = Resources.Load<InventoryDatabase>("InventoryDatabase");
    private static DialogDatabase dialogDatabase = Resources.Load<DialogDatabase>("DialogDatabase");
    private static TasksDatabase tasksDatabase = Resources.Load<TasksDatabase>("TasksDatabase");
    private static ItemsToDamageDatabase itemsToDamageDatabase = Resources.Load<ItemsToDamageDatabase>("ItemsToDamageDatabase");
    private static ItemsToBeDroppedDatabase itemsToBeDroppedDatabase = Resources.Load<ItemsToBeDroppedDatabase>("ItemsToBeDroppedDatabaseInstance");
#if UNITY_EDITOR
    public static string[] AllTasks()
    {
        return tasksDatabase.GetNamesArray();

    }


    public static string[] AllItemsToBeDropped()
    {
        return itemsToBeDroppedDatabase.GetItemsToBeDroppedArray();

    }

    public static string[] AllDialogs() 
    { 
        return dialogDatabase.GetNamesArray();

    }

    public static string[] AllInventories()
    {
        return inventoryDatabase.GetNamesArray();
    }


    public static string[] AllNames()
    {
        return nameDatabase.GetNamesArray();
    }

    public static string[] AllItems()
    {
        return itemsDatabase.GetItemsNamesArray();
    }

    public static string[] AllLetters()
    {
        return letterDatabase.GetItemsNamesArray();
    }


    public static string[] AllItemsToDamage ()
    {
        return itemsToDamageDatabase.GetItemsToDamageArray();
    }


    public static string[] AllItemsAndLetters()
    {
        string[] arr1 = letterDatabase.GetItemsNamesArray();
        string[] arr2 = itemsDatabase.GetItemsNamesArray();
        arr1 = arr1.Concat(arr2).ToArray();
        return arr1;
    }
   




#endif
}