using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using Webaby.Utils;

public class LadderController : InteractableBase
{
    // Start is called before the first frame update
    [Header("Walk Ladder Cut Scene")]
    public CutsceneMode walkLadderCutscene;
    public CutsceneTimelineBehaviour walkLadderCutsceneBehaviour;
    public CutsceneTimelineBehaviour walkLadderDownCutsceneBehaviour;
    private bool onBalustrade = false;
    private bool blockade = false;



    protected override void InteractControllerAction() {

        WalkLadder();


    }

    public void WalkLadderHeroLayerEnable()
    {
        // layerIndex = playerAnimator.GetLayerIndex("WalkLadder");
        // playerAnimator.SetLayerWeight(layerIndex, 1);
    }

    public void StopWalkLadder()
    {
        // playerAnimator.SetLayerWeight(layerIndex, 0);
    }

    public void WalkLadder()
    {
        if (!blockade)
        {
            switch (walkLadderCutscene)
            {
                case CutsceneMode.Play:

                    if (!onBalustrade)
                        StartWalkLadderCutScene();
                    else
                        StartWalkLadderDownCutScene();
                    break;
                case CutsceneMode.Down:
                    break;
                case CutsceneMode.None:
                    break;
            }
        }
    }

    public void TimeLineUpLadderFinished()
    {
        onBalustrade = true;
        blockade = false;

    }

    public void TimeLineDownLadderFinished()
    {
        onBalustrade = false;
        blockade = false;

    }


    private void StartWalkLadderCutScene()
    {
        GameLog.LogMessage("StartWalkLadderCutScene entered:" + Time.time);
        blockade = true;
        player.TurnRight();
        walkLadderCutsceneBehaviour.StartTimeline();

    }

    private void StartWalkLadderDownCutScene()
    {
        blockade = true;
        player.TurnRight();
        GameLog.LogMessage("StartWalkLadderDownCutScene entered:" + Time.time);
        walkLadderDownCutsceneBehaviour.StartTimeline();

    }

       

}
