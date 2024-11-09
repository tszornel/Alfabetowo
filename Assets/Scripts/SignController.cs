using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.U2D.Animation;
using Webaby.Utils;

public class SignController : MonoBehaviour
{
    private DialogLanguage lang;
    private SpriteResolver text_polishResolver;
    public Transform text_polish;

    const string categorySign = "Sign";
    const string labelTextPolish = "text_polish";
    const string labelTextEnglish = "text_english";
    const string labelTextDeutsch = "text_deutsch";

    public Renderer objectToChangeLayer;
    public GameObject additionalLigt;


    void Awake()
    {
        ChangeSortingLayer("Foreground");
        lang = GameHandler.Instance.language;
        GameLog.LogMessage("lang is " + lang);
        
        text_polishResolver = text_polish.GetComponent<SpriteResolver>();

        GameLog.LogMessage("Setted category:" + text_polishResolver.GetCategory());

        switch (lang){
            case DialogLanguage.Polski:
                   SetupLanguage(labelTextPolish);
                   break;
            case DialogLanguage.Angielski:
                SetupLanguage(labelTextEnglish);
                
                    break;
            case DialogLanguage.Niemiecki:
                SetupLanguage(labelTextDeutsch);
                    break;
            default:
                SetupLanguage(labelTextPolish);
                break;
        }
        



    }
    private void SetupLanguage(string label) 
    {
        Player.SetClotherWithID(text_polishResolver, categorySign, label);
    }



    public void UpdateLayerWhenBoatMoving() {
        additionalLigt?.SetActive(true);     
        ChangeSortingLayer("Background");
    }

    public void FinishUpdateLayerWhenBoatMoving()
    {
        ChangeSortingLayer("Foreground");
        additionalLigt?.SetActive(false);
    }

    private void ChangeSortingLayer(string layerName)
    {
        if (objectToChangeLayer != null)
        {
            objectToChangeLayer.sortingLayerName = layerName;
        }
        else
        {
            Debug.LogWarning("Object to change layer is not assigned.");
        }
    }

 
    
    

 

 

}
