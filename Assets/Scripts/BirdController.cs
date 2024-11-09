using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : InteractableBase
{
    // Start is called before the first frame update
    [Header("Feather Cut Scene")]
    public CutsceneMode FeatherCutsceneMode;
    [Header("Worms Cut Scene")]
    public CutsceneMode WormsCutsceneMode;
    public CutsceneTimelineBehaviour FeatherCutsceneBehaviour;
    public CutsceneTimelineBehaviour WormsCutsceneBehaviour;
    private static bool blockade = false;
    Action action;

    public object Gamelog { get; private set; }

    protected override void InteractControllerAction()
    {
        action();
    }

    protected override void SetupTimelineAction(string taskName) {


        GameLog.LogMessage("SetupTimelineAction ");
        switch (taskName)
        {
            
            case "Feather":
                action = FeatherAction;
                break;
            case "Worms":
                action = WormsAction;
                break;
            default:
                   break;

        }

    }

   

    private void WormsCutScene()
    {
        WormsCutsceneMode = CutsceneMode.None;
        GameLog.LogMessage("StartWormsCutScene entered:" + Time.time);
        blockade = true;
        //disable controls
        SteeringPanel.Instance.HideSteeringPanel();
        if (disableArrowDelegate != null)
            disableArrowDelegate.Invoke();
        WormsCutsceneBehaviour?.StartTimeline();

    }

    protected override void OnInteractableStart()
    {
        FeatherCutsceneMode = CutsceneMode.Play;
        action = FeatherAction;
        checkEquipmentTool = true;

    }

    private void StopCollisions()
    {
        GameLog.LogMessage("StopCollisions entered");
        player.disableCollisions = true;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

    }

    public void StopAction()
    {
        StopAllCoroutines();    
        GameLog.LogMessage("StopFeatherAction entered");
        blockade = false;
        showArrow = false;
        //SetupInteractableObject(null, null);
        SetCheckEquipmentTool();
        SteeringPanel.Instance.ShowSteeringPanel(0.5f);
        GameHandler.Instance.DisableArrow();


        // playerAnimator.SetLayerWeight(layerIndex, 0);
    }

    public void RemoveFeatherOnPlayer()
    {
        player.GetComponent<CharacterEquipment>().removeWeaponItem();
    }
    


    public void FeatherAction()
    {
        GameLog.LogMessage("FeatherAction entered");
        if (!blockade)
        {
            switch (FeatherCutsceneMode)
            {
                case CutsceneMode.Play:
                    FeatherCutScene();
                    break;

                case CutsceneMode.None:
                    break;
            }
        }
    }

    public void WormsAction()
    {
        GameLog.LogMessage("WormsAction entered");
        if (!blockade)
        {
            switch (WormsCutsceneMode)
            {
                case CutsceneMode.Play:
                    WormsCutScene();
                    break;

                case CutsceneMode.None:
                    break;
            }
        }
        else 
        {
            GameLog.LogMessage("Blockade");
        
        }
    }



    private void FeatherCutScene()
    {
        FeatherCutsceneMode = CutsceneMode.None;
        GameLog.LogMessage("StartFeatherCutScene entered:" + Time.time);
        blockade = true;
        //disable controls
        SteeringPanel.Instance.HideSteeringPanel();
        if (disableArrowDelegate != null)
            disableArrowDelegate.Invoke();
        FeatherCutsceneBehaviour?.StartTimeline();

    }
}
