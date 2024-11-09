using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UI_ItemSlot : MonoBehaviour, IDropHandler
{
    public int slotNumber;
    private Action onDropAction;
    private int ItemId;
    public void SetSlotItemId(int Id) {
        ItemId = Id;
    }
    public int GetSlotItemId() {
        return ItemId;
    }
    public void SetOnDropAction(Action onDropAction)
    {
        this.onDropAction = onDropAction;
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameLog.LogMessage("Drop instance!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        UI_itemTouch.Instance.Hide();
        if (onDropAction != null) {
            GameLog.LogMessage("On drop Action entered");
            onDropAction();
        }
    }
}
