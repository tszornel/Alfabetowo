using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Webaby.Utils;

public class Friend : MonoBehaviour
{
    //private Animator anim;
    private bool inTrigger;
    [Header("Friend Dialogs")]
    private UnitDialogBehaviour friendDialogs;
    private UnitTasksBehaviour friendTasks;
    private Name friendName;
    [Header("Friend Inventory settings")]
    private InventoryObject inventoryObject;
    [StringInList(typeof(PropertyDrawerHelper), "AllInventories")]
    public string InventoryName;
    private InventoryDatabase id;
    private bool allTasksDone;
    public ShopDisplay shop;
    private Dialog dialogToBePlayed;
    [Header("Friend Tasks")]
    private AbecadlowoTask currentFriendTask;
    private TasksManager taskManager;
    [Header("Dialog Animations")]
    public Animator characterAnim;
    [SerializeField] private Animator playerAnim;
    public GameObject wowEffect;
    private CutsceneTimelineBehaviour timelineBehaviour;
    [Header("Events")]
    public UnityEvent buttonEvent;
    public UnityEvent doubleClickEvent;
    public UnityEvent taskDoneSuccessEvent;
    private bool displayed = false;
    private bool dialogPlayed;
    public Transform placeToSpawn;
    public InteractableBase interactableObject;
    private bool startChecking = false;
    public bool DropRight = false;
    private UnitAudioBehaviour audioBehaviour;
    private CharacterEquipment equipment;


    public bool reactOnSpriteWhenTasksDone = false;
    [SerializeField] private Button_Sprite button;
    public bool playRandomSound = false;
    // private RaycastHit2D playerInfoCheck;
    // public Transform rayTransform;
    private bool isVisible;
    [SerializeField] private GameObject questionMark;
    private bool showShopOnClick = false;

    //string[] successItemNames;

    Dialog WaitingDialog;
    private ItemsToBeDropped successItemsToBeDropped;
    private Coroutine waitForDialogRoutine;
    private Coroutine waitForSuccessDialogRoutine;

    public void SetNameSuccessListener()
    {
        GameLog.LogMessage("SetNameSuccessListener OnPlaySuccess");
        friendName.OnPlaySuccess += Name_OnPlaySuccess;
        GameLog.LogMessage("name object listener=" + friendName.name);
    }

    public void UnSetNameSuccessListener()
    {
        GameLog.LogMessage("UnSetNameSuccessListener OnPlaySuccess");
        friendName.OnPlaySuccess -= Name_OnPlaySuccess;
    }

    private void Name_OnPlaySuccess(object sender, System.EventArgs e)
    {
        GameLog.LogMessage("Name_OnPlaySuccess entered:" + sender+" current task= "+currentFriendTask);
        UnSetNameSuccessListener();
        if (currentFriendTask.nameObjectName.Equals("None") || currentFriendTask.nameObjectName.Equals("") || currentFriendTask.nameObjectName == null)
            return;
        //FinishCurrentTask();
        PlaySuccess();
        GameLog.LogMessage("Name_OnPlaySuccess left");
        ProcessNewTask();
    }


    private void OnDestroy()
    {
        if (friendName && friendName.NameIn)
            UnSetNameSuccessListener();
    }

    public void FinishCurrentTask()
    {
        GameLog.LogMessage("FinishCurrentTask entered");
        currentFriendTask = friendTasks.GetCurrentTask();
        if (currentFriendTask != null)
            currentFriendTask.FinishTask();

    }


    IEnumerator WaitForDialogToDropItem(string _dialogName, ItemsToBeDropped droppedItems)
    {

        GameLog.LogMessage("Checking task dialog:" + _dialogName + " item name" + droppedItems.itemsToBeDropped);
        WaitingDialog = DialogManager.Instance.GetDialog(_dialogName);
        dialogPlayed = WaitingDialog.dialogPlayed;
        startChecking = true;
        yield return new WaitUntil(() => dialogPlayed);
        WaitingDialog = null;
        // successItemNames = "";
        DropItemFromTask(droppedItems);
        


    }


    private void DropItemFromTask(ItemsToBeDropped dropedItems)
    {

        GameLog.LogMessage("DropItemFromTask entered");
        foreach (var itemName in dropedItems.itemsDictionary.Keys)
        {


            if (itemName != null)
                GameLog.LogMessage("Item name from item  be dropped" + itemName);
            else
                GameLog.LogMessage("Item name for succes is null");
            if (!itemName.Equals("") || !itemName.Equals("None"))
            {
                GameLog.LogMessage("DropItem" + itemName);
                ItemObject itemObject = dropedItems.itemsDictionary[itemName.ToString()];

                if (itemObject)
                {
                    GameLog.LogMessage("found itemobject" + itemObject.name);
                    itemObject.damaged = friendTasks.GetCurrentTask().damaged;
                    DropItem(itemObject.createItem());

                }
                else {

                    GameLog.LogMessage("not found itemobject from name" + itemName);
                }


            }
        }
        //Fire task Done success Event
        taskDoneSuccessEvent?.Invoke();

    }
    public void PlaySuccess()
    {
        GameLog.LogMessage("PlaySuccess friend entered");

        string successDialogName = currentFriendTask.GetSuccessDialogName();
        successItemsToBeDropped = friendTasks.GetCurrentTask().GetItemsToBeDropped();
        if (successDialogName != null && !successDialogName.Equals("") && !successDialogName.Equals("None") )
        {
            bool dialogPlayed = friendDialogs.PlaySuccesDialog(successDialogName);
           
                if(waitForSuccessDialogRoutine!=null)
                    StopCoroutine(waitForSuccessDialogRoutine); 

                waitForSuccessDialogRoutine =  StartCoroutine(WaitForDialogToDropItem(successDialogName, successItemsToBeDropped));
            
        }
        else
        {
            DropItemFromTask(successItemsToBeDropped);

        }
        FinishCurrentTask();

        HideName();
        interactableObject?.RemoveInteractableItemCheck();   
        if (wowEffect)
            Instantiate(wowEffect, transform.position, Quaternion.identity);
        audioBehaviour?.PlaySuccess();



        if (timelineBehaviour)
        {
            timelineBehaviour?.StartTimeline();

        }
        else if (characterAnim)
        {
            characterAnim?.SetTrigger("success");

        }
        
        //process next task if exists

        if (allTasksDone)
        {
            AllTasksDone();
        }


    }

    void AllTasksDone()
    {
        
        GameLog.LogMessage("All Tasks done check");
        if (dialogPlayed  && WaitingDialog == null) { // && (successItemNames == null && successItemNames.Length == 0)) {

            ShowQuestionMark();
        }
        else if(WaitingDialog !=null)
        {
            
                friendDialogs?.PlaySuccesDialog(WaitingDialog.name);
                waitForSuccessDialogRoutine = StartCoroutine(WaitForDialogToDropItem(WaitingDialog.name, successItemsToBeDropped));

            // StartCoroutine(WaitForDialogToDropItem(WaitingDialog.name, successItemsToBeDropped));

        }
        //DisplayShop();


    }

    public void ShowQuestionMark()
    {

        if (questionMark)
        {
            questionMark.SetActive(true);
            showShopOnClick = true;
        }

    }

    public void HideQuestionMark()
    {
        if (questionMark && questionMark.activeSelf)
            questionMark?.SetActive(false);
        showShopOnClick = false;
    }

    public void DisplayShop()
    {
        if (InventoryName != null && friendTasks.CheckAllTasksDone())
        {
            HideQuestionMark();
            inventoryObject = id.GetInventoryObjectFromName(InventoryName);
            GameLog.LogMessage("Set shop:" + inventoryObject.name);
            shop.SetInventory(inventoryObject);
            shop.DisplayShop();
            friendDialogs.PlayCanBuySomething();
        }
        else
        {

            //odegranie co teraz
            //friendDialogs.PlayWhatNow();
        }
    }

   public void ToggleShop()
    {
        if (shop.isActiveAndEnabled)
            shop.HideShop();
        else {
            //
            DisplayShop();
            HideQuestionMark();
        }
            



    }

    public void ShowShop()
    {
        GameLog.LogMessage("ShowShop Entered");
            DisplayShop();
        GameLog.LogMessage("ShowShop Left");
    }

    public void DropItem(Transform placeToSpawn, Item item)
    {
        GameLog.LogMessage("Drop item entered");
        if (item != null)
        {
            PickupItem.SpawnItemWorld(placeToSpawn.position, Quaternion.identity, item, transform.parent);

        }
    }
    public void DropItem(Item item)
    {
        if (!placeToSpawn)
            placeToSpawn = transform;


        PickupItem.DropItem(placeToSpawn.position, item, DropRight);
        // DropItem(placeToSpawn, item);
    }


    private void DisplayNameIn()
    {

        if (friendName.nameIn && !displayed)
        {
            DisplayNameIn(friendName.nameIn);
            displayed = true;

        }
    }

    private void DisplayNameIn(NameObject _nameIn)
    {
        Name.ShowNameBox();
        Name.ShowName(_nameIn);
        GameHandler.Instance.ShowInventory();
    }


    private void Awake()
    {
        startChecking = false;
        id = PickupItemAssets.Instance.inventoryDatabase;
        allTasksDone = false;
        if (!characterAnim)
            characterAnim = GetComponent<Animator>();
        friendName = GetComponent<Name>();
        friendDialogs = GetComponent<UnitDialogBehaviour>();
        friendTasks = GetComponent<UnitTasksBehaviour>();
        //anim = GetComponent<Animator>(); 
        taskManager = TasksManager.Instance;
        timelineBehaviour = GetComponent<CutsceneTimelineBehaviour>();
        if (!interactableObject)
            interactableObject = GetComponent<InteractableBase>();
        dialogPlayed = false;
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
        if (!button)
            button = GetComponent<Button_Sprite>();
        if (button) {
            GameLog.LogMessage("Set Button  click !!!!", this);
            button.ClickFunc = () =>
            {
                GameLog.LogMessage("Clicked Sprite Button !!!!",this);

                if (!reactOnSpriteWhenTasksDone || (reactOnSpriteWhenTasksDone && friendTasks.CheckAllTasksDone()))
                    buttonEvent?.Invoke();

            };
            if (button.doubleClick) {
                GameLog.LogMessage("Set Button Double click !!!!", this);
                button.DoubleClickFunc = () =>
                {
                    GameLog.LogMessage("Doiuble click Sprite Button !!!!",this );

                    if (!reactOnSpriteWhenTasksDone || (reactOnSpriteWhenTasksDone && friendTasks.CheckAllTasksDone()))
                        doubleClickEvent?.Invoke();

                };
            }
        }


    }


    void OnBecameVisible()
    {
        isVisible = true;
        PlayFriendSoundRoutine();

    }


    void OnBecameInvisible()
    {

        isVisible = false;
    }

    void PlayFriendSound()
    {
        GameLog.LogMessage("Play friend sound entered:" + transform.name);
        StartCoroutine(PlayFriendSoundRoutine());

    }




    IEnumerator PlayFriendSoundRoutine()
    {
        GameLog.LogMessage("PlayFriendSoundRoutine entered:" + transform.name + "playRandomSound" + playRandomSound + " isVisible:" + isVisible);
        while (playRandomSound && isVisible)
        {
            int randomSeconds = UnityEngine.Random.Range(5, 20);
            // GameLog.LogMessage("PlayFriendSoundRoutine Random:"+ randomSeconds);
            yield return new WaitForSeconds(randomSeconds);
            // GameLog.LogMessage("PlayFriendSoundRoutine Random sound entered:");
            audioBehaviour?.PlayRandomSound();

        }
    }
    // Start is called before the first frame update
    private void Start()
    {

        PlayFriendSound();

    }


    


    void OnEquipmentAddedAction(Item addedItem)
    {

        if (interactableObject && currentFriendTask.interactableItemName.Equals(interactableObject.interactableItemName))
            interactableObject.PerformCheckAndAction();
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {


            isVisible = true;
            //playRandomSound = false;
            PlayFriendSound();
            collision.GetComponent<CharacterEquipment>().OnEquipmentAdded += OnEquipmentAddedAction;
            inTrigger = true;
            if (!friendTasks)
            {
                GameLog.LogMessage("Missing Unit Tasks Behaviour !");
                return;
            }
            //Sprawdzenie czy wszsytkie taski wykonane
            allTasksDone = friendTasks.CheckAllTasksDone();
            if (allTasksDone)
            {
                AllTasksDone();
                //DisplayShop();
                return;
            }
            if (characterAnim)
                DialogManager.Instance?.SetCharacterAnimator(characterAnim);
            if (!playerAnim)
                playerAnim = collision.gameObject.GetComponent<Animator>();

            // if (playerAnim)
            DialogManager.Instance?.SetPlayerAnimator(playerAnim);


            //Pobranie taska do wykonania - pierwsze nie zrobionego
            var next_task = friendTasks.GetNextUnitTask();
            //Sprawdzenie czy poprzednie taski od ktorych task zalezy zostaly wykonane
            GameLog.LogMessage("OnTriggerEnter ProcessTask" + next_task.TaskName);
            ProcessTask(next_task);



        }
    }

    private void ProcessTask(AbecadlowoTask _task)
    {
        GameLog.LogMessage("Process Task entered" + _task);
        if (_task.dependOnPreviousTasks)
        {
            bool allTasksDone = _task.CheckPreviousTasksDone();
            GameLog.LogMessage("Previous tasks done" + allTasksDone);
            if (!allTasksDone)
            {
                //Debug.Break();
                friendDialogs.PlayComeBackLater();
                return;
            }

        }
        currentFriendTask = friendTasks.SetCurrentTask(_task);
        if (currentFriendTask.interactableItemName != null)
        {
            GameLog.LogMessage("Setup interactable Name" + currentFriendTask.interactableItemName + "Task name=" + currentFriendTask.TaskName);
            interactableObject?.SetupInteractableObject(currentFriendTask.interactableItemName, currentFriendTask.TaskName);


        }
        //setup Task to set Item from task to intertable object




        //odegranie dialogu z taska
        dialogPlayed = friendDialogs.PlayDialogOrShort(_task.relatedDialog);
        // Task jest aktywny   
        GameLog.LogMessage("Checking task" + _task.name + " Aktywny" + _task.active + " name Object z taska" + _task.nameObjectName);
        // if(!_task.nameObjectName.Equals("None"))
         
           GameLog.LogMessage("Start Waiting for related dialog to task"+ _task.relatedDialog);

            if (waitForDialogRoutine!=null)
                StopCoroutine(waitForDialogRoutine);    

            waitForDialogRoutine = StartCoroutine(WaitForDialog(_task.relatedDialog, _task.nameObjectName));
        

        //task jest nieaktywny - poczatek nowego tasku


    }

    IEnumerator WaitForDialog(string _dialogName, string nameObjectName)
    {
           
        GameLog.LogMessage("Checking task dialog:" + _dialogName + " name Object name" + nameObjectName);
        WaitingDialog = DialogManager.Instance.GetDialog(_dialogName);
        dialogPlayed = WaitingDialog.dialogPlayed;
        startChecking = true;
        yield return new WaitUntil(() => dialogPlayed);
        WaitingDialog = null;
        GameLog.LogMessage("SUCCESS Checking task dialog:" + _dialogName + "currentFriendTask.interactableItemName:" + currentFriendTask.interactableItemName);
        GameLog.LogMessage("Current friend task:" + currentFriendTask.name);
        GameHandler.Instance.ShowInventory();
        if (interactableObject && currentFriendTask.interactableItemName.Equals(interactableObject.interactableItem.Name))
            interactableObject.PerformCheckAndAction();
        GameLog.LogMessage("Name object =" + nameObjectName);
        if (!nameObjectName.Equals("") && !nameObjectName.Equals("None"))
        {
            friendName.NameIn = friendName.GetNameObject(currentFriendTask.nameObjectName);//friendName.GetNameObject(nameObjectName);
            GameLog.LogMessage("Set name listener on " + friendName);
            DisplayNameIn();
            SetNameSuccessListener();


        }
    }


   

    private void OnTriggerStay2D(Collider2D collision)
    {
        //  GameLog.LogMessage("start checking " + startChecking);
        if (startChecking)
        {
            if (WaitingDialog)
                dialogPlayed = WaitingDialog.dialogPlayed;
            //  GameLog.LogMessage("Dialog Played from update " + dialogPlayed);
            if (dialogPlayed)
                startChecking = false;
        }



    }

    private bool ProcessNewTask()
    {
        
        bool processNewTask = false;
        if (WaitingDialog != null && WaitingDialog.dialogPlayed)
            return false;

        if (inTrigger && currentFriendTask != null && !currentFriendTask.active && currentFriendTask.done)
        {
            GameLog.LogMessage("Process task from OnTriggerStay2D:" + currentFriendTask.name + " active:" + currentFriendTask.active);
            AbecadlowoTask next_task = friendTasks.GetNextUnitTask();
            UnSetNameSuccessListener();
            if (next_task.interactableItemName != null)
            {
                GameLog.LogMessage("Setup interactable Name" + next_task.interactableItemName + "Task name=" + next_task.TaskName);
                interactableObject?.SetupInteractableObject(next_task.interactableItemName, next_task.TaskName);


            }
            
            GameLog.LogMessage("OnTriggerStay2D entered, starting next task" + next_task.name);
            if (next_task)
            {
                processNewTask = true;
                GameLog.LogMessage("Process task from OnTriggerStay2D:" + next_task.name + "currentFriendTask.TaskName" + currentFriendTask.TaskName);
                if (next_task.TaskName != currentFriendTask.TaskName)
                {
                    GameLog.LogMessage("Przed Process Taskiem");
                    ProcessTask(next_task);
                }

            }

        }
        return processNewTask;

    }




    private void HideName()
    {

        Name.HideNameBox();
        displayed = false;


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!equipment)
                equipment = collision.GetComponent<CharacterEquipment>();
            stopProcessing();

        }
    }



    private void stopProcessing()
    {

        inTrigger = false;
        if (equipment)
            equipment.OnEquipmentAdded -= OnEquipmentAddedAction;
        if (DialogManager.Instance)
            DialogManager.Instance.StopDialog();
        if (waitForDialogRoutine != null)
            StopCoroutine(waitForDialogRoutine);
        if (waitForSuccessDialogRoutine != null)
            StopCoroutine(waitForSuccessDialogRoutine);
        HideName();
        shop?.HideShop();
        HideQuestionMark();
    }


    private void OnDisable()
    {

        StopAllCoroutines();
        //   stopProcessing();
    }

}
