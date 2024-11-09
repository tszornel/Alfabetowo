using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UI_CharacterEquipmentSlot : MonoBehaviour, IDropHandler
{
    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
 

    private void Awake()
    {
       
    }


    public class OnItemDroppedEventArgs : EventArgs
    {
        public Item item;
    }



    public void OnDrop(PointerEventData eventData)
    {
        GameLog.LogMessage("on equipent droped");
        Item item = UI_itemTouch.Instance.GetItem();


        //or in Furnace
        if (item.Location == ItemLocation.Equipment || item.Location == ItemLocation.FurnaceSlot)
        {
            GameLog.LogMessage("Item dmoved from equipment to equipment");
            UI_itemTouch.Instance.Hide();
            UI_itemTouch.Instance.touchItemDropedinEquipment = true;
            return;

        }
        //

        GameLog.LogMessage("Item droped to slot:" + item.ToString());
        if (OnItemDropped != null)
        {

            GameLog.LogMessage("OnDrop invoke executed droped ");
           // Debug.Break();
            OnItemDropped.Invoke(this, new OnItemDroppedEventArgs { item = item });
        }
        else {

            GameLog.LogMessage("OnItemDropped null brak ustawionego eventu !!!!!!!!!!!!!!!!!!!!!");


        }
           
    }
}
