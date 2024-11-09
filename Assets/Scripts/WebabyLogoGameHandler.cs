using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebabyLogoGameHandler : MonoBehaviour
{


    public SceneTransitions sceneTransitions;
    PlayerPlatformerController player;
    public string nextscene = "NowaPrzygoda.StartMenu";
    // Start is called before the first frame update
    private void Awake()
    {
        MoveAlongPath.FinishedPath += OnFinishedPath;
        player = FindObjectOfType<PlayerPlatformerController>();
        if(!sceneTransitions)
            sceneTransitions = FindObjectOfType<SceneTransitions>();

    }

    private void Start()
    {
        player.FireLogo();
       // sceneTransitions.LoadSceneAsyncTest("NowaPrzygodaStartMenu");
    }
    private void OnFinishedPath(object sender, EventArgs e)
    {
        GameLog.LogMessage("OnFinishedPath entered");
        PlayerPlatformerController player = FindObjectOfType<PlayerPlatformerController>();

        player.StopFireLogo();
        StartCoroutine(LoadSceneStartMenu(0.8f));
    
        
       // sceneTransitions.LoadScene("NowaPrzygodaStartMenu", 2);
    }

    IEnumerator LoadSceneStartMenu(float sec)
    {
        yield return new WaitForSeconds(sec);
        sceneTransitions.LoadScene(nextscene);
    }

    private void OnStartPath()
    {
        GameLog.LogMessage("OnFinishedPath entered");
        
        player.FireLogo();
       // sceneTransitions.LoadSceneAsyncTest("NowaPrzygodaStartMenu");
    }

    private void OnDisable()
    {
        MoveAlongPath.FinishedPath -= OnFinishedPath;
    }
    // Update is called once per frame

}
