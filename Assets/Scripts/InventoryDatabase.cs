using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InventoryDatabase", menuName = "Inventory System/Inventory/Database")]
public class InventoryDatabase : ScriptableObject, ISerializationCallbackReceiver
{

    public InventoryObject[] inventoryArray;

    public SerializableDictionary<string, InventoryObject> GetInventoryDictionary = new SerializableDictionary<string, InventoryObject>();
   
    public void OnBeforeSerialize()
    {
       


    }
    public void OnAfterDeserialize()
    {
       


    }


    public string[] GetNamesArray()
    {
        List<string> stringNamesList = new List<string>();
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            stringNamesList.Add(inventoryArray[i].name);
        }
        return stringNamesList.ToArray();

    }


    [ContextMenu("LoadDataIntoDictionary")]
    public void LoadDataToDict() {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            GetInventoryDictionary.Add(inventoryArray[i].name, inventoryArray[i]);
        }
    
    }

    public InventoryObject GetInventoryObjectFromName(string name) {

        InventoryObject inventoryFromName =null;
        try
        {
          inventoryFromName =  GetInventoryDictionary[name];
        }
        catch (System.Exception)
        {
            GameLog.LogError("inventory Name not found in database=" + name);
            LoadDataToDict();
            inventoryFromName = GetInventoryDictionary[name];
        }

        return inventoryFromName;
      
    }

       


    

    public void AddInventoryToDatabase(InventoryObject inventory)
    {
        GetInventoryDictionary.Add(inventory.name,inventory);
    }


    public void ClearInventoryDatabase()
    {
        GetInventoryDictionary.Clear();
    }

}