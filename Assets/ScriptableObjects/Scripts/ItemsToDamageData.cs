using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;


[CreateAssetMenu(fileName = "_Data", menuName = "Webaby/Unit/DamageItems Data", order = 1)]

public class ItemsToDamageData : ScriptableObject
{

    public int initialHealth;
    public int initialStrenght;

    public int health;
    public int strenght;
    public bool damaged;
    [HideInInspector]
    public ItemsToBeDropped itemsToBeDropped;


    [StringInList(typeof(PropertyDrawerHelper), "AllItemsToBeDropped")]
    public string ItemsToBeDroppedName;

    public void SetItemsToBeDropped() 
    {
        if (!ItemsToBeDroppedName.Equals("") && !ItemsToBeDroppedName.Equals("None")) { 
            itemsToBeDropped = PickupItemAssets.Instance.itemsToBeDroppedDatabase.GetItemToBeDroppedByName(ItemsToBeDroppedName);
           // itemsToBeDropped.SetupItemsDictionary();    
        }
    }


    public void ResetData() 
    {
        SetItemsToBeDropped();

        health = initialHealth;
        strenght = initialStrenght;
        damaged = false;
    }
}
