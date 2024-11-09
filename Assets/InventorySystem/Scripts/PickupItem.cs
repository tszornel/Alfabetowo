
using TMPro;
using UnityEngine;

public class PickupItem : MonoBehaviour,ICollect,IPooled, IDamage
{
    [StringInList(typeof(PropertyDrawerHelper), "AllItems")]
    public string itemName;
    //public List<ItemObject> items;
    private Item item;
    TMP_Text text;
    public SpriteRenderer spriteRenderer;
    public TMP_Text textMeshPro;
    private Transform amountTextMeshProTransform;
    public TMP_Text amountTextMeshPro;
    public Transform sprite;
    private Transform textMeshProTransform;
    public bool pickupFromPool;
    public static event OnPickupCollected OnCollect;
    public delegate bool OnPickupCollected(Item item);
    public static event OnPickupCollected OnCollectCoin;
    private ItemObjectDatabase itemsDatabase;
    private ItemObjectDatabase letterDatabase;
    private static bool collecting;

    private void Awake()
    {
        // 
        //  itemsDatabase = PickupItemAssets.Instance.itemsDatabase;
        itemsDatabase = Resources.Load<ItemObjectDatabase>("ItemsDatabase");
        letterDatabase = Resources.Load<ItemObjectDatabase>("LetterDatabase");
        textMeshProTransform = gameObject.transform.Find("text");
        if (textMeshProTransform)
            textMeshPro = textMeshProTransform.GetComponent<TMP_Text>();
        amountTextMeshProTransform = gameObject.transform.Find("amount");
        if(amountTextMeshProTransform)
            amountTextMeshPro = amountTextMeshProTransform.GetComponent<TMP_Text>();
        if (!sprite) 
        { 
            sprite = gameObject.transform.Find("sprite");
            if (!spriteRenderer&&sprite)
                spriteRenderer = sprite.GetComponent<SpriteRenderer>();
            else
                spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (item == null && itemName != null && !itemName.Equals(""))
        {
            if (itemName.ToCharArray().Length > 1)
            {
                if(itemsDatabase && itemsDatabase.GetItemObjectFromName(itemName))
                    item = itemsDatabase.GetItemObjectFromName(itemName).createItem();
            }
            else
            {
                item = letterDatabase.GetItemObjectFromName(itemName).createItem();
            }
        }
    }
    private void Start()
    {
       /* if (item==null && itemName != null && !itemName.Equals("")) {
            if (itemName.ToCharArray().Length > 1)
            {
                item = itemsDatabase.GetItemObjectFromName(itemName).createItem();
            }
            else 
            {
                item = letterDatabase.GetItemObjectFromName(itemName).createItem();
            }
        }*/
     }
    public static Transform InstantiateStandardItemEquipment(Transform parent, Item item)
    {
        Transform go = PickupItemAssets.Instance.pfPickupItem;
        Transform test = Instantiate(go, parent, false);
        Rigidbody2D rb = test.GetComponent<Rigidbody2D>();
        PickupItem itemWorld = test.GetComponent<PickupItem>();
        itemWorld.pickupFromPool = false;
        item.SetLocation(ItemLocation.Equipment);
        itemWorld.SetItem(item);
        rb.isKinematic = true;
        itemWorld.pickupFromPool = false;
        item.SetLocation(ItemLocation.Equipment);
        itemWorld.SetItem(item);
        if (test.GetComponent<CapsuleCollider2D>().isActiveAndEnabled)
            test.GetComponent<CapsuleCollider2D>().enabled = false;
        test.SetParent(parent, false);
        GameLog.LogMessage("InstantiateItemWorld");
        //var obj = ObjectPoolerGeneric.Instance.SpawnFromPool("PickupItems", position, Quaternion.identity, null);
        //Transform test = obj.transform;
       // PickupItem itemWorld = test.GetComponent<PickupItem>();
        return test;
    }
    public static PickupItem SpawnItemWorld(Vector3 position, Item item)
    {
        // Transform test = Instantiate(PickupItemAssets.Instance.pfPickupItem, position, Quaternion.identity);
        GameLog.LogMessage("SpawnItemWorld");
        var obj = ObjectPoolerGeneric.Instance.SpawnFromPool("PickupItems", position, Quaternion.identity, null);
        Transform test = obj.transform;
        test.name = item.Name;
        PickupItem itemWorld = test.GetComponent<PickupItem>();
        itemWorld.pickupFromPool = true;
        //item.SetLocation(ItemLocation.None);
        itemWorld.SetItem(item);
        return itemWorld;
    }

    public static PickupItem SpawnItemWorld(Vector3 position, Item item, Transform parent)
    {
        // Transform test = Instantiate(PickupItemAssets.Instance.pfPickupItem, position, Quaternion.identity);
        GameLog.LogMessage("SpawnItemWorld");
        var obj = ObjectPoolerGeneric.Instance.SpawnFromPool("PickupItems", position, Quaternion.identity, parent);
        Transform test = obj.transform;
        test.name = item.Name;
        PickupItem itemWorld = test.GetComponent<PickupItem>();
        itemWorld.pickupFromPool = true;
       // item.SetLocation(ItemLocation.None);
        itemWorld.SetItem(item);
        return itemWorld;
    }
    public static PickupItem SpawnItemWorld(Vector3 position, GameObject prefab)
    {
        GameLog.LogMessage("SpawnItemWorld");
        var obj = Instantiate(prefab, position, Quaternion.identity);
        Transform test = obj.transform;
        PickupItem itemWorld = test.GetComponent<PickupItem>();
        if(itemWorld)
            itemWorld.pickupFromPool = false;
        return itemWorld;
    }
    public static void SpawnEquipmentItem(Vector3 position, Item item, Transform parent)
    {
        GameLog.LogMessage("SpawnItem");
        ItemObject itemObject = item.GetItemObject();
        GameObject obj;
        if (itemObject.type == ItemObjectType.Equipment) { 
            GameObject itemPrefab  = itemObject.itemPrefab;
            if(itemPrefab)
                 obj = Instantiate(itemPrefab, position, Quaternion.identity);
            else
                 obj = ObjectPoolerGeneric.Instance.SpawnFromPool("PickupItems", position, Quaternion.identity, parent);
        }
    }
    public static PickupItem SpawnItemWorld(Vector3 position,Quaternion rotation, Item item,Transform parent)
    {
        GameObject obj;
        ItemObject itemObject = item.GetItemObject();
        GameObject itemPrefab = itemObject?.itemPrefab;
        bool pickupFromPoolTest;
        GameLog.LogMessage("Check itemPrefab: " + itemPrefab);
        if (itemPrefab)
        {
            GameLog.LogMessage("instantiate prefab");
            obj = Instantiate(itemPrefab, position, Quaternion.identity);
            pickupFromPoolTest = false;
        }
        else 
        {
            GameLog.LogMessage("spawn from Pools");
            obj = ObjectPoolerGeneric.Instance?.SpawnFromPool("PickupItems", position, Quaternion.identity, parent);
            pickupFromPoolTest = true;
        }
        if(obj != null) { 
       // ObjectPoolerGeneric.Instance.SpawnFromPool("PickupItems", position, rotation, parent);
            Transform test = obj.transform;
            PickupItem itemWorld = test.GetComponent<PickupItem>();
            item.SetLocation(ItemLocation.None);
            itemWorld.SetItem(item);
            itemWorld.itemName = item.Name;
            itemWorld.name = item.Name;
            itemWorld.pickupFromPool = pickupFromPoolTest;
            return itemWorld;
        }
        return null;
    }
    public static PickupItem DropItem(Vector3 dropPosition, Item item, bool faceRight)
    {

        GameLog.LogMessage("Drop item !!!!!!!!!!!!!!!!!!!"+item);
        if (item != null) { 
         GameLog.LogMessage("odrzuc item" + item.ToString()+"position "+ dropPosition+" prawa strona ?"+ faceRight);
            Vector3 randomDir;
            if (faceRight)
               randomDir = new Vector3(1f, 1f).normalized;
            else
              randomDir = new Vector3(-1f, 1f).normalized;
             PickupItem itemWorld = SpawnItemWorld(dropPosition + randomDir * 8f, item);
            // itemWorld.item.SetLocation(ItemLocation.None);
             itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 2f, ForceMode2D.Impulse);
            return itemWorld;
        }
        return null;
    }


    public static PickupItem DropItem(Vector3 dropPosition, Item item, bool faceRight,Transform parent)
    {

        GameLog.LogMessage("Drop item !!!!!!!!!!!!!!!!!!!" + item);
        if (item != null)
        {
            GameLog.LogMessage("odrzuc item" + item.ToString() + "position " + dropPosition + " prawa strona ?" + faceRight);
            Vector3 randomDir;
            if (faceRight)
                randomDir = new Vector3(1f, 1f).normalized;
            else
                randomDir = new Vector3(-1f, 1f).normalized;
            PickupItem itemWorld = SpawnItemWorld(dropPosition + randomDir * 8f, item, parent);
           // itemWorld.item.SetLocation(ItemLocation.None);
            itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 2f, ForceMode2D.Impulse);
            return itemWorld;
        }
        return null;
    }
    public static PickupItem DropItem(Vector3 dropPosition, Item item)
    {
        Vector3 randomDir = new Vector3(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)).normalized;
        PickupItem itemWorld = SpawnItemWorld(dropPosition + Vector3.up + randomDir * 6f, item);
        //itemWorld.item.SetLocation(ItemLocation.None);
        itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 2f, ForceMode2D.Impulse);
        return itemWorld;
    }
    public void SetItem(Item item)
    {
        this.item = item;
        ItemObject itemObject;
        if (item.Type == ItemObjectType.Letter)
        {
            itemObject = item.GetLetterObject();
            GameLog.LogMessage("Set Item:"+item);
            if (textMeshPro) { 
                textMeshPro.text = ((LetterObject)itemObject).GetText();
            }
            if (itemObject.amount != 0 || itemObject.amount != 1)
            { 
                if(amountTextMeshPro)
                    amountTextMeshPro.text = itemObject.amount.ToString();
            }
            GameLog.LogMessage("Set item method " + item.Name + " item sprite" + itemObject.itemSprite);
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = null;
                // EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
            }
            else {
                sprite = gameObject.transform.Find("sprite");
                spriteRenderer = sprite.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = null;
            }
        }
        else if (item.Type == ItemObjectType.Equipment)
        {
            itemObject = item.GetItemObject();
            if (spriteRenderer)
            {
                if (!item.damaged)
                    spriteRenderer.sprite = itemObject.itemSprite;
                else
                    spriteRenderer.sprite = itemObject.itemSpriteDamaged;
                if (amountTextMeshPro)
                    amountTextMeshPro.text = "";
                if(textMeshPro)
                    textMeshPro.text = "";
            }
            else {
                sprite = gameObject.transform.Find("sprite");
                if (sprite) { 
                    spriteRenderer = sprite.GetComponent<SpriteRenderer>();
                    if (!item.damaged)
                        spriteRenderer.sprite = itemObject.itemSprite;
                    else
                        spriteRenderer.sprite = itemObject.itemSpriteDamaged;
                    amountTextMeshPro.text = "";
                    textMeshPro.text = "";
                }
            }
        }
        else
        {
            GameLog.LogMessage("Set item when spawn=" + item + " item Damaged" + item.damaged);
            itemObject = item.GetItemObject();
            if (spriteRenderer)
            {
                if (!item.damaged)
                    spriteRenderer.sprite = itemObject.itemSprite;
                else
                    spriteRenderer.sprite = itemObject.itemSpriteDamaged;
                amountTextMeshPro.text = "";
                textMeshPro.text = "";
            }
            else
            {
                sprite = gameObject.transform.Find("sprite");
                spriteRenderer = sprite.GetComponent<SpriteRenderer>();
                if (!item.damaged)
                    spriteRenderer.sprite = itemObject.itemSprite;
                else
                    spriteRenderer.sprite = itemObject.itemSpriteDamaged;
            }
        }
        if (itemObject.amount > 1)
        {
            amountTextMeshPro.SetText(itemObject.amount.ToString());
           // EditorUtility.SetDirty(GetComponentsInChildren<TMP_Text>()[1]);
        }
        else {
            amountTextMeshPro?.SetText("");
          //  EditorUtility.SetDirty(GetComponentsInChildren<TMP_Text>()[1]);
        }
    }
    public Item GetItem()
    {
        return item;
    }
    public bool Collect() {

       /* if (collecting) 
        {

            GameLog.LogMessage("Already Collecting");
            return false;
        }
           */
        
       // collecting = true;
        bool success = false;
        GameLog.LogMessage("Collect !!!!!");
        if (item.Type == ItemObjectType.Coin) { 
            success = OnCollectCoin.Invoke(this.item);
        }
        else { 
            success = OnCollect.Invoke(this.item);
        }
        GameLog.LogMessage("Success with adding obecjt to inventory");
        if (success) 
        {
            ObjectPoolerGeneric.Instance.SpawnFromPool("PickUpEffect", transform.position, Quaternion.identity, null);
            if (pickupFromPool)
            {
                ObjectPoolerGeneric.Instance.ReleaseToPool("PickupItems", this.gameObject);
                GameLog.LogMessage("Releasing Object to pool" + gameObject);
            }
            else
            {
                GameLog.LogMessage("Removin  Object" + gameObject);
                Destroy(this.gameObject);
            }
            
         
             


        }
       // collecting = false;
        return success;
    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void ReleseToPool()
    {
        ObjectPoolerGeneric.Instance.ReleaseToPool("PickupItems", gameObject);
    }
    void OnBecameInvisible()
    {
        GameLog.LogMessage("OnBecomeInvisible", gameObject);
        if (transform.position.y < -100)
            ReleseToPool();
    }
    public void TakeDamage(int damage)
    {
       /* if (collecting) 
        {
            GameLog.LogMessage("Already under Collection");
            return;
        }*/

        
        GameLog.LogMessage("Take damage and Collect on Pickup Item!!!!!");
        Collect();
       
      
    }
}
