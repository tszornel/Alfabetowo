using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneTransitions : MonoBehaviour
{
    Animator transitionAnim;
    public Animator playerAnim;
    public Animator haloAnim;
    public Image loadingProgressBar;
    public GameObject loadingInterface;
    // public Toggle togglebutton;
    //List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    private string sceneNameToLoad;
    public Transform loadingTextTransform;
    private static Color loadingTextColor;
    private TextMeshProUGUI loadingText;
    public Image fadePanel;
    private AsyncOperation handleLoading;
    public static SceneTransitions Instance { get; private set; }
    // Start is called before the first frame update

    private void Awake()
    {
        fadePanel = GetComponent<Image>();
        if (loadingTextTransform)
            loadingText = loadingTextTransform.GetComponent<TextMeshProUGUI>();
        transitionAnim = GetComponent<Animator>();
    }

    void Start()
    {
       
        //StartCoroutine(LoadingBarRoutine());        
    }
    IEnumerator LoadingBarRoutine()
    {
        if (loadingInterface)
            loadingInterface.SetActive(true);
        if (loadingProgressBar && loadingText)
        {
            loadingProgressBar.gameObject.SetActive(true);
            loadingText.gameObject.SetActive(true);
            TweenArrow(loadingProgressBar.transform);
          TweenLoadingText(loadingText.color);
        }
        yield return null;
    }
    void updateValueExampleCallback(Color val)
    {
        loadingText.color = val;
    }
    void updateValuePanelCallback(Color val)
    {
        fadePanel.color = val;
    }
    async void StartLoadingInterfaceAsync()
    {
        await Task.Run(() => StartLoadingInterfaceSeparate());
        Debug.Log("All Done!");
    }
    void StartLoadingInterfaceSeparate()
    {
        if (loadingProgressBar && loadingText)
        {
            loadingProgressBar.gameObject.SetActive(true);
            loadingText.gameObject.SetActive(true);
            TweenArrow(loadingProgressBar.transform);
            // LeanTween.alphaCanvas(fadePanel, 1, 2).setOnComplete(FadeFInished); 
            TweenLoadingText(loadingText.color);
        }
    }
    // Loading Scene 
    public void LoadScene(string sceneName, int seconds)
    {
        StartCoroutine(Transition(sceneName, seconds));
    }
    public void StartGame()
    {
        // transitionAnim.SetTrigger("end");
        LoadScene("NowaPrzygoda");
    }
    void ShowLoadingInterface()
    {
        StartCoroutine(LoadingBarRoutine());
    }
    public void LoadScene(string sceneName)
    {
        GameLog.LogMessage("Scene name " + sceneName);
        sceneNameToLoad = sceneName;
        // ShowLoadingInterface();
        //ShowLoadingInterface();
       // StartLoadingInterfaceAsync();
        FadePanel();
        ShowLoadingInterface();
    }
    private void FadePanel() {
        if (fadePanel)
        {
            fadePanel.gameObject.SetActive(true);
            TweenPanelText(fadePanel.color);
        }
    }
    public void AppQuit()
    {
        transitionAnim.SetTrigger("end");
        AppQuitTransition();
    }
    IEnumerator AppQuitTransition()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1);
        Application.Quit();
    }
    IEnumerator Transition(string sceneName, int seconds)
    {
        yield return new WaitForSeconds(seconds);
         GameLog.LogMessage("Load scene" + sceneName);
        handleLoading = SceneManager.LoadSceneAsync(sceneName);
       /* while (!handleLoading.isDone)
        {
            if (loadingInterface)
                loadingInterface.SetActive(true);
            // ShowLoadingInterface();
            StartLoadingInterfaceSeparate();
             yield return null;
        }*/
        // StartSceneAsync();
    }
   
    private static void TweenArrow(Transform arrow)
    {
        float tweenTime = 1.5f;
        //LeanTween.cancel(arrow.gameObject);
        arrow.localScale = Vector3.one;
        //  LeanTween.scale(arrow.gameObject, Vector3.one * 1.2f, tweenTime).setEaseOutElastic().setLoopPingPong();
        LeanTween.rotateAround(arrow.gameObject, Vector3.forward, 360, tweenTime).setLoopClamp();
    }
    private void TweenLoadingText(Color loadingTextcolor)
    {
        var color = loadingTextcolor;
        var fadeoutcolor = color;
        fadeoutcolor.a = 0;
        LeanTween.value(loadingTextTransform.gameObject, updateValueExampleCallback, fadeoutcolor, color, 0.5f).setLoopPingPong();
    }
    private void TweenPanelText(Color loadingTextcolor)
    {
        GameLog.LogMessage("Fade Panel entered");
        var color = loadingTextcolor;
        var fadeoutcolor = color;
        fadeoutcolor.a = 1f;
        LeanTween.value(fadePanel.gameObject, updateValuePanelCallback, color, fadeoutcolor, 0.5f).setOnComplete(FadeFinished);
    }
    private void FadeFinished()
    {
        StartCoroutine(Transition(sceneNameToLoad, 0));
    }
    private static void setAlphaColorCallback(float c)
    {
        // For some reason it also tweens my image's alpha so to set alpha back to 1 (I have my color set from inspector). You can use the following
        var tempColor = loadingTextColor;
        tempColor.a = c;
        loadingTextColor = tempColor;
    }
}
 
