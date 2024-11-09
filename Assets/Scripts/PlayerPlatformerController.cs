using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Webaby.Utils;
using Random = UnityEngine.Random;
public class PlayerPlatformerController : PhysicsObject
{
    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    public string inventoryName;
    private InventoryObject playerLetterInventory;
    // private float dustIstantiateTime = 1f;
    public Joystick joystick;
    private Animator animator;
    private bool isJumping;
    public bool croach;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public GameObject dust;
    private bool spawnDust;
    [SerializeField] private Health heroHealth;
    [SerializeField] private CutsceneTimelineBehaviour jupiTimeline;
    [SerializeField] private CutsceneTimelineBehaviour clockSettingTimeline;
    public static event EventHandler OnGameOver = delegate { };
    private CinemachineImpulseSource ShakeImpulseSource;
    // [SerializeField]
    // private AudioSource audioSource;
    [SerializeField]
    public AudioClip[] stepsSounds;
    [SerializeField]
    public AudioClip[] jumpSounds;
    [SerializeField]
    public AudioClip[] waterSounds;
   // [SerializeField]
  //  public GameObject[] attackEffects;
    //needed for attack
    public GameObject trailObject;
    public LayerMask whatIsEnemies;
    public LayerMask[] whatIsItem;
    public float startTimeBtwAttacks;
    private float timeBtwAttack;
    public float attackRange;
    public int damageAmount;
    private bool damage;
    private int heroHitLayerIndex;
    private bool attack;
    private int attackMode = 0;
    public Transform attackPosition;
    private TrailRenderer trail;
    public GameObject weaponManager;
    //needed for taking damage
    private Player player;
    private bool moveRight;
    private bool faceRight = true;
    public GameObject drawningEffect;
    public GameObject fillEffect;
    public DialogDatabase dialogDatabase;
    public readonly string[] attackAnimNames = { "attackIdle", "attackAndJump", "attackRunning", "attackAndJumpLong" };
    public DialogLanguage language;
    private int comboStep;
    private bool comboPossible;
    private int attackPower;
    public Transform reSpawnPoint;
    public GameObject usedObject;
   // public Material glowMaterial;
   // public Material standardMaterial;
    public Transform leg_left;
    public Transform leg_right;
    private bool interact;
    private Item interactItem;
    private Item recentlyUsedItem;
    private CharacterEquipment equipment;
    private WalletController wallet;
    bool abilityReady = true;
    [Header("Ability")]
    public UnitAbilityBehaviour abilityBehaviour;
    //private bool abilityAnimFinished = false;
    private UnitAbilitiesBehaviour unitAbilities;
    private UnitDialogBehaviour dialogBehaviour;
    private UnitDamageDisplayBehaviour damageDisplay;
    private UnitAudioBehaviour audioBehaviour;
    private AttachmentSetBehaviour attachementBehaviour;
    InventoryDatabase inventoryDB;
    private SceneTransitions sceneTransitions;
    private bool blockMovement;
    public  bool disableCollisions;
    private Collider2D playerCollider;
    public CinemachineVirtualCamera targetCamera;
    [SerializeField]private Transform lighterTransform;
    private bool playerMoving = false;
    public static event EventHandler<OnHeroStateChangedEventArgs> HeroController;
    public GameObject teleportEffect;
    public GameObject usedLetter;
    ContactFilter2D contactFilter2D;
    Collider2D userCollider;
    List<Collider2D> list;
 //   public TextMeshProUGUI letterDisplay;

    //shakeObj shaker;

    public bool isPlayerMoving() 
    {
        return playerMoving;
    }
    public bool isPlayerAttacking()
    {
        return attack;
    }
    public class OnHeroStateChangedEventArgs : EventArgs
    {
        public bool isInventoryOpen;
    }
    //ItemObjectDatabase itemObjectDB;
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    private void OnNameChangeUpdate(object sender, DisplayName.OnLetterDroppedEventArgs e)
    {
        if (e.changedSegment.letterSound !=null && playerData.playLetterSounds)
        {
            // 

            //if success play jupi 
            if (e.nameObject.done)
            {
                PlayJupiTimeLine();

            }

            PlayLetterSound(e.changedSegment.letterSound);
        }
    }

    public void PlayTeleportEffect()
    {
 
        StartCoroutine(PlayEffectRoutine());
        audioBehaviour.PlayTeleportSound();

    }

    IEnumerator PlayEffectRoutine()
    {
        teleportEffect.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        teleportEffect.SetActive(false);

    }
    private void PlayLetterSound(AudioClip letterSound)
    {
        if (audioBehaviour)
            audioBehaviour.PlaySoundLetter(letterSound);
    }
    public void PlayAttackSound() {
        if (audioBehaviour)
            audioBehaviour.PlayAttack(equipment.GetWeaponItem());
    }
    public void FireLogo() {
        int index = animator.GetLayerIndex("HeroFireLogo");
        animator.SetLayerWeight(index, 1);
    }
    public void StopFireLogo()
    {
        int index = animator.GetLayerIndex("HeroFireLogo");
        animator.SetLayerWeight(index, 0);
    }

    private void EnableAbility()
    {
        abilityReady = true;
    }
    // Use this for initialization
    void Awake()
    {
        blockMovement = false;  
        //  if (!playerLetterInventory)
        inventoryDB = PickupItemAssets.Instance.inventoryDatabase;
        dialogDatabase = PickupItemAssets.Instance.dialogDatabase;
        sceneTransitions = FindObjectOfType<SceneTransitions>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        disableCollisions = false;
        //itemObjectDB = PickupItemAssets.Instance.itemsDatabase;
        if (inventoryDB)
            playerLetterInventory = inventoryDB.GetInventoryObjectFromName(inventoryName);
        // GameLog.LogMessage("Player invenotry name:" + playerLetterInventory.name);
        interact = false;
        damageDisplay = GetComponent<UnitDamageDisplayBehaviour>();
        animator = GetComponent<Animator>();
        if (trailObject != null)
        {
            trail = trailObject.GetComponent<TrailRenderer>();
            trail.enabled = false;
        }
        player = GetComponent<Player>();
        ShakeImpulseSource = GetComponent<CinemachineImpulseSource>();
        faceRight = true;
        //language = DialogLanguage.Polski;
        language = UtilsClass.GetLanguageFromPrefs(PlayerPrefs.GetString("language", "Polski"));
        equipment = GetComponent<CharacterEquipment>();
        unitAbilities = GetComponent<UnitAbilitiesBehaviour>();
        wallet = GetComponent<WalletController>();
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
        attachementBehaviour = GetComponent<AttachmentSetBehaviour>();
        dialogBehaviour = GetComponent<UnitDialogBehaviour>();

        if(heroHealth)
            heroHealth.UpdateHealthAction += UpdateHealth;

        if (!weaponManager)
            weaponManager = GameObject.FindGameObjectWithTag("WeaponManager");

        contactFilter2D = new ContactFilter2D();
        contactFilter2D.useTriggers = true;
        contactFilter2D.SetLayerMask(gameObject.layer);
        list = new List<Collider2D>();
        userCollider = GetComponent<Collider2D>();
       // shaker = GetComponent<shakeObj>();

    }

    private void UpdateHealth(Health obj)
    {
        ShowBloodEffect();
    }

    public void RotateText()
    {
        //
            usedLetter.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);

    }


    private void Start()
    {
        if (inventoryDB)
            playerLetterInventory = inventoryDB.GetInventoryObjectFromName("ScampInventory");
        InventoryDisplay.UseItemAction += UseItem;
        DescriptionDisplay.TryToBuyItemAction += TryToBuyItem;
        TweenClockPanel.TryToBuyTimeAction += TryToBuyTime;
       // DescriptionDisplay.TryToBuyItemAction += TryToBuyTime;
        abilityBehaviour.AbilityReadyEvent += EnableAbility;
        DisplayName.OnNameChanged += OnNameChangeUpdate;
        GetInventory().SetListenerOnCollection();
        GetInventory().SetListenerOnFurnaceCollected();
        attackPower = player.playerData.GetValueBuff(AttributesName.Power);
        // playerLetterInventory.inventory.slots = new InventorySlot[9];
        // currentHealthNew = hearts.Length;
    }
    public void SetIsJumpingFalse() {
        //  GameLog.LogMessage("Is jumping false!!!!!!!!!");
        IsJumping = false;
    }
    public bool GetInteract()
    {
        return interact;
    }
    public void SetInteract(bool _interact)
    {
        this.interact = _interact;
    }
    public Item GetInteractItem()
    {
        return interactItem;
    }
    public void SetAttackPower(int power)
    {
        attackPower = power;
    }
    public InventoryObject GetInventory()
    {
        if (!playerLetterInventory)
            playerLetterInventory = PickupItemAssets.Instance.inventoryDatabase.GetInventoryObjectFromName("ScampInventory");
        return playerLetterInventory;
    }
    public CharacterEquipment GetEquipment()
    {
        return equipment;
    }
    private bool UseItem(Item obj)
    {
        SetInteract(true);
        interactItem = obj;
        recentlyUsedItem = obj;
        // GameLog.LogMessage("Use item from player controller !!!!!!!!!!!!!!!!");
        StartCoroutine(UseItemCouroutine(obj, 30));
        return interact;
    }

    public bool TryToBuyTime(int price)
    {

        bool success = wallet.PayWithWallet(price);
        if (success)
        {
            //Check if Item not stackable and alterady in inventory a
            PlayJupiTimeLine();
            return true;
        }
        else
        {
            //odegraj ze nie mam pieniedzy
            PlayDoNotHaveMoney();
            return false;
        }
    }


    public void DeactivateAllColliders() {

        disableCollisions = true;
        GetComponent<Collider2D>().enabled = false;

    }

    public void ActivateAllColliders()
    {

       
        GetComponent<Collider2D>().enabled = true;
        disableCollisions = false;



    }

    public bool TryToBuyItem(Item obj)
    {
        GameLog.LogMessage("Try to buy  !!!!!!!!!!!!!!!!!" + obj);
        if (!obj.stackable && playerLetterInventory.CheckItemInInventory(obj))
        {
            // Item already in inventory
            GameLog.LogMessage("The same Item already in inventory !!!");
            dialogBehaviour.PlayDoesNotWork();
            return false;
        }
        bool success = wallet.PayWithWallet(obj.GetItemObject().price);
        if (success)
        {
            //Check if Item not stackable and alterady in inventory a
            playerLetterInventory.AddItem(obj);
            PlayJupiTimeLine();
            return true;
        }
        else
        {
            //odegraj ze nie mam pieniedzy
            PlayDoNotHaveMoney();
             return false;
        }
    }
    public void PlayDoesNotFit()
    {
        if (dialogBehaviour)
        {
            //audioSource.PlayOneShot(doesNotFitSound);
            //  audioBehaviour.PlaySFXDoeasNotFit();
            dialogBehaviour.PlayDoesNotFitDialog();
            // GameLog.LogMessage("This item doeas not fit");
        }
    }



    public void PlayDoNotHaveMoney() {


        if (dialogBehaviour)
        {
            //audioSource.PlayOneShot(doesNotFitSound);
            //  audioBehaviour.PlaySFXDoeasNotFit();
            dialogBehaviour.PlayNoMoney();
            // GameLog.LogMessage("This item doeas not fit");
        }
    }
    
    public void PlayDoesNotWork()
    {
        if (dialogBehaviour)
        {
            //audioSource.PlayOneShot(doesNotFitSound);
            //  audioBehaviour.PlaySFXDoeasNotFit();
            dialogBehaviour.PlayDoesNotWork();
            // GameLog.LogMessage("This item doeas not fit");
        }
    }
    IEnumerator UseItemCouroutine(Item item, int time)
    {
        GameLog.LogMessage("UseIte Routine entered"+item.Name+" GetItemType" + item.GetItemType() +"Type"+ item.Type);
        if (usedObject != null && item.Type != ItemObjectType.Letter)
        {
            usedObject.GetComponent<SpriteRenderer>().sprite = item.GetItemObject().itemSprite;
            usedObject.SetActive(false);
            TextManager.Instance.ActivateText(item.DisplayedName);
            if (item.damaged) { 
                dialogBehaviour.PlayFixIt();
                yield break;
            }
            if (item.Type == ItemObjectType.Equipment)
            {
                equipment.TryEquipItem(item.GetEquipSlot(), item);
                yield break;
            }
            if (item.Type == ItemObjectType.Health)
            {
                GameLog.LogMessage("Try to add Health");
                foreach (var parameter in item.buffs)
                {
                    GameLog.LogMessage("parameter" + parameter.attribute+" value="+parameter.value);

                }


                for(int i = 0;i< item.buffs[0].value;i++)
                {
                    heroHealth.AddLife();

                }
                playerLetterInventory.RemoveItem(item);
                yield break;
            }
            GameLog.LogMessage("Use item" + item);

            if (item.GetItemType() == ItemObjectType.Default)
            {
                if (item.Name.Equals("SuperJumpElixir")) 
                {
                    SuperSkok(item);
                }
            }
            //  GameLog.LogMessage("Use ITem couroutine"+item.GetType());
            if (item.GetItemType() == ItemObjectType.Potion)
            {
                SuperSkok(item);
            }
            else if (item.GetItemType() == ItemObjectType.Key)
            {
                // GameLog.LogMessage("Uzyj Klucza");
            }
            else if (item.GetItemType() == ItemObjectType.Map)
            {
                // GameLog.LogMessage("Uzyj mapy");
                ((MapObject)item.GetItemObject()).SetMapUsed(true);
                playerLetterInventory.RemoveItem(item);
            }
            else if (item.GetItemType() == ItemObjectType.Feather)
            {
                //GameLog.LogMessage("Uzyj Piórka");
            }
            else if (item.GetItemType() == ItemObjectType.Health)
            {
                //efekt odnowienia życia ?
                playerLetterInventory.RemoveItem(item);

                //GameLog.LogMessage("Uzyj Piórka");
            }
            usedObject.SetActive(true);
           
            yield return new WaitForSeconds(time);
            usedObject.SetActive(false);
            interact = false;
            interactItem = null;
            usedObject.GetComponent<SpriteRenderer>().sprite = null;
            if (item.GetItemType() == ItemObjectType.Potion)
            {
                //wypij napoj
                // GameLog.LogMessage("Koniec dziania  napoju");
                player.DisableSuperJump();
                playerData.jumpTakeOffSpeed = 11f;
                playerData.gravityModifier = 3f;
                superjump = false;
            }
        } else if (usedObject != null && item.Type == ItemObjectType.Letter)
        {
            if (usedLetter != null) {
                RotateText();
                usedLetter?.SetActive(true);
                usedLetter.GetComponent<TextMeshPro>().text = item.GetLetterObject().text;
                yield return new WaitForSeconds(playerData.letterDisplayTime);
                usedLetter.SetActive(false);
                // TextManager.Instance.ActivateText(item.DisplayedName);
            }


        }

    }


    public void DisableUsedLetter() {


        usedLetter?.SetActive(false);


    }

    public void DisplayInventoryAndStats() {
        GameLog.LogMessage("DisplayInventoryAndStats entered");
        bool isOpen = false;
        if(GameHandler.Instance)
            isOpen = GameHandler.Instance.CheckInventoryOpen();
     /*   if (GameHandler.Instance && !isOpen)
        {
            GameHandler.Instance.ShowInventory();
        }
        else if (GameHandler.Instance && isOpen)
        {
            GameHandler.Instance.HideInventoryImmediate();
        }*/
        HeroController.Invoke(this, new OnHeroStateChangedEventArgs { isInventoryOpen = isOpen });
    }
    public void DisplayInventoryAndStatsForFiveSeconds()
    {
        GameLog.LogMessage("DisplayInventoryAndStats entered");
        playerLetterInventory.ShowInventory();
        HeroController.Invoke(this, new OnHeroStateChangedEventArgs { isInventoryOpen = GameHandler.Instance.CheckInventoryOpen() });
    }
    private void SuperSkok(Item _item)
    {
        StartCoroutine(SuperJumpCouroutine());
        //wypij napoj
        GameLog.LogMessage("Wypilem napoj");
        playerLetterInventory.RemoveItem(_item);
        //Instantiate(fillEffect, transform.position, Quaternion.identity);
        // ObjectPoolerGeneric.Instance.SpawnFromPool("FillEffect", transform.position, Quaternion.identity, null);
        // leg_right.GetComponent<SpriteRenderer>().material = glowMaterial;
        // leg_left.GetComponent<SpriteRenderer>().material = glowMaterial;
        // playerData.jumpTakeOffSpeed = 16f;
        // playerData.gravityModifier = 2f;
        // superjump = true;
    }


    private void ShowFillEffect() 
    { 
        ObjectPoolerGeneric.Instance.SpawnFromPool("FillEffect", transform.position, Quaternion.identity, null);
    }

    private void ShowBloodEffect()
    {
        ObjectPoolerGeneric.Instance.SpawnFromPool("DestroyEffects", transform.position, Quaternion.identity, null);
    }

    IEnumerator SuperJumpCouroutine()
    {
        superjump = true;
        GameLog.LogMessage("Wypilem napoj");
        //Instantiate(fillEffect, transform.position, Quaternion.identity);
        ObjectPoolerGeneric.Instance.SpawnFromPool("FillEffect", transform.position, Quaternion.identity, null);
        player.EnableSuperJump();
        yield return new WaitForSeconds(playerData.superJumpTime);
        player.DisableSuperJump();
        superjump = false;
    }
    public void DisableCollisions() {
        GameLog.LogMessage("DisableCollisions entered");
        disableCollisions = true;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
    public void EnableCollisions()
    {
        disableCollisions = false;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }
    public bool faceRightCheck()
    {
        return faceRight;
    }
    private void flipPlayer(bool turnRight)
    {
        RotateText();
        if (equipment.GetWeaponItem() != null)
        {
             switchHands(turnRight);
        }
    }
    private void switchHands(bool turnRight)
    {
        //Change Weapon Manager to other hand
        player.switchHands(turnRight);
    }
    public void TurnRight()
    {
        if (!faceRight)
        {
            faceRight = true;
            Vector2 move = Vector2.zero;
            move.x = joystick.Horizontal;
            transform.eulerAngles = new Vector3(0, 0f, 0);
            animator.SetBool("turnRight", true);
            flipPlayer(true);
        }
    }
    public void TurnLeft()
    {
        if (faceRight)
        {
            Vector2 move = Vector2.zero;
            move.x = joystick.Horizontal;
            faceRight = false;
            transform.eulerAngles = new Vector3(0, 180f, 0);
            animator.SetBool("turnRight", false);
            flipPlayer(false);
        }
    }

    private void OnBecameInvisible()
    {
        if (velocity.y < -10000) 
        {
            GameLog.LogMessage("Player fall somewhere");
            PlayerDeath();
        }
    }

    protected override void ComputeVelocity()
    {
        if (River.drawning || blockMovement) {
            return;
        }
        if (!joystick)
        {
            playerMoving = false;
            return;
        }
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            animator.SetBool("isTalking", false);
            playerMoving = true;
        }

        if (animator.GetLayerWeight(heroHitLayerIndex) != 0)
        {

            animator.SetLayerWeight(heroHitLayerIndex, 1);      
        }
     
        if (unitAbilities.abilityCurrentlyActive)
        {
            //  if (!abilityAnimFinished) { 
            // GameLog.LogMessage("Ability currenty activate " + unitAbilities.abilityCurrentlyActive);
            return;
        }
        attackMode = 0;
        Vector2 move = Vector2.zero;
        float verticalMove = joystick.Vertical;
        move.x = joystick.Horizontal;
        //GameLog.LogMessage("Movex." + move.x);
        //varticall move
        if ((verticalMove >= .05f && grounded) || (Input.GetButtonUp("Jump") && grounded))
        {
            //  dust.gameObject.SetActive(false);
            IsJumping = true;
            /* if (superjump)
             {
                 playerData.jumpTakeOffSpeed = 15;
                 velocity.y += playerData.jumpTakeOffSpeed;
             }*/
            //  else

            int agility = playerData.GetValueBuff(AttributesName.Agility);
                velocity.y = playerData.jumpTakeOffSpeed+ agility;
            animator.SetTrigger("jumping");
            attackMode = 1;
            dust.gameObject.SetActive(false);
        }
        else
        {
            ShowDust(groundType);
            // dust.transform.GetComponent<ParticleSystem>().Play();
        }
        if (verticalMove < -.5f)
        {
            dust.gameObject.SetActive(false);
            croach = true;
            if (isOnOneWayPlatform)
                LeavePlatform?.Invoke();
        }
        else
        {
            croach = false;
            ShowDust(groundType);
        }
        animator.SetBool("isCrouching", croach);
        //turn hero right
        if ((move.x > 0) && !faceRight)
        {
            faceRight = true;
            transform.eulerAngles = new Vector3(0, 0f, 0);
            flipPlayer(true);
            animator.SetBool("turnRight", true);
        }
        else if (move.x < 0 && faceRight)
        {
            //   GameLog.LogMessage("Move left");
            // Debug.Break();
            faceRight = false;
            transform.eulerAngles = new Vector3(0, 180f, 0);
            flipPlayer(false);
            animator.SetBool("turnRight", false);
        }
        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / playerData.maxSpeed);
        //horizontall move
        if (joystick.Horizontal > .2f && croach || joystick.Horizontal < -.2f && croach)
        {
            targetVelocity = (move * (playerData.maxSpeed+ playerData.GetValueBuff(AttributesName.Speed))) / 2;
        }
        else if (joystick.Horizontal > .2f || joystick.Horizontal < -.2f)
        {
            targetVelocity = move * (playerData.maxSpeed + playerData.GetValueBuff(AttributesName.Speed));
            ShowDust(groundType);
            //  Instantiate(dust, groundCheck.position, Quaternion.identity);
            //Animacja 
            if (!IsJumping)
            {
                attackMode = 2;
            }
            else
            {
                attackMode = 3;
            }
        }
        else
        {
            dust.gameObject.SetActive(false);
            playerMoving = false;
            targetVelocity = Vector2.zero;
            attackMode = 0;
        }
    }
    public void ShowDust(GroundType _groundType)
    {
        switch (_groundType)
        {
            case GroundType.NormalGround:
                dust.gameObject.SetActive(true);
                break;
            case GroundType.Stones:
                break;
            case GroundType.Leafs:
                break;
            case GroundType.Water:
                break;
            case GroundType.Home:
                break;
            case GroundType.None:
                break;
            default:
                break;
        }
    }
    public void AbilityHappened(int abilityValue, TargetType unitTargetType)
    {
        // abilityAnimFinished = true;
        switch (unitTargetType)
        {
            case TargetType.BasicEnemy:
                int randomUnit = Random.Range(0, 4);
                //  whatIsEnemies = LayerMask.GetMask("BasicEnemy");
                break;
            case TargetType.BossEnemy:
                // whatIsEnemies = LayerMask.GetMask("BossEnemy");
                break;
        }
        // int attackRangeAbility = 10; 
        GameLog.LogMessage("what is enemy to damage" + whatIsEnemies);
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, abilityValue, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            GameLog.LogMessage("enemy to damage" + enemiesToDamage[i].name);
            if (enemiesToDamage[i].GetComponent<Enemy>())
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(abilityValue);
        }
        animator.SetBool("isAttacking", false);



        //weaponManager?.GetComponentInChildren<Animator>()?.SetTrigger("attack");
        //attachementBehaviour?.GetWeaponTransform()?.GetComponent<Animator>()?.SetTrigger("attack");
        /*  List<UnitController> targetUnits = targetsBehaviour.FilterTargetUnits(unitTargetType);
          if (targetUnits.Count > 0)
          {
              for (int i = 0; i < targetUnits.Count; i++)
              {
                  targetUnits[i].RecieveAbilityValue(abilityValue);
              }
          }*/
    }
    IEnumerator AttackRoutine()
    {
         attack = true;
        // if (whatIsEnemies==null)
        //whatIsEnemies = LayerMask.GetMask("Enemy");
        //check weapon swing
        GameLog.LogMessage("attack Routine entered:" + whatIsEnemies);
        if (trail)
            trail.enabled = true;
        weaponManager?.GetComponentInChildren<Animator>()?.SetTrigger("attack");
        PlayAttackSound();
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            GameLog.LogMessage("enemy to damage" + enemiesToDamage[i].name);
            if (enemiesToDamage[i].GetComponent<Enemy>())
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(player.playerData.GetValueBuff(AttributesName.Power));
        }
        int mask = whatIsItem[0];
        if (whatIsItem.Length == 2)
            mask = whatIsItem[0] | whatIsItem[1];
        Collider2D[] itemsToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, mask);
        for (int i = 0; i < itemsToDamage.Length; i++)
        {
            GameLog.LogMessage("items to damage" + itemsToDamage[i].ToString()+" active?"+ itemsToDamage[i].isActiveAndEnabled);
            if (!itemsToDamage[i].isActiveAndEnabled)
                continue;
            
            itemsToDamage[i].GetComponent<IDamage>().TakeDamage(player.playerData.GetValueBuff(AttributesName.Power));
            
        }
        //animator.SetBool("isAttacking", false);
        attack = false;
        yield return null;
        if (trail)
            trail.enabled = false;
        // comboPossible = true;
        // yield return null;
    }
    public void StopAttacking()
    {
        if(animator)
            animator.SetBool("isAttacking", false);
    }
    public void ComboPossible()
    {
        // GameLog.LogMessage("COMBO possible !!!!");
        comboPossible = true;
    }
    public void AttackEnemy()
    {
        attack = true;
        SteeringPanel.Instance?.PressAttackButton();

        GameLog.LogMessage("attackMode="+ attackMode+" comboStep="+ comboStep+" comboPossibe"+comboPossible);
        if (animator)
            animator.SetBool("isAttacking", true);
        else
        {
            // GameLog.LogMessage("animator is null");
            animator = transform.GetComponent<Animator>();
        }
        SetAttackMode(attackMode);
        if (attackMode != 0)
        {
            StartCoroutine(AttackRoutine());
            return;
        }
        else if (attackMode == 0 && comboStep == 0 && grounded)
        {
            // GameLog.LogMessage("attack mode" + attackMode);
            animator.Play("Hero-comboAttack1");
            StartCoroutine(AttackRoutine());
            comboStep += 1;
            return;
        }
        else if (attackMode == 0 && comboStep != 0)
        {
            // GameLog.LogMessage("Combo Possible=" + comboPossible);
            if (comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
            else {

                GameLog.LogMessage("Combo Impossible but attackMode =0");
                //resetr combo ?
                ComboReset();   
            
            }
        }
        // SetAttackMode(attackMode);
        StartCoroutine(AttackRoutine());
    }
    public void Combo()
    {
        GameLog.LogMessage("Combo !!!!!" + comboStep);
        if (comboStep == 2)
        {
            SteeringPanel.Instance?.ActivateAbilityFVX();
            ObjectPoolerGeneric.Instance.SpawnFromPool("EnemyBloodEffect", transform.position, Quaternion.identity, null);
            audioBehaviour?.PlaySuperAttack(equipment.GetWeaponItem());
            
                
            
            
            //Jezeli mieczyk i ability ready 
            //if (equipment) && equipment.GetWeaponItem().Id == 7)
            if (equipment.GetWeaponItem() != null && equipment.GetWeaponItem().Name == "Sword")
            {
                GameLog.LogMessage("Weapon iD" + equipment.GetWeaponItem().Id);
                if (abilityReady)
                {
                    GameLog.LogMessage("Starting ability!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    GetComponent<UnitAbilitiesBehaviour>().AddAbilityToQueue(0);
                    // GetComponent<UnitAbilitiesBehaviour>().
                }
            }
            else if (equipment.GetWeaponItem() != null && equipment.GetWeaponItem().Name == "Lash")
            {
                GameLog.LogMessage("Weapon iD" + equipment.GetWeaponItem().Id);
                if (abilityReady)
                {
                    //  GameLog.LogMessage("Starting ability!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    // abilityReady = false;
                    // abilityBehaviour.AddAbilityToQueue();
                    // abilityBehaviour.BeginAbilitySequence();
                    GetComponent<UnitAbilitiesBehaviour>().AddAbilityToQueue(1);
                }
            }
            else
            {
                animator.Play("Hero-comboAttack2");
            }
        }
        else if (comboStep == 3)
        {
            animator.Play("Hero-comboAttack3");
        }
    }
    public void ComboReset()
    {
        // GameLog.LogMessage("Combo Reset !!!!!");
        comboPossible = false;
        comboStep = 0;
    }
    private void SetAttackMode(int mode)
    {
        animator.SetFloat(attackAnimNames[0], 0);
        animator.SetFloat(attackAnimNames[1], 0);
        animator.SetFloat(attackAnimNames[2], 0);
        animator.SetFloat(attackAnimNames[3], 0);
        animator.SetFloat(attackAnimNames[mode], 1);
        // GameLog.LogMessage("attack mode = " + attackAnimNames[attackMode]);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (attackPosition != null)
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
    public void PlayerDeath() {
         audioBehaviour?.PlaySFXDeath();
        // scene
        LostLife();
    }
    void GameOverClearing() 
    {
        heroHealth.UpdateHealthAction -= UpdateHealth;
        playerLetterInventory.Clear();
        ClearDialogPlayed();
        //ClearWallet();
    }
    void ClearDialogPlayed() 
    {
        dialogDatabase.ResetDialogs();
    }
    public void Footstep()
    {
        //  GameLog.LogMessage("Foot step entered");
        if (audioBehaviour)
        {
            // audioSource.PlayOneShot(stepsSounds[Random.Range(0, stepsSounds.Length)]);
            if (!River.drawning)
                audioBehaviour.PlayWalking(groundType, transform);
        }
    }
    public void PlayDrowning()
    {
        //  GameLog.LogMessage("Foot step entered");
        if (audioBehaviour)
        {
            // audioSource.PlayOneShot(stepsSounds[Random.Range(0, stepsSounds.Length)]);
            audioBehaviour.PlayDrowning();
        }
    }
    public void GetHitSound()
    {
        //  GameLog.LogMessage("Foot step entered");
        if (audioBehaviour)
        {
            // audioSource.PlayOneShot(stepsSounds[Random.Range(0, stepsSounds.Length)]);
            audioBehaviour.PlaySFXGetHit();
        }
    }
    public void LandingSound()
    {
        GameLog.LogMessage("Foot step entered");
        if (audioBehaviour)
        {
            // audioSource.PlayOneShot(stepsSounds[Random.Range(0, stepsSounds.Length)]);
            audioBehaviour.PlayLanding(groundType);
        }
    }
    /*GameObject Getattackeffect()
    {
        return attackEffects[Random.Range(0, attackEffects.Length)];
    }*/
    public void PlayJupiTimeLine() {
        BlockMovement(jupiTimeline);
        jupiTimeline?.StartTimeline();
    }
    public void BlockMovement(CutsceneTimelineBehaviour cutScene) 
    {
        StartCoroutine(BlockMovementRoutine(cutScene));
    }
    IEnumerator BlockMovementRoutine(CutsceneTimelineBehaviour cutScene) {
        blockMovement = true;
        DisableCollisions();
        float seconds;
        if(SteeringPanel.Instance)
            SteeringPanel.Instance.HideSteeringPanel();
        if (cutScene == null)
            seconds = 2;
        else
            seconds = (float)cutScene.GetTimelineDuration();
        yield return new WaitForSeconds(seconds);
        UnBlockMovement();
        if (SteeringPanel.Instance)
            SteeringPanel.Instance.ShowSteeringPanel(0.3f);
    }
    public void PlayClockSettingTimeLine()
    {
        blockMovement = true;
        clockSettingTimeline?.StartTimeline();
    }
    public void OnPlayJupiFinished() {
        UnBlockMovement();
    }
    public void UnBlockMovement()
    {
        blockMovement = false;
        State lighterState;
        if (lighterTransform) { 
            lighterState = lighterTransform.GetComponent<LighterController>().GetState();
            GameLog.LogMessage("Lighter State =" + lighterState);
        }
        EnableCollisions();
    }
    void PlayFullBagSound()
    {
        if (dialogBehaviour)
        {
            dialogBehaviour.PlayFullBag();
            //audioSource.PlayOneShot(fullBag);
            //GameLog.LogMessage("full bag  sound played");
        }
    }
    void Jumpstep()
    {
        if (audioBehaviour)
        {
            audioBehaviour.PlayJumpSound();
        }
    }
    public Health GetPlayerHealth() 
    { 
        return heroHealth;
    }
    IEnumerator TakeDamageRoutine(int amount)
    {

        int protection = player.playerData.GetValueBuff(AttributesName.Protection);
        if (amount > protection)
            amount -= protection;
        else {
            amount = 0;
            yield return null;
        }
        GameLog.LogMessage("TakeDamageRoutine for player entered damage" + amount);
        damage = true;
        heroHitLayerIndex = animator.GetLayerIndex("HeroGetHit");
        animator.SetLayerWeight(heroHitLayerIndex, 1);
        int previouslivesAmount = heroHealth.LivesAmount;
        GameLog.LogMessage("Health=" + heroHealth + "previous="+ previouslivesAmount);
       // Debug.Break();
        heroHealth.DecreseLife(amount);
        int currentLivesAmount = heroHealth.LivesAmount;
        GameLog.LogMessage("Health=" + heroHealth+ "currentLivesAmount=" + currentLivesAmount);
        //  Debug.Break();
        //ShakeCurrentCamera();
       

        audioBehaviour.PlaySFXGetHit();
        //animation to take Damage.
        damageDisplay.DisplayDamage(amount);
        ObjectPoolerGeneric.Instance.SpawnFromPool("HitEffectEnemies", transform.position, Quaternion.identity, null);
        yield return new WaitForSeconds(0.3f);
        animator.SetLayerWeight(heroHitLayerIndex, 0);
       
        damage = false;
        
/*        if (heroHealth.isAlive == false)
        {
            GameLog.LogMessage("healt is deadth" + heroHealth);
            PlayerDeath();
        } else */
        if (previouslivesAmount > currentLivesAmount)
        {
           //strata zycia
           LostLife();
        }
    }
    public void TakeDamage(int amount)
    {
        if (!damage && !blockMovement)
        {            
           StartCoroutine(TakeDamageRoutine(amount));

            
        }
    }
    private void ShakeCurrentCamera(float amplitudeGain = 1f, float duration = 0.3f)
    {
        GameLog.LogMessage("Generate Shake Impuls");
        if (ShakeImpulseSource != null)
        {
            ShakeImpulseSource.enabled = true;
            ShakeImpulseSource.m_ImpulseDefinition.m_AmplitudeGain = amplitudeGain;
            ShakeImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = duration;
            ShakeImpulseSource.GenerateImpulse();
            ShakeImpulseSource.enabled = false;
        }
    }
    public bool GetCrouching()
    {
        return croach;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameLog.LogMessage("OnCollision entered ! diSABLE COLLISION="+ disableCollisions+" COLLISIOSN name="+ collision.collider.name);
        
        Debug.Log("Overlaping colliders On colision"+ userCollider.OverlapCollider(contactFilter2D, list));
        if (disableCollisions)
        {
            GameLog.LogMessage("Collision disables !");
            Physics2D.IgnoreCollision(collision.collider, playerCollider);
           // return;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Overlaping colliders" + userCollider.OverlapCollider(contactFilter2D, list));
        if (disableCollisions)
        {
            GameLog.LogMessage("Collision disables !");
            Physics2D.IgnoreCollision(other, playerCollider, false);
            return;
        }
        var pickupitem = other.GetComponent<PickupItem>();
        if (pickupitem)
        {
            GameLog.LogMessage("Item type:!" + pickupitem.GetItem().Type);
            bool success = pickupitem.Collect();
            if (!success)
            {
                PlayFullBagSound();
            }
        }
    }
    public void DisableKinematic() 
    {
        rb2d.isKinematic = false;
        rb2d.simulated = true;
        float previousMass = rb2d.mass;
        rb2d.mass = 0.05f;
    }
    public void EnableKinematic()
    {
        rb2d.mass = 1f;
        rb2d.isKinematic = true;
        rb2d.velocity = Vector3.zero;
        rb2d.angularVelocity = 0;
    }
    public void LostLife() 
    {
        blockMovement = true;
        if (CameraSwitcher.ActiveCamera != targetCamera)
            CameraSwitcher.SwitchCamera(targetCamera);
        int index = animator.GetLayerIndex("HeroFall");
        audioBehaviour.PlaySFXDeath();
            StartFall();
            //StartCoroutine(RespawnCouroutine(1, index));
    }
    private void StartFall() {
        int index = animator.GetLayerIndex("HeroFall");
        animator.SetLayerWeight(index, 1);
    }
    public void OnStopFall() {
        int index = animator.GetLayerIndex("HeroFall");
        animator.SetLayerWeight(index, 0);
        if (!heroHealth.isAlive) 
        {
            GameOver();
        }
        else
        {
            StartCoroutine(RespawnCouroutine(1));
        }
    }
    public void GameOver() {
        blockMovement = true;
        OnGameOver(null, EventArgs.Empty);
        //sceneTransitions.LoadSceneAsyncTest("NowaPrzygoda.GameOver");
        GameOverClearing();
    }
    IEnumerator RespawnCouroutine(int seconds)
    {
        GameLog.LogMessage("Respawn routine entered");
        //animator.SetLayerWeight(index, 1);
        reSpawnPoint.parent.parent.gameObject.SetActive(true);
        ReSpawnPlayer(transform);
        yield return new WaitForSeconds(seconds);
    }
    public void ReSpawnPlayer(Transform player) {
        player.position = reSpawnPoint.position;
        bool lighterFollows = player.GetComponent<Player>().IsLighterFollowing();
        GameLog.LogMessage("Teleportation with lighter" + lighterFollows);
        if (lighterFollows)
        {
            GameLog.LogMessage("Change Lighter position");
            lighterTransform.position = reSpawnPoint.position;
        }
        blockMovement = false;
    }


    public void GeneratePickupEffect() 
    {
        StartCoroutine(PickupEffect(transform.position));
    }
    
    private IEnumerator PickupEffect(Vector3 position)
    {
        // Instantiate(pickupEffect, position, Quaternion.identity);
        ObjectPoolerGeneric.Instance.SpawnFromPool("PickUpEffect", position, Quaternion.identity, null);
        yield return null;
    }
    private void OnApplicationQuit()
    {
        //playerLetterInventory.inventory.Cle
        playerLetterInventory.inventory.slots = new InventorySlot[9];
        GetInventory()?.UnsetListenerOnCollection();
        GetInventory()?.UnSetListenerOnFurnaceCollected();
       // ClearDialogPlayed();
       // ClearWallet();
    }
    //ClearDialogPlayed();
    private void ClearWallet()
    {
        if (wallet && wallet.clearOnSceneLoad) {
            wallet.ClearWallet();
        }
    }
    private void OnDisable()
    {
        DisplayName.OnNameChanged -= OnNameChangeUpdate;
        InventoryDisplay.UseItemAction -= UseItem;
        DescriptionDisplay.TryToBuyItemAction -= TryToBuyItem;
        TweenClockPanel.TryToBuyTimeAction -= TryToBuyTime;


        if (abilityBehaviour)
            abilityBehaviour.AbilityReadyEvent -= EnableAbility;
        if(heroHealth)
            heroHealth.UpdateHealthAction -= UpdateHealth;
        GetInventory()?.UnsetListenerOnCollection();
        GetInventory()?.UnSetListenerOnFurnaceCollected();
    }

    public Item GetUsedItem()
    {
        return recentlyUsedItem;
    }
    public void NullUsedItem()
    {
        recentlyUsedItem =null;
    }


    
}
