using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[CreateAssetMenu(fileName = "FurnaceSlot", menuName = "Repaire System/FurnaceData", order = 6)]
public class FurnaceSlotData : ScriptableObject
{
    public Item furnaceSlotItem;
    public Name furnaceName;

    public FurnaceSlotData()
    {
    }

    public void Reset()
    {
        furnaceSlotItem = null;
        furnaceName = null;
    }
}

