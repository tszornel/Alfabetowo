/* 
 Tomasz Szornel
    --------------------------------------------------
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class UI_letterDrag : MonoBehaviour
{
    public static UI_letterDrag Instance { get; private set; }
    private Canvas canvas;
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private CanvasGroup canvasGroup;
    public TextMeshProUGUI letterText;
    public TextMeshProUGUI amountText;
    private LetterObject item;
   // private bool _isDragActive;
    private void Awake()
    {
        Instance = this;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        letterText = transform.Find("letterText").GetComponent<TextMeshProUGUI>();
        amountText = transform.Find("amountText").GetComponent<TextMeshProUGUI>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
       // _isDragActive = false;
        //  Hide();
    }
    private void Update()
    {
        /*  if (Input.touchCount > 0)
          {
              for (int i = 0; i < Input.touchCount; i++)
              {
                  GameLog.LogMessage("Touch Count" + i);
                  if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
                  {
                      Drag();
                      RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.GetTouch(i).position, null, out Vector2 localPoint);
                      transform.localPosition = localPoint;
                  }
                  else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                  {
                      if (_isDragActive) {
                          Drop();
                      }
                  }
              }
          }
          else 
          {
              GameLog.LogMessage("_isDragActive=" + _isDragActive);
              if (_isDragActive && Input.GetMouseButtonDown(0))
              {
                  Drop();
              }
              else if (!_isDragActive && Input.GetMouseButtonUp(0))
              {
                  Drag();
                  GameLog.LogMessage("move item with mouse");
                  UpdatePosition();
              }
              else if (_isDragActive == true) {
                  UpdatePosition();
              }
          }
          //*/
    }
    void Drop()
    {
        GameLog.LogMessage("Item Dropped");
      //  _isDragActive = false;
    }
    void Drag()
    {
        GameLog.LogMessage("Item Dragged");
      //  _isDragActive = true;
    }
    public void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Input.mousePosition, null, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }
    public LetterObject GetItem()
    {
        return item;
    }
    public void SetItem(LetterObject item)
    {
        this.item = item;
    }
    public void SetLetterText(string _text)
    {
        if (!letterText)
            letterText = transform.Find("letterText").GetComponent<TextMeshProUGUI>();
        letterText.text = _text;
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
        gameObject.SetActive(false);
    }
    public void Show(LetterObject item)
    {
        //  UpdatePosition();
    }
}
