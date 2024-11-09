using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tween();
    }
    private void Tween()
    {

        float tweenTime = 0.8f;
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 2, tweenTime).setEasePunch().setRepeat(-1);


    }

}
