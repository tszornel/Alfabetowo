using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class UI_itemTouch : MonoBehaviour,IBeginDragHandler, IDragHandler,IEndDragHandler
{
    public static UI_itemTouch Instance { get; private set; }
    private Canvas canvas;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private CanvasGroup canvasGroup;
    private UnityEngine.UI.Image image;
    private Item item;
    private TextMeshProUGUI amountText;
    private TextMeshProUGUI letterText;
    public bool touchItemMoving;
    public bool touchItemDropedinEquipment;
    public bool touchItemDropedinInventory;
    public bool touchItemDropedinFurnace;
    public bool touchItemDropedinName;
    // public static ItemDatabaseObject database; 
    private void Awake()
    {
        Instance = this;
        touchItemDropedinEquipment = false;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        image = transform.Find("image").GetComponent<UnityEngine.UI.Image>();
        letterText = transform.Find("letterText").GetComponent<TextMeshProUGUI>();
        amountText = transform.Find("amountText").GetComponent<TextMeshProUGUI>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        Hide();
    }
    private void Update()
    {
      UpdatePosition();
    }
    private void UpdatePosition()
    {
        touchItemMoving = true;
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                GameLog.LogMessage("Touch Count" + i);
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.GetTouch(i).position, null, out Vector2 localPoint);
                    transform.localPosition = localPoint;
                    // RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvasOfImageToMove.transform as RectTransform, Input.mousePosition, parentCanvasOfImageToMove.worldCamera, out pos);
                    //  imageToMove.transform.position = parentCanvasOfImageToMove.transform.TransformPoint(pos);
                }
                else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    //
                    touchItemMoving = false;
                }
            }
        }
        else if (Input.mousePresent)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.mousePosition, null, out Vector2 localPoint);
            transform.localPosition = localPoint;
            touchItemMoving = true;
        }
        else {
            touchItemMoving = false;
        }
    }
    public Item GetItem()
    {
        return item;
    }
    public void SetItem(Item item)
    {
        this.item = item;
        GameLog.LogMessage("Item set to item touch:" + item);
        if (item.GetItemHolder()!=null)
            GameLog.LogMessage("Item Holder in UI_ItemTouch:" + item.GetItemHolder());
        else
            GameLog.LogMessage("Item Holder in UI_ItemTouch null" + item.GetItemHolder());
    }
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
    public void SetAmountText(int amount)
    {
        if (amount <= 1)
        {
            amountText.text = "";
        }
        else
        {
            // More than 1
            amountText.text = amount.ToString();
        }
    }
    public void Hide()
    {
        touchItemMoving = false;
        gameObject.SetActive(false);
    }
    public bool isActive() {
        return transform.gameObject.activeSelf;
    }
    public void SetLetterText(string _text)
    {
        if (!letterText)
            letterText = transform.Find("letterText").GetComponent<TextMeshProUGUI>();
        letterText.text = _text;
    }
    public void Show(Item _item)
    {
        if (_item.GetItemType() == ItemObjectType.None)
        {
            return;
        }
        touchItemMoving = true;
        touchItemDropedinInventory = false;
        touchItemDropedinEquipment = false;
        touchItemDropedinName = false;
        touchItemDropedinFurnace = false;
        GameLog.LogMessage("Show draged executed"+_item.ToString());

       // if(_item.Location ==)

      //  _item.GetItemHolder()?.RemoveItem(_item);
        ItemObject itemObject = _item.GetItemObject();   
        if (_item.GetItemType()  == ItemObjectType.Letter) {
           // GameLog.LogMessage("Activate item view in inventory:" + _item.ToString());
            SetItem(_item);
            letterText.gameObject.SetActive(true);
            SetLetterText(((LetterObject)itemObject).GetText());
         //   GameLog.LogMessage("activate  draged letter object");
            transform.gameObject.SetActive(true);
            image.gameObject.SetActive(false);
            gameObject.SetActive(true);
        } else {
           // GameLog.LogMessage("activate  draged letter object");
            SetItem(_item);
            SetSprite(itemObject.itemSprite);
            gameObject.SetActive(true);
            transform.gameObject.SetActive(true);
            image.gameObject.SetActive(true);
            letterText.gameObject.SetActive(false);
            //SetAmountText(item.amount);
        }
       // UpdatePosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
