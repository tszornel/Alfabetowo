using UnityEngine;

public class BoatController : InteractableBase
{
    // Start is called before the first frame update
    [Header("Boat swim  Cut Scene")]
    public CutsceneMode swimBoatCutsceneMode;
    public CutsceneTimelineBehaviour boatSwimCutsceneBehaviour;
    private bool blockade = false;
    private tweenUI tween;
  



    protected override void InteractControllerAction()
    {
        SwimBoat();
    }

    protected override void AwakeControllerAction()
    {
        tween = GameObject.FindObjectOfType<tweenUI>(true);
    }

    public void SwimBoatEvents()
    {
        GameLog.LogMessage("SwimBoatEvents entered");
       
        //ignore collisions

        // layerIndex = playerAnimator.GetLayerIndex("WalkLadder");
        // playerAnimator.SetLayerWeight(layerIndex, 1);
    }

    private void StopCollisions()
    {
        GameLog.LogMessage("StopCollisions entered");
        player.disableCollisions = true;
        player.TurnRight();
        player.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

       

    }

    public void StopSwimBoat()
    {
        GameLog.LogMessage("StopSwimBoat entered");
        player.disableCollisions = false;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
        gameObject.layer = LayerMask.NameToLayer("Default");
        // SteeringPanel.Instance.ShowSteeringPanel(0.5f);
        tween.ShowSuccesPanel();
        // playerAnimator.SetLayerWeight(layerIndex, 0);
    }

    public void SwimBoat()
    {
        GameLog.LogMessage("SwimBoat entered");
        if (!blockade)
        {
            switch (swimBoatCutsceneMode)
            {
                case CutsceneMode.Play:
                    StartSwimBoatCutScene();
                    break;
               
                case CutsceneMode.None:
                    break;
            }
        }
    }

    


    private void StartSwimBoatCutScene()
    {
        GameLog.LogMessage("StartWalkLadderCutScene entered:" + Time.time);
        blockade = true;
        StopCollisions();
        //disable controlss
        SteeringPanel.Instance.HideSteeringPanel();
        boatSwimCutsceneBehaviour.StartTimeline();

    }

   


}
