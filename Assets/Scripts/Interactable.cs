using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Interactable : MonoBehaviour
{
    public bool isInRange;
   // public KeyCode interactKey;
    public UnityEvent interactAction;
    private PlayerPlatformerController player;
    private GameObject playerTransform;
    public bool removeInteractableItemWhenUsed = true;
    
    [StringInList(typeof(PropertyDrawerHelper), "AllItems")]
    public string interactableItemName;
    private Item interactableItem;

    void Start()
    {
        if (!player)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player");
            if (playerTransform)
                playerTransform.GetComponent<PlayerPlatformerController>();
        }

        if (interactableItem == null && interactableItemName != null && interactableItemName != "None")
        {

            interactableItem = PickupItemAssets.Instance.itemsDatabase.GetItemObjectFromName(interactableItemName).createItem();
        }
        // interactableItem = transform.parent.GetComponent<Item>();
    }
    // Update is called once per frame



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isInRange)
        {
            if (interactableItem!=null && player.GetInteract() && interactableItem.Name.Equals(player.GetInteractItem().Name))
            {
                player.SetInteract(false);
                interactAction.Invoke();
                if(removeInteractableItemWhenUsed)
                    player.GetInventory().RemoveItem(interactableItem);
            }
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // 
        if (collision.tag == "Player")
        {
            GameLog.LogMessage("Interactoable object collision entered");
            this.isInRange = true;
            if(player ==null)
                player = collision.gameObject.GetComponent<PlayerPlatformerController>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameLog.LogMessage("Interactoable object collision entered");
        if (collision.tag == "Player")
        {
            this.isInRange = true;
            // GameLog.LogMessage("colide with player"+transform.name);
        }
    }
}
