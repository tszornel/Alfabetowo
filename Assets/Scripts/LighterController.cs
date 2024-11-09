using System;
using System.Collections;
using UnityEngine;
using Webaby.Utils;
public enum State
{
    None,
    Follow,
    Stay,
    ControlledByTouch
}
public class LighterController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform flashLight;
    [SerializeField]private Animator _animator;
    State state;
    public static event EventHandler<OnStateChangedEventArgs> LighterControl;
    private bool lighterVisible = false;
    public Transform groundDetection;
    public int flashTimeWhenTouchedOnFollow = 10;
    public GameObject lighterpanel;
    public bool ignoreCollisionsWithPlayer = false;
    private Collider2D lighterCollider2D;

    // public Cinemachine.CinemachineVirtualCameraBase vcam;




    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            SetLighterVisible();
        }


    }
    public void SetLighterVisible()
    {

        lighterVisible = true;

    }

    public void UnSetLighterVisible()
    {

        lighterVisible = false;
    }


    public bool GetLighterVisible()
    {

        return lighterVisible;
    }
    void OnBecameVisible()
    {
        GameLog.LogMessage("OnBecome Visible");
        SetLighterVisible();
    }

    void OnBecameInvisible()
    {
        GameLog.LogMessage("OnBecome InVisible");
        if(state != State.Follow)
            UnSetLighterVisible();

    }
    public class OnStateChangedEventArgs : EventArgs
    {
        public State currentState;
    }



    private void Awake()
    {
        if(!_animator)
            _animator = Transform.FindObjectOfType<Animator>();
        state = State.None;

        lighterCollider2D = GetComponent<Collider2D>();

        Button_Sprite button = transform.GetComponent<Button_Sprite>();
        button.ClickFunc = () => {
            GameLog.LogMessage("click Sprite Button !!!!", this);
            if (state != State.None)
                FlashLight();


        };

        if (button.doubleClick)
        {
            GameLog.LogMessage("Set Button Double click !!!!", this);
            button.DoubleClickFunc = () =>
            {
                GameLog.LogMessage("Double click Sprite Button !!!!"+" State = "+state,this);
                if (state != State.None)
                    GetComponent<Friend>().ShowShop();

            };
        }
    }


    public void FlashLight()
    {
        GameLog.LogMessage("Flash light enteresd",this);
        StartCoroutine(FlashLight(flashTimeWhenTouchedOnFollow));

    }


    
    
    
    IEnumerator FlashLight(int seconds)
    {

        flashLight.gameObject.SetActive(true);
       // GetComponent<Friend>()?.ShowQuestionMark();
        yield return new WaitForSeconds(seconds);
        flashLight.gameObject.SetActive(false);

    }

    public State GetState()
    {

        return state;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (ignoreCollisionsWithPlayer)
                Physics2D.IgnoreCollision(collision.collider, lighterCollider2D);



        }
    }

    private void switchLayerToLighter() {


        gameObject.layer = LayerMask.NameToLayer("Lighter");
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Lighter");
        }
        ignoreCollisionsWithPlayer = true;
    }
    public void ToggleLighter()
    {

        GameLog.LogMessage("Toggle Lighter entered " + state+" lighter Visible="+ GetLighterVisible());
        switch (state)
        {
            case State.None:
                state = State.ControlledByTouch;
                switchLayerToLighter();

                    SetLighterVisible();
                _animator.SetTrigger("Move");
                break;
            case State.Follow:
                if (GetLighterVisible())
                {
                    state = State.ControlledByTouch;
                    _animator.SetTrigger("Move");
                }
                break;
            case State.Stay:
                if (GetLighterVisible())
                {
                    state = State.Follow;
                    _animator.SetTrigger("Follow");
                }
                else
                {

                    GameLog.LogMessage("Lighter not visible");
                }
                break;
            case State.ControlledByTouch:

                state = State.Stay;
                _animator.SetTrigger("Stay");
                break;
        }

        //activate lighter panel
        if(!lighterpanel.activeSelf)
            lighterpanel.SetActive(true);
        LighterControl?.Invoke(this, new OnStateChangedEventArgs { currentState = state });

    }


}

