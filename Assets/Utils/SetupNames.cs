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
using System.Runtime.InteropServices.ComTypes;


#if UNITY_EDITOR
namespace Webaby.Utils
{
    /// <summary>
    ///  /* Setup Language version for Abecadlowo. It creates Scriptable Object for Dialog System in the language version that is choosen in GameHandler */
    /// </summary>
    public class SetupNames
    {
        [SerializeField]
        public static DialogLanguage language = UtilsClass.GetLanguageFromPrefs(PlayerPrefs.GetString("language", "Polski"));
        const string URL = "http://51.68.172.64:3002/names";
        const int RequestTimeout = 5;
        public static Dictionary<string, NameObject> nameDictionary = new Dictionary<string, NameObject>();
        public static List<DialogSegment> nameSegments = new List<DialogSegment>();
        public static int nameId = 0;
        [SerializeField]
        private static NamesDatabase nameDatabase;
        private static ItemObjectDatabase letterDatabase;
        private static Dictionary<string, Sprite> dictSprites = new Dictionary<string, Sprite>();
        private static AudioDatabase audioDatabase;

        char[] vowelsCharTable = new[] { 'a', 'e', 'i', 'o', 'y' ,'u'};
        static string vowels = "aeioyu";

        /// <summary>
        /// </summary>
        // [MenuItem("Webaby/SetupLanguage/SetupNames")]
        public static void CreateLanguageFiles()
        {
            CreateLanguageFilesProcess(language);   
        }

        public static void CreateLanguageFilesProcess(DialogLanguage _language)
        {
            ClearNameFolder();
            CreateNameFolder();
            GameLog.LogMessage("Create language Files url=" + URL + " language=" + language);
            //Kasujemy zawartosc katalogu z dialogami danego jezyka:
            // Assets/DialogSystem/ScriptableObjects/PL
            //Sciagamy dane zprogramu zewnetrzenego 
            Uri uri = new Uri(URL);
            GameLog.LogMessage("uri host=" + uri.Host + "uri port=" + uri.Port);
            UriBuilder uriBuilder = new UriBuilder();
            GetJsonName(uri, _language);
        }



        //   [MenuItem("Webaby/SetupLanguage/Create Name Folder")]
        static void CreateNameFolder()
        {
            string savePath = "Assets/ScriptableObjects/GeneratedNames/" + GetLanguageJsonFieldName(language.ToString()) + "/";
            GameLog.LogMessage("savePath:" + savePath);
            // UnityEditor.AssetDatabase.Refresh();
            string guid = AssetDatabase.CreateFolder("Assets/ScriptableObjects/GeneratedNames", GetLanguageJsonFieldName(language.ToString()));
            GameLog.LogMessage("GUID:" + guid);
            //UnityEditor.AssetDatabase.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
      //  [MenuItem("Webaby/SetupLanguage/Clear Name Folders")]
        static void ClearNameFolder()
        {
            nameDatabase = Resources.Load<NamesDatabase>("NamesDatabase");
            letterDatabase = Resources.Load<ItemObjectDatabase>("LettersDatabase");
            audioDatabase = Resources.Load<AudioDatabase>("AudioDatabase");

            //PickupItemAssets.Instance.namesDatabase;
            string savePath = "Assets/ScriptableObjects/GeneratedNames";
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
        static NameSegment AddSegment(string itemName,bool active)
        {
            NameSegment segment = new NameSegment();
            ItemObject itemFromObject = letterDatabase.GetItemObjectFromName(itemName);
            GameLog.LogMessage("itemName:"+itemName+" itemFromObject:" + itemFromObject);
            segment.nameLetter = itemFromObject.createItem();
            if(itemName != null && !itemName.Equals("") && vowels.Contains(itemName.ToLower()))
                segment.letterSound = audioDatabase.GetAudioClipObject(itemName.ToLower());
            GameLog.LogMessage("Adding segment letter with ID:" + segment.nameLetter.Id);
            segment.nameLetter.SetLocation(ItemLocation.Name);
            segment.active = active;
            segment.initialActive = active; 
            return segment;
        }


        static NameObject CreateNameScriptableObject(DialogLanguage language, int id, string name, string displayName,Sprite characterSprite, List<NameSegment> nameSegments) {


            NameObject newName = ScriptableObject.CreateInstance<NameObject>();
            newName.name = name;
            newName.language = language;
            newName.displayedName = displayName;
            if (characterSprite != null)
                newName.characterImage = characterSprite;
            newName.nameLetters = nameSegments;
            newName.done = false;
            return newName;


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
                    string pathNames = "names";
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


        static void ProcessJSon(string json) {
             
            ListName items = JsonUtility.FromJson<ListName>(json);
            nameDictionary.Clear();

          //  Sprite[] facesSprites = Resources.LoadAll<Sprite>("Sprites/names-atlas");
           // Sprite[] itemsSprites = Resources.LoadAll<Sprite>("Sprites/items-atlas_2");
          //  Sprite[] damagedItemsSprites = Resources.LoadAll<Sprite>("Sprites/atlas-damaged-items");
            dictSprites.Clear();

            /*  foreach (Sprite sprite in facesSprites)
              {
                  dictSprites.Add(sprite.name, sprite);
                  GameLog.LogMessage("Sprite:" + sprite.name + " added to dict");
              }
              foreach (Sprite sprite in itemsSprites)
              {
                  dictSprites.Add(sprite.name, sprite);
                  GameLog.LogMessage("Sprite:" + sprite.name + " added to dict");
              }

              foreach (Sprite sprite in damagedItemsSprites)
              {
                  dictSprites.Add(sprite.name, sprite);
                  GameLog.LogMessage("Sprite:" + sprite.name + " added to dict");
              }
  */
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

            int index = GetIndex(language);
            for (int i = 0; i < items.data.Length; i++)
            {
                NameObject nameObject;
                nameDictionary.TryGetValue(items.data[i].name, out nameObject);
                if (!nameObject)
                {
                   // string pathSprite = "Sprites/";
                    string characterName = items.data[i].displayName[index];
                    string spriteName = items.data[i].spriteName;
                    GameLog.LogMessage("Searched sprite Name" + spriteName);
                    Sprite characterSprite=null;
                    if (spriteName!="")
                         characterSprite = dictSprites[items.data[i].spriteName];//Resources.Load<Sprite>(linkSprite);
                    string name = items.data[i].name;
                    List<NameSegment> newSegmentList = new List<NameSegment>();
                    NameSegmentData[] word = items.data[i].GetFieldValue<NameSegmentData[], NameData>(GetLanguageJsonFieldName(language.ToString()));
                    for (int j = 0; j < word.Length; j++)
                    {
                        NameSegmentData data = word[j];
                        string itemName = word[j].itemName;
                       
                        bool active = word[j].active;
                        NameSegment newNameSegment = AddSegment(itemName, active);
                        newSegmentList.Add(newNameSegment);
                    }
                    NameObject newName = CreateNameScriptableObject(language, index, name, characterName, characterSprite, newSegmentList);
                    GameLog.LogMessage("Create scriptable name" + newName.name);
                    nameDictionary.Add(newName.name, newName);
                    CreateScriptableNameAndAddToDB();

                }
            }


            EditorUtility.SetDirty(nameDatabase);
        }


        private static void CreateScriptableNameAndAddToDB()
        {
            nameDatabase.ClearToDatabase(language);
            nameDatabase.namesArray = nameDictionary.Values.ToArray();
            nameDatabase.LoadToDatabase();
            
            foreach (var namefromdict in nameDictionary)
            {
                NameObject nameToSave = ((NameObject)(namefromdict.Value));
                string fileName = nameToSave.name + ".asset";
                GameLog.LogMessage("name fileanme to create" + fileName);
                string savePath = "Assets/ScriptableObjects/GeneratedNames/";
                nameToSave.savePath = savePath;
                GameLog.LogMessage("created file folder name:" + savePath);
                GameLog.LogMessage("file name:" + savePath + "/" + fileName);
                if (!AssetDatabase.Contains(nameToSave))
                    AssetDatabase.CreateAsset(nameToSave, savePath + "/" + fileName);
                GameLog.LogMessage("Name:" + nameToSave.name);
                //Add dialog to database
                nameDatabase.AddNameToDatabase(nameToSave);
                foreach (NameSegment nameLetter in nameToSave.nameLetters)
                {
                    GameLog.LogMessage("Name Item" + nameLetter.nameLetter.ToString()+" active:"+ nameLetter.active);
                   
                }
            }
            UnityEditor.AssetDatabase.Refresh();
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
    public class ListName
    {
        public NameData[] data;
        public static ListName CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ListName>(jsonString);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class NameData
    {
        public string _id;
        public string name;
        public string[] displayName;
        public string spriteName;
        public NameSegmentData[] pl;
        public NameSegmentData[] en;
        public NameSegmentData[] de;


    }
    [System.Serializable]
    public class NameSegmentData
    {
        public Boolean active;
        public string itemName;

    }

   
}
#endif