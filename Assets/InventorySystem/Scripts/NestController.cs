using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NestController : InteractableBase
{
    // Start is called before the first frame update
    [Header("Worms Cut Scene")]
    public CutsceneMode WormsCutsceneMode;
    public CutsceneTimelineBehaviour WormsCutsceneBehaviour;
    private bool blockade = false;
    Action action;
    protected override void InteractControllerAction()
    {
        action();
    }

    protected override void OnInteractableStart()
    {
        WormsCutsceneMode = CutsceneMode.Play;
        action = WormsAction;
        checkEquipmentTool = true;
    }

    private void StopCollisions()
    {
        GameLog.LogMessage("StopCollisions entered");
        player.disableCollisions = true;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    public void StopFeatherAction()
    {
        GameLog.LogMessage("StopDigStones entered");
        SteeringPanel.Instance.ShowSteeringPanel(0.5f);
        blockade = false;
        showArrow = false;

        // playerAnimator.SetLayerWeight(layerIndex, 0);
    }

    public void RemoveFeatherOnPlayer()
    {
        player.GetComponent<CharacterEquipment>().removeWeaponItem();
    }
    public void WormsAction()
    {
        GameLog.LogMessage("FeatherAction entered");
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
        WormsCutsceneBehaviour.StartTimeline();

    }
}
