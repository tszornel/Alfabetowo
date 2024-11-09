using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Webaby.Utils;
using UnityEngine.Assertions.Must;
public class ItemToDamage : MonoBehaviour, IDamage
{
    [SerializeField] private ItemsToDamageData itemToDamageData;
    [StringInList(typeof(PropertyDrawerHelper), "AllItemsToDamage")]
    public string itemToDamageName;
    //private Item[] items;
    public Transform placeToSpawn;
    AudioSource audioSource;
    private bool inTrigger;
    public AudioClip[] damageSounds;
    public GameObject attackEffect;
    public AudioClip needMorePowerClip;
    private UnitDamageDisplayBehaviour damageDisplay;
    private UnitAudioBehaviour audioBehaviour;
    private UnitDialogBehaviour dialogBehaviour;
    [SerializeField] Animator animator;
    List<Item> itemList;
    public UnityEvent damageEvent;
    private bool setupDone;
    private bool alreadyUnderAttack;
    private WaveSpawner relatedWafeSpawner;



    public ItemsToDamageData GetItemToDamageData()
    {

        return itemToDamageData;
    }

    private void Awake()
    {
        setupDone = false;
        SetupItemList();
        GameLog.LogMessage("Item in list count in awake before start :" + itemList.Count, this);
        if (!placeToSpawn)
            placeToSpawn = transform;   
    }
    public void SetupItemList() {
        itemList = new List<Item>();
        setupDone = true;
        GameLog.LogMessage("Setup item list");
        if (!itemToDamageName.Equals(""))
        {

            GameLog.LogMessage("Get Item to damage object:"+ itemToDamageName);

            itemToDamageData = PickupItemAssets.Instance.itemsToDamageDatabase.GetItemToDamageByName(itemToDamageName);

            GameLog.LogMessage("Items to damage =" + itemToDamageData.name);

           /* if (data != null)
            {
                data.SetItemsToBeDropped();
            }
            else {
                GameLog.LogMessage("Item to Damage missing in database" + itemToDamageName);
            }*/
        }
        //Jesli zepsuty juz raz to nie twórz ponownie listy 
        if (itemToDamageData.damaged) {
            GameLog.LogMessage("Item to damaged already destgryed");
            return;
        }
            
        itemList.Clear();
        //data.itemsToBeDropped.SetupItemsDictionary();
        if (itemToDamageData.itemsToBeDropped == null || itemToDamageData.itemsToBeDropped.itemsToBeDropped == null || itemToDamageData.itemsToBeDropped.itemsDictionary.Count==0)
        {
            GameLog.LogMessage("Not found items To be dropped", this);
            return;
        }
        else {
            GameLog.LogMessage("Found Item to be droped",this);
            foreach (var item in itemToDamageData.itemsToBeDropped.itemsDictionary.Keys)
            {
                GameLog.LogMessage(this.name + " Item in List " + item, this);
            }
        }
        foreach (var itemName in itemToDamageData.itemsToBeDropped.itemsDictionary.Keys)
        {
            Item item = null;
            if (!itemName.Equals("None"))
                item = itemToDamageData.itemsToBeDropped.itemsDictionary[itemName.ToString()].createItem();
            if (item != null)
            {
                itemList.Add(item);
                GameLog.LogMessage(this.name + " Add to item list to Damage" + item.Name + "itemList Count:"+itemList.Count, this);
            }
        }
    }


 



    private void OnEnable()
    {
        if (itemToDamageData.damaged)
        {
           
            if (animator)
            {
                GameLog.LogMessage("ChangeLayerToDamaged whenn awake!");
                ChangeLayerToDamaged();
            }



        }
    
    }
    public void ChangeLayerToDamaged() {
        int index = animator.GetLayerIndex("Damaged");
        if(index!=-1)
            animator.SetLayerWeight(index, 1);
    }
    private void Start()
    {
        inTrigger = false;
        //data.damaged = false;
        audioSource = GetComponent<AudioSource>();
        damageDisplay = GetComponent<UnitDamageDisplayBehaviour>();
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
        dialogBehaviour = GetComponent<UnitDialogBehaviour>();  
        if(!animator)
            animator = GetComponent<Animator>();
        GameLog.LogMessage(this.name+ " :Item in list count in start before setup :" + itemList.Count,this);
        if (!setupDone)
          SetupItemList();
        GameLog.LogMessage(this.name + "Item in list count in start:" + itemList.Count,this);
       // itemList = items.ToList();
    }
    public void TakeDamage(int damage)
    {
        GameLog.LogMessage("attacking item" + transform.name + "in trigger=" + inTrigger+" damage "+damage+ " strenght"+ itemToDamageData.strenght+"blocked="+alreadyUnderAttack+"Damaged flag"+ itemToDamageData.damaged);
        if (alreadyUnderAttack)
            return;
        alreadyUnderAttack = true;
        if (damage >= itemToDamageData.strenght)
        {
            GameLog.LogMessage(" damage>= strenght");
            //ww zaleznosci od typu obiektu do zniszczenia mozna zrobic spawn kiedy chcemy.
            //if ( health==0)
            if (!itemToDamageData.damaged)
            {
                itemToDamageData.health -= damage;
                if (itemToDamageData.health <= 0)
                    itemToDamageData.damaged = true;
                damageDisplay.DisplayDamage(damage);
                if (animator && animator.isActiveAndEnabled && UtilsClass.AnimatorHasParameter(animator,"takeDamage"))
                {
                    animator.SetTrigger("takeDamage");
                }
                var random = new System.Random();
                GameLog.LogMessage("Damage amount:" + damage);
                for (int i = 0; i < damage; i++)
                {
                    //  int random = Random.Range(0, items.Length);
                    // if (random)
                    GameLog.LogMessage("TakeDamage i=" + i+ " items In List= "+ itemList.Count, this );
                    if (itemList.Count > 0) 
                    { 
                        int index = random.Next(itemList.Count);
                        var _item = itemList.ElementAt(index);
                        GameLog.LogMessage("item NAME:"+ _item.Name+"placeToSpawnPosition:"+ placeToSpawn?.position+ "placeToSpawn name"+ placeToSpawn.name,placeToSpawn);
                        PickupItem.SpawnItemWorld(placeToSpawn.position, Quaternion.identity, _item, transform);
                        itemList.RemoveAt(index);
                        foreach (Item _itemTest in itemList)
                        {
                            GameLog.LogMessage("item in list after remove" + _itemTest.ToString());
                        }
                    }
                }
                if (itemToDamageData.health <= 0)
                    if (damageEvent != null)
                        damageEvent.Invoke();
                CheckDestroyed();
                
            }
        }
        else {
            GameLog.LogMessage("damage < strenght"+damage);
           if (dialogBehaviour)
               dialogBehaviour?.PlayDoesNotWork();
        }
        alreadyUnderAttack = false;
        GameLog.LogMessage("Already under attack set to false:"+alreadyUnderAttack);
    }
    private bool CheckDestroyed() {
        if (itemToDamageData.health <= 0 & !itemToDamageData.damaged)
        {
            itemToDamageData.damaged = true;
            if (animator && UtilsClass.AnimatorHasParameter(animator, "damage"))
            {
                animator.SetTrigger("damage");
                animator.SetBool("isDestroyed", true);
            }
            //change to destryed object anim
            DamageEffects();
        }
        else if (itemToDamageData.damaged)
        {
            if (animator)
            {
                if (UtilsClass.AnimatorHasParameter(animator, "damage"))
                    animator.SetTrigger("damage");
                if (UtilsClass.AnimatorHasParameter(animator, "isDestroyed") && animator.GetBool("isDestroyed") != true)
                    animator.SetBool("isDestroyed", true);
            }
        }

        return itemToDamageData.damaged;
    }
    
    public void SelfDestroy() {
        Destroy(transform.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // GameLog.LogMessage("ItemSpawner collider entered");
        if (collision.tag == "Player")
        {
            this.inTrigger = true;
            // GameLog.LogMessage("colide with player"+transform.name);
        }
    }
   /* private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.inTrigger = true;
        }
    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        // if (inTrigger) { 
        if (collision.tag == "Player") {
            this.inTrigger = false;
            if (!itemToDamageData.damaged)
                StartCoroutine(TriggerStopCouroutine());
        }
    }
    IEnumerator TriggerStopCouroutine()
    {
        yield return new WaitForSeconds(1);
        this.inTrigger = false;
        StopAllCoroutines();    
      //  GameLog.LogMessage("set inTrigger false");
    }
    void DamageEffects()
    {
        if (audioBehaviour)
        {
            audioBehaviour.PlaySFXGetHit();
           // audio.PlayOneShot(damageSounds[Random.Range(0, damageSounds.Length)]);
            //  GameLog.LogMessage("FootStep sound played");
        }
        if (attackEffect)
            Instantiate(attackEffect, transform.position, Quaternion.identity);
    }
}