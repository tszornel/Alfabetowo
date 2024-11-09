using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour, IPointerClickHandler
{
    public bool empty;
    public int ID;
    public string type;
    public string description;
    public Sprite icon;
    public GameObject item;
    public void OnPointerClick(PointerEventData pointerEventData) {
        UseItem();
    }
    public void UseItem()
    {
        item.GetComponent<Pickup>().ItemUsage();
    }
    public override string ToString() {
        string test = "ID=" + ID + " type=" + type + " decription=" + description + " icon" + icon.name+" empty="+empty;
        return test;
    }
    public void UpdateSlot()
    {
        this.GetComponent<UnityEngine.UI.Image>().sprite = this.icon;
    }
}
