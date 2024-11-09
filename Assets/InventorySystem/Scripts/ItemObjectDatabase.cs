using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;



[CreateAssetMenu(fileName = "Items Database ", menuName = "Inventory System/Items/Database")]
public class ItemObjectDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public ItemObject[] items;
    public SerializableDictionary<int, ItemObject> GetItem = new SerializableDictionary<int, ItemObject>();
    public List<ItemObject> AllItems = new List<ItemObject>();
    public static List<LetterObject> Letters = new List<LetterObject>();
    public static List<EquipmentObject> Equipment = new List<EquipmentObject>();
    public static List<RepaireObject> EquipmentToRepair = new List<RepaireObject>();
    public SerializableDictionary<string, ItemObject> GetItemByName = new SerializableDictionary<string, ItemObject>();
    static string itemsDirectory = "Assets/ScriptableObjects/items/";
    static string lettersDirectory = "Assets/ScriptableObjects/Letters/";
    public bool findOnlyItems = false;
    public bool findOnlyLetters = false;

    public void OnBeforeSerialize()
    {

    }

#if UNITY_EDITOR
    [ContextMenu("FindItemFiles")]
    public void GetScriptableItems()
    {
        items = FindItems();
        LoadToDatabase();
    }

    public ItemObject[] FindItems()
    {

        //  audioLenghtDict = new Dictionary<AudioClip, string>();
        // Process the list of files found in the directory.
        AllItems?.Clear();
        if (!findOnlyLetters)
            SearchDirectoryAsync(itemsDirectory);
        if (!findOnlyItems)
            SearchDirectoryAsync(lettersDirectory);
        ItemObject[] itemsArray = AllItems.ToArray();
        return itemsArray;
        /*string[]

                // GameLog.LogMessage("audioClipList count po wyjsciu:" + audioClipList.Count);

                guids = AssetDatabase.FindAssets("t:Object l:item");

        foreach (string guid in guids)
        {
            Debug.Log("Item: " + AssetDatabase.GUIDToAssetPath(guid));
            AssetDatabase.LoadAssetAtPath<ItemObject>(guid);
        }*/

    }


    public void SearchDirectoryAsync(string targetDirectory)
    {

        string[] fileEntries = Directory.GetFiles(targetDirectory);

        foreach (string filePath in fileEntries)
        {
            if (!filePath.Contains("meta"))
            {
                string filePathNew = filePath.Replace(@"\", @"/");

                GameLog.LogMessage("Item Object file:" + filePathNew);
                ItemObject item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemObject>(filePathNew);
                AllItems.Add(item);

                //  audioLenghtDict.Add(audioClip,(audioClip.length/100).ToString("f3"));
                char[] delimiterChars = { '.', '/', '\t' };
                string[] names = filePathNew.Split(delimiterChars);

            }
        }

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            SearchDirectoryAsync(subdirectory);
        }
    }

#endif
    public string[] GetItemsNamesArray()
    {

        List<string> stringItemsList = new List<string>();

        for (int i = 0; i < items.Length; i++)
        {
            stringItemsList.Add(items[i].name);
        }
        return stringItemsList.ToArray();

    }


    public void ClearToDatabase()
    {
        GetItem?.Clear();
        AllItems?.Clear();
        Letters?.Clear();
        Equipment?.Clear();
        EquipmentToRepair?.Clear();

    }

    public void LoadToDatabase()
    {

        for (int i = 0; i < items.Length; i++)
        {


            ItemObject itemFromId = null;
          
            GetItem.TryGetValue(i,out itemFromId);
           
            if(!itemFromId)
                GetItem.Add(i, items[i]);





            AllItems.Add(items[i]);
            //  GameLog.LogMessage(items[i].objectName);
            if (items[i].type == ItemObjectType.Letter)
            {

                Letters.Add((LetterObject)items[i]);
            }
            else if (items[i].type == ItemObjectType.Equipment)
            {

                Equipment.Add((EquipmentObject)items[i]);
            }
            else if (items[i].type == ItemObjectType.Repaire)
            {

                try
                {
                    EquipmentToRepair.Add((RepaireObject)items[i]);
                }
                catch (InvalidCastException ex)
                {

                    //GameLog.LogError("invalid cast" + items[i]);
                    throw ex; ;
                }

            }

        }
    }

    [ContextMenu("LoadData into dictionary")]
    public void LoadDataToDict()
    {
        for (int i = 0; i < items.Length; i++)
        {

            ItemObject test;
            GetItemByName.TryGetValue(items[i].name, out test);
            if(!test)
                 GetItemByName.Add(items[i].name, items[i]);
        }

    }
    public ItemObject GetItemObjectFromName(string name)
    {

        ItemObject itemFromName = null;
        try
        {
            itemFromName = GetItemByName[name];
        }
        catch (System.Exception ex)
        {
            GameLog.LogError("Not found in items Database"+ex.Message+"name="+ name);    
            LoadDataToDict();
            return null;//itemFromName = GetItemByName[name];
        }

        return itemFromName;


       
        
        /* for (int i = 0; i < items.Length; i++)
        {
            //GameLog.LogMessage("item[i].objectName=" + items[i].objectName);
            if (items[i].name.Equals(name))
                return items[i];
        }*/
        // GameLog.LogError("return null for name " + name);
        
    }

    public ItemObject GetItemObjectFromID(int id)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //GameLog.LogMessage("item[i].objectName=" + items[i].objectName);
            if (items[i].ID == id)
                return items[i];
        }
        // GameLog.LogError("return null for name " + name);
        return null;
    }

    public void AddItemToDatabase(ItemObject itemObject)
    {
        GetItem.Add(itemObject.ID, itemObject);
    }
    public void OnAfterDeserialize()
    {
        Letters.Clear();
        AllItems.Clear();
        Equipment.Clear();
        LoadToDatabase();
    }
}



