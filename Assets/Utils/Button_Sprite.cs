/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
//#define SOUND_MANAGER // Has Sound_Manager in project
//#define CURSOR_MANAGER // Has Cursor_Manager in project

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Webaby.Utils {

    /*
     * Button Actions on a World BoxCollider
     * */
    public class Button_Sprite : MonoBehaviour,IPointerClickHandler{

        private static Func<Camera> GetWorldCamera;

        public static void SetGetWorldCamera(Func<Camera> GetWorldCamera) {
            Button_Sprite.GetWorldCamera = GetWorldCamera;
        }

        private RaycastHit2D hit;
        public Action ClickFunc = null;
        public Action DoubleClickFunc = null;
        public Action MouseRightDownOnceFunc = null;
        public Action MouseRightDownFunc = null;
        public Action MouseRightUpFunc = null;
        public Action MouseDownOnceFunc = null;
        public Action MouseUpOnceFunc = null;
        public Action MouseOverOnceFunc = null;
        public Action MouseOutOnceFunc = null;
        public Action MouseOverOnceTooltipFunc = null;
        public Action MouseOutOnceTooltipFunc = null;
        private bool draggingMouseRight;
        private Vector3 mouseRightDragStart;
        public Action<Vector3, Vector3> MouseRightDragFunc = null;
        public Action<Vector3, Vector3> MouseRightDragUpdateFunc = null;
        public bool triggerMouseRightDragOnEnter = false;
        public bool doubleClick;

        public enum HoverBehaviour {
            Custom,
            Change_Color,
            Change_Image,
            Change_SetActive,
        }
        public HoverBehaviour hoverBehaviourType = HoverBehaviour.Custom;
        private Action hoverBehaviourFunc_Enter, hoverBehaviourFunc_Exit;
        public Color hoverBehaviour_Color_Enter = new Color(1, 1, 1, 1), hoverBehaviour_Color_Exit = new Color(1, 1, 1, 1);
        public SpriteRenderer hoverBehaviour_Image;
        public Sprite hoverBehaviour_Sprite_Exit, hoverBehaviour_Sprite_Enter;
        public bool hoverBehaviour_Move = false;
        public Vector2 hoverBehaviour_Move_Amount = Vector2.zero;
        private Vector3 posExit, posEnter;
        public bool triggerMouseOutFuncOnClick = false;
        public bool clickThroughUI = false;
        private float touchPerSecFuncTimer = 0;
        private Action internalOnMouseDownFunc, internalOnMouseEnterFunc, internalOnMouseExitFunc;

#if SOUND_MANAGER
        public Sound_Manager.Sound mouseOverSound, mouseClickSound;
#endif
#if CURSOR_MANAGER
        public CursorManager.CursorType cursorMouseOver, cursorMouseOut;
#endif




        public void SetHoverBehaviourChangeColor(Color colorOver, Color colorOut) {
            hoverBehaviourType = HoverBehaviour.Change_Color;
            hoverBehaviour_Color_Enter = colorOver;
            hoverBehaviour_Color_Exit = colorOut;
            if (hoverBehaviour_Image == null) hoverBehaviour_Image = transform.GetComponent<SpriteRenderer>();
            hoverBehaviour_Image.color = hoverBehaviour_Color_Exit;
            SetupHoverBehaviour();
        }
        void OnMouseDown() {
          //  GameLog.LogMessage("Mouse down sprite button:"+ !clickThroughUI +"  "+ IsPointerOverUI());
            if (!clickThroughUI && IsPointerOverUI()) {
                GameLog.LogMessage("Over UI");

                return; // Over UI!
            }
            
           // GameLog.LogMessage("internal mouse down entered");
            if (internalOnMouseDownFunc != null) internalOnMouseDownFunc();
            // GameLog.LogMessage("ClickFunc mouse down entered");


            if (doubleClick)
            {
                if (touchPerSecFuncTimer > 0)
                {
                    GameLog.LogMessage("double click=" + touchPerSecFuncTimer);
                    if (DoubleClickFunc != null) DoubleClickFunc();

                    // break;return
                }
                else
                {
                    //single click
                    GameLog.LogMessage("single  click start counting");

                    touchPerSecFuncTimer = 0.8f;
                    if (ClickFunc != null) ClickFunc();

                }
            }
            else
            {
                if (ClickFunc != null) ClickFunc();
            }



            /*if (ClickFunc != null)
            {
               // GameLog.LogMessage("click func != null");




                ClickFunc();
            }*/

                
            if (triggerMouseOutFuncOnClick) OnMouseExit();
        }
        public void Manual_OnMouseExit() {
            OnMouseExit();
        }
        void OnMouseUp() {
            if (MouseUpOnceFunc != null) MouseUpOnceFunc();
        }
        void OnMouseEnter() {
            if (!clickThroughUI && IsPointerOverUI()) return; // Over UI!
            
            if (internalOnMouseEnterFunc != null) internalOnMouseEnterFunc();
            if (hoverBehaviour_Move) transform.localPosition = posEnter;
            if (hoverBehaviourFunc_Enter != null) hoverBehaviourFunc_Enter();
            if (MouseOverOnceFunc != null) MouseOverOnceFunc();
            if (MouseOverOnceTooltipFunc != null) MouseOverOnceTooltipFunc();
        }
        void OnMouseExit() {
            if (internalOnMouseExitFunc != null) internalOnMouseExitFunc();
            if (hoverBehaviour_Move) transform.localPosition = posExit;
            if (hoverBehaviourFunc_Exit != null) hoverBehaviourFunc_Exit();
            if (MouseOutOnceFunc != null) MouseOutOnceFunc();
            if (MouseOutOnceTooltipFunc != null) MouseOutOnceTooltipFunc();
        }

        void OnMouseOver() {
            if (!clickThroughUI && IsPointerOverUI()) return; // Over UI!

            if (Input.GetMouseButton(1)) {
                if (MouseRightDownFunc != null) MouseRightDownFunc();
                if (!draggingMouseRight && triggerMouseRightDragOnEnter) {
                    draggingMouseRight = true;
                    mouseRightDragStart = GetWorldPositionFromUI();
                }
            }
            if (Input.GetMouseButtonDown(1)) {
                draggingMouseRight = true;
                mouseRightDragStart = GetWorldPositionFromUI();
                if (MouseRightDownOnceFunc != null) MouseRightDownOnceFunc();
            }
        }
        void Update() {
            if(doubleClick)
                if (touchPerSecFuncTimer > 0)
                {
                    GameLog.LogMessage("touchPerSecFuncTimer=" + touchPerSecFuncTimer + " gameobjectName:" + gameObject.name);
                    touchPerSecFuncTimer -= Time.unscaledDeltaTime;
                }
            // GameLog.LogMessage("Update in sprite button");
            /* if (Input.touchCount > 0)
             {
                 for (int i = 0; i < Input.touchCount; i++)
                 {
                     //GameLog.LogMessage("Touch Count" + i);
                     if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
                     {
                         // GameLog.LogMessage("Touch Phase:" + Input.GetTouch(i).phase);
                         //   Vector2 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                         Vector2 point = GetWorldPositionFromUITouch(Input.GetTouch(i).position);
                         hit = Physics2D.Raycast(point, Vector2.zero, 10);

                         if (hit.collider != null)
                         {
                             //GameLog.LogMessage("Collider touch hit " + hit.collider.name + " " + hit.collider.gameObject.tag);
                             //if (hit.collider.gameObject.tag == "player")
                             if (hit.collider.gameObject.tag == gameObject.tag)
                             {

                                 if (ClickFunc != null) ClickFunc();


                             }
                         }
                     }
                 }
             }*/



            if (draggingMouseRight) {
                if (MouseRightDragUpdateFunc != null) MouseRightDragUpdateFunc(mouseRightDragStart, GetWorldPositionFromUI());
            }
            if (Input.GetMouseButtonUp(1)) {
                if (draggingMouseRight) {
                    draggingMouseRight = false;
                    if (MouseRightDragFunc != null) MouseRightDragFunc(mouseRightDragStart, GetWorldPositionFromUI());
                }
                if (MouseRightUpFunc != null) MouseRightUpFunc();
            }
        }


        void Awake() {
            if (GetWorldCamera == null) SetGetWorldCamera(() => Camera.main); // Set default World Camera
            posExit = transform.localPosition;
            posEnter = transform.localPosition + (Vector3)hoverBehaviour_Move_Amount;
            SetupHoverBehaviour();

#if SOUND_MANAGER
            // Sound Manager
            internalOnMouseDownFunc += () => { if (mouseClickSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseClickSound); };
            internalOnMouseEnterFunc += () => { if (mouseOverSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseOverSound); };
#endif

#if CURSOR_MANAGER
            // Cursor Manager
            internalOnMouseExitFunc += () => { if (cursorMouseOut != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOut); };
            internalOnMouseEnterFunc += () => { if (cursorMouseOver != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOver); };
#endif
        }
        private void SetupHoverBehaviour() {
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

        private static Vector3 GetWorldPositionFromUI() {
            Vector3 worldPosition = GetWorldCamera().ScreenToWorldPoint(Input.GetTouch(0).position);
            return worldPosition;
        }


        private static Vector3 GetWorldPositionFromUITouch(Vector3 position)
        {
            Vector3 worldPosition = GetWorldCamera().ScreenToWorldPoint(position);
            return worldPosition;
        }

        private static bool IsPointerOverUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return true;
            } else {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!clickThroughUI && IsPointerOverUI())
            {
                GameLog.LogMessage("Over UI");

                return; // Over UI!
            }
            //   Vector2 point = GetWorldPositionFromUITouch(Input.GetTouch(0).position);

            GameLog.LogMessage(name + "Clicked !!!!!");
            hit = Physics2D.Raycast(eventData.pointerPressRaycast.worldPosition, Vector2.zero, 10);

           // eventData.pointerPress

            if (hit.collider != null)
            {
                GameLog.LogMessage("Collider touch hit " + hit.collider.name + " " + hit.collider.gameObject.tag);
                //if (hit.collider.gameObject.tag == "player")
                if (hit.collider.gameObject.tag == gameObject.tag)
                {
                    if (doubleClick)
                    {
                        if (touchPerSecFuncTimer > 0)
                        {
                            GameLog.LogMessage("double click=" + touchPerSecFuncTimer);
                            if (DoubleClickFunc != null) DoubleClickFunc();
                         
                            // break;return
                        }
                        else
                        {
                            //single click
                            GameLog.LogMessage("single  click start counting");

                           touchPerSecFuncTimer = 0.8f;
                            if (ClickFunc != null) ClickFunc();
                           
                        }
                    }
                    else 
                    {
                        if (ClickFunc != null) ClickFunc();
                    }
  
                }
            }
        }
    }

}