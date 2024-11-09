using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Webaby.Utils;

public class MirrorController : InteractableBase
{
    [Header("Walk Ladder Cut Scene")]
    public CutsceneMode jumpIntoMirrorMode;
    public CutsceneTimelineBehaviour jumpIntoMirrorCutsceneBehaviour;
      
    private void JumpInventoryCutScene()
    {
        jumpIntoMirrorCutsceneBehaviour.StartTimeline();
    }

    protected override void InteractControllerAction()
    {
        JumpInventoryCutScene();
    }

    void JumpIntoMirrorStartEvent()
    {


    }
    void JumpIntoMirrorFinishEvent()
    {


    }


   
}
