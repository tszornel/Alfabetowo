using UnityEngine;
using System.Collections;
using DentedPixel;
using System;

public class MoveAlongPath : MonoBehaviour
{
    public bool getTransformsFromParent;
    public bool deactivateTransforms = true;
    public bool reversePath;
    public GameObject parentTransform;

    public Transform[] transforms;
    public Vector3[] path;

    public bool randomizePath;
    public Vector3 randomizeMin;
    public Vector3 randomizeMax;

    public GameObject toAnimate;

    public float startingTime = 0;

    public float speed = 6f;
	public float speedRandomness = 0;

    private LTSpline visualizePath;

    public LeanTweenType loopType;
    public LeanTweenType easeType;
    public AudioClip clip;
    public bool sendFinishedPath = false;
    public bool moveOnStart;

    public bool startFromPlayer=false;
   // public bool isFinished;

    



    public static event System.EventHandler FinishedPath;


   
    void Start()
    {
		speed += UnityEngine.Random.Range(-speedRandomness, speedRandomness);

        if (getTransformsFromParent)
        {
            transforms = parentTransform.GetComponentsInChildren<Transform>();
        }

        path = new Vector3[transforms.Length - 1];


        if (reversePath)
        {
            for (int i = transforms.Length - 1; i > 1; i--)
            {
                path[path.Length - i] = transforms[i].position;
            }
        }
        else
        {
            for (int i = 1; i < transforms.Length; i++)
            {
                if (startFromPlayer && i == 1)
                    path[i - 1] = toAnimate.transform.position;
                else
                    path[i - 1] = transforms[i].position;
            }
        }

        if (randomizePath)
        {
            for (int i = 0; i < path.Length; i++)
            {
				if (path[i] != toAnimate.transform.position) {
					path[i] += new Vector3(
                         UnityEngine.Random.Range(randomizeMin.x, randomizeMax.x),
                         UnityEngine.Random.Range(randomizeMin.y, randomizeMax.y),
                         UnityEngine.Random.Range(randomizeMin.z, randomizeMax.z)
					);
				}
            }
        }

        if (deactivateTransforms) {
            parentTransform.SetActive(false);
        }

        visualizePath = new LTSpline(path);

        if(!startFromPlayer)
            toAnimate.transform.position = path[0];




        if (moveOnStart) {
            moveonSpline();
        }

    }


    public void WalkToClock() {

        moveonSpline();


    }

    private void moveonSpline() {

        LTDescr tween = LeanTween.moveSpline(toAnimate, path, speed)
           .setOrientToPath2d(true)
           .setSpeed(speed)
           .setEase(easeType)
           .setLoopType(loopType)
           .setDelay(startingTime).setOnComplete(() =>
           {
               if(sendFinishedPath)
                   FinishedPath?.Invoke(this, EventArgs.Empty);

               




           });

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (visualizePath != null)
        {
            visualizePath.gizmoDraw();
        }
    }
}