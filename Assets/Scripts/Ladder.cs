using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Ladder : MonoBehaviour
{
    private GameObject player;
    private RaycastHit2D hit;
    [SerializeField] private Transform lightTransform;
    private Light2D ladderLight;
    Coroutine walkLadderRoutine;


    [Header("Walk Ladder Cut Scene")]
    public CutsceneMode walkLadderCutscene;
    public CutsceneTimelineBehaviour walkLadderCutsceneBehaviour;



    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        lightTransform = transform.Find("lightPortal");

        if (lightTransform)
            ladderLight = lightTransform.GetComponent<Light2D>();
    }


    private void StartWalkLadderCutScene()
    {

        walkLadderCutsceneBehaviour.StartTimeline();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (lightTransform)
            {
                lightTransform.gameObject.SetActive(true);
                ladderLight.intensity = 1;
            }
            walkLadderRoutine = StartCoroutine(WalkLadder());
        }
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        //  GameLog.LogMessage("Trigger Stay");
        if (collision.gameObject.tag == "Player")
        {
            if(walkLadderRoutine==null)
                walkLadderRoutine=  StartCoroutine(WalkLadder());
        }
        //anim.SetTrigger("leftArea");
    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        // GameLog.LogMessage("Left Area");
        if (walkLadderRoutine != null)
            StopCoroutine(walkLadderRoutine);
        if (lightTransform)
        {
            lightTransform.gameObject.SetActive(false);
            ladderLight.intensity = 0;
        }

        // anim.SetTrigger("leftArea");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator WalkLadder()
    {
        yield return new WaitForSeconds(2);
        if (player.GetComponent<PlayerPlatformerController>().GetCrouching())
        {
            GameLog.LogMessage("Walk Ladder !!!");
            /* if (switchSounds) {
                 GameLog.LogMessage("switch sounds teleport entered !!!");
                 switchSounds.PlaySwitchFrameSound();
                 yield return new WaitForSeconds(0.5f);
             }*/
            GameLog.LogMessage("Walk ladder via crouch !!!!!");
            switch (walkLadderCutscene)
            {
                case CutsceneMode.Play:
                    StartWalkLadderCutScene();
                    break;
                case CutsceneMode.None:
                    break;
            }

            yield return null;
        }
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                //GameLog.LogMessage("Touch Count" + i);
                if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetTouch(i).phase == TouchPhase.Moved)
                {
                    //GameLog.LogMessage("Touch Phase:" + Input.GetTouch(i).phase);
                    Vector2 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
                    hit = Physics2D.Raycast(point, Vector2.zero, 10);
                    if (hit.collider != null)
                    {
                        //GameLog.LogMessage("Collider touch hit " + hit.collider.name + " " + hit.collider.gameObject.tag);
                        if (hit.collider.gameObject.tag == "Ladder")
                        {

                            switch (walkLadderCutscene)
                            {
                                case CutsceneMode.Play:
                                    StartWalkLadderCutScene();
                                    break;
                                case CutsceneMode.None:
                                    break;
                            }
                            yield return null;
                        }





                    }

                }
            }
        }
    }
}
