using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Webaby/PrefabDatabase")]
public class PrefabDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    // Start is called before the first frame update
    public GameObject[] prefabArray;


    public void OnBeforeSerialize()
    {

    }
    public void OnAfterDeserialize()
    {


    }


    public string[] GetPrefabArray()
    {

        List<string> stringNamesList = new List<string>();

        for (int i = 0; i < prefabArray.Length; i++)
        {
            stringNamesList.Add(prefabArray[i].name);
        }


        return stringNamesList.ToArray();

    }



    public GameObject GetPrefabObject(string name)
    {

        GameObject prefabObject = null;

        for (int i = 0; i < prefabArray.Length; i++)
        {
            if (prefabArray[i].name.Equals(name))
            {

                prefabObject = prefabArray[i];


            }
        }

        return prefabObject;

    }

}
