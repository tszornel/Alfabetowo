using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsToBeDroppedDatabase", menuName = "Task System/ItemsToBeDroppedDatabase")]
public class ItemsToBeDroppedDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public ItemsToBeDropped[] itemsArray;
    static List<ItemsToBeDropped> itemsToBeDroppedList = new List<ItemsToBeDropped>();
    static string itemsDirectory = "Assets/ScriptableObjects/ItemsToBeDroppedSO";
    // public DialogLanguage[] languages;

    public Dictionary<string, ItemsToBeDropped> GetItemToBeDroppedByNameDictionary = new Dictionary<string, ItemsToBeDropped>();

#if UNITY_EDITOR
    [ContextMenu("FindItemsToDamageFiles")]
    public void GetItemsTobeDropped()
    {
        itemsArray = FindItemsToBeDropped();
    }

    public static ItemsToBeDropped[] FindItemsToBeDropped()
    {
        //  audioLenghtDict = new Dictionary<AudioClip, string>();
        // Process the list of files found in the directory.
        itemsToBeDroppedList?.Clear();
        SearchDirectoryAsync(itemsDirectory);
        ItemsToBeDropped[] itemArrayData = itemsToBeDroppedList.ToArray();
        return itemArrayData;

        // GameLog.LogMessage("audioClipList count po wyjsciu:" + audioClipList.Count);

    }

    public void ClearToDatabase()
    {
        itemsToBeDroppedList?.Clear();



    }

    public void LoadToDatabase()
    {
        if (itemsArray != null && itemsToBeDroppedList != null)
            for (int i = 0; i < itemsArray.Length; i++)
            {
                itemsToBeDroppedList.Add(itemsArray[i]);



            }
    }



    public static void SearchDirectoryAsync(string targetDirectory)
    {

        string[] fileEntries = Directory.GetFiles(targetDirectory);

        foreach (string filePath in fileEntries)
        {
            if (!filePath.Contains("meta"))
            {
                string filePathNew = filePath.Replace(@"\", @"/");

                GameLog.LogMessage("item file:" + filePathNew);
                ItemsToBeDropped item = AssetDatabase.LoadAssetAtPath<ItemsToBeDropped>(filePathNew);
                item.SetupItemsDictionary();
                itemsToBeDroppedList.Add(item);

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

    public void OnBeforeSerialize()
    {
        // GetDialogDictionary = new Dictionary<DialogLanguage, List<Dialog>>();
    }

    public string[] GetItemsToBeDroppedArray()
    {
        List<string> stringNamesList = new List<string>();
        for (int i = 0; i < itemsArray.Length; i++)
        {
            stringNamesList.Add(itemsArray[i].name);

            ItemsToBeDropped test=null;
            GetItemToBeDroppedByNameDictionary.TryGetValue(itemsArray[i].name, out test);
            if (!test)
            {
                GetItemToBeDroppedByNameDictionary.Add(itemsArray[i].name, itemsArray[i]);

            }
           
        }

        return stringNamesList.ToArray();

    }

    public void OnAfterDeserialize()
    {

    }

    public ItemsToBeDropped GetItemToBeDroppedByName(string name)
    {

        for (int i = 0; i < itemsArray.Length; i++)
        {
            if (itemsArray[i].name == name)
                return itemsArray[i];
        }
        GameLog.LogError("return null for name " + name);
        return null;


    }

    public void AddItemToDmaageToDatabase(ItemsToBeDropped item)
    {


        GetItemToBeDroppedByNameDictionary.Add(item.name, item);
    }


    public void ClearDatabase()
    {

        GetItemToBeDroppedByNameDictionary.Clear();
    }

}

