using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layerChanger : MonoBehaviour
{
public Renderer objectToChangeLayer;

  
    public void ChangeSortingLayer(string layerName)
    {
        if (objectToChangeLayer != null)
        {
            objectToChangeLayer.sortingLayerName = layerName;
        }
        else
        {
            Debug.LogWarning("Object to change layer is not assigned.");
        }
    }

    // Metoda, która przywraca pierwotną warstwę sortowania 
    public void ChangeSortingLayerWithDelay(string layerName, float delay)
    {
        ChangeSortingLayer(layerName);
        Invoke("RestoreOriginalLayer", delay);
    }

    private string originalLayer;
    
    private void Awake()
    {
        if (objectToChangeLayer != null)
        {
            originalLayer = objectToChangeLayer.sortingLayerName;
        }
    }

    private void RestoreOriginalLayer()
    {
        if (objectToChangeLayer != null)
        {
            objectToChangeLayer.sortingLayerName = originalLayer;
        }
    }

}




