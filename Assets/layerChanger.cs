using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layerChanger : MonoBehaviour
{
    public GameObject objectToChangeLayer;
    private int originalLayer;

    
    void Start()
    {
        originalLayer = objectToChangeLayer.layer;
    }
    public void ChangeLayer(int newLayer)
    {
        objectToChangeLayer.layer = newLayer;
    }

    public void ResetLayer()
    {
        objectToChangeLayer.layer = originalLayer;
    }
    
    void Update()
    {
        
    }
}




