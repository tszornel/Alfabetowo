using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;



[System.Serializable]
public class PickupItemWorldSpawner : MonoBehaviour {

    public ItemsToBeDropped itemsToBeDropped;
    [StringInList(typeof(PropertyDrawerHelper), "AllItemsToBeDropped")]
    public string ItemsToBeDroppedName;
    private Item item;
    public bool damagedItem;
    PickupItem itemSpawned;
    private float nextSpawn = 0;
    public Transform prefabToSpawn;
    public float spawnRate = 5f;
    public float randomDelay = 10f;
    public static PickupItemWorldSpawner Instance { get; private set; }

   

    public void SetItemsToBeDropped()
    {
        if(!itemsToBeDropped && !ItemsToBeDroppedName.Equals("None"))
            itemsToBeDropped = PickupItemAssets.Instance.itemsToBeDroppedDatabase.GetItemToBeDroppedByName(ItemsToBeDroppedName);

    }


    [ExecuteInEditMode]
    private void Awake()
    {
        SetItemsToBeDropped();
    }

    private void Start()
    {
        ItemObject itemObject;
        if(itemsToBeDropped != null)  
        foreach (var itemName in itemsToBeDropped.itemsToBeDropped)
        {

                if (Time.time > nextSpawn)
                {
                
                    itemsToBeDropped.itemsDictionary.TryGetValue(itemName, out itemObject);
                    item = itemObject.createItem();
                    item.damaged = damagedItem;
                    SpawnItem(item);
                    nextSpawn = Time.time + spawnRate + Random.Range(4, randomDelay);
                }
            }

           Destroy(this.gameObject);
    }

    public void SpawnItem(Item item)
    {
        GameLog.LogMessage("Spawn item"+item.Name+" damaged="+item.damaged); 
        itemSpawned = PickupItem.SpawnItemWorld(transform.position, Quaternion.identity, item, transform.parent);

    }

    public void SpawnItemAtPlayerPosition(Item item)
    {
        GameLog.LogMessage("Spawn item" + item.Name + " damaged=" + item.damaged);
        Transform playerTransform = GameObject.FindObjectOfType<PlayerPlatformerController>(true).transform;
        Vector3 playerPosition = playerTransform.position;
        itemSpawned = PickupItem.SpawnItemWorld(playerPosition, Quaternion.identity, item, playerTransform.parent);

    }
    public void SpawnObject(GameObject prefab) {

        if (prefab != null)
        {
            itemSpawned = PickupItem.SpawnItemWorld(transform.position, prefab);
            itemSpawned.itemName = item.Name;

        }
        else
        {

            itemSpawned = PickupItem.SpawnItemWorld(transform.position, Quaternion.identity, item, transform.parent);
            

        }

    }
}
