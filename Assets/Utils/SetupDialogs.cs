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
using TMPro;



//using System.Text.Json;


#if UNITY_EDITOR
namespace Webaby.Utils
{
    /// <summary>
    ///  /* Setup Language version for Abecadlowo. It creates Scriptable Object for Dialog System in the language version that is choosen in GameHandler */
    /// </summary>
    public class SetupDialogs
    {
        [SerializeField]
        public static DialogLanguage language = UtilsClass.GetLanguageFromPrefs(PlayerPrefs.GetString("language", "Polski"));
        const string URL = "http://51.68.172.64:3005/dialogs";
        const int RequestTimeout = 5;
        public static Dictionary<string, Dialog> dialogDictionary = new Dictionary<string, Dialog>();
        public static List<DialogSegment> dialogSegments = new List<DialogSegment>();
        public static int dialogId = 0;
        [SerializeField]
        public static DialogDatabase database;
        public static AudioDatabase audioDatabase;
        private static Dictionary<string, Sprite> dictSprites = new Dictionary<string, Sprite>();

        private static Dictionary<string, TMP_SpriteAsset> dictSpritesAssets = new Dictionary<string, TMP_SpriteAsset>();
        public static SpritesDatabase allSpritesDatabase;
        public static TMPSpriteAssetDatabase tmpSpriteAssetDatabase;


        /// <summary>
        /// </summary>
       // [MenuItem("Webaby/SetupLanguage/SetupDialogs")]
        public static void CreateLanguageFiles()
        {
            CreateLanguageProcess(language);
        }

       
        public static void CreateLanguageProcess(DialogLanguage _language) {

            ClearLanguageFolder();
            CreateFolder(_language);
            GameLog.LogMessage("Create language Files url=" + URL + " language=" + language);
            //Kasujemy zawartowsc katalogu z dialogami danego jezyka:
            // Assets/DialogSystem/ScriptableObjects/PL
            //Sciagamy dane zprogramu zewnetrzenego 
            Uri uri = new Uri(URL);
            GameLog.LogMessage("uri host=" + uri.Host + "uri port=" + uri.Port);
            UriBuilder uriBuilder = new UriBuilder();
            GetJsonDialog(uri, _language);



        }

       /* public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }*/

        /*public static void WriteToJsonFileIntendent(string data) 
        {
            JsonDocument jdoc = JsonDocument.Parse(data);

            var fileName = @"data.json";
            using FileStream fs = File.OpenWrite(fileName);

            using var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true });
            jdoc.WriteTo(writer);

        }*/

        //[MenuItem("Webaby/SetupLanguage/Create Dialog Language Folder")]
        static void CreateFolder(DialogLanguage _language)
        {
            string savePath = "Assets/ScriptableObjects/GeneratedDialogs/" + GetLanguageJsonFieldName(_language.ToString()) + "/";
            GameLog.LogMessage("savePath:" + savePath);
            // UnityEditor.AssetDatabase.Refresh();
            string guid = AssetDatabase.CreateFolder("Assets/ScriptableObjects/GeneratedDialogs/", GetLanguageJsonFieldName(_language.ToString()) + "/");
            GameLog.LogMessage("GUID:" + guid);
            //UnityEditor.AssetDatabase.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
       // [MenuItem("Webaby/SetupLanguage/Clear Dialog Language Folders")]
        static void ClearLanguageFolder()
        {
            database = Resources.Load<DialogDatabase>("DialogDatabase");
            audioDatabase = Resources.Load<AudioDatabase>("AudioDatabase");
            allSpritesDatabase = Resources.Load<SpritesDatabase>("SpritesDatabase");
            tmpSpriteAssetDatabase = Resources.Load<TMPSpriteAssetDatabase>("TMPSpritesAssetDatabase");

            //  allSpritesDatabase.FindImages();
            //  audioDatabase.FindAudio();
            string savePath = "Assets/ScriptableObjects/GeneratedDialogs/";
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
        static DialogSegment AddSegment(string dialogName,string characterSpriteName, string dialogText, float time, AudioClip audioClip, string characterName)
        {
            DialogSegment segment = new DialogSegment();
            segment.lineOfText = dialogText;
            segment.characterName = characterName;
            GameLog.LogMessage("adding dialog text" + dialogText);
            if (time == 0)
                segment.dialogLineDisplayTime = dialogText.Length * 0.1f;
            else
                segment.dialogLineDisplayTime = (time * 0.01f)+0.01f;
          //  string path = "Audio/";
           // string linkAudio = path + dialogName +"/" +filename;
          //  GameLog.LogMessage("path audio clip:" + linkAudio);

            string pathSprite = "Sprites/";
            string linkSprite = pathSprite + characterSpriteName;

            if (!characterSpriteName.Equals("") || characterSpriteName!="") { 
                GameLog.LogMessage("Looking for character Sprite" + characterSpriteName);
                TMP_SpriteAsset characterSprite = dictSpritesAssets[characterSpriteName];//Resources.Load<Sprite>(linkSprite);
                if (characterSprite)
                    segment.characterImage = characterSprite;
                else
                    GameLog.LogMessage("not found sprite:" + linkSprite);

            }

            
            if (!audioDatabase)
                audioDatabase = Resources.Load<AudioDatabase>("AudioDatabase");
          
            if (audioClip) { 
                segment.audio = audioClip;
           
                GameLog.LogMessage("Display audio clip name:" + audioClip.name);
            }
           // else
           //     GameLog.LogMessage("not found clip:" + linkAudio) ;
            return segment;
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
        static void GetJsonDialog(Uri uri, DialogLanguage language)
        {
            // GameLog.LogMessage("GetJsonDialog entered");
            FieldInfo[] property_infos = typeof(DialogData).GetFields();
           /* foreach (var item in property_infos)
            {
                GameLog.LogMessage(item.Name);
            }*/
            UnityWebRequest _unityWebRequest = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbGET);
            _unityWebRequest.timeout = RequestTimeout;
            _unityWebRequest.SetRequestHeader("Accept", "application/json, text/plain, */*");

            _unityWebRequest.SetRequestHeader("Accept_encoding", "gzip, deflate");
            _unityWebRequest.downloadHandler = new DownloadHandlerBuffer();
            string json="";

            ProcessJsonFromFile(json);
            /*_unityWebRequest.SendWebRequest().completed += (AsyncOperation) =>
            {
                GameLog.LogMessage("Receive");

                if (_unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    GameLog.LogMessage(_unityWebRequest.error);
                    string pathNames = "dialogs";
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

               
            };*/



        }



        static void ProcessJsonFromFile(string json) {

            
            string pathNames = "dialogs";
            json = LoadResourceTextfile(pathNames);
            GameLog.LogMessage("json from file :" + json);
            ProcessJSon(json);

        }

        private static void ProcessJSon(string json) {


                ListDialog items = JsonUtility.FromJson<ListDialog>(json);

        //    Sprite[] facesSprites = Resources.LoadAll<Sprite>("Sprites/names-atlas");
         //   Sprite[] itemsSprites = Resources.LoadAll<Sprite>("Sprites/items-atlas_2");
         //   Sprite[] damagedItemsSprites = Resources.LoadAll<Sprite>("Sprites/atlas-damaged-items");
            dictSprites.Clear();
            dictSpritesAssets.Clear();

           foreach (TMP_SpriteAsset _spriteAsset in tmpSpriteAssetDatabase.spritesArray)
            {
                GameLog.LogMessage("Add sprite asset:" + _spriteAsset.name);
                TMP_SpriteAsset value = null;
                if (_spriteAsset.name.Contains("Clone"))
                {
                    _spriteAsset.name.TrimEnd("(Clone)".ToCharArray());
                    GameLog.LogMessage("Add sprite:" + _spriteAsset.name);
                }
                if (!dictSpritesAssets.TryGetValue(_spriteAsset.name, out value))
                    dictSpritesAssets.Add(_spriteAsset.name, _spriteAsset);
            }

            foreach (var _spriteAssetName in dictSpritesAssets)
            {
                GameLog.LogMessage("Sprite Asset name in dict"+ _spriteAssetName.Key);
            }


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
            //string jsonFilepath = "Assets/Resources/SetupData/dialogs.json";
            //WriteToJsonFile(jsonFilepath, items);
            dialogDictionary.Clear();
                for (int i = 0; i < items.data.Length; i++)
                {
                    // Debug.Log(items.data[i].pl);
                    // string dialogText = items.data[i].GetFieldValue<string, DialogData>(GetLanguageJsonFieldName(PlayerPrefs.GetString("language", "Polski")));
                    // 
                    //  GameLog.LogMessage(dialogText);
                    Dialog dialog;
                    dialogDictionary.TryGetValue(items.data[i].dialogName, out dialog);
                    if (!dialog)
                    {
                        Dialog newDialog = ScriptableObject.CreateInstance<Dialog>();
                        newDialog.Id = ++dialogId;
                        newDialog.shortDialogName = items.data[i].shortDialogName;
                        newDialog.hasShort = items.data[i].hasShort;
                        newDialog.dialogName = items.data[i].dialogName;
                        newDialog.language = language;
                        int index = GetIndex(language);
                        GameLog.LogMessage("Language index:" + index);
                        GameLog.LogMessage("Creating dialog with name" + newDialog.dialogName);
                        GameLog.LogMessage("dialogShortName:" + items.data[i].shortDialogName);
                        GameLog.LogMessage("hasShort:" + items.data[i].hasShort);
                    List<DialogSegment> newSegmentList = new List<DialogSegment>();
                        GameLog.LogMessage("Lenht" + items.data[i].segments.Length);
                        for (int j = 0; j < items.data[i].segments.Length; j++)
                        {
                            DialogSegmentData data = items.data[i].segments[j];
                            string text = data.GetFieldValue<string, DialogSegmentData>(GetLanguageJsonFieldName(language.ToString()));
                            GameLog.LogMessage("FileName i " + i + "filename j " + j + " text:" + text);

                            GameLog.LogMessage("Dialog" + newDialog.dialogName + " segment filename lenght" + items.data[i].segments[j].fileName.Length);
                            string filename = items.data[i].segments[j].fileName[index];
                            string characterName = items.data[i].segments[j].characterName[index];
                            string characterSpriteName = items.data[i].segments[j].characterSprite;
                       
                        GameLog.LogMessage(characterSpriteName + "index" + index + " i " + i + " j:" + j + "textDuration:" + items.data[i].segments[j].textDuration[index]);
                        AudioClip audioClip=null;
                        if (filename != null)
                        {
                            GameLog.LogMessage("Szukamy filename audio:" + filename);
                            audioClip = audioDatabase.GetAudioClipObject(filename);
                        }//(linkAudio);
                        else
                            GameLog.LogMessage("Brakuje filename dla dialogu" + text);
                        string textDuration = "0";
                        if (audioClip != null)
                            textDuration = (audioClip.length).ToString("f3");
                        DialogSegment newSegment = AddSegment(newDialog.dialogName, characterSpriteName, text, float.Parse(textDuration)/* items.data[i].segments[j].textDuration[index]*/, audioClip, characterName);
                            newSegmentList.Add(newSegment);
                        }
                        newDialog.dialogSegments = newSegmentList;
                        GameLog.LogMessage("Adding new Dialog:" + newDialog.dialogName);
                        dialogDictionary.Add(newDialog.dialogName, newDialog);
                        CreateScriptableObjectAndAddToDB();
                    }
                    else
                    {
                        GameLog.LogMessage("Dialog already exists in dictionary- add new segment into it:" + dialog.dialogName);
                    }
                    //clear database

                }

            EditorUtility.SetDirty(database);

        }



        private static void CreateScriptableObjectAndAddToDB()
        {
            database.ClearToDatabase(language);
            database.dialogArray = dialogDictionary.Values.ToArray();
            database.LoadDataToDict();
            //foreach (var dialogfromdict in dialogDictionary)

            var dialogfromdict = dialogDictionary.Values.Last();
            {
                Dialog dialogToSave = dialogfromdict;
                string fileName = dialogToSave.dialogName + ".asset";
                GameLog.LogMessage("dialog fileanme to create" + fileName);
                string savePath = "Assets/ScriptableObjects/GeneratedDialogs";
                dialogToSave.savePath = savePath;
                GameLog.LogMessage("created file folder name:" + savePath);
                GameLog.LogMessage("file name:" + savePath + "/" + fileName);
                if (!AssetDatabase.Contains(dialogToSave))
                    AssetDatabase.CreateAsset(dialogToSave, savePath + "/" + fileName);
                GameLog.LogMessage("Dialog Name" + dialogToSave.dialogName);
                //Add dialog to database
                database.AddDialogToDatabase(dialogToSave);
                foreach (DialogSegment dialogLine in dialogToSave.dialogSegments)
                {
                    GameLog.LogMessage("Dialog line:" + dialogLine.lineOfText);
                    if (dialogLine.audio)
                        GameLog.LogMessage(dialogLine.audio.name);
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
    public class ListDialog
    {
        public DialogData[] data;
        public static ListDialog CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ListDialog>(jsonString);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public class DialogData
    {
        public string _id;
        public string dialogName;
        public bool hasShort;
        public string shortDialogName;
        public DialogSegmentData[] segments;
    }
    [System.Serializable]
    public class DialogSegmentData
    {
        public string[] fileName;
        public Double[] textDuration;
        public string[] characterName;
        public string characterSprite;
        public string pl;
        public string en;
        public string de;
    }
    /// <summary>
    /// 
    /// </summary>
    /*public static class Ex
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFieldType"></typeparam>
        /// <typeparam name="TObjectType"></typeparam>
        /// <param name="obj"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
       
    }*/
}
#endif