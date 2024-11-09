using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ReadParams : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //if (PlayerPrefs.GetInt("LevelReached") == null)
        //{
        //    PlayerPrefs.SetInt("LevelReached", 1)
        //}
        int levelReached = PlayerPrefs.GetInt("levelReached", -1);
        GameLog.LogMessage("Level reached = " + levelReached);
    }  
}
