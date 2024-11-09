using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;



#if UNITY_EDITOR
namespace Webaby.Utils
{
    /// <summary>
    ///  /* Setup Language version for Abecadlowo. It creates Scriptable Object for Dialog System in the language version that is choosen in GameHandler */
    /// </summary>
    public class SetupDropedItems
    {
        [SerializeField]
        public static DialogLanguage language = UtilsClass.GetLanguageFromPrefs(PlayerPrefs.GetString("language", "Polski"));

        const int RequestTimeout = 5;
        public static int itemId = 0;
        [SerializeField]
        public static ItemsToDamageDatabase itemsToDamageDatabase;
        public static Dictionary<string, ItemsToDamageData> itemsToDamageDictionary = new Dictionary<string, ItemsToDamageData>();





        /// <summary>
        /// </summary>
        // [MenuItem("Webaby/SetupLanguage/SetupItems")]
        public static void CreateLanguageFiles()
        {
            CreateLanguageFilesProcess(language, "items");
        }

        public static void CreateLanguageFilesProcess(DialogLanguage _language, string pathNames)
        {
           language = _language;
           ClearNameFolder(pathNames);
           CreateNameFolder(_language, pathNames);
           GetJsonName(_language, pathNames);

        }

        // [MenuItem("Webaby/SetupLanguage/Create Items Folder")]
        static void CreateNameFolder(DialogLanguage _language, string pathNames)
        {


            string savePath = "Assets/ScriptableObjects/" + pathNames + "/";
            GameLog.LogMessage("savePath:" + savePath);
            string guid = AssetDatabase.CreateFolder("Assets/ScriptableObjects/", pathNames);
            GameLog.LogMessage("GUID:" + guid);

        }

        /// <summary>
        /// 
        /// </summary>
      //  [MenuItem("Webaby/SetupLanguage/Clear Items Folders")]
        static void ClearNameFolder(string pathNames)
        {
            if (!itemsToDamageDatabase)
                itemsToDamageDatabase = Resources.Load<ItemsToDamageDatabase>("ItemsToDamageDatabase");

            //allSpritesDatabase.FindImages();
            string savePath = "Assets/ScriptableObjects/" + pathNames + "/";
            ProcessDirectory(savePath);
            GameLog.LogMessage("savePath:" + savePath);
            UnityEditor.AssetDatabase.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetDirectory"></param>
        public static void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string filePath in fileEntries)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                File.Delete(filePath);
                UnityEditor.FileUtil.DeleteFileOrDirectory(filePath);
            }
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(subdirectory);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefsLanguage"></param>
        /// <returns></returns>
        public static string GetLanguageJsonFieldName(string prefsLanguage)
        {
            string languageJsonFieldName;
            switch (prefsLanguage)
            {
                case "Polski":
                    languageJsonFieldName = "pl";
                    break;
                case "Niemiecki":
                    languageJsonFieldName = "de";
                    break;
                case "Angielski":
                    languageJsonFieldName = "en";
                    break;
                case "Francuski":
                    languageJsonFieldName = "fr";
                    break;
                default:
                    languageJsonFieldName = "pl";
                    break;
            }
            return languageJsonFieldName;
        }
   
        static ItemsToDamageData CreateItemsToDamageScriptableObject(string ItemName, int initialHealth, int initialStrenth, bool damaged, string _Items)
        {
            ItemsToDamageData newItemToDamage = ScriptableObject.CreateInstance<ItemsToDamageData>();

            newItemToDamage.name = ItemName;
            newItemToDamage.health = initialHealth;
            newItemToDamage.strenght = initialStrenth;
            newItemToDamage.initialHealth = initialHealth;
            newItemToDamage.initialStrenght = initialStrenth;
            newItemToDamage.damaged = damaged;
            newItemToDamage.ItemsToBeDroppedName = _Items;
            newItemToDamage.SetItemsToBeDropped();
           // newItemToDamage.itemNames = _Items;



            // newItemToDamage.letterNames = itemNamesList.ToArray();
            return newItemToDamage;


        }

        public static string LoadResourceTextfile(string path)
        {

            string filePath = "SetupData/" + path.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="language"></param>
        static void GetJsonName(DialogLanguage language, string pathNames)
        {

            string jsonFileName = "";
            switch (pathNames)
            {
                case "ItemsToDamageSO":
                    jsonFileName = "ItemsToDamage.json";
                    break;
                case "EnemyItemsSO":
                    jsonFileName = "EnemyItems.json";
                    break;
                case "FriendItemsSO":
                    jsonFileName = "FriendItems.json";
                    break;
                default:
                    break;
            }


            string json;
            json = LoadResourceTextfile(jsonFileName);
            GameLog.LogMessage("json from file :" + json);


            ProcessJSon(json, pathNames);


        }


        static void ProcessJSon(string json, string pathNames)
        {

            ListItemToDamageTest items = JsonUtility.FromJson<ListItemToDamageTest>(json);

            int index = GetIndex(language);
            int dbId = 0;

            itemsToDamageDictionary.Clear();    
            for (int i = 0; i < items.data.Length; i++)
            {
                string itemObjectName = items.data[i].ItemToDamageName;
                ItemsToDamageData itemsToDamageObject;
                if (!itemObjectName.Equals("")) { 
                itemsToDamageDictionary.TryGetValue(itemObjectName, out itemsToDamageObject);
                    if (!itemsToDamageObject)
                    {
                        string droppedItemsName = items.data[i].ItemsToBeDroppedName;
                        bool damaged = items.data[i].damaged;
                        int health = items.data[i].health;
                        int strenght = items.data[i].strenght;
                        ItemsToDamageData newItemToDamage = CreateItemsToDamageScriptableObject(itemObjectName, health, strenght, damaged, droppedItemsName);
                        dbId++;
                        // GameLog.LogMessage("Create scriptable name" + newName.name);
                        if (newItemToDamage != null)
                        {
                            itemsToDamageDictionary.Add(itemObjectName, newItemToDamage);
                            CreateScriptableFile(newItemToDamage, pathNames);
                            AddItemsToDB(itemsToDamageDatabase, itemsToDamageDictionary);
                        }
                    }
                }

            }
        }




            static void CreateScriptableFile(ItemsToDamageData itemObjectInstance, string pathNames)
            {
                ItemsToDamageData itemToSave = itemObjectInstance;
                string fileName = itemToSave.name + ".asset";
                GameLog.LogMessage("name fileanme to create" + fileName);
                string savePath = "Assets/ScriptableObjects/" + pathNames + "/";
                string[] setLabels = new string[] { "item", itemToSave.name };
                AssetDatabase.SetLabels(itemToSave, setLabels);
                GameLog.LogMessage("created file folder name:" + savePath);
                GameLog.LogMessage("file name:" + savePath + "/" + fileName);
                if (!AssetDatabase.Contains(itemToSave))
                    AssetDatabase.CreateAsset(itemToSave, savePath + "/" + fileName);
                GameLog.LogMessage("Name:" + itemToSave.name);
                UnityEditor.AssetDatabase.Refresh();

            }

            static void AddItemsToDB(ItemsToDamageDatabase database, Dictionary<string, ItemsToDamageData> _itemDictionary)
            {
                database.ClearToDatabase();
                database.itemsArray = _itemDictionary.Values.ToArray();
                database.LoadToDatabase();
                UnityEditor.AssetDatabase.Refresh();
                EditorUtility.SetDirty(database);

            }



            static string DecodeEncodedNonAsciiCharacters(string value)
            {
                return Regex.Replace(
                    value,
                    @"\\u(?<Value>[a-fA-F0-9]{4})",
                    m =>
                    {
                        return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                    });
            }


            /// <summary>
            /// 
            /// </summary>
            /// <param name="language"></param>
            /// <returns></returns>
            static int GetIndex(DialogLanguage language)
            {
                int index;
                switch (language)
                {
                    case DialogLanguage.Polski:
                        index = 0;
                        break;
                    case DialogLanguage.Niemiecki:
                        index = 2;
                        break;
                    case DialogLanguage.Angielski:
                        index = 1;
                        break;
                    case DialogLanguage.Francuski:
                        index = 3;
                        break;
                    default:
                        index = 0;
                        break;
                }
                return index;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class ListItemToDamageTest
        {
            public ItemToDamageData[] data;
            public static ListItemToDamageTest CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<ListItemToDamageTest>(jsonString);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        public class ItemToDamageData
        {
            public string ItemToDamageName;
            public string Description;
            public int health;
            public int strenght;
            public bool damaged;
            public string ItemsToBeDroppedName;
            

        }

    public class LettersSegmentsData
    {
        public string pl;
        public string en;
        public string de;
    }


}
#endif