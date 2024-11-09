using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_FurnaceSlot : MonoBehaviour
{
    public GameObject SlotUIGO;
    public GameObject UIcamera;
    public GameObject wowParticle;
    [SerializeField] private UI_ItemMain itemInSlot;


    private void Awake()
    {
        FurnaceController.OnFurnaceSlotSuccess += OnFurnaceSuccessUpdateUI;
    }

    private void OnFurnaceSuccessUpdateUI(object sender, EventArgs e)
    {
          wowParticle?.SetActive(true);
          wowParticle?.GetComponent<ParticleSystem>()?.Play();
    }

    private void OnDestroy()
    {
        FurnaceController.OnFurnaceSlotSuccess -= OnFurnaceSuccessUpdateUI;
    }

    public static UI_FurnaceSlot Instance { get; private set; }


    public void ActivateUISlot()
    {
        GameLog.LogMessage("Activate UI Furnace Slot");
        UIcamera.SetActive(true);
        if (SlotUIGO && !SlotUIGO.activeSelf)
        {

            SlotUIGO?.SetActive(true);
        }

    }

    public void DeactivateUISlot()
    {

        if (SlotUIGO && SlotUIGO.activeSelf)
        {
            SlotUIGO?.SetActive(false);
            UIcamera?.SetActive(false);
        }


    }

    public UI_ItemMain GetItemInSlot() {

        return itemInSlot;
      
    
    }


    public void SetItemInFurnaceSlot(Item item) {

        itemInSlot?.SetItem(item);
    }

    public void RemoveItemInFurnaceSlot()
    {

        itemInSlot?.SetItem(null);
    }

    public void TweenUISlot()
    {
        GameLog.LogMessage("Tween UI slot");
        if (SlotUIGO && !SlotUIGO.activeSelf)
            SlotUIGO.SetActive(true);

        else
        {
            //GameObject.FindObjectOfType<DisplayName>(true);
            SlotUIGO.SetActive(false);
        }


    }


}
