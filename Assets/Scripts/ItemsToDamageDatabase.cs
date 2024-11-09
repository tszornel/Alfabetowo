using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(fileName = "ItemsToDamageDatabase", menuName = "Task System/ItemsToDamageDatabase")]
public class ItemsToDamageDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public ItemsToDamageData[] itemsArray;
    static List<ItemsToDamageData> itemsToDamageList =  new List<ItemsToDamageData>();
    static string itemsDirectory = "Assets/ScriptableObjects/ItemsToDamageSO";
    // public DialogLanguage[] languages;

    public Dictionary<string, ItemsToDamageData> GetTaskByNameDictionary = new Dictionary<string, ItemsToDamageData>();

#if UNITY_EDITOR
    [ContextMenu("FindItemsToDamageFiles")]
    public void GetAudioClips()
    {
        itemsArray = FindItemsToDamage();
    }

    public static ItemsToDamageData[] FindItemsToDamage()
    {
         //  audioLenghtDict = new Dictionary<AudioClip, string>();
        // Process the list of files found in the directory.
        itemsToDamageList?.Clear();
        SearchDirectoryAsync(itemsDirectory);
        ItemsToDamageData[] itemArrayData = itemsToDamageList.ToArray();
        return itemArrayData;

        // GameLog.LogMessage("audioClipList count po wyjsciu:" + audioClipList.Count);

    }

    public void ClearToDatabase()
    {
        itemsToDamageList?.Clear();



    }

    public void LoadToDatabase()
    {
        if (itemsArray != null && itemsToDamageList != null)
        for (int i = 0; i < itemsArray.Length; i++)
        {
            itemsToDamageList.Add(itemsArray[i]);   



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
                ItemsToDamageData item = AssetDatabase.LoadAssetAtPath<ItemsToDamageData>(filePathNew);
                itemsToDamageList.Add(item);

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

    public void ResetItemsToDamage()
    {
        for (int i = 0; i < itemsArray.Length; i++)
        {
            itemsArray[i].ResetData();
        }


    }
    public string[] GetItemsToDamageArray()
    {
        List<string> stringNamesList = new List<string>();
        for (int i = 0; i < itemsArray.Length; i++)
        {
            stringNamesList.Add(itemsArray[i].name);

            ItemsToDamageData test;
            GetTaskByNameDictionary.TryGetValue(itemsArray[i].name, out test);
            if(test == null)
                GetTaskByNameDictionary.Add(itemsArray[i].name, itemsArray[i]);
        }

        return stringNamesList.ToArray();

    }

    public void OnAfterDeserialize()
    {

       /* for (int i = 0; i < itemsArray.Length; i++)
        {
            GetTaskByNameDictionary.Add(itemsArray[i].name, itemsArray[i]);
        }*/


    }

    public ItemsToDamageData GetItemToDamageByName(string name)
    {

        for (int i = 0; i < itemsArray.Length; i++)
        {
            if (itemsArray[i].name == name)
                return itemsArray[i];
        }
        GameLog.LogError("return null for name " + name);
        return null;


    }

    public void AddItemToDmaageToDatabase(ItemsToDamageData item)
    {


        GetTaskByNameDictionary.Add(item.name, item);
    }


    public void ClearDatabase()
    {

        GetTaskByNameDictionary.Clear();
    }

}