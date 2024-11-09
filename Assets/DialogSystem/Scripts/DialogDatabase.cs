using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DialogDatabase", menuName = "Dialog System/Database")]
public class DialogDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public Dialog[] dialogArray;
    // public DialogLanguage[] languages;
    public Dictionary<DialogLanguage, List<Dialog>> GetDialogDictionary = new Dictionary<DialogLanguage, List<Dialog>>();
    public Dictionary<string, Dialog> GetDialogByNameDictionary = new Dictionary<string, Dialog>();
    public void OnBeforeSerialize()
    {
       // GetDialogDictionary = new Dictionary<DialogLanguage, List<Dialog>>();
    }

    public void ResetDialogs()
    {
        for (int i = 0; i < dialogArray.Length; i++)
        {
            dialogArray[i].dialogPlayed = false;
        }


    }
    public string[] GetNamesArray()
    {
        List<string> stringNamesList = new List<string>();
        for (int i = 0; i < dialogArray.Length; i++)
        {
            stringNamesList.Add(dialogArray[i].name);
        }
        return stringNamesList.ToArray();

    }
    public void LoadDataToDict()
    {
        for (int i = 0; i < dialogArray.Length; i++)
        {
            GetDialogByNameDictionary.Add(dialogArray[i].dialogName, dialogArray[i]);
        }

    }
    public void OnAfterDeserialize()
    {
        GetDialogDictionary.Add(DialogLanguage.Polski, new List<Dialog>());
        GetDialogDictionary.Add(DialogLanguage.Angielski, new List<Dialog>());
        GetDialogDictionary.Add(DialogLanguage.Niemiecki, new List<Dialog>());
        for (int i = 0; i < dialogArray.Length; i++)
        {
          
                GetDialogDictionary[dialogArray[i].language].Add(dialogArray[i]);
          
                          
        }
    
    }

    public Dialog GetDialogByName(string name) {

        for (int i = 0; i < dialogArray.Length; i++)
        {
            if (dialogArray[i].name == name)
                return dialogArray[i];
        }
        GameLog.LogError("return null for name " + name);
        return null;


    }

    public Dialog[] GetDialogsFromNameArray(string[] names)
    {
        List<Dialog> dialogs = new List<Dialog> ();

        for (int j = 0; j < names.Length; j++)
        {
            for (int i = 0; i < dialogArray.Length; i++)
            {
                if (dialogArray[i].name == names[j])
                    dialogs.Add(dialogArray[i]);
            }
        }
       
       
        return dialogs.ToArray();


    }

    public void AddDialogToDatabase(Dialog dialog) {

        
        GetDialogDictionary[dialog.language].Add(dialog);
    }


    public void ClearToDatabase(DialogLanguage language)
    {
        GetDialogDictionary[language].Clear();
        GetDialogByNameDictionary.Clear();  
    }

}