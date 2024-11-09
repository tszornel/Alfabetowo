using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class leanTweanRorate : MonoBehaviour
{
    void Update()
    {
       /* Vector3 myVector = new Vector3(0.0f, 1.0f, 0.0f); */
        LeanTween.rotateAround(gameObject, Vector3.back, 10f, 0.1f); /* 1f or 10 * Time.deltaTime */
    }
}
