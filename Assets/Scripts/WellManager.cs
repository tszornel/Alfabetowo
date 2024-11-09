using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellManager : MonoBehaviour
{

    Transform bucket;
    // Start is called before the first frame update
    private void Awake()
    {
        bucket = transform.Find("bucket");
    }


    public void MoveBucket()
    {

       // LeanTween.rotateZ


    }

}
