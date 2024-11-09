using TMPro;
using UnityEngine;
using Webaby.Utils;
using Vector3 = UnityEngine.Vector3;

public class MenuGameManager : MonoBehaviour
{
   // public Text roundsText;
    public TMP_Text toggleIntroText;
    public TMP_Text quitText;
    public SceneTransitions scenetransition;
    MeshRenderer mesh;
    int index = 0;
    public bool playIntro;

    public CutsceneMode letterVanishingCutSceneMode;
    public CutsceneTimelineBehaviour letterVanishingCutsceneBehaviour;
    public Button_Sprite nieumialekButton;
    public string nextsceneWithoutIntro = "NowaPrzygoda";
    public string introscene = "NowaPrzygoda.Intro";
    public bool GameLogEnabled;


    public void TogglePlayIntro() 
    {
        GameLog.LogMessage("Toggle play intro entered");
        Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
        string text;
        if (playIntro)
        {
            text = "Pomin intro";
            playIntro = false;
            //zmien etykiete na pomiń intro
           
        }
        else {

            text = "Odegraj intro";
            playIntro = true;

        }
        LeanTween.scale(toggleIntroText.gameObject, new Vector3(2f,2f, 2f), .5f).setEase(LeanTweenType.easeOutElastic).setLoopOnce();
        toggleIntroText.text = text;
        LeanTween.scale(toggleIntroText.gameObject, new Vector3(1f, 1f, 1f), .5f).setEase(LeanTweenType.easeOutBounce).setLoopOnce();



       
        GameLog.LogMessage("Toggle play intro left, play Intro="+ playIntro);

    }
    private void Awake()
    {
        index = 0;
        playIntro = true;
        clicked = false;
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;
        //Logging eanbled or not 
        if (!GameLogEnabled)
            Debug.unityLogger.logEnabled = GameLogEnabled;


        if (nieumialekButton)
        {
            GameLog.LogMessage("set click fun on HERO !!!!!!!!!!!!!!!!!!!");
            nieumialekButton.GetComponent<Button_Sprite>().ClickFunc = () =>
            {
                GameLog.LogMessage("Clicked Sprite Button !!!!");
                // ShowInventory();

                nieumialekButton.transform.GetComponent<Animator>().SetTrigger("jump");
                // windowCharacterPortrait.Show();
            };
        }
    }
    public Transform[] letters;
    private bool clicked;
    private bool quit;

    public void StartGame() {

        GameLog.LogMessage("Start Game entered, play intro" + playIntro+" clicked"+clicked);
        if (!clicked) { 
        clicked = true;
            quit= false;
            // Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
           // Music.Instance.StopToPlayMusic();
             PlayLetterTimeline();

        

            if (playIntro)
            {
                scenetransition.LoadScene(introscene);
              
            }
            else
            {
                if (Music.Instance.track2.clip)
                    Music.Instance.PlayAudio(Music.Instance.track2.clip);
                scenetransition.LoadScene(nextsceneWithoutIntro);
            }
         
           
        }
    }

    private void UpdateLettersSortingLayer(string SortingLayerName,int sortingOrder) {

        if (index >= letters.Length)
            return;


        GameLog.LogMessage("Letter = " + letters[index].name+" index="+index);
            mesh = ((MeshRenderer)letters[index].GetComponent("MeshRenderer"));
            mesh.sortingLayerName = SortingLayerName;
            mesh.sortingOrder = sortingOrder;
            index = index+1;
        
    }

    private void UpdateAllLettersSortingLayer(string SortingLayerName, int sortingOrder)
    {
        for (int i = 0; i < letters.Length; i++)
        {
            mesh = ((MeshRenderer)letters[i].GetComponent("MeshRenderer"));
            mesh.sortingLayerName = SortingLayerName;
            mesh.sortingOrder = sortingOrder;
   
        }
        mesh = ((MeshRenderer)letters[index].GetComponent("MeshRenderer"));
        mesh.sortingLayerName = SortingLayerName;
        mesh.sortingOrder = sortingOrder;
        

    }

    public void UpdateLetters()
    {
        UpdateLettersSortingLayer("Default", 0);
    }

    public void TimeLineStarted() 
    {
        UpdateAllLettersSortingLayer("Items",0);
    }


    public void QuitAbecadlowoTimeline()
    {
        Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
        LeanTween.scale(toggleIntroText.gameObject, new Vector3(2f, 2f, 2f), .5f).setEase(LeanTweenType.easeOutElastic).setLoopOnce();
        quitText.text = "";
        LeanTween.scale(toggleIntroText.gameObject, new Vector3(1f, 1f, 1f), .5f).setEase(LeanTweenType.easeOutBounce).setLoopOnce();
        quit = true;
        PlayLetterTimeline();


    }


    void PlayLetterTimeline() {

        switch (letterVanishingCutSceneMode)
        {
            case CutsceneMode.Play:
                letterVanishingCutsceneBehaviour.StartTimeline();
                
                break;
            case CutsceneMode.None:

                break;
        }

    }
    public void QuitAbecadlowo() 
    {
        Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
        if (quit)
            Application.Quit();
    
    }
}
