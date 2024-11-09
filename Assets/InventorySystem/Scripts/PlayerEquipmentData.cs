using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inventory ", menuName = "Inventory System/EqipmentData")]
public class PlayerEquipmentData : ScriptableObject
{
    public Item weaponItem;
    public Item helmetItem;
    public Item armorItem;
    //  private List<Item> equipmentDataList = new List<Item>();
    public PlayerEquipmentData()
    {
    }

    public void Reset(CharacterEquipment equipment)
    {
        if(weaponItem != null) {
            equipment.RemoveItem(weaponItem); 
            //weaponItem = null;
        }
        if (helmetItem != null)
        {
            equipment.RemoveItem(helmetItem);
            //helmetItem = null;
        }
        if (armorItem != null)
        {
            equipment.RemoveItem(armorItem);
           // armorItem = null;
        }
      
    }

}
   