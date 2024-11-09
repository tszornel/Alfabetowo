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

//using UnityEngine.WSA;


#if UNITY_EDITOR
namespace Webaby.Utils
{
    /// <summary>
    ///  /* Setup Language version for Abecadlowo. It creates Scriptable Object for Dialog System in the language version that is choosen in GameHandler */
    /// </summary>
    public class SetupItems
    {
        [SerializeField]
        public static DialogLanguage language = UtilsClass.GetLanguageFromPrefs(PlayerPrefs.GetString("language", "Polski"));
        const string URL = "http://51.68.172.64:3002/items";
        const int RequestTimeout = 5;
        public static Dictionary<string, ItemObject> itemDictionary = new Dictionary<string, ItemObject>();
        public static List<ItemBuff> itemAttributes = new List<ItemBuff>();
        public static int itemId = 0;
        [SerializeField]
        public static ItemObjectDatabase itemDatabase;
        public static ItemObjectDatabase letterDatabase;
        public static ItemObjectDatabase allItemDatabase;
        private static NamesDatabase namesDatabase;
        private static PrefabDatabase prefabDatabase;
        private static Dictionary<string, Sprite> dictSprites = new Dictionary<string, Sprite>();
        public static SpritesDatabase allSpritesDatabase;
        // static SpriteAtlas spriteAtlas;




        /// <summary>
        /// </summary>
        // [MenuItem("Webaby/SetupLanguage/SetupItems")]
        public static void CreateLanguageFiles()
        {
            CreateLanguageFilesProcess(language,"items");
        }

        public static void CreateLanguageFilesProcess(DialogLanguage _language,string pathNames)
        {
            ClearNameFolder(pathNames);
           // CreateNameFolder(_language, pathNames);
            GameLog.LogMessage("Create language Files url=" + URL + " language=" + language);

            Uri uri = new Uri(URL);
            GameLog.LogMessage("uri host=" + uri.Host + "uri port=" + uri.Port);
            UriBuilder uriBuilder = new UriBuilder();
            GetJsonName(uri, _language,pathNames);
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
            if(!itemDatabase)
                itemDatabase = Resources.Load<ItemObjectDatabase>("ItemsDatabase");
            if (!letterDatabase)
                letterDatabase = Resources.Load<ItemObjectDatabase>("LettersDatabase");
            if (!allItemDatabase)
                allItemDatabase = Resources.Load<ItemObjectDatabase>("AllItemsDatabase");

            namesDatabase = Resources.Load<NamesDatabase>("NamesDatabase");
            prefabDatabase = Resources.Load<PrefabDatabase>("PrefabDatabase");
            allSpritesDatabase = Resources.Load<SpritesDatabase>("SpritesDatabase");
            //allSpritesDatabase.FindImages();
            string savePath = "Assets/ScriptableObjects/"+ pathNames+"/";
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
        static ItemBuff AddBuffs(string attributeName,int min, int max)
        {
            if (attributeName == null) 
                return null;

            AttributesName attributeEnum = attributeName.ToEnum<AttributesName>();
            ItemBuff itembuffs = new ItemBuff(attributeEnum, min, max);
            return itembuffs;
        }
      

        static ItemObject CreateNameScriptableObject(string itemType, int id, string name, string displayName, Sprite characterSprite, Sprite objectSpriteIcon, Sprite characterDamagedSprite, List<ItemBuff> arguments, string text, string nameObject, string itemAfterRepair, string weaponType,int price,GameObject itemPrefab, string description,int amount,bool stackable,bool damaged)
        {
            ItemObject newItem;
            ItemObjectType itemObjectType = itemType.ToEnum<ItemObjectType>();

            switch (itemObjectType)
            {
                case ItemObjectType.Coin:
                    newItem = ScriptableObject.CreateInstance<DefaultObject>();
                    newItem.type = ItemObjectType.Coin;
                    break;
                case ItemObjectType.Letter:
                    newItem =  ScriptableObject.CreateInstance<LetterObject>();
                    ((LetterObject)newItem).text = text;
                    break;
                case ItemObjectType.Equipment:
                    newItem = ScriptableObject.CreateInstance<EquipmentObject>();
                    ((EquipmentObject)newItem).WeaponType= weaponType.ToEnum<EquipmentType>();
                    break;
               case ItemObjectType.Default:
                    newItem = ScriptableObject.CreateInstance<DefaultObject>();
                    newItem.type = ItemObjectType.Default;
                    break;
                case ItemObjectType.Health:
                    newItem = ScriptableObject.CreateInstance<HealthObject>();
                    break;
                case ItemObjectType.Map:
                    newItem = ScriptableObject.CreateInstance<MapObject>();
                    ((MapObject)newItem).mapUsed = false;
                    break;
                default:
                    newItem = ScriptableObject.CreateInstance<DefaultObject>();
                    break;
            }
            newItem.ID = id;
            newItem.itemPrefab = itemPrefab;
            newItem.name = name;
            newItem.objectName = name;
            newItem.amount = amount;
            newItem.stackable = stackable;
            //newName.language = language;
            newItem.displayName = displayName;
            GameLog.LogMessage("Set Character Sprite:" + characterSprite);
            newItem.itemSprite = characterSprite;
            newItem.itemSpriteDamaged = characterDamagedSprite;
            newItem.itemIcon = objectSpriteIcon;
            newItem.price = price;
            newItem.buffs = arguments.ToArray();
            newItem.description = description;
            if (!nameObject.Equals(""))
            {
                GameLog.LogMessage("Name object is not null:" + nameObject+"\"");
                newItem.nameObject = namesDatabase.GetNameObject(nameObject);

            }
               
            else {
                GameLog.LogMessage("Name object is null:"+newItem.name);
            
            }
            return newItem;


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
        static void GetJsonName(Uri uri, DialogLanguage language,string pathNames)
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
                    
                    json = LoadResourceTextfile(pathNames);
                    GameLog.LogMessage("json from file :" + json);
                  

                    ProcessJSon(json, pathNames);
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
                    ProcessJSon(json, pathNames);


                }
            };
        }


        static void ProcessJSon(string json,string pathNames)
        {

            ListItem items = JsonUtility.FromJson<ListItem>(json);
            itemDictionary.Clear();
            
            dictSprites.Clear();

           // SpritesDatabase allSpritesDatabase = Resources.Load<SpritesDatabase>("SpritesDatabase");
           // allSpritesDatabase.FindImages();
            GameLog.LogMessage("All sprites lenght:" + allSpritesDatabase.spritesArray.Length);
            foreach (Sprite _sprite in allSpritesDatabase.spritesArray)
            {
                    GameLog.LogMessage("Add sprite:" + _sprite.name);
                    Sprite value=null;
                    if (_sprite.name.Contains("Clone")) {
                    _sprite.name.TrimEnd("(Clone)".ToCharArray());
                        GameLog.LogMessage("Add sprite:" + _sprite.name);
                    }
                    if (!dictSprites.TryGetValue(_sprite.name,out value))
                        dictSprites.Add(_sprite.name, _sprite);
             }
             foreach (var item in dictSprites)
             {
                 GameLog.LogMessage("Sprites from dict:"+item.Key);
             }
          
         
            int index = GetIndex(language);
            int dbId = 0;
          
            for (int i = 0; i < items.data.Length; i++)
            {
                ItemObject itemObject;
                itemDictionary.TryGetValue(items.data[i].objectName, out itemObject);
                if (!itemObject)
                {
                   // string pathSprite = "Sprites/";
                 //   string pathPrefabs = "_Prefabs/";
                    string objectDisplayName = items.data[i].displayName[index];
                    string objectName = items.data[i].objectName;
                    string prefabName = items.data[i].prefabName;
                   // string linkSprite = pathSprite + "items-atlas-6.0.png";
                    string itemSprite = items.data[i].itemSprite;
                    string itemSpriteIcon = itemSprite +"Icon";
                    string itemSpriteDamaged = items.data[i].itemSpriteDamaged;
                   // string linkPrefab = pathPrefabs + prefabName;
                    string description = items.data[i].description[index];
                    bool stackable = items.data[i].stackable;
                    int amount = items.data[i].amount;
                    bool damaged = items.data[i].damaged;
                    // Sprite objectSprite = Resources.Load<Sprite>(linkSprite);
                    // SpriteAtlas atlas = Resources.Load<SpriteAtlas>(linkSprite);
                    Sprite objectSprite;// = spriteAtlas.GetSprite(itemSprite);
                    dictSprites.TryGetValue(itemSprite,out objectSprite);

                    Sprite objectSpriteIcon;// = spriteAtlas.GetSprite(itemSprite);
                    dictSprites.TryGetValue(itemSpriteIcon, out objectSpriteIcon);
                    Sprite objectSpriteDamaged=null;// = spriteAtlas.GetSprite(itemSprite);
                    if(itemSpriteDamaged!=null)
                        dictSprites.TryGetValue(itemSpriteDamaged, out objectSpriteDamaged);

                    GameLog.LogMessage("Object sprite for objectName:" + objectName + " itemSprite:" + itemSprite);
                    GameObject itemPrefab =null;
                    if (!prefabName.Equals(""))
                        itemPrefab = prefabDatabase.GetPrefabObject(prefabName);
                    int id = items.data[i]._id;
                    string letterText = items.data[i].letterText;
                    string nameRef = items.data[i].nameRef;
                    if (!nameRef.Equals(""))
                        GameLog.LogMessage("name ref for repaire object" + nameRef);
                    string itemAfterRepair = items.data[i].itemAfterRepair;
                    string equipmentTypeName = items.data[i].equipmentTypeName;
                    List <ItemBuff> argumentList = new List<ItemBuff>();
                    ItemBuffsData[] itemBuffs = items.data[i].buffs;
                    string itemType = items.data[i].itemType;
                    int price = items.data[i].price;


                    switch (itemType?.ToEnum<ItemObjectType>())
                    {
                        default:
                            break;
                    }

                    GameLog.LogMessage("itemBuffs.Length:" + itemBuffs.Length);
                    if (itemBuffs.Length!=0)
                    for (int j = 0; j < itemBuffs.Length; j++)
                    {
                       
                        ItemBuff itemAttributes = AddBuffs(itemBuffs[j].AttributeName, itemBuffs[j].minValue, itemBuffs[j].maxValue);
                        argumentList.Add(itemAttributes);
                    }
                    ItemObject newItem = CreateNameScriptableObject(itemType, dbId, objectName, objectDisplayName, objectSprite, objectSpriteIcon, objectSpriteDamaged, argumentList, letterText, nameRef, itemAfterRepair, equipmentTypeName, price, itemPrefab, description,amount, stackable, damaged);
                    dbId++;
                    // GameLog.LogMessage("Create scriptable name" + newName.name);

                    itemDictionary.Add(newItem.name, newItem);

                    CreateScriptableFile(newItem,pathNames);
                    AddItemsToDB(allItemDatabase, itemDictionary);
                    if (pathNames.Equals("Items"))
                        AddItemsToDB(itemDatabase, itemDictionary);
                    else
                        AddItemsToDB(letterDatabase, itemDictionary);

                }
                if (pathNames.Equals("Items"))
                     AddItemsToDB(itemDatabase,itemDictionary);
                else
                    AddItemsToDB(letterDatabase, itemDictionary);
            }

            
           
        }




        private static void CreateScriptableFile(ItemObject itemObjectInstance,string pathNames) {


            ItemObject itemToSave = itemObjectInstance;
            string fileName = itemToSave.name + ".asset";
            GameLog.LogMessage("name fileanme to create" + fileName);
            string savePath = "Assets/ScriptableObjects/"+ pathNames+"/";



            string[] setLabels = new string[] { "item", itemToSave.name };
            AssetDatabase.SetLabels(itemToSave, setLabels);
            GameLog.LogMessage("created file folder name:" + savePath);
            GameLog.LogMessage("file name:" + savePath + "/" + fileName);
            if (!AssetDatabase.Contains(itemToSave))
                AssetDatabase.CreateAsset(itemToSave, savePath + "/" + fileName);
            GameLog.LogMessage("Name:" + itemToSave.name);
            UnityEditor.AssetDatabase.Refresh();

        }
        private static void AddItemsToDB(ItemObjectDatabase database, Dictionary<string, ItemObject> _itemDictionary)
        {
            database.ClearToDatabase();
            database.items = _itemDictionary.Values.ToArray();
            database.LoadToDatabase();
            UnityEditor.AssetDatabase.Refresh();
            EditorUtility.SetDirty(database);

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
    public class ListItem
    {
        public ItemData[] data;
        public static ListItem CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ListItem>(jsonString);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class ItemData
    {
        public int _id;
        public string objectName;
        public string[] displayName;
        public string itemType;
        public string itemSprite;
        public string itemSpriteDamaged;
        public int amount;
        public string[] description;
        public int price;
        public int restoreHealthAmount;
        public string prefabName;
        public string equipmentTypeName;
        public string letterText;
        public string nameRef;
        public string itemAfterRepair;
        public bool stackable;
        public bool damaged;
        public ItemBuffsData[] buffs;
  
    }
    [System.Serializable]
    public class ItemBuffsData
    {
        public string AttributeName;
        public int minValue;
        public int maxValue;

    }
    

}
#endif