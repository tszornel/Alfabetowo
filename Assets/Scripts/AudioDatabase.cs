using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[System.Serializable]
[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Webaby/AudioDatabase")]
public class AudioDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    static string soundDirectory = "Assets/Sounds/DialogsAudio";
    public  AudioClip[] audioArray;
    //public Dictionary<AudioClip, string> audioLenghtDict;
    public string savePath;
    static AudioClip audioClip;

    private static List<AudioClip> audioClipList;

    public void OnBeforeSerialize()
    {

    }
    public void OnAfterDeserialize()
    {

    }

    [ContextMenu("FindAudioFiles")]
    public void GetAudioClips() {
        audioArray = FindAudio();
    }

    public static AudioClip[] FindAudio()
    {
        audioClipList = new List<AudioClip>();
      //  audioLenghtDict = new Dictionary<AudioClip, string>();
        // Process the list of files found in the directory.
        audioClipList?.Clear();
        SearchDirectoryAsync(soundDirectory);
        AudioClip[] audioArray = audioClipList.ToArray();
        return audioArray;
        
       // GameLog.LogMessage("audioClipList count po wyjsciu:" + audioClipList.Count);

    }

    [ContextMenu("Save")]
    public void Save()
    {
        //GameLog.LogMessage("Execute Save Inventory");
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat("C:/Users/tszor/Abecadlowo2022/Abecadlowo2021/Assets/", savePath));
        GameLog.LogMessage("Saved to C:/Users/tszor/Abecadlowo2022/Abecadlowo2021/Assets/ " + savePath);
        bf.Serialize(file, saveData);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        //GameLog.LogMessage("Execute Load Inventory");
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat("C:/Users/tszor/Abecadlowo2022/Abecadlowo2021/Assets/", soundDirectory), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
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

                GameLog.LogMessage("Mp3 file:" + filePathNew);
                AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(filePathNew);
                audioClipList.Add(audioClip);

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


    public string[] GetAudioArray()
    {

        List<string> stringNamesList = new List<string>();

        for (int i = 0; i < audioArray.Length; i++)
        {
            stringNamesList.Add(audioArray[i].name);
        }


        return stringNamesList.ToArray();

    }



    public AudioClip GetAudioClipObject(string name)
    {

        AudioClip audioObject = null;
        if (name == null || name.Equals("")) 
        {

            GameLog.LogMessage("Brakujacy name");
            return null;
        }
           

        for (int i = 0; i < audioArray.Length; i++)
        {
            if (audioArray[i].name.Equals(name))
            {

                audioObject = audioArray[i];


            }
        }

        return audioObject;

    }

}
#endif