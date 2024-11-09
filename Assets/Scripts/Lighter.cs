using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
public class Lighter : MonoBehaviour
{
    [Header("Lighter Cutscenes")]
   // public CutsceneMode introFrame1Cutscene;
    public CutsceneTimelineBehaviour lighterGigleline;
    //public CutsceneTimelineBehaviour lighterIdleTImeline;
    // Start is called before the first frame update
    public AudioClip lighterFx;
    void Start()
    {
        transform.GetComponent<Button_Sprite>().ClickFunc = () => {
           
            lighterGigleline?.gameObject.SetActive(true);
            lighterGigleline?.StartTimeline();
            // lighterFlyingTImeline.StartTimeline();
        };
    }
    public void TimelineFlyingFinished() {
        GameLog.LogMessage("Timlien Finished");

        lighterGigleline.gameObject.SetActive(false);
       
    }
}
