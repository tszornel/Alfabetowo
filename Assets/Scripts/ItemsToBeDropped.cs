using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "_itemsToDrop", menuName = "Webaby/ItemsToDrop")]
public class ItemsToBeDropped : ScriptableObject
{
    ItemObjectDatabase allItemDatabase;
    public string description;
    [StringInList(typeof(PropertyDrawerHelper), "AllItemsAndLetters")]
    public string[] itemsToBeDropped;
    

    public DictionaryOfStringAndItemObject itemsDictionary;
    public List<ItemObject> itemsList;




    public void SetupItemsDictionary() 
    {
        if (itemsDictionary == null)
        {
            itemsDictionary = new DictionaryOfStringAndItemObject();
            
        }
        if (itemsList == null)
        {
            itemsList = new List<ItemObject>();

        }
        else {
            itemsDictionary.Clear();

        }
        if (allItemDatabase == null)
        {
            allItemDatabase = Resources.Load<ItemObjectDatabase>("AllItemsDatabase");


        }

        LoadToDictionaryAndList();


    }


    [ContextMenu("LoadData into dictionary")]
    public void LoadToDictionaryAndList() {



        if (allItemDatabase == null)
        {
            allItemDatabase = Resources.Load<ItemObjectDatabase>("AllItemsDatabase");


        }
        itemsDictionary.Clear();
        itemsList.Clear();


        foreach (var itemName in itemsToBeDropped)
        {

            ItemObject itemObject;
            itemsDictionary.TryGetValue(itemName, out itemObject);
            if (!itemObject)
            {
                itemObject = allItemDatabase.GetItemObjectFromName(itemName);
                itemsDictionary.Add(itemName, itemObject);
                itemsList.Add(itemObject);
            }
            else
            {
                itemObject = allItemDatabase.GetItemObjectFromName(itemName);
                itemsList.Add(itemObject);

            }


        }

    }

}



