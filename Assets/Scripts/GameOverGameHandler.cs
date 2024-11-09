using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
public class GameOverGameHandler : MonoBehaviour
{
    public SceneTransitions transitions;
    [Header("Cut Scenes Game Over Scene")]
   // public CutsceneMode starNewGameCutscene;
    public CutsceneTimelineBehaviour starNewGameCutsceneBehaviour;
    public CutsceneTimelineBehaviour lettersCutsceneBehaviour;
    public CutsceneTimelineBehaviour startMoonBehaviour;
    public Button_Sprite moonButton;
       public void StartNewGameTimeline()
    {
        if (starNewGameCutsceneBehaviour)
        {
            starNewGameCutsceneBehaviour.gameObject.SetActive(true);
            if (lettersCutsceneBehaviour)
            {
                lettersCutsceneBehaviour.TimelineFinished();
                lettersCutsceneBehaviour.gameObject.SetActive(false);
            }
            starNewGameCutsceneBehaviour.StartTimeline();
        }
    }
    private void Start()
    {
        moonButton.ClickFunc = () =>
        {
            StartMoonTimeLine();
        };
    }
    public void StartNewGameTimeLineFinished() {
        transitions.LoadScene("NowaPrzygoda");
    }
    public void StartMoonTimeLine()
    {
        startMoonBehaviour.StartTimeline();
    }

    public void CloseApp() {
        GameLog.LogMessage("Close app executed");
        Application.Quit();
       // Application.wantsToQuit
    }


    void OnApplicationQuit()
    {
        PlayerPrefs.SetString("QuitTime", "The application last closed at: " + System.DateTime.Now);
    }
}
