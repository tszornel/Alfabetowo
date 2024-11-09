using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Repaire Object", menuName = "Inventory System/Items/RepaireObject")]
public class RepaireObject : ItemObject
{
   // public GameObject itemPrefab;
   // public NameObject nameObject;
    [StringInList(typeof(PropertyDrawerHelper), "AllItems")]
    public string itemNameAfterFix;

    private void Awake()
    {
        type = ItemObjectType.Repaire;

    }
}
