using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "_CheckPointData", menuName = "Webaby/CheckPoint")]
public class CheckpointData : ScriptableObject
{
    public string checkPointName;
    public bool active;
    public bool registered;


    public void Activate() 
    { 
        active = true;  
        registered = true;  
    }

  
    public void Deactivate()
    {
        active = false;
        
    }


    public void Reset()
    {
        active = false;
        registered = false;
    }
    

    
}
