/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               webaby.com
    --------------------------------------------------
 */
 
//#define SOUND_MANAGER // Has Sound_Manager in project
//#define CURSOR_MANAGER // Has Cursor_Manager in project

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.EnhancedTouch;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Webaby.Utils {
    
    /*
     * Button in the UI
     * */
    public class Button_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,IDragHandler, IEndDragHandler
    {

        public Action ClickFunc = null;
        public Action LongTouchFunc = null;
        public Action ShortTouchFunc = null;
        public Action MouseRightClickFunc = null;
        public Action MouseMiddleClickFunc = null;
        public Action MouseDownOnceFunc = null;
        public Action MouseUpFunc = null;
        public Action MouseOverOnceTooltipFunc = null;
        public Action MouseOutOnceTooltipFunc = null;
        public Action MouseOverOnceFunc = null;
        public Action MouseOutOnceFunc = null;
        public Action MouseOverFunc = null;
        public Action MouseOverPerSecFunc = null; //Triggers every sec if mouseOver
        public Action MouseUpdate = null;
        public Action<PointerEventData> OnPointerClickFunc;
        GameObject pressedObject;

        public enum HoverBehaviour {
            Custom,
            Change_Color,
            Change_Image,
            Change_SetActive,
        }
        public HoverBehaviour hoverBehaviourType = HoverBehaviour.Custom;
        private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;
        public Color hoverBehaviour_Color_Enter, hoverBehaviour_Color_Exit;
        public Image hoverBehaviour_Image;
        public Sprite hoverBehaviour_Sprite_Exit, hoverBehaviour_Sprite_Enter;
        public bool hoverBehaviour_Move = false;
        public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
        private Vector2 posExit, posEnter;
        public bool triggerMouseOutFuncOnClick = false;
        private bool mouseOver;
        private float mouseOverPerSecFuncTimer;
        private float touchPerSecFuncTimer;
        private Action internalOnPointerEnterFunc, internalOnPointerExitFunc, internalOnPointerClickFunc;
        public bool checkLongTouch;
        PointerEventData m_PointerEventData;


        // If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
        public const float MAX_SWIPE_TIME = 0.5f;

        // Factor of the screen width that we consider a swipe
        // 0.17 works well for portrait mode 16:9 phone
        public const float MIN_SWIPE_DISTANCE = 0.55f;

        public static bool swipedRight = false;
        public static bool swipedLeft = false;
        public static bool swipedUp = false;
        public static bool swipedDown = false;
        Vector2 startPos;
        // private float timePressed = 0.0f;

        // private float timeLastPress = 0.0f;

        // public float timeDelayThreshold = 0.2f;

#if SOUND_MANAGER
        public Sound_Manager.Sound mouseOverSound, mouseClickSound;
#endif
#if CURSOR_MANAGER
        public CursorManager.CursorType cursorMouseOver, cursorMouseOut;
#endif


        public virtual void OnPointerEnter(PointerEventData eventData) {
            if (internalOnPointerEnterFunc != null) internalOnPointerEnterFunc();
            if (hoverBehaviour_Move) transform.localPosition = posEnter;
            if (hoverBehaviourFunc_Enter != null) hoverBehaviourFunc_Enter();
            if (MouseOverOnceFunc != null) MouseOverOnceFunc();
            if (MouseOverOnceTooltipFunc != null) MouseOverOnceTooltipFunc();
            mouseOver = true;
            mouseOverPerSecFuncTimer = 0f;
           // timePressed = Time.time;
           // GameLog.LogMessage("On pointer enter" + Time.time);
            
        }
        public virtual void OnPointerExit(PointerEventData eventData) {
            if (internalOnPointerExitFunc != null) internalOnPointerExitFunc();
            if (hoverBehaviour_Move) transform.localPosition = posExit;
            if (hoverBehaviourFunc_Exit != null) hoverBehaviourFunc_Exit();
            if (MouseOutOnceFunc != null) MouseOutOnceFunc();
            if (MouseOutOnceTooltipFunc != null) MouseOutOnceTooltipFunc();
            mouseOver = false;
           // GameLog.LogMessage("On pointer exit" + Time.time);
            

            /* if (Time.time - timePressed > timeDelayThreshold) {
                 Debug.Log("Long Pressed on " + transform.name);
                     if (LongTouchFunc!=null)
                              LongTouchFunc();
                 }*/

            //touchPerSecFuncTimer = 0;

        }

        private enum DraggedDirection
        {
            Up,
            Down,
            Right,
            Left
        }
       
        public void OnEndDrag(PointerEventData eventData)
        {
          
            Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
           
            if (GetDragDirection(dragVectorDirection) == DraggedDirection.Down && dragVectorDirection.magnitude > MIN_SWIPE_DISTANCE)
                if (LongTouchFunc != null) { LongTouchFunc(); } else {

                    GameLog.LogMessage("NO long fun defined yet");
                
                }
            
            ;
            //think if cancel is needed or move click to EndDrag
        }

        //It must be implemented otherwise IEndDragHandler won't work 
        public void OnDrag(PointerEventData eventData)
        {
           // GameLog.LogMessage("OnDrag enter" + Time.time);
        }
        private DraggedDirection GetDragDirection(Vector3 dragVector)
        {
            float positiveX = Mathf.Abs(dragVector.x);
            float positiveY = Mathf.Abs(dragVector.y);
            DraggedDirection draggedDir;
            if (positiveX > positiveY)
            {
                draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
            }
            else
            {
                draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
            }
            Debug.Log(draggedDir);
            return draggedDir;
        }
        public virtual void OnPointerClick(PointerEventData eventData) {
            GameLog.LogMessage("On pointer click");
            Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
            if (internalOnPointerClickFunc != null) internalOnPointerClickFunc();
            if (OnPointerClickFunc != null) OnPointerClickFunc(eventData);
            GameLog.LogMessage("Left click"+ eventData.button.ToString());
            if (eventData.button == PointerEventData.InputButton.Left) {
                GameLog.LogMessage("Left click click count" + eventData.clickCount+ " click time"+ eventData.clickTime);

                
                pressedObject = EventSystem.current.currentSelectedGameObject;
                if(pressedObject)
                GameLog.LogMessage("pressed object" + pressedObject.name);
                if (touchPerSecFuncTimer > 0)
                {
                    GameLog.LogMessage("double click=" + touchPerSecFuncTimer + " clickCount:" + eventData.clickCount);
                    touchPerSecFuncTimer -= Time.unscaledDeltaTime;
                }
                touchPerSecFuncTimer = 0.9f;

                /*if (checkLongTouch)
                {
                    GameLog.LogMessage("touchPerSecFuncTimer=" + touchPerSecFuncTimer);
                    if (touchPerSecFuncTimer > 0)
                    {
                        GameLog.LogMessage("Short func click");
                        if (ShortTouchFunc != null) ShortTouchFunc();
                        touchPerSecFuncTimer = 0;
                    }
                    else
                    {
                        GameLog.LogMessage("Long func click");
                        if (LongTouchFunc != null) LongTouchFunc();
                        //short touch
                        touchPerSecFuncTimer = 0;

                    }
                }*/

                //else 
                //{
                if (triggerMouseOutFuncOnClick)
                    {
                        OnPointerExit(eventData);
                    }

                    if (ClickFunc != null)
                    {

                        GameLog.LogMessage("Click FUn != null");
                        ClickFunc();
                    }
               // }
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                //double touch ??
                GameLog.LogMessage("Double Touch !!!!!!!!!!!!!!!!!!!!!!");
                if (MouseRightClickFunc != null) MouseRightClickFunc();

            }
            if (eventData.button == PointerEventData.InputButton.Middle)
                if (MouseMiddleClickFunc != null) MouseMiddleClickFunc();
        }
        public void Manual_OnPointerExit() {
            OnPointerExit(null);
        }
        public bool IsMouseOver() {
            GameLog.LogMessage("IsMouseOver enter" + Time.time);
            return mouseOver;
        }
        public void OnPointerDown(PointerEventData eventData) {
            if (MouseDownOnceFunc != null) MouseDownOnceFunc();
        }
        public void OnPointerUp(PointerEventData eventData) {
            if (MouseUpFunc != null) MouseUpFunc();
        }

        void Update() {
            if(pressedObject)
            if(gameObject.name != pressedObject.name)
                if (touchPerSecFuncTimer > 0)
                {
                    GameLog.LogMessage("touchPerSecFuncTimer=" + touchPerSecFuncTimer + " gameobjectName:" + gameObject.name);
                    touchPerSecFuncTimer -= Time.unscaledDeltaTime;
                }

            /*
            if (checkLongTouch && Input.touchCount > 0)
            {
                if (gameObject.name != "SoundControl") {
                    return;
                }
                Touch singleTouch = Input.GetTouch(0);
                int id = singleTouch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    // GameLog.LogMessage("UI pressed");
                    //Set up the new Pointer Event
                    touchPerSecFuncTimer = 0.9f;
                    /*m_PointerEventData = new PointerEventData(EventSystem.current);
                    m_PointerEventData.position = singleTouch.position;
                    List<RaycastResult> results = new List<RaycastResult>();
                    //Raycast using the Graphics Raycaster and mouse click position
                    gameObject.GetComponentInParent<GraphicRaycaster>().Raycast(m_PointerEventData, results);
                    if (results.Count>0 && results[0].gameObject.name == gameObject.name) 
                    {
                        GameLog.LogMessage("Touch phase" + singleTouch.phase);
                        if (singleTouch.phase == TouchPhase.Began)
                        {
                            GameLog.LogMessage("Touch begins" + touchPerSecFuncTimer);
                      
                            touchPerSecFuncTimer = 0.9f;
                            timePressed = Time.time;
                            GameLog.LogMessage("Press touch from time:" + timePressed);
                            startPos = new Vector2(singleTouch.position.x / (float)Screen.width, singleTouch.position.y / (float)Screen.width);
                        }
                       
                    }
               
                }
      
             }*/

            if (mouseOver) {
             
                if (ClickFunc != null) {
                
                }
                if (MouseOverFunc != null) MouseOverFunc();
                mouseOverPerSecFuncTimer -= Time.unscaledDeltaTime;
                if (mouseOverPerSecFuncTimer <= 0) {
                    mouseOverPerSecFuncTimer += 1f;
                    if (MouseOverPerSecFunc != null) MouseOverPerSecFunc();
                }
            }
            if (MouseUpdate != null) MouseUpdate();

         }
        void Awake() {
            posExit = transform.localPosition;
            posEnter = (Vector2)transform.localPosition + hoverBehaviour_Move_Amount;
            SetHoverBehaviourType(hoverBehaviourType);

#if SOUND_MANAGER
            // Sound Manager
            internalOnPointerEnterFunc += () => { if (mouseOverSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseOverSound); };
            internalOnPointerClickFunc += () => { if (mouseClickSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseClickSound); };
#endif

#if CURSOR_MANAGER
            // Cursor Manager
            internalOnPointerEnterFunc += () => { if (cursorMouseOver != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOver); };
            internalOnPointerExitFunc += () => { if (cursorMouseOut != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOut); };
#endif
        }
        public void SetHoverBehaviourType(HoverBehaviour hoverBehaviourType) {
            this.hoverBehaviourType = hoverBehaviourType;
            switch (hoverBehaviourType) {
            case HoverBehaviour.Change_Color:
                hoverBehaviourFunc_Enter = delegate () { hoverBehaviour_Image.color = hoverBehaviour_Color_Enter; };
                hoverBehaviourFunc_Exit = delegate () { hoverBehaviour_Image.color = hoverBehaviour_Color_Exit; };
                break;
            case HoverBehaviour.Change_Image:
                hoverBehaviourFunc_Enter = delegate () { hoverBehaviour_Image.sprite = hoverBehaviour_Sprite_Enter; };
                hoverBehaviourFunc_Exit = delegate () { hoverBehaviour_Image.sprite = hoverBehaviour_Sprite_Exit; };
                break;
            case HoverBehaviour.Change_SetActive:
                hoverBehaviourFunc_Enter = delegate () { hoverBehaviour_Image.gameObject.SetActive(true); };
                hoverBehaviourFunc_Exit = delegate () { hoverBehaviour_Image.gameObject.SetActive(false); };
                break;
            }
        }
              
      public class InterceptActionHandler {

            private Action removeInterceptFunc;

            public InterceptActionHandler(Action removeInterceptFunc) {
                this.removeInterceptFunc = removeInterceptFunc;
            }
            public void RemoveIntercept() {
                removeInterceptFunc();
            }
        }
        public InterceptActionHandler InterceptActionClick(Func<bool> testPassthroughFunc) {
            return InterceptAction("ClickFunc", testPassthroughFunc);
        }
        public InterceptActionHandler InterceptAction(string fieldName, Func<bool> testPassthroughFunc) {
            return InterceptAction(GetType().GetField(fieldName), testPassthroughFunc);
        }
        public InterceptActionHandler InterceptAction(System.Reflection.FieldInfo fieldInfo, Func<bool> testPassthroughFunc) {
            Action backFunc = fieldInfo.GetValue(this) as Action;
            InterceptActionHandler interceptActionHandler = new InterceptActionHandler(() => fieldInfo.SetValue(this, backFunc));
            fieldInfo.SetValue(this, (Action)delegate () {
                if (testPassthroughFunc()) {
                    // Passthrough
                    interceptActionHandler.RemoveIntercept();
                    backFunc();
                }
            });

            return interceptActionHandler;
        }

        private static bool IsPointerOverUITouch()
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.GetTouch(0).position;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }
    }
}