using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Webaby.Utils;

public class DisplayName : MonoBehaviour
{
    public static DisplayName Instance { get; private set; }
    public class OnLetterDroppedEventArgs : EventArgs
    {
        public NameObject nameObject;
        public NameSegment changedSegment;
    }

    public RectTransform mainBoxSize;
    public RectTransform textBoxSize;
    public static event EventHandler<OnLetterDroppedEventArgs> OnNameChanged;
     public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEN_ITEM;
    private int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    public Transform player;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public Transform itemNamePrefab;
    public Image nameImage;
    public TextMeshProUGUI portraitName;
    private PlayerPlatformerController playerController;
    List<GameObject> slotsToRelease;
    List<GameObject> itemsToRelease;
    public bool isOpen = false;
    private NameObject displayedName;
    public float tweenTime = 0.5f;
    public Vector3 scaleFactor;
    
    public LeanTweenType tweenType;
    private UnitAudioBehaviour audioBehaviour;
    private Item recentlyUsedItem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        slotsToRelease = new List<GameObject>();
        itemsToRelease = new List<GameObject>();
        playerController = player.GetComponent<PlayerPlatformerController>();
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
    }
    private void OnEnable()
    {
         Name.OnShowName += DisplayNameData;
    }

    private void OnDisable()
    {
        Name.OnShowName -= DisplayNameData;
    }



    void ClearInventoryDisplay()
    {

        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            if (child.name.Contains("NameSlot"))
            {
                Transform itemToRelease = child.Find("nameLetterItem");
                if (itemToRelease != null)
                {
                    Debug.Log("FOUND ITEM TO RELEASE" + itemToRelease.GetComponent<UI_ItemMain>().ToString());
                    itemToRelease.GetComponent<UI_ItemMain>().SetItem(null);
                    itemToRelease.GetComponent<UI_ItemMain>().SetLetterText("");
                    if (itemToRelease.gameObject.activeSelf)
                        itemsToRelease.Add(itemToRelease.gameObject);
                 }
                child.GetComponent<UI_ItemSlot>().SetSlotItemId(0);
                slotsToRelease.Add(child.gameObject);
        
            }
        }
        foreach (GameObject slot in slotsToRelease)
        {
            ObjectPoolerGeneric.Instance.ReleaseToPool("InventorySlots", slot);
        }
        slotsToRelease.Clear();
        foreach (GameObject item in itemsToRelease)
        {
            ObjectPoolerGeneric.Instance.ReleaseToPool("UILetters", item);
        }
        itemsToRelease.Clear();
    }


    // Update is called once per frame
    public NameObject GetDisplayedName() {

        return displayedName;
    }
    
    public void DisplayNameData(NameObject nameInfo)
    {

        GameLog.LogMessage("DisplayNameData entered:" + nameInfo);
        displayedName = nameInfo;
        //ShowNameBox();
        ClearInventoryDisplay();
        nameImage.sprite = nameInfo.characterImage;
        portraitName.text = nameInfo.displayedName;
        NUMBER_OF_COLUMN = nameInfo.nameLetters.Count;
        UpdateNamePanelSize(NUMBER_OF_COLUMN);
       // GameLog.LogMessage("Canvas width" + transform.GetComponent<RectTransform>().rect.width);

       // rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, other.rect.width);
       // rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, other.rect.height);

        for (int i = 0; i < nameInfo.nameLetters.Count; i++)
        {
            GameObject itemSlot = ObjectPoolerGeneric.Instance.SpawnFromPool("NameSlots", itemSlotTemplate.position, Quaternion.identity, itemSlotContainer);
            itemSlot.name = "NameSlot";
            RectTransform itemSlotRectTransform = itemSlot.GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
          //  itemSlotRectTransform.anchoredPosition = new Vector2(itemSlotRectTransform.anchoredPosition.x+ X_START, itemSlotRectTransform.anchoredPosition.y);//GetPosition(i);



            // float ScaleFactor = ((Screen.width/Screen.height*10f)/(1900f/600f));
            // itemSlotRectTransform.localScale = new Vector3(ScaleFactor, 1f, 1f);
            UI_ItemSlot uiItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
            //ustaw 
            uiItemSlot.slotNumber = i;
            Boolean tmpActive = nameInfo.nameLetters[i].active;
            GameLog.LogMessage("TMP active" + tmpActive);
            if (!tmpActive)
            {

               
                int letterId = nameInfo.nameLetters[i].nameLetter.Id;
                GameLog.LogMessage("Set slot letter id" + letterId);
                uiItemSlot.SetSlotItemId(letterId);
                uiItemSlot.GetComponent<Button_UI>().ClickFunc = () =>
                {
                    // Use item
                    GameLog.LogMessage("SUPER !!! Nacisniety Button Slota ");
                    ClickOnSlot(uiItemSlot);    
                    //  displayItem.gameObject.SetActive(true);
                    // displayItem.ShowItemDescription(item);
                };
                GameLog.LogMessage("Referencja przed onDrop" + nameInfo.ToString());
                int storeIndex = i;
                uiItemSlot.SetOnDropAction(() =>
                {
                    // Dropped on this UI Item Slot
                    UI_itemTouch.Instance.touchItemDropedinName = true;
                    if (UI_itemTouch.Instance)
                        UI_itemTouch.Instance.Hide();
                    GameLog.LogMessage("Referencja w ondrop" + nameInfo.ToString() + nameInfo.GetInstanceID());
                    Item draggedItem = UI_itemTouch.Instance.GetItem();
                    
                    GameLog.LogMessage("Dragged item" + draggedItem);

                    CheckSlot(uiItemSlot, draggedItem);
                    /*if (uiItemSlot?.GetSlotItemId() == draggedItem.Id)
                    {
                        GameLog.LogMessage("Slot item id equal to dragged item");
                        NameSegment ms = nameInfo.nameLetters[storeIndex];
                        ms.active = true;
                        nameInfo.nameLetters[storeIndex] = ms;
                       // itemSlotTemplate.gameObject.SetActive(true);
                        var obj = ObjectPoolerGeneric.Instance.SpawnFromPool("UILetters", itemSlot.transform.position, itemSlot.transform.rotation, itemSlot.transform);
                        obj.transform.SetParent(itemSlot.transform,false);
                        obj.transform.SetAsLastSibling();
                        obj.name = "nameLetterItem";
                        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
                        canvasGroup.blocksRaycasts = false;
                        UI_ItemMain uiItem = obj.GetComponent<UI_ItemMain>();
                        uiItem.SetItem(draggedItem);
                        uiItem.Show();
                        draggedItem.GetItemHolder()?.RemoveItem(draggedItem);
                        //player.GetComponent<PlayerPlatformerController>().GetInventory().RemoveItem(draggedItem);
                        OnNameChanged?.Invoke(this, new OnLetterDroppedEventArgs { nameObject = nameInfo, changedSegment = ms });
                      
                    }
                    else
                    {
                        GameLog.LogMessage("Nie zgadza sie przedmiot ze slotem");
                        PlayDoesNotFitSound();
                    }*/
                });
            } else {
                GameLog.LogMessage("adding not active letter to name");
                // var obj = Instantiate(itemNamePrefab, itemSlot);
                var obj = ObjectPoolerGeneric.Instance.SpawnFromPool("UILetters", itemSlot.transform.position, itemSlot.transform.rotation, itemSlot.transform);
                obj.transform.SetParent(itemSlot.transform, false);
                obj.transform.position = itemSlot.transform.position;
                obj.transform.SetAsLastSibling();
                obj.name = "nameLetterItem";
                UI_ItemMain uiItem = obj.GetComponent<UI_ItemMain>();
                CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
                canvasGroup.blocksRaycasts = false;
                //canvasGroup.alpha = 1f;
                Item letterToAdd = nameInfo.nameLetters[i].nameLetter;
                GameLog.LogMessage("Set letter="+ letterToAdd,obj);
                letterToAdd.SetLocation(ItemLocation.Name);
                uiItem.SetItem(letterToAdd);
                uiItem.Show();
            }

        }
        itemSlotTemplate.gameObject.SetActive(false);
        ShowNameBox();
    }


    public void ClickOnSlot(UI_ItemSlot uiItemSlot) {

        
        if(!playerController)
            playerController = GameObject.FindObjectOfType<PlayerPlatformerController>(true);


        recentlyUsedItem = playerController.GetUsedItem();
        if (recentlyUsedItem == null)
            return;


        CheckSlot(uiItemSlot, recentlyUsedItem);


    }

    private void CheckSlot(UI_ItemSlot uiItemSlot, Item item)
    {
        if(item!=null)
        if (uiItemSlot.GetSlotItemId() == item.Id)
        {
            playerController.NullUsedItem();
            GameLog.LogMessage("Slot item id equal to dragged item");
            NameSegment ms = displayedName.nameLetters[uiItemSlot.slotNumber];
            ms.active = true;
            displayedName.nameLetters[uiItemSlot.slotNumber] = ms;
            // itemSlotTemplate.gameObject.SetActive(true);
            var obj = ObjectPoolerGeneric.Instance.SpawnFromPool("UILetters", uiItemSlot.transform.position, uiItemSlot.transform.rotation, uiItemSlot.transform);
            obj.transform.SetParent(uiItemSlot.transform, false);
            obj.transform.SetAsLastSibling();
            obj.name = "nameLetterItem";
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
            UI_ItemMain uiItem = obj.GetComponent<UI_ItemMain>();
            uiItem.SetItem(item);
            uiItem.Show();
            TweenSlot(item, uiItemSlot.gameObject, ms);

        }
        else
        {
            GameLog.LogMessage("Nie zgadza sie przedmiot ze slotem");
            PlayDoesNotFitSound();
        }
    }

    public void TweenSlot(Item _item, GameObject go,NameSegment changedSegment) 
    {

        audioBehaviour?.PlayUIPanelBounce();
        go.LeanScale(scaleFactor, tweenTime).setEase(tweenType).setLoopOnce();
        go.LeanScale(Vector3.one, tweenTime).setEase(tweenType).setDelay(.2f).setLoopOnce().setOnComplete(() => {
            _item.GetItemHolder()?.RemoveItem(_item);
            playerController.DisableUsedLetter();
            //player.GetComponent<PlayerPlatformerController>().GetInventory().RemoveItem(draggedItem);
            OnNameChanged?.Invoke(this, new OnLetterDroppedEventArgs { nameObject = displayedName, changedSegment = changedSegment });

        });

    }

  

    void PlayDoesNotFitSound()
    {
            playerController.PlayDoesNotFit();
    }
    private void UpdateNamePanelSize(int numberOfLetters)
    {
        GameLog.LogMessage("Update Panel Size number of letters=" + numberOfLetters);
        float ScalerFactor = 0;
        var sizeDeltaX = 200 * NUMBER_OF_COLUMN + 200;
        mainBoxSize.sizeDelta = new Vector2(sizeDeltaX, 260);
        if (sizeDeltaX > 1600)
        {

            ScalerFactor = (1600 * 1f / sizeDeltaX * 1f);
            GameLog.LogMessage("ScalerFactor=" + ScalerFactor);
            mainBoxSize.localScale = new Vector3(ScalerFactor, ScalerFactor, 1f);


        }
        else {

            mainBoxSize.localScale = new Vector3(1f, 1f, 1f);

        }

        mainBoxSize.ForceUpdateRectTransforms();
        var textBoxSizeDeltaX = 200 * NUMBER_OF_COLUMN;
        
        if (textBoxSizeDeltaX > 1600)
        {
            // ScalerFactor = 0.9f; // (1600 * 1f / textBoxSizeDeltaX * 1f);
            // GameLog.LogMessage("ScalerFactor=" + ScalerFactor);
            // textBoxSize.localScale = new Vector3(ScalerFactor, ScalerFactor, 1f);
            
            textBoxSize.sizeDelta = new Vector2(180 * NUMBER_OF_COLUMN, 180);

        }
        else 
        {
            textBoxSize.sizeDelta = new Vector2(200 * NUMBER_OF_COLUMN, 200);
           // textBoxSize.localScale = new Vector3(1f, 1f, 1f);

        }
       // textBoxSize.ForceUpdateRectTransforms();
    }
    private Vector2 GetPosition(int i)
    {
        return new Vector2(X_START + (X_SPACE_BETWEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)));
    }
    void OnDestroy()
    {
        Name.OnShowName -= DisplayNameData;
    }


    private void SetNameWindow(GameObject nameBox, float from, float to, bool close)
    {
        GameLog.LogMessage("SetWindow Entered " + to);
       // LeanTween.cancel(nameBox);
        float tweenTime = 0.5f;

        LeanTween.value(nameBox, from, to, tweenTime).setOnUpdate((to) =>
        {
            nameBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, to);
        }
        ).setEaseOutSine().setOnCompleteParam(close).setOnComplete(() => DisableNameBox(close));
    }
    public void DisableNameBox(bool close)
    {
        GameLog.LogMessage("DisableNameBox entered:" + close);
        if (close) {
            gameObject.SetActive(false);
            
        }
    }

    public void HideNameBox()
    {
      if(isOpen)
        SetNameWindow(gameObject, -250f, 200f, true);
        
        isOpen = false;
    }
    public void ShowNameBox()
    {
        GameLog.LogMessage("ShowNameBox entered");
       if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        if (!isOpen) 
        {
            isOpen = true;
            SetNameWindow(gameObject, 200f, -250f, false);
        }
       
    }
}