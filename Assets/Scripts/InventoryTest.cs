 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class InventoryTest : MonoBehaviour
{
    private bool InventoryEnabled;
    public GameObject inventory;
    public GameObject pickupEffect;
    private int allSlots;
    private int[] enabledSlots;
    private GameObject[] slot;
    public GameObject slotHolder;
    Animator inventoryAnim;
    private TMP_Text spawnedTextObjectComponent;
    private Pickup pickupItem;
    // Start is called before the first frame update
    void Start()
    {
        allSlots = 16;
        slot = new GameObject[allSlots];
        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;
            if (slot[i].GetComponent<Slot>().item == null)
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
        // inventoryAnim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)){
            InventoryEnabled = !InventoryEnabled;
        }
        if (InventoryEnabled)
        {
            inventory.SetActive(true);
        }
        else {
            inventory.SetActive(false);
        }
    }
    // Loading Scene 
    public void LoadInventory()
    {
        StartCoroutine(InventoryCourutine());
    }
    IEnumerator InventoryCourutine()
    {
       // inventoryAnim.SetTrigger("open");
        InventoryEnabled = !InventoryEnabled;
        yield return new WaitForSeconds(1);
    }
     public void AddPickup(GameObject item,  int itemID, string itemType, string itemDescription, Sprite itemIcon) {
        GameLog.LogMessage("Adding Item to Inventory" + item.ToString() + " itemIcon" + itemIcon.ToString()+" type"+itemType+ ", itemDescription="+itemDescription);
        for (int i = 0; i < allSlots; i++)
        {
            GameLog.LogMessage("Loping allslots i=" + i+" allslots="+allSlots);
            if (slot[i].GetComponent<Slot>().empty) {
                // add pickupitem to Slot
                GameLog.LogMessage("ITEM " + i + " is empty and can be filled !!!");
                item.GetComponent<Pickup>().pickedUp = true;
                slot[i].GetComponent<Slot>().icon = itemIcon;
                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().description = itemDescription;
                slot[i].GetComponent<Slot>().ID = itemID;
                slot[i].GetComponent<Slot>().item = item;
                item.transform.parent = slot[i].transform;
                item.SetActive(false);
                slot[i].GetComponent<Slot>().UpdateSlot();
                slot[i].GetComponent<Slot>().empty = false;
                GameLog.LogMessage("SLOT " + slot[i].ToString());
                return;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       GameLog.LogMessage("'Collides with" + collision.collider.tag.ToString());
       if (collision.collider.CompareTag("Letter"))
            {
                GameLog.LogMessage("Collides with letter ENTERD !!!!");
                GameObject other = collision.gameObject;
                spawnedTextObjectComponent = other.GetComponent<TMP_Text>();
                pickupItem = other.GetComponent<Pickup>();
                string letter = spawnedTextObjectComponent.text;
                string spriteName = "Literka_" + letter.ToUpper();
                Sprite icontest = Resources.Load<Sprite>(spriteName);
                //set dinamicaly letter icon
                pickupItem.icon = icontest;
            if (icontest != null)
            {
                GameLog.LogMessage("JEST!!!:" + icontest.ToString());
            }
            else {
                GameLog.LogMessage("NIE DZIAŁA!");
            }
                AddPickup(other, pickupItem.ID, pickupItem.type, pickupItem.description, pickupItem.icon);
                GameLog.LogMessage("Letter found" + letter);
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
                //Destroy(other);
            }
      }
}
