using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.U2D;
//using static UnityEditor.Progress;


#if UNITY_EDITOR
namespace Webaby.Utils
{
    /// <summary>
    ///  /* Setup Language version for Abecadlowo. It creates Scriptable Object for Dialog System in the language version that is choosen in GameHandler */
    /// </summary>
    public class SetupInventories
    {
        [SerializeField]
        public static DialogLanguage language = UtilsClass.GetLanguageFromPrefs(PlayerPrefs.GetString("language", "Polski"));
        const string URL = "http://51.68.172.64:3002/items";
        const int RequestTimeout = 5;
        public static Dictionary<string, InventoryObject> inventoryDictionary = new Dictionary<string, InventoryObject>();
        public static List<InventorySlot> InventorySlots = new List<InventorySlot>();
        public static int itemId = 0;
        [SerializeField]
        private static ItemObjectDatabase itemDatabase;
        private static InventoryDatabase inventoryDatabase;
        private static Dictionary<string, Sprite> dictSprites = new Dictionary<string, Sprite>();
        


        /// <summary>
        /// </summary>
       // [MenuItem("Webaby/SetupLanguage/SetupInventories")]
        public static void CreateLanguageFiles()
        {
            CreateLanguageFilesProcess(language);
        }

        public static void CreateLanguageFilesProcess(DialogLanguage _language)
        {
            ClearNameFolder();
            CreateNameFolder();
            GameLog.LogMessage("Create language Files url=" + URL + " language=" + language);

            Uri uri = new Uri(URL);
            GameLog.LogMessage("uri host=" + uri.Host + "uri port=" + uri.Port);
            UriBuilder uriBuilder = new UriBuilder();
            GetJsonName(uri, _language);
        }



       // [MenuItem("Webaby/SetupLanguage/Create Inventories Folder")]
        static void CreateNameFolder()
        {
            string savePath = "Assets/ScriptableObjects/GeneratedInventories/" + GetLanguageJsonFieldName(language.ToString() + "/");
            GameLog.LogMessage("savePath:" + savePath);
            string guid = AssetDatabase.CreateFolder("Assets/ScriptableObjects/GeneratedInventories/", GetLanguageJsonFieldName(language.ToString()));
            GameLog.LogMessage("GUID:" + guid);
           
        }

        /// <summary>
        /// 
        /// </summary>
      //  [MenuItem("Webaby/SetupLanguage/Clear Inventories Folders")]
        static void ClearNameFolder()
        {
            itemDatabase = Resources.Load<ItemObjectDatabase>("AllItemsDatabase");
            inventoryDatabase = Resources.Load<InventoryDatabase>("InventoryDatabase");
            string savePath = "Assets/ScriptableObjects/GeneratedInventories/";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialogText"></param>
        /// <param name="time"></param>
        /// <param name="filename"></param>
        /// <param name="characterName"></param>
        /// <returns></returns>
        static InventorySlot AddSlots(string itemName,int amount)
        {
            InventorySlot slot = new InventorySlot();
           
            if (itemName == "None" || itemName == null)
            {
                slot.item = null;
                slot.ID = -1;
                slot.amount = 0;
            } else 
            {
                GameLog.LogMessage("Searched item name:" + itemName);
                ItemObject itemFromObject = itemDatabase.GetItemObjectFromName(itemName);
                slot.item = itemFromObject.createItem();
                slot.ID = itemFromObject.ID;
                slot.amount = itemFromObject.amount;
            }
            return slot;
        }

       

        static InventoryObject CreateInventoryScriptableObject(string inventoryName,InventorySlot[] slots)
        {
            GameLog.LogMessage("Create Scriptable inventory:" + inventoryName);
            InventoryObject inventoryObject = ScriptableObject.CreateInstance<InventoryObject>();
            inventoryObject.name = inventoryName;
            inventoryObject.database = itemDatabase;
            Inventory newInventory = new Inventory();
            newInventory.slots = slots;
            inventoryObject.inventory = newInventory;
            return inventoryObject;


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
        static void GetJsonName(Uri uri, DialogLanguage language)
        {
            // GameLog.LogMessage("GetJsonDialog entered");
            FieldInfo[] property_infos = typeof(DialogData).GetFields();
            foreach (var item in property_infos)
            {
                GameLog.LogMessage(item.Name);
            }
            UnityWebRequest _unityWebRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbGET);
            _unityWebRequest.timeout = RequestTimeout;
            _unityWebRequest.SetRequestHeader("Accept", "application/json, text/plain, */*");

            _unityWebRequest.SetRequestHeader("Accept_encoding", "gzip, deflate");
            _unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            string json;
            _unityWebRequest.SendWebRequest().completed += (AsyncOperation) =>
            {
                GameLog.LogMessage("Receive");
                if (_unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    GameLog.LogMessage(_unityWebRequest.error);
                    string pathNames = "inventories";
                    json = LoadResourceTextfile(pathNames);
                    GameLog.LogMessage("json from file :" + json);
                    ProcessJSon(json);
                }
                else
                {
                    GameLog.LogMessage("get complete json!" + _unityWebRequest.downloadHandler.text);
                    string jsonString = System.Text.Encoding.UTF8.GetString(_unityWebRequest.downloadHandler.data, 3, _unityWebRequest.downloadHandler.data.Length - 3);
                    // GameLog.LogMessage("jsonString !" + jsonString);
                    string replacement = "{\"data\":[";
                    json = replacement + _unityWebRequest.downloadHandler.text.Substring(1);
                    json = json.Substring(0, json.Length - 1) + "]}";
                    json = DecodeEncodedNonAsciiCharacters(json);
                    ProcessJSon(json);


                }
            };
        }


        static void ProcessJSon(string json)
        {

            ListInventory items = JsonUtility.FromJson<ListInventory>(json);
            inventoryDictionary.Clear();
          //   Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/items-atlas");
             dictSprites.Clear();

            /* foreach (Sprite sprite in sprites)
             {
                 dictSprites.Add(sprite.name, sprite);
                 GameLog.LogMessage("Sprite:" + sprite.name+" added to dict");
             }*/
           
            SpritesDatabase allSpritesDatabase = Resources.Load<SpritesDatabase>("SpritesDatabase");
            GameLog.LogMessage("All sprites lenght:" + allSpritesDatabase.spritesArray.Length);
            foreach (Sprite _sprite in allSpritesDatabase.spritesArray)
            {
                GameLog.LogMessage("Add sprite:" + _sprite.name);
                Sprite value = null;
                if (_sprite.name.Contains("Clone"))
                {
                    _sprite.name.TrimEnd("(Clone)".ToCharArray());
                    GameLog.LogMessage("Add sprite:" + _sprite.name);
                }
                if (!dictSprites.TryGetValue(_sprite.name, out value))
                    dictSprites.Add(_sprite.name, _sprite);
            }
            foreach (var item in dictSprites)
            {
                GameLog.LogMessage("Sprites from dict:" + item.Key);
            }

            GameLog.LogMessage("Items amount in json file:" + items.data.Length);
            int index = GetIndex(language);
            int dbId = 0;
            for (int i = 0; i < items.data.Length; i++)
            {
                InventoryObject inventoryObject;

                List<InventorySlot> newSlotsList = new List<InventorySlot>();
                InventorySlotData[] slots = items.data[i].slots;
                for (int j = 0; j < slots.Length; j++)
                {
                    InventorySlotData slot = slots[j];
                    string itemName = slots[j].itemName;
                    int amount = slots[j].amount;
                    InventorySlot inventorySlot = AddSlots(itemName,amount );
                    newSlotsList.Add(inventorySlot);
                }


                inventoryDictionary.TryGetValue(items.data[i].inventoryName, out inventoryObject);
                if (!inventoryObject)
                {
                    // string pathSprite = "Sprites/";
                    // string pathPrefabs = "_Prefabs/";
                    inventoryObject = CreateInventoryScriptableObject(items.data[i].inventoryName, newSlotsList.ToArray());
                    dbId++;
                    // GameLog.LogMessage("Create scriptable name" + newName.name);

                    inventoryDictionary.Add(inventoryObject.name, inventoryObject);
                    CreateScriptableItemAndAddToDB();

                }
                else { 
                    
                }
            }
        }


        private static void CreateScriptableItemAndAddToDB()
        {

            inventoryDatabase.ClearInventoryDatabase();
            inventoryDatabase.inventoryArray = inventoryDictionary.Values.ToArray();
            inventoryDatabase.LoadDataToDict();
            //inventoryDatabase.GetInventoryDictionary = inventoryDictionary;
            //itemDatabase.LoadToDatabase();

            foreach (var inventoryfromdict in inventoryDictionary)
            {
                InventoryObject itemToSave = ((InventoryObject)(inventoryfromdict.Value));
                string fileName = itemToSave.name + ".asset";
                GameLog.LogMessage("name fileanme to create" + fileName);
                string savePath = "Assets/ScriptableObjects/GeneratedInventories/";
               
                GameLog.LogMessage("created file folder name:" + savePath);
                GameLog.LogMessage("file name:" + savePath + "/" + fileName);
                if (!AssetDatabase.Contains(itemToSave))
                    AssetDatabase.CreateAsset(itemToSave, savePath + "/" + fileName);
                GameLog.LogMessage("Name:" + itemToSave.name);
                
            }
            UnityEditor.AssetDatabase.Refresh();
            EditorUtility.SetDirty(inventoryDatabase);

        }



        static string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-fA-F0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private static int GetIndex(DialogLanguage language)
        {
            int index;
            switch (language)
            {
                case DialogLanguage.Polski:
                    index = 0;
                    break;
                case DialogLanguage.Niemiecki:
                    index = 1;
                    break;
                case DialogLanguage.Angielski:
                    index = 2;
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
    public class ListInventory
    {
        public InventoryData[] data;
        public static InventoryData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<InventoryData>(jsonString);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        public int _id;
        public string inventoryName;
        public InventorySlotData[] slots;
  
    }
    [System.Serializable]
    public class InventorySlotData
    {
       // public int slotID;
        public string itemName;
        public int amount;
     }
    

}
#endif