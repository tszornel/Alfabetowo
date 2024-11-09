using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class InteractableByTouch : MonoBehaviour
{
    public bool isInRange;
    // public KeyCode interactKey;
    public UnityEvent interactAction;

    //public GameObject interactableItem;
    private RaycastHit2D hit;
    public string interactableTag;
    private bool interact;
    private Light2D ladderLight;
    private float tweenTime;
    private bool lightOn = false;
    private float timeBeetwenLightTween = 0;
    private float touchPerSecFuncTimer = 0;
    Touch touch;

    void Start()
    {
        ladderLight = GetComponent<Light2D>();
        // interactableItem = transform.parent.GetComponent<Item>();
    }
    // Update is called once per frame


    private void Update()
    {

        if (touchPerSecFuncTimer > 0)
        {
            GameLog.LogMessage("touchPerSecFuncTimer=" + touchPerSecFuncTimer + " gameobjectName:" + gameObject.name);
            touchPerSecFuncTimer -= Time.unscaledDeltaTime;
        }

        if (interact)
        {
            interact = false;
            interactAction.Invoke();
        }
    }



    void ToggLadderLight()
    {


        if (Time.time >= timeBeetwenLightTween)
        {
            LeanTween.cancel(gameObject);

            tweenTime = 1f;
            if (!lightOn)
                LeanTween.value(gameObject, 0, 1, tweenTime).setEaseLinear().setOnUpdate(setLight);
            else
                LeanTween.value(gameObject, 1, 0, tweenTime).setEaseLinear().setOnUpdate(setLight);
            timeBeetwenLightTween = Time.time + tweenTime;
            lightOn = !lightOn;
        }



    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (isInRange)
        {
            ToggLadderLight();
            if (Input.touchCount > 0)
            {
                // for (int i = 0; i < Input.touchCount; i++)
                // {
                GameLog.LogMessage("Touch Count" + Input.touchCount);
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    GameLog.LogMessage("Touch Phase:" + Input.GetTouch(0).phase);
                    Vector2 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    hit = Physics2D.Raycast(point, Vector2.zero, 1);
                    if (hit.collider != null)
                    {
                        GameLog.LogMessage("Collider touch hit " + hit.collider.name + " " + hit.collider.gameObject.tag);
                        if (hit.collider.gameObject.tag == interactableTag)
                        {
                            GameLog.LogMessage("Click=" + touchPerSecFuncTimer);
                            if (touchPerSecFuncTimer > 0)
                            {
                                interact = true;
                                GameLog.LogMessage("double click=" + touchPerSecFuncTimer);
                                //touchPerSecFuncTimer -= Time.unscaledDeltaTime;
                                return;
                                // break;return
                            }
                            else
                            {
                                touchPerSecFuncTimer = 0.3f;

                            }
                        }
                    }

                    //  }
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameLog.LogMessage("Interactable object collision entered");
        if (collision.tag == "Player")
        {
            this.isInRange = true;
            // GameLog.LogMessage("colide with player"+transform.name);
        }
        ToggLadderLight();
    }

    private void setLight(float value)
    {
        ladderLight.intensity = value;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        LeanTween.value(gameObject, 1, 0, tweenTime).setEaseLinear().setOnUpdate(setLight);
        lightOn = false;
        GameLog.LogMessage("Interactoable object collision entered");
        if (collision.tag == "Player")
        {
            this.isInRange = false;
            // GameLog.LogMessage("colide with player"+transform.name);
        }
    }
}


