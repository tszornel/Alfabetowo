using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{

    public static event EventHandler OnSwipeNext;
    public static event EventHandler OnSwipePrevious;
    [SerializeField] int maxPage;
    public int currentPage;
    Vector3 defaultPosition, targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;

    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    private LTDescr tween;
    float dragThreshold;

    public TweenClockPanel clockPanel;
    public bool toggleLabel;
    private bool block;
    private Vector2 anchoredPosition;

    public int GetCurrentPage()
    {
        return currentPage;


    }

    public Transform GetLevelPages() {

        return levelPagesRect;
    }
    
    private void OnEnable()
    {
        maxPage = levelPagesRect.transform.childCount;
        GameLog.LogWarning("Max Page for shop panel" + maxPage);
    }
    private void Awake()
    {
        block = false;
        currentPage = 1;
        defaultPosition = levelPagesRect.localPosition;
        GameLog.LogMessage("Deafult position=" + defaultPosition);
        anchoredPosition = levelPagesRect.anchoredPosition;
        GameLog.LogMessage("Anchored position=" + anchoredPosition);
        targetPos = levelPagesRect.localPosition;
        dragThreshold = Screen.width / 15;
        if(!clockPanel)
            clockPanel = TweenClockPanel.Instance;
    }

   
    public void Next()
    {
        maxPage = levelPagesRect.transform.childCount;
        if (block)
            return;
        block = true;
        GameLog.LogMessage("Swipe Next" + currentPage);
        if (currentPage < maxPage)
        {
            currentPage++;
            targetPos += pageStep;

        }
        else 
        {
            currentPage = 1;
            targetPos = defaultPosition;
            //targetPos -= pageStep * (maxPage-1);
            


        }
        GameLog.LogMessage("New Page:" + currentPage + " targetPos:" + targetPos);
        MovePage();
        OnSwipeNext?.Invoke(this, EventArgs.Empty);
        block = false;


    }

    public void Previous() {
        if (block)
            return;
        block = true;

        GameLog.LogMessage("Swipe Previous"+ currentPage);
    if(currentPage > 1)
        {
            currentPage--;
            targetPos -= pageStep;
           
        }
        else
        {
            currentPage = maxPage;
            targetPos += pageStep * (maxPage - 1);
           


        }
        GameLog.LogMessage("New Page:" + currentPage+" targetPos:"+targetPos);
        MovePage();
        OnSwipePrevious?.Invoke(this, EventArgs.Empty);
        block = false;

    }

    void MovePage() {
                       
        if(tween != null)
             tween.reset();
        Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
        tween = levelPagesRect.LeanMoveLocal(targetPos,tweenTime).setEase(tweenType);
        if(clockPanel && toggleLabel)
            clockPanel.ToggleLabel(currentPage);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
            if (eventData.position.x > eventData.pressPosition.x)
                Previous();
            else 
                Next();
        else
           MovePage();
        
    }

    private void OnDisable()
    {
        currentPage = 1;
       // levelPagesRect.localPosition = defaultPosition;   
    }

    public void ResetCurrentPage()
    {
       /* ContentSizeFitter cf;
        cf = levelPagesRect.GetComponent<ContentSizeFitter>();

        DestroyImmediate(cf);
        HorizontalLayoutGroup hl;
        hl = levelPagesRect.GetComponent<HorizontalLayoutGroup>();
        DestroyImmediate(hl);
*/
        
        
        

        targetPos = defaultPosition;
        levelPagesRect.localPosition = defaultPosition;
        levelPagesRect.anchoredPosition = Vector2.zero;
        levelPagesRect.ForceUpdateRectTransforms();

        currentPage = 1;
        GameLog.LogMessage("new local position" + levelPagesRect.localPosition + "Anchored position=" + levelPagesRect.anchoredPosition);
        //Next();
        //levelPagesRect.ForceUpdateRectTransforms();
        //SetDirty();
      /*  levelPagesRect.gameObject.AddComponent<HorizontalLayoutGroup>();
        HorizontalLayoutGroup hl2 = levelPagesRect.GetComponent<HorizontalLayoutGroup>();


        hl2.childForceExpandHeight = true;
        hl2.childForceExpandWidth = true;
        hl2.childControlHeight = false;
        hl2.childControlWidth = false;
        hl2.childAlignment = TextAnchor.UpperLeft;
       // hl2.SetLayoutHorizontal();
       // hl2.SetLayoutVertical();

        levelPagesRect.gameObject.AddComponent<ContentSizeFitter>();
        ContentSizeFitter cf2 = levelPagesRect.GetComponent<ContentSizeFitter>();
        cf2.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        cf2.verticalFit = ContentSizeFitter.FitMode.Unconstrained;*/

      //  cf2.SetLayoutVertical();
      //  cf2.SetLayoutHorizontal();

    }

   
}
