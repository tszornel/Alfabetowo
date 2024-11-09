using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class ShowTextOnSprite : MonoBehaviour
{
    MeshRenderer mesh;
    //public int sortingLayerId = 6;
    public int sortingOrder = 14;
    public string sortingLayerName = "Items";
    private void Awake()
    {
        mesh = ((MeshRenderer)transform.GetComponent("MeshRenderer"));
        mesh.sortingLayerName = sortingLayerName;
        mesh.sortingOrder = sortingOrder;
        //mesh.sortingLayerID = sortingLayerId;
    }
}
