using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
[CreateAssetMenu(fileName = "NamesDatabase", menuName = "Name System/Database")]
public class NamesDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public NameObject[] namesArray;
    // public DialogLanguage[] languages;
    public Dictionary<DialogLanguage, List<NameObject>> GetNamesDictionary = new Dictionary<DialogLanguage, List<NameObject>>();
    public Dictionary<string, NameObject> GetNamesByNameDictionary = new Dictionary<string, NameObject>();
    NameObject test;
    public void OnBeforeSerialize()
    {
    }
    public void OnAfterDeserialize()
    {
        GetNamesDictionary.Add(DialogLanguage.Polski, new List<NameObject>());
        GetNamesDictionary.Add(DialogLanguage.Angielski, new List<NameObject>());
        GetNamesDictionary.Add(DialogLanguage.Niemiecki, new List<NameObject>());
        for (int i = 0; i < namesArray.Length; i++)
        {
            //  GetNamesByNameDictionary.Add(namesArray[i].name, namesArray[i]);
            GetNamesDictionary[namesArray[i].language].Add(namesArray[i]);
        }
    }
    public NameObject[] GetAllNames()
    {
        return namesArray;
    }
    public NameObject[] GetAllDoneNames()
    {
        List<NameObject> result = new List<NameObject>();
        foreach (var item in namesArray)
        {
            if (item.done)
                result.Add(item);
        }
        return result.ToArray();
    }
    public string[] GetNamesArray()
    {
        List<string> stringNamesList = new List<string>();
        for (int i = 0; i < namesArray.Length; i++)
        {
            stringNamesList.Add(namesArray[i].name);
        }
        return stringNamesList.ToArray();
    }
    public NameObject GetNameObject(string name)
    {
        NameObject nameObject = null;
        for (int i = 0; i < namesArray.Length; i++)
        {
            if (namesArray[i].name.Equals(name))
            {
                nameObject = namesArray[i];
            }
        }
        return nameObject;
    }
    public void printDatabase()
    {
        foreach (KeyValuePair<string, NameObject> name in GetNamesByNameDictionary)
        {
            GameLog.LogMessage("Name key:" + name.Key + "Name Value" + name.Value);
        }
        foreach (KeyValuePair<DialogLanguage, List<NameObject>> name in GetNamesDictionary)
        {
            GameLog.LogMessage("Name key:" + name.Key + "Name Value" + name.Value);
            foreach (NameObject x in name.Value)
            {
                GameLog.LogMessage("Name :" + x.name);
            }
        }
    }
    public void AddNameToDatabase(NameObject name)
    {
        GetNamesDictionary[name.language].Add(name);
    }
    public void ClearToDatabase(DialogLanguage language)
    {
        GetNamesDictionary[language].Clear();
        GetNamesByNameDictionary.Clear();
    }
    public void LoadToDatabase()
    {
        for (int i = 0; i < namesArray.Length; i++)
        {
            GetNamesDictionary[namesArray[i].language].Add(namesArray[i]);
        }
    }
    public void loadDataToDict(DialogLanguage language)
    {
        foreach (NameObject name in GetNamesDictionary[language])
        {
            GetNamesByNameDictionary.TryGetValue(name.name, out test);
            if (!test)
                GetNamesByNameDictionary.Add(name.name, name);
        }
    }
}
