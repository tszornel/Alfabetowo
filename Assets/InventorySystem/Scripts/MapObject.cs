using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Map")]
public class MapObject : ItemObject
{
    public event EventHandler OnMapused;
    public bool mapUsed;
    [SerializeField] private Transform mapButton;
    public void Awake()
    {
        type = ItemObjectType.Map;
    }
    public void SetMapUsed(bool _mapUsed)
    {
        this.mapUsed = _mapUsed;
        mapButton = GameObject.FindGameObjectWithTag("Map").transform.Find("mapButton");
        mapButton?.gameObject.SetActive(true);      
        OnMapused?.Invoke(this, EventArgs.Empty);
    }
}