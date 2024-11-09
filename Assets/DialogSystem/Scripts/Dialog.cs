using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

[System.Serializable]
public enum DialogLanguage
{
    Polski,
    Niemiecki,
    Angielski,
    Francuski
}
[System.Serializable]
public struct DialogSegment
{
    [TextArea(3, 10)]
    public string lineOfText;
    public float dialogLineDisplayTime;
    public string characterName;
    public TMP_SpriteAsset characterImage;
    public AudioClip audio;
}
[CreateAssetMenu(fileName = "New Dialog ", menuName = "Dialog System/Dialog")]
public class Dialog : ScriptableObject
{
    public int Id;
    public string dialogName;
    public List<DialogSegment> dialogSegments = new List<DialogSegment>();
    public DialogLanguage language;
    public bool dialogPlayed;
    public bool hasShort;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string shortDialogName; 


    public string savePath;
    [ContextMenu("Save")]
    public void Save()
    {
        GameLog.LogMessage("Execute Save Dialog");
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        GameLog.LogMessage(Application.persistentDataPath + savePath);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        GameLog.LogMessage("Execute Load Inventory");
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
        }
    }

    public void PlayDialog() { 
    
        DialogManager.Instance.StartDialog(this);   
    
    
    }

    public string GetDialogShortName() {

        if (hasShort)
            return shortDialogName;
        else
            return null;
    
    }

    
}
