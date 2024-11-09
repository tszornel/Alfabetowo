using Cinemachine;
using System;
using UnityEngine;
using Transform = UnityEngine.Transform;

public class MoveLighterBehaviour : StateMachineBehaviour
{

    // Start is called before the first frame update
    private GameObject flashLight;
    public Transform playerTransform;
    public float stopDistance;
    public float speed;
    private PlayerPlatformerController playerController;
    bool lighterMoving;
    Vector2 touchedPoint;
    private RaycastHit2D hit;
    bool moveRight = true;
    public Transform groundDetection;
    int touchCount = 0;
    public SpriteRenderer lighterCorpseRenderer;
    private bool touched_hidden_layer;
    private Player player;
    private CinemachineVirtualCameraBase vcam;
    LighterController lighterController;

    public float confinerBoundsWidth { get; private set; }
    public float confinerBoundsHeight { get; private set; }

    private void CheckDirection(Vector2 _touchedPoint, Transform lighter)
    {

        if (lighter.position.normalized.x < Vector2.MoveTowards(lighter.position, _touchedPoint, speed * Time.deltaTime).normalized.x)
        {
            //  GameLog.LogMessage("MOVERIGHT");
            moveRight = true;
        }
        else
        {

            //  GameLog.LogMessage("MOVELEFT");
            moveRight = false;
        }

        if (moveRight)
            lighter.eulerAngles = new Vector3(0, 0, lighter.eulerAngles.z);
        else
            lighter.eulerAngles = new Vector3(0, 180, lighter.eulerAngles.z);
    }



    private void CheckBordersConfinerBounds(CinemachineVirtualCameraBase vcam, Transform lighter)
    {
        // See if the camera has a confiner associated with it
        CinemachineConfiner2D confiner = vcam.GetComponent<CinemachineConfiner2D>();
        if (confiner == null)
        {
              return;
        }

        Collider2D confinerBounds = confiner.m_BoundingShape2D;
        if (!(confinerBounds is PolygonCollider2D))
        {
           
            return;
        }

        PolygonCollider2D polyCollider = confinerBounds as PolygonCollider2D;

        // Work out the total width and height of the confiner bounds
      //  confinerBoundsWidth = polyCollider.bounds.max.x - polyCollider.bounds.min.x;
      //  confinerBoundsHeight = polyCollider.bounds.max.y - polyCollider.bounds.min.y;
        Vector3 playerSize = lighterCorpseRenderer.bounds.size;

        lighter.position = new Vector3(
       Mathf.Clamp(lighter.position.x, polyCollider.bounds.min.x + playerSize.x / 2, polyCollider.bounds.max.x - playerSize.x / 2),
       Mathf.Clamp(lighter.position.y, polyCollider.bounds.max.y + playerSize.y / 2, polyCollider.bounds.min.y - playerSize.y / 2),
       lighter.position.z
       );

    }


    private void CheckBorders(Transform lighter)
    {
        var dist = (lighter.position - Camera.main.transform.position).z;

        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        Vector3 playerSize = lighterCorpseRenderer.bounds.size;

        lighter.position = new Vector3(
        Mathf.Clamp(lighter.position.x, leftBorder + playerSize.x / 2, rightBorder - playerSize.x / 2),
        Mathf.Clamp(lighter.position.y, topBorder + playerSize.y / 2, bottomBorder - playerSize.y / 2),
        lighter.position.z
        );

    }
    private HitState VerifyHit(Vector2 touchpoint)
    {
        touched_hidden_layer = false;
        hit = Physics2D.Raycast(touchedPoint, Vector2.zero, 10);

        if (hit.collider != null)
        {
            GameLog.LogMessage("Collider touch hit:" + hit.collider.name + " tag:" + hit.collider.gameObject.tag);
            //if (hit.collider.gameObject.tag == "player")
            if (hit.collider.gameObject.tag == "joystick" || hit.collider.gameObject.tag == "attackButton")
            {
                GameLog.LogMessage("clicked joystick");
                return HitState.Joystick;
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Hidden"))
            {

                touched_hidden_layer = true;
                return HitState.HiddenLayer;

            }
        }
        return HitState.None;
    }


    private bool UpdateLighterPosition(Transform lighter)
    {
        if (lighterController == null)
            lighterController = lighter.GetComponentInParent<LighterController>();

        if (lighterController && !lighterController.GetLighterVisible())
            return false;
            // GameLog.LogMessage("Update Lighter position:" + Input.touchCount);
        if (playerController.isPlayerMoving() || playerController.isPlayerAttacking())
        {
            GameLog.LogMessage("Player is moving or attacking");
            return false;
        }


        lighterMoving = true;

        touchCount++;
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                GameLog.LogMessage("Touch Count" + i + "Touch phase = " + Input.GetTouch(i).phase.ToString());
                if (Input.GetTouch(i).phase == TouchPhase.Began) //|| Input.GetTouch(i).phase == TouchPhase.Moved)
                {

                    GameLog.LogMessage("Touched done, update position");

                    touchedPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    // GameLog.LogMessage("Touched done,"+ transform.position+ "to  MoveToward"+ Input.GetTouch(i).position);
                    hit = Physics2D.Raycast(touchedPoint, Vector2.zero, 10);

                    CheckDirection(touchedPoint, lighter);
                    HitState hitState = VerifyHit(touchedPoint);
                    if (hitState == HitState.Joystick)
                        return false;



                    GameLog.LogMessage("Touched done," + lighter.position.x + "to  MoveToward x=" + touchedPoint.x + " + touchedPoint.y" + touchedPoint.y);




                }
                else if (Input.GetTouch(i).phase == TouchPhase.Ended)
                {
                    //
                    lighterMoving = false;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) && Input.mousePresent)
        {

            touchedPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckDirection(touchedPoint, lighter);
            HitState hitState = VerifyHit(touchedPoint);

            if (hitState == HitState.Joystick)
                return false;


        }
        return true;

    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!playerTransform)
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = playerTransform.GetComponent<PlayerPlatformerController>();

        if (!player)
            player = playerTransform.GetComponent<Player>();

        player.UnSetFireFlyFollow();
        if (!lighterCorpseRenderer)
            lighterCorpseRenderer = animator.transform.Find("corpse")?.GetComponent<SpriteRenderer>();

        if (!flashLight)
            flashLight = animator.transform.parent.GetComponent<LighterController>().flashLight.gameObject;

        flashLight.SetActive(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameLog.LogMessage("StateControlledByTouch entered:" + Input.touchCount);
        bool updatePosition = UpdateLighterPosition(animator.transform);
        GameLog.LogMessage("UpdateLighterPosition:" + updatePosition);
        if (lighterMoving)
        {
            Vector2 target;
            if (touchCount == 1)
            {
                //Go to the middle of the screen
                GameLog.LogMessage("GO TO THE MIDDLE");
                Vector3 middle = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
                touchedPoint = middle; // new Vector2(0, 0);
            }
            GameLog.LogMessage("Update position ?" + updatePosition);
            if (updatePosition)
            {


                if (!touched_hidden_layer)
                {
                    target = touchedPoint;

                }
                else
                {

                    target = new Vector2(touchedPoint.x, touchedPoint.y + 0.9f);

                }


                animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, speed * Time.deltaTime);

                //vcam = animator.transform.parent.GetComponent<LighterController>().vcam;
                // CheckBorders(animator.transform);
                //CheckBorders(animator.transform);

                CheckBordersConfinerBounds((CinemachineVirtualCameraBase)CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera, animator.transform);

            }
            else
            {
                GameLog.LogMessage("Do not move" + updatePosition);
                target = animator.transform.position;
            }

        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
