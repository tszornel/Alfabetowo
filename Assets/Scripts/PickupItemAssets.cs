using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PickupItemAssets : MonoBehaviour {
    // Internal instance reference
    private static PickupItemAssets _Instance;
    static ItemObject io;
    public static PickupItemAssets Instance {
        get
        {
           if (_Instance == null) 
                _Instance = Instantiate(Resources.Load<PickupItemAssets>("PickupItemAssets"));
            return _Instance;
        }
      }
    private void Awake() {
        if (_Instance != null && _Instance != this)
        {
            Destroy(_Instance);
          //  throw new System.Exception("An instance of this singleton already exists.");
        }
        _Instance = this;
    }
    public Item GetAnyItem(string name)
    {
        return allItemsDatabase.GetItemObjectFromName(name).createItem();       
    }
    public Item GetItem(string name)
    {
        Item returnedItem = null;
        if (!itemsDatabase)
            itemsDatabase = Resources.Load<ItemObjectDatabase>("ItemsDatabase");
        io = itemsDatabase.GetItemObjectFromName(name);
        if (io)
            returnedItem = io.createItem();
        else
            GameLog.LogMessage("Item not found in database");
        //GameLog.LogMessage("Static damaged=" + damagedItem + " returnedItem damaged=" + returnedItem.damaged);
        return returnedItem;
    }
    public Item GetLetter(string name)
    {
        Item returnedItem = null;
        if (!lettersDatabase)
            lettersDatabase = Resources.Load<ItemObjectDatabase>("LettersDatabase");
        io = lettersDatabase.GetItemObjectFromName(name);
        if (io)
            returnedItem = io.createItem();
        else
            GameLog.LogMessage("Item not found in database");
        //GameLog.LogMessage("Static damaged=" + damagedItem + " returnedItem damaged=" + returnedItem.damaged);
        return returnedItem;
    }
    public Transform pfPickupItem;
    public ItemObjectDatabase itemsDatabase;
    public ItemObjectDatabase lettersDatabase;
    public ItemObjectDatabase allItemsDatabase;
    public NamesDatabase namesDatabase;
    public DialogDatabase dialogDatabase;
    public InventoryDatabase inventoryDatabase;
    public TasksDatabase tasksDatabase;
    public ItemsToDamageDatabase itemsToDamageDatabase;
    public ItemsToBeDroppedDatabase itemsToBeDroppedDatabase;
    public DialogLanguage language;
    public PickupItemWorldSpawner _spawner;
    //public PickupItemWorldSpawner spawner;
    private void OnApplicationQuit()
    {
        Destroy(_Instance);
    }
}
