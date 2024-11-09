using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LillyCollider : MonoBehaviour
{
    // Start is called before the first frame update
    UnityEngine.U2D.Spline spline;
    Transform player;
    float distance = 1.5f;

    private void Awake()
    {
        spline = GetComponent<SpriteShapeController>().spline;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameLog.LogMessage("entered spline lilly collision");
        ShakeSpline(collision.transform.position);

    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { 
        GameLog.LogMessage("entered spline lilly collision");
        ShakeSpline(collision.transform.localPosition);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // GameLog.LogMessage("exit spline lilly collision");
            ShakeSpline(collision.transform.localPosition);
        }
    }



    bool ShakeSpline(Vector3 position)
    {
        int pointCount = spline.GetPointCount();
        if (pointCount < 2)
            return false;
        for (int i = 0; i < pointCount - 1; ++i)
        {

                       float pointWordPosition = transform.position.x + spline.GetPosition(i).x;
            float nextPointWordPosition = transform.position.x + spline.GetPosition(i).x;
               ;


            if (System.Math.Abs(player.position.x - pointWordPosition) <= distance)
            {
                GameLog.LogMessage("Spline shake entered");
                Vector3 previoiusPosition = spline.GetPosition(i);
                //Shake point
                //spline.SetPosition(i, previoiusPosition + Vector3.up * 2);
                StartCoroutine(CouroutineMoveUpAndDown(spline,i, previoiusPosition));
                return true;
            }
        }
        return false;
    }

    //shakes spline point

    IEnumerator CouroutineMoveUpAndDown(Spline spline, int index, Vector3 previous)
    {
        GameLog.LogMessage("Shaked index" + index);
        float multiplier = 0.1f;
        float time = 0.1f;
        spline.SetPosition(index, previous + Vector3.down * multiplier);
        yield return new WaitForSeconds(time);
        spline.SetPosition(index, previous);


    }
    IEnumerator LerpCouroutineMoveUpAndDown(Spline spline,int index, Vector3 previous) {
        GameLog.LogMessage("Shaked index" + index);
        float multiplier = 0.01f;
        float time = 0.1f;
        float duration = 2f;
        Vector3 startPosition = previous;
        float transformationTime = 0f;
        while (transformationTime < duration/2) 
        {
            spline.SetPosition(index, previous + Vector3.down * multiplier);
            time += Time.deltaTime;
            yield return null;
        }
        spline.SetPosition(index, previous + Vector3.down * 0.2f);
        transformationTime = 0f;
        while (transformationTime < duration/2)
        {
            spline.SetPosition(index, previous);
            time += Time.deltaTime;
            yield return null;
        }
        spline.SetPosition(index, previous);
        //spline.SetPosition(index, previous + Vector3.down*0.2f);
        // yield return new WaitForSeconds(time);
        // spline.SetPosition(index, previous);


    }



    private void OnDisable()
    {
        StopAllCoroutines();    
    }

    /* OnControllerColliderHit(hit : ControllerColliderHit)
     {
         
     }*/

}
