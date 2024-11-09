using UnityEngine;
using System.Collections;

public class SetCanvasCamera : MonoBehaviour
{

    public Camera MyCamera;
    public GameObject MyCanvas;
    // Use this for initialization

    void Awake()
    {
        MyCamera = Camera.main; 
        if (MyCamera ==null)
            MyCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Start()
    {
        if (MyCamera == null)
            MyCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        Canvas theCanvas = GetComponent<Canvas>();
        theCanvas.worldCamera = MyCamera;
    }

}
