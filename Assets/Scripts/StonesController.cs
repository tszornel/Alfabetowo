using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonesController : InteractableBase
{
    // Start is called before the first frame update
    [Header("Dig StonesCut Scene")]
    public CutsceneMode DigStonesCutsceneMode;
    public CutsceneTimelineBehaviour DigStonesCutsceneBehaviour;
    private bool blockade = false;

    protected override void InteractControllerAction()
    {
        DigStones();


    }
    protected override void OnInteractableStart() {
        DigStonesCutsceneMode = CutsceneMode.Play;
        checkEquipmentTool = true;


    }

    private void StopCollisions()
    {
        GameLog.LogMessage("StopCollisions entered");
        player.disableCollisions = true;
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

    }

    public void StopDigStones()
    {
        GameLog.LogMessage("StopDigStones entered");
        SteeringPanel.Instance.ShowSteeringPanel(0.5f);
        blockade = false;
    
        // playerAnimator.SetLayerWeight(layerIndex, 0);
    }

    public void DigStones()
    {
        GameLog.LogMessage("SwiDigStones entered");
        if (!blockade)
        {
            switch (DigStonesCutsceneMode)
            {
                case CutsceneMode.Play:
                    StartDigStonesCutScene();
                    showArrow = false;
                    if (disableArrowDelegate != null)
                        disableArrowDelegate.Invoke();
                    break;

                case CutsceneMode.None:
                    break;
            }
        }
    }



    private void StartDigStonesCutScene()
    {
        DigStonesCutsceneMode = CutsceneMode.None;
        GameLog.LogMessage("StartDigStonesCutScene entered:" + Time.time);
        blockade = true;
               //disable controlss
        SteeringPanel.Instance.HideSteeringPanel();
      
        DigStonesCutsceneBehaviour.StartTimeline();

    }
}
