using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Health Object", menuName = "Inventory System/Items/Health")]
public class HealthObject : ItemObject
{
    // public TMP_Text text;
    public int restoreHealtAmount;
    private void Awake()
    {
        type = ItemObjectType.Health;
    }
}
