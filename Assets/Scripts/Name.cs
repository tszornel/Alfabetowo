using System;
using UnityEngine;

public class Name : MonoBehaviour
{
    public event EventHandler OnPlaySuccess;
   // private Animator anim;
    public static Action<NameObject> OnShowName;
   // private DialogTrigger dialog;
  //  private bool nameBoxShown;
    [StringInList(typeof(PropertyDrawerHelper), "AllNames")]
    public string  nameName;
    public NameObject nameIn;
    // Start is called before the first frame update
    public GameObject namePanel;
    private bool successPlayed;
    [SerializeField] WindowCharacter_Portrait inventoryWindow;
    [HideInInspector]
    [SerializeField] private Item item;
    [StringInList(typeof(PropertyDrawerHelper), "AllItemsAndLetters")]
    //[SerializeField] private string itemName;
    [HideInInspector]
    [SerializeField] private GameObject itemPrefab;
    public static DisplayName nameBox;
    private NamesDatabase nameDatabase;
    public bool dynamicName= false;
    public Transform placeToSpawn;
    // public bool repairedObjectIn = false;
    public GameObject ItemPrefab { get => itemPrefab; set => itemPrefab = value; }
    public NameObject NameIn { get => nameIn; set => nameIn = value; }
    public Item ItemIn { get => item; set => item = value; }
    public NameObject GetNameObject(string nameString) {
        if (!nameDatabase)
        {
            //  nameDatabase = Resources.Load<NamesDatabase>("NamesDatabase");

            nameDatabase =PickupItemAssets.Instance.namesDatabase;
        }
            
        return nameDatabase?.GetNameObject(nameString);
    }
    private void Awake()
    {
        successPlayed = false;
        if (!nameBox) {
            nameBox = GameObject.FindObjectOfType<DisplayName>(true);
            
        }
            
        //nameDatabase = PickupItemAssets.Instance.namesDatabase;
        if(!nameDatabase)
            nameDatabase = Resources.Load<NamesDatabase>("NamesDatabase");
       // dialog = GetComponent<DialogTrigger>();
       // anim = GetComponent<Animator>();
        if (!nameIn && nameName != null && !dynamicName)
        {
            NameIn = nameDatabase?.GetNameObject(nameName);
        }
        
        
       // ItemObject itemObject = PickupItemAssets.Instance.allItemsDatabase.GetItemObjectFromName(itemName.ToString());
        if (!dynamicName && nameName != null)
            nameIn = nameDatabase?.GetNameObject(nameName);
       /* if (itemObject) {
            item = itemObject.createItem();
            itemPrefab = itemObject.itemPrefab;
        }
        else
            item = null;*/
        //GameLog.LogError("Item Name not specified in name object");
    }
    public void UncheckSuccess() {

        successPlayed = false;
    }
    
    
    public static void HideNameBox() 
    {
        if (nameBox && nameBox.isOpen)
            nameBox.HideNameBox();
        GameHandler.Instance.HideInventory();
    }
    public static void ShowName(NameObject name)
    {
        GameLog.LogMessage("ShowName Entered:"+name);
        if (OnShowName != null)
        {
            OnShowName(name);
        }
    }
    public void ShowName()
    {
        GameLog.LogMessage("Show Name:"+ nameIn.name);
        if (OnShowName != null)
        {
            OnShowName(nameIn);
        }
    }
    public static void ShowNameBox() 
    {
        GameLog.LogMessage("ShowNameBox entered");
        nameBox.gameObject.SetActive(true);
        nameBox?.ShowNameBox();
    }
    private void OnEnable()
    {
        DisplayName.OnNameChanged += OnNameChangeUpdate;
    }
    private void OnDisable()
    {
        DisplayName.OnNameChanged -= OnNameChangeUpdate;
    }
    private void PlaySuccess()
    {
        GameLog.LogMessage("Wielki Sukces");
        successPlayed = true;
        if (OnPlaySuccess == null) 
        {
            GameLog.LogMessage("OnPlaySuccess nie ustawiony !!!!!!!!!!!!!"+this);
        }
        OnPlaySuccess?.Invoke(this, EventArgs.Empty);
        if (dynamicName)
        {
            item.damaged = false;
            if (!placeToSpawn)
                placeToSpawn = transform;
             DropItem(placeToSpawn); 
        }
    }
   public void DropItem(Transform placeToSpawn)
    {
        GameLog.LogMessage("Drop item entered");
        if (item != null)
        {
            item.damaged = false;
            PickupItem.SpawnItemWorld(placeToSpawn.position, Quaternion.identity, item, transform.parent);
        }
    }
    public void DropItem() 
    {
        DropItem(transform);
    }
  /* private void OnTriggerStay2D(Collider2D collision)
    {
        if(dynamicName)
            if (collision.tag == "Player")
            {
                bool currentTaskDone = false;
                AbecadlowoTask currentTask = GetComponent<UnitTasksBehaviour>()?.GetCurrentTask();
                if(currentTask)
                    currentTaskDone = currentTask.done; 
                if (nameIn && !nameBox.isOpen&& !currentTaskDone)
                {
                    nameBox.ShowNameBox();
                    ShowName(nameIn);
                    if (!inventoryWindow.gameObject.activeSelf)
                       inventoryWindow.gameObject.SetActive(true);
                    inventoryWindow.Show();
                 }
                 else if (nameBox.isOpen)
                    nameBox.HideNameBox();
             }
      }*/
   /*private void OnTriggerExit2D(Collider2D collision)
    {
        GameLog.LogMessage("Name OnTriggerExit2D entered");
        if (dynamicName)
            if (collision.tag == "Player")
            {
                GameLog.LogMessage("Name on trigger exit colllide with player");
                nameBox.HideNameBox();
                if (inventoryWindow.gameObject.activeSelf)
                    inventoryWindow.Hide(3);
            }
     }*/
    private void OnNameChangeUpdate(object sender, DisplayName.OnLetterDroppedEventArgs e) {
        if (nameIn != null && nameIn.displayedName == e.nameObject.displayedName)
            GameLog.LogMessage("Success already played ?" + successPlayed);
        
            if (nameIn.done = nameIn.CheckCorrect() && !successPlayed)
            {
                GameLog.LogMessage("Play success in Update");
                PlaySuccess();
            }
    }
}
