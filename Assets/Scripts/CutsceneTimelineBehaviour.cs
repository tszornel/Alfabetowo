using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using Webaby.Utils;

public class CutsceneTimelineBehaviour : MonoBehaviour
{
    [Header("Timeline")]
    public PlayableDirector cutsceneTimeline;
    [Header("Marker Events")]
    public UnityEvent cutsceneTimelineFinished;
    public List<UnityEvent> cutsceneTimelineStarted;
    public UnityEvent cutsceneAdditionalEvent;
    public void StartTimeline()
    {
        GameLog.LogMessage("StartTimeLine entered"+ gameObject.name);
        cutsceneTimeline.Play();
    }
    public void TimelineFinished()
    {
         GameLog.LogMessage("Timeline finished");
        if (cutsceneTimelineFinished!=null)
        cutsceneTimelineFinished.Invoke();
       
    }
    public void StartTimelineEvent() {
        GameLog.LogMessage("StartEvent");
        if (cutsceneTimelineStarted != null && cutsceneTimelineStarted.Count>0)
            foreach (var item in cutsceneTimelineStarted)
            {
                item.Invoke();
               // cutsceneTimelineStarted.Invoke();
            }
            
            
    }
    public double GetTimelineDuration() {


        return cutsceneTimeline.duration;
    
    }

    

    public void AdditionalEvent()
    {
        GameLog.LogMessage("Additional Event");
        cutsceneAdditionalEvent?.Invoke();
    }

}
