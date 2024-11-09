using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class WindowCharacter_Portrait : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform followTransform;
    public Transform closeBtn;
    public InventoryDisplay inventory_ui;
    private static bool doNotHide = false;



    public static void SetDoNotHide() 
    {
        doNotHide = true;


    }

    public static void UnSetDoNotHide()
    {
        doNotHide = false;


    }



   /* private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }*/


  //  public static WindowCharacter_Portrait Instance { get; private set; }
    private void Start()
    {
        
        cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, cameraTransform.position.z);
    }
    private void FixedUpdate()
    {
        if (!followTransform)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                followTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
       // GameLog.LogMessage("Player: " + followTransform.ToString() + " camera:" + cameraTransform.ToString());
        if (cameraTransform && followTransform)
        cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, cameraTransform.position.z);
    }
    private void LateUpdate()
    {
        if (!followTransform)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                followTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if(cameraTransform && followTransform)
         cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, cameraTransform.position.z);
    }


    public void ToggleInventory() {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
        if (!inventory_ui.gameObject.activeSelf)
            inventory_ui.gameObject.SetActive(true);
        else
            inventory_ui.gameObject.SetActive(false);

    }
    public void Show() {
        GameLog.LogMessage("Show Inventory");
        if (!gameObject.activeSelf) {
            GameLog.LogMessage("Camera activate");
            gameObject.SetActive(true);
           
        }
            
        if (!inventory_ui.gameObject.activeSelf) {
            GameLog.LogMessage("Inventory activate");
            inventory_ui.gameObject.SetActive(true);

        }
           
    }
    private void Hide() {
        GameLog.LogMessage("Hide clicked");
        gameObject.SetActive(false);
        inventory_ui.gameObject.SetActive(false);
    }
    public void Hide(int seconds)
    {

        if(gameObject.activeSelf)
            StartCoroutine(HideCourotine(seconds));
    }
    private IEnumerator HideCourotine(int seconds)
    {
        GameLog.LogMessage("Hide clicked"+ seconds+" do not hide:"+ doNotHide);

        if (doNotHide)
        {
            GameLog.LogMessage("Przerwij ukrywanie");
            yield break;
        }
        yield return new WaitForSeconds(seconds);

        
        Hide();
        
            
         
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();    
    }
}
