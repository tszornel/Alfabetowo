using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCheckPoints : MonoBehaviour
{
    public void Start()
    {
        TweenArrow(transform);  
    }


    public void TweenArrow(Transform arrow)
    {

        float tweenTime = 2.5f;
        LeanTween.cancel(arrow.gameObject);
          arrow.localScale = Vector3.one;
        //  LeanTween.scale(arrow.gameObject, Vector3.one * 1.2f, tweenTime).setEaseOutElastic().setLoopPingPong();

        LeanTween.rotateAround(arrow.gameObject, Vector3.forward, 360, tweenTime).setLoopClamp();
        
    }

    
}
