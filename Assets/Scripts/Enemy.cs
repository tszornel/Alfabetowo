using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;


public class Enemy : MonoBehaviour,IPooled
{

    //private bool setupDone;
    public EnemyData enemyData;
    public int health;
    public float speed;
    public GameObject bloodEffect;
    public float timeBtwAttack;
    public ItemBuff[] buffs;
    [HideInInspector]
    public Animator anim;
    protected Transform player;
    protected PlayerPlatformerController playerController;
    protected int originalHealth;
    private UnitDamageDisplayBehaviour displayDamage;
    protected UnitAudioBehaviour audioBehaviour;
    protected List<Item> itemList = new List<Item>();
    protected bool isVisible;
    protected bool movingLeft = false;
    [StringInList(typeof(PropertyDrawerHelper), "AllItemsToBeDropped")]
    public string itemsToBeDroppedName;
    [SerializeField] private ItemsToBeDropped data;
    public int droppedItemsAmount;
    private bool moveRight;
    public bool isFollowing;
    public Button_Sprite buttonSprite;
    public DisplayEnemyPanel enemyPanel;
    protected bool isAlive;
    public  UnitHealthBehaviour healthBehaviour;
    [SerializeField]private UISliderBehaviour healthSlider;
    shakeObj shaker;

    // Start is called before the first frame update
    protected void Awake()
    {
      //  health = GetHealtValueBuff();
        SetupItemList();
        isVisible = false;
        displayDamage = GetComponent<UnitDamageDisplayBehaviour>();
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
        enemyPanel  = GameObject.FindObjectOfType<DisplayEnemyPanel>(true);
        healthBehaviour = GetComponent<UnitHealthBehaviour>();
        isAlive = true;
        shaker = GetComponent<shakeObj>();
    }

    void SignUpToHealthChange()
    {
        healthBehaviour.HealthChangedEvent += DisplayHealhPanel;
    }

 
    void UnSignUpToHealthChange()
    {
        healthBehaviour.HealthChangedEvent -= DisplayHealhPanel;
    }

    void SetupHealthDisplay(UnitHealthBehaviour _healthBehaviour)
    {
        GameLog.LogMessage("Setup Health Display on Enemy" + _healthBehaviour);
        if (!healthSlider)
            healthSlider = transform.Find("UI_Slider_Healthbar")?.GetComponent<UISliderBehaviour>();
        healthBehaviour = _healthBehaviour;
        int totalHealth = _healthBehaviour.GetCurrentHealth();
        int maxHealth = _healthBehaviour.GetMaxValue();
        GameLog.LogMessage("Enemy" + gameObject.name + " health=" + totalHealth,gameObject);
        SignUpToHealthChange();
        healthSlider.SetupDisplay((float)maxHealth);
        UpdateHealthDisplay(totalHealth);
    }

    void UpdateHealthDisplay(int newHealthAmount)
    {
        healthSlider.SetCurrentValue((float)newHealthAmount);
    }


    private void DisplayHealhPanel(int newHealthAmount)
    {
        UpdateHealthDisplay(newHealthAmount);
    }


    public void SetupItemList()
    {
        itemList = new List<Item>();
        //setupDone = true;
        GameLog.LogMessage("Setup item list");

        if (!data && !itemsToBeDroppedName.Equals(""))
        {
            data = PickupItemAssets.Instance.itemsToBeDroppedDatabase.GetItemToBeDroppedByName(itemsToBeDroppedName);
         
        }

        //Jesli zepsuty juz raz to nie twórz ponownie listy 

        
        itemList.Clear();
       
        if (!data || data.itemsToBeDropped == null || data.itemsDictionary.Count == 0)
        {
            GameLog.LogMessage("Not found items To be dropped", this);

            return;
        }
        else
        {

            GameLog.LogMessage("Found Item to be droped", this);
            foreach (var item in data.itemsDictionary.Keys)
            {
                GameLog.LogMessage(this.name + " Item in List " + item, this);
            }
        }

        foreach (var itemName in data.itemsDictionary.Keys)
        {
            Item item = null;
            if (!itemName.Equals("None"))
                item = data.itemsDictionary[itemName.ToString()].createItem();
            if (item != null)
            {
                itemList.Add(item);
                GameLog.LogMessage(this.name + " Add to item list to Damage" + item.Name + "itemList Count:" + itemList.Count, this);
            }

        }

    }


    protected void PlayEnemySound() {
        isVisible = true;
         StartCoroutine(PlayEnemySoundRoutine());

    }

    IEnumerator PlayEnemySoundRoutine() 
    {

        GameLog.LogMessage("Play Enemy sound:"+transform.name);
        while (health > 0) {
            int randomSeconds = UnityEngine.Random.Range(3, 5);
            yield return new WaitForSeconds(randomSeconds);
            if (audioBehaviour != null) 
            {
                audioBehaviour.PlayRandomSound();
            }
        }
    }


    public int GetPowerValueBuff()
    {
        foreach (var buff in buffs)
        {
            if (buff.attribute == AttributesName.Power)
            {
                return buff.value;
            }
            else
            {
                continue;
            }
        }
        return 0;
    }

    public int GetHealtValueBuff()
    {
        foreach (var buff in buffs)
        {
            if (buff.attribute == AttributesName.Health)
            {
               return buff.value;
            }
            else
            {
                continue;
            }
        }
        return 0;
    }


    protected void Start()
    {

        anim = GetComponent<Animator>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        playerController = player.GetComponent<PlayerPlatformerController>();

        if (!buttonSprite)
            buttonSprite = GetComponent<Button_Sprite>();
        if (buttonSprite != null)
            buttonSprite.ClickFunc = () =>
            {

                if (!enemyPanel.gameObject.activeSelf)
                {
                    GameLog.LogMessage("Show Enemy Panel on CLick");
                    enemyPanel.gameObject.SetActive(true);
                    enemyPanel.ShowEnemyDesription(this);
                }
                else 
                {
                    enemyPanel.UnsignToPreviousHealthChange(healthBehaviour);
                    enemyPanel.Hide();
                    enemyPanel.gameObject.SetActive(false);
                }
             };
        if (healthBehaviour)
        {
            healthBehaviour.SetupCurrentHealth(GetHealtValueBuff());
            healthBehaviour.SetupMaxHealth(GetHealtValueBuff());
            SetupHealthDisplay(healthBehaviour);
        }

    }

    protected bool CheckDirection(Transform hero, Transform enemy)
    {

        if (hero.position.x > enemy.position.x)
        {
            // If object A was previously on the left, change direction
            if (!moveRight)
            {
                // Change direction code here
                Debug.Log("Object A is now on the right change DIRECtioN !!!");
                moveRight = true; // Update the flag
                
            }
        }
        else
        {
            // If object A was previously on the right, change direction
            if (moveRight)
            {
                // Change direction code here
                Debug.Log("Object A is now on the left change DIRECtioN !!");
                moveRight = false; // Update the flag
            }
        }


        if (moveRight) 
        { 
            enemy.eulerAngles = new Vector3(0, 0, enemy.eulerAngles.z);
           
        }
        else
            enemy.eulerAngles = new Vector3(0, 180, enemy.eulerAngles.z);

        healthSlider?.RotateText(moveRight);
        return moveRight;   
    }
    // Update is called once per frame
    void Update()
    {
        GameLog.LogMessage("Enemy update health="+ healthBehaviour.GetCurrentHealth(), transform);
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        if (healthBehaviour.GetCurrentHealth() <= 0)
        {
            GameLog.LogMessage("Enemy death",transform);
            EnemyDeath();


        }
    }

    protected void DeathEffect()
    {
        
        GameLog.LogMessage("Death effect" + transform.name, transform);
        
        if (audioBehaviour)
        {
            GameLog.LogMessage("Odegranie dzwieku!!!!!! smierci " + transform.name, transform);
            player.GetComponent<UnitAudioBehaviour>()?.PlaySFXDeath(audioBehaviour.GetDeathClip());
        }
        else
        {

            GameLog.LogMessage("Brak dzwiekow " + transform.name, transform);

        }
        GameLog.LogMessage("Death Effect start !!!!!!!!!!!!!!");
        ObjectPoolerGeneric.Instance?.SpawnFromPool("EnemyBloodEffect", transform.position, Quaternion.identity, null);

    }
    public void TakeDamage(int damage)
    {
    
        enemyPanel.gameObject.SetActive(true);
        GameLog.LogMessage("Enemy take damage executed"+damage+" is Alive"+ isAlive);
        if (isAlive)
        {

            healthBehaviour.ChangeHealth(damage);
            displayDamage.DisplayDamage(damage);
            if (shaker)
                shaker.Shake();
            if (audioBehaviour)
            {
                GameLog.LogMessage("TakeDamageSound on audioSource" + audioBehaviour.audioSource.gameObject.name);
                audioBehaviour.PlaySFXGetHit();

            }
            ObjectPoolerGeneric.Instance.SpawnFromPool("HitEffectEnemies", transform.position, Quaternion.identity, null);

            //  characterAnimationBehaviour.CharacterWasHit();
            audioBehaviour.PlaySFXGetHit();

        }
    }



    public void EnemyDeath() 
    {
        GameLog.LogMessage("Execute EnemyDeath entered");
        isAlive = false;
        DeathEffect();
        UnSignUpToHealthChange();
       
        enemyPanel.Hide();
        //enemyPanel.gameObject.SetActive(false);
        // Destroy(gameObject);
        ResetEnemyLife();
        ObjectPoolerGeneric.Instance.ReleaseToPool("Enemies", gameObject);
        GameLog.LogMessage("Execute EnemyDeath left");
    }

    public void RecieveDamageValue(int damage)
    {
        if (isAlive)
        {
            healthBehaviour.ChangeHealth(damage);
            displayDamage.DisplayDamage(damage);
            ObjectPoolerGeneric.Instance.SpawnFromPool("HitEffectEnemies", transform.position, Quaternion.identity, null);

            //  characterAnimationBehaviour.CharacterWasHit();
            audioBehaviour.PlaySFXGetHit();
        }
    }

 
   

    public void ReleseToPool()
    {
        ResetEnemyLife();
        ObjectPoolerGeneric.Instance.ReleaseToPool("Enemies", gameObject);
        enemyPanel.UnsignToPreviousHealthChange(healthBehaviour);
        UnSignUpToHealthChange();
    }


    protected void OnDestroy()
    {
        StopAllCoroutines();

    }

    protected void OnApplicationQuit()
    {
       // ResetEnemyLife();
    }

    private void ResetEnemyLife()
    {
        healthBehaviour.SetupCurrentHealth(GetHealtValueBuff());
        UpdateHealthDisplay(GetHealtValueBuff());
        isAlive = true;


    }

    protected void OnBecameInvisible()
    {
       // GameLog.LogMessage("OnBecome INVisible" + transform.name);
        StopAllCoroutines();
        isVisible = false;
        GameLog.LogMessage("OnBecomeInvisible", gameObject);
        if (transform.position.y < -100)
            ReleseToPool();
        //ReleseToPool();
    }

    protected void OnBecameVisible()
    {
        //GameLog.LogMessage("OnBecome Visible !!!!!"+transform.name);
        isVisible = true;   
        PlayEnemySound();
    }


    protected void OnDisable()
    {
        StopAllCoroutines();
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayEnemySound();
        }
    }
}
