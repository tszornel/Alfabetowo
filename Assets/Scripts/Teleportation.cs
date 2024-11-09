using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal;
public class Teleportation : InteractableBase
{

    public GameObject portal;
    public GameObject frame1;
    public GameObject frame2;
    public AudioClip frame1AudioClip;
    public AudioMixerSnapshot frame1Snapshot;
    public AudioClip frame2AudioClip;
    public AudioMixerSnapshot frame2Snapshot;
    GameObject disabledFrame;


    protected override void InteractControllerAction()
    {
        interactActionEvent?.Invoke();
        RemoveInteractableItemCheck();
        Teleport();
    }

    protected override void DoWhenItemNotMatch()
    {
        player.GetComponent<UnitDialogBehaviour>().PlayComeBackLater();
    }
    public void Teleport()
    {
        
        if (frame1.activeSelf)
        {
            GameLog.LogMessage("Frame 1 was active");
            frame2.SetActive(true);
            Music.Instance.SwitchMusic(frame2AudioClip,frame2Snapshot,0.1f);
            //frame1.SetActive(false);
            disabledFrame = frame1;

           // player.transform.SetParent(frame2.transform, true);



        }
        else if (!frame1.activeSelf)
        {
            GameLog.LogMessage("Frame 1 wasn't active");
            frame1.SetActive(true);
            //frame2.SetActive(false);
            disabledFrame = frame2;
            //player.transform.SetParent(frame1.transform, true);

            Music.Instance.SwitchMusic(frame1AudioClip,frame1Snapshot,0.1f);


        }
        GameLog.LogMessage("Previous position" + player.transform.position);
        // player.transform.SetAsFirstSibling();
        player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);

        bool lighterFollows = player.GetComponent<Player>().IsLighterFollowing();

        GameLog.LogMessage("TEleportation with lighter" + lighterFollows);
        if (lighterFollows) 
        {

            GameLog.LogMessage("Change Lighter position");
            lighterTransform.position = new Vector2(portal.transform.position.x-2, portal.transform.position.y+2);
        }

        GameLog.LogMessage("Switch by portal to new place"+ player.transform.position);
        disabledFrame.SetActive(false);

        if (checkEquipmentTool && UnblockPortalWhenEquipmentToolUsed) {
            checkEquipmentTool = false;
           

         }


    }

    public void DisableTeleport() {

       // transform.gameObject.SetActive(false);
        showArrow = false;
        clickableSprite = false;
    }

    public void EnableTeleport()
    {
        transform.gameObject.SetActive(true);
        showArrow = true;
        clickableSprite = true;
        checkEquipmentTool = false;

    }
}


  
