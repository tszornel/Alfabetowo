using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IItemHolderNew
{
    public void RemoveItem(Item item);
    public void AddItem(Item item);
    public bool CanAddItem(Item item);
}