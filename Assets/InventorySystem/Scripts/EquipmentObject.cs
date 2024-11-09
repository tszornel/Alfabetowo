using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EquipmentType
{
    None,
    Weapon,
    Armour,
    Helmet,
    Tool
}


[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    //public GameObject itemPrefab;
    public EquipmentType WeaponType;

    private void Awake()
    {
        type = ItemObjectType.Equipment;
       
    }
}
