using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform trackedObject;
    public float minX;
    public float maxX;
    public float DeltaX;
    // Update is called once per frame
    private void Start()
    {
    }
    void Update()
    {
        if (trackedObject != null)
        {
          //  float clampedX = Mathf.Clamp(trackedObject.position.x + DeltaX, minX, maxX);
            transform.position = new Vector3(trackedObject.position.x, trackedObject.position.y, transform.position.z);
        }
    }
}
