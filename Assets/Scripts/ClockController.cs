using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;
//using UnityEditor.U2D.SpriteShape;
using System;

public class ClockController : InteractableBase
{
    [SerializeField] public float _speed = 4.0f;
    private LTDescr _tween;
    [SerializeField] private GameObject pathNearClock;
    public static ClockUI clock;
    [SerializeField] private CutsceneTimelineBehaviour clockTimeline;
    [SerializeField] private GameObject clockKey;
    //this line added by MM to get access to ClockKey in tests 
    public GameObject ClockKey { get { return clockKey; } set { clockKey = value; } }
    // [SerializeField] private SpriteShapeController pathToClock;


    public void ActivateClockKey() 
    { 

        clockKey.SetActive(true);   
    
    }

    public void DeactivateClockKey()
    {
        clockKey.SetActive(false);
        SteeringPanel.Instance.ShowSteeringPanel(1);
        player.UnBlockMovement();
        RemoveClockKeyOnPlayer();

    }


    public void PlayClockTimeLine()
    {
        SteeringPanel.Instance.HideSteeringPanel();
        player.BlockMovement(clockTimeline);
        // ActivateClockKey();
        player.TurnRight();
                
        
        clockTimeline?.StartTimeline();

    }

    protected override void InteractControllerAction()
    {

        //  WalkToClock();

        PlayClockTimeLine();


        ResetClockTime();


    }

    protected override void OnInteractableStart()
    {

       // _movePath = GetComponent<MoveAlongPath>();
        //MoveAlongPath.FinishedPath += OnFinishedPath;
        base.OnInteractableStart();
        if(!clock)
            clock = GetComponent<ClockUI>();
    }

    private void OnFinishedPath(object sender, EventArgs e)
    {
        pathNearClock.SetActive(true);
        SetRunLayer(0);

    }

    private void SetRunLayer(int value) { 
    
        Animator animator = player.GetComponent<Animator>();
        int index  = animator.GetLayerIndex("HeroRun");
        animator.SetLayerWeight(index, value);

    }

    public void RemoveClockKeyOnPlayer()
    {
        player.GetComponent<CharacterEquipment>().removeWeaponItem();
    }

    public void ResetClockTime()
    {

        //player.SetInteract(false);

        // interactAction();
        clock.ResetClockTime();
       // interactActionEvent?.Invoke();
       

        //Play Jupi timeline
        // player.PlayJupiTimeLine();  

    }


    private void WalkToClock() {

        SetRunLayer(1);
       // _movePath.WalkToClock();

       // Vector3[] _positions = setBezierPath();

      /*  _tween = LeanTween.move(player.gameObject, _positions,1f)
           // .setOrientToPath2d(true)
           //.setEase(LeanTweenType.easeInOutQuad)
           .setSpeed(_speed)
           .setPassed(Time.time).setLoopOnce()
          .setDelay(0);
      */

    }
    /*
    private Vector3[] setBezierPath() 
    {

        GameLog.LogMessage("Walk to Clock entered");
        Vector3 clockPosition = transform.position;
        GameObject playerTransform = player.gameObject;
        Spline spline = pathToClock.spline;
        int bezierCount = spline.GetPointCount() - 1;
        GameLog.LogMessage("POSITIONS:" + bezierCount);
        Vector3[]  _positions = new Vector3[bezierCount * 4];
        for (int i = 0; i < bezierCount; ++i)
        {
            Vector2 startPoint;
            Vector2 startControl;
            Vector2 endPoint;
            Vector2 endControl;

           /* if (i == 0)
            {
                startPoint = (Vector2)player.transform.position;
                startControl = startPoint + Vector2.right;
                endPoint = (Vector2)(pathToClock.transform.position + pathToClock.spline.GetPosition(i));
                endControl = endPoint + (Vector2)pathToClock.spline.GetLeftTangent(i + 1);


            }*/
           // else
        /*    {
                startPoint = (Vector2)(pathToClock.transform.position+ pathToClock.spline.GetPosition(i));
                startControl = startPoint + (Vector2)pathToClock.spline.GetRightTangent(i);
                endPoint = (Vector2)(pathToClock.transform.position + pathToClock.spline.GetPosition(i + 1));
                endControl = endPoint + (Vector2)pathToClock.spline.GetLeftTangent(i + 1);
            }
            // Get points in local space
           

            // Note: the control for the end and start are reversed! This is just a quirk of the LeanTween API.
            _positions[4 * i + 0] = startPoint;
            _positions[4 * i + 1] = endControl;
            _positions[4 * i + 2] = startControl;
            _positions[4 * i + 3] = endPoint;
            GameLog.LogMessage(" HERO POSITION :" + i + " " + player.transform.position);
            GameLog.LogMessage(pathToClock.spline.GetPosition(i)+" POSITION :" +i+" "+  startPoint);
            GameLog.LogMessage((Vector2)pathToClock.spline.GetRightTangent(i)+" POSITION :" + i + " " + startControl);
            GameLog.LogMessage((Vector2)pathToClock.spline.GetLeftTangent(i + 1)+" POSITION :" + i + " " + endControl);
            GameLog.LogMessage(pathToClock.spline.GetPosition(i + 1)+" POSITION :" + i + " " + endPoint);
          
          
        }
        return _positions;
    } */
}
