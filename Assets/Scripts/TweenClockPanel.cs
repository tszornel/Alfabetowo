using System;
using UnityEngine;
using UnityEngine.UIElements;


public class TweenClockPanel : MonoBehaviour
{
    [SerializeField]
    GameObject backPanel, clockButton, repeatButton, quitButton;

    [SerializeField]
    GameObject clockIcon, repeatIcon, quitIcon;

    [SerializeField]
    GameObject clockButtonLabel, repeatButtonLabel, quitButtonLabel;
    private bool clockPanelShown;
    private GameObject currentLabel;
    public static Func<int, Boolean> TryToBuyTimeAction;

    [SerializeField] ClockUI clockUI;
    UnitAudioBehaviour unitAudioBehaviour;




    public void BuyTime() {


        bool succes = TryToBuyTimeAction.Invoke(2);
        if (succes) {

            clockUI?.ResetClockTime();  
        
        }   

    }

    public static TweenClockPanel Instance
    {
      get; private set;
    }



    public void Awake()
    {
        backPanel.SetActive(false);
        clockPanelShown = false;
        if (!clockUI)
            clockUI = ClockUI.Instance;

        unitAudioBehaviour = GetComponent<UnitAudioBehaviour>();
    }


    // Start is called before the first frame update

  

    public bool TryToBuyTime()
    {
        bool success = TryToBuyTimeAction(2);
        return success;
    }


    public void ShowSuccesPanel()
    {
        clockPanelShown = true; 
        LeanTween.scale(clockButton, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(repeatButton, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(quitButton, new Vector3(0f, 0f, 0f), 0.3f);
        backPanel.SetActive(true);


        DialogManager.Instance?.StopDialog();

        SteeringPanel.Instance?.HideSteeringPanel();
        //GameHandler.Instance.steeringPanel.SetActive(false);    
        LeanTween.scale(backPanel, new Vector3(0.5f, 0.5f, 0.5f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setOnComplete(Complete);
        LeanTween.moveLocal(backPanel, new Vector3(-30f, 300f, 2f), .7f).setDelay(.3f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(PlaySoundForPanel);
        LeanTween.scale(backPanel, new Vector3(1f, 1f, 1f), 2f).setDelay(.4f).setEase(LeanTweenType.easeInOutCubic);



    }


    public void ToggleLabel(int page)
    {
        GameLog.LogMessage("Toggle label entered");
        
        switch (page)
        {
            case 1:
                unitAudioBehaviour?.PlayClockTicking();
                currentLabel = clockButtonLabel; 
                break;
            case 2:
                unitAudioBehaviour?.StopAudio();
                currentLabel = repeatButtonLabel; 
                break;
            case 3:
                unitAudioBehaviour?.StopAudio();
                currentLabel = quitButtonLabel; 
                break;

            default:
                break;
        }
        LeanTween.scale(currentLabel.gameObject, new Vector3(2f, 2f, 2f), .5f).setEase(LeanTweenType.easeOutElastic).setLoopOnce();
      //  toggleIntroText.text = text;
        LeanTween.scale(currentLabel.gameObject, new Vector3(1f, 1f, 1f), .5f).setEase(LeanTweenType.easeOutBounce).setLoopOnce();





    }

    void PlaySoundForPanel()
    {
        GetComponent<UnitAudioBehaviour>()?.PlayUIPanelBounce();
    }

    void Complete()
    {
        LeanTween.scale(clockButton, new Vector3(1f, 1f, 1f), 0.3f);
        LeanTween.scale(repeatButton, new Vector3(1f, 1f, 1f), 0.3f);        
        LeanTween.scale(quitButton, new Vector3(1f, 1f, 1f), 0.3f);
        LeanTween.moveLocal(backPanel, new Vector3(0f, 0f, 0f), 0.7f).setEaseOutBounce();
        // LeanTween.scale(buyButton, new Vector3(1f, 1f, 1f), 0.3f).setDelay(.9f);



    }


    public void ClickCoins(GameObject clickedProduct)
    {
        LeanTween.cancel(clockIcon);
        LeanTween.cancel(repeatIcon);
        LeanTween.cancel(quitIcon);
        LeanTween.scale(clickedProduct, new Vector3(1.1f, 1.1f, 1.1f), 0.5f).setEase(LeanTweenType.easeInBounce).setLoopPingPong();

    }

    public void TweenSuccessPanel()
    {
        GameLog.LogMessage("TweenClockPanel"+ clockPanelShown);
        if (!clockPanelShown)
        {
            ShowSuccesPanel();
        }
        else
        {
            unitAudioBehaviour?.StopAudio();
            CloseSuccesPanel(false);
        }
    }

    public void CloseSuccesPanel(bool quitGame)
    {
        GameLog.LogMessage("CloseSuccesPanel entered");
        clockPanelShown = false;
        LeanTween.cancelAll();
        // mapPanel.GetComponent<Animator>().SetInteger("mapState",1);
        LeanTween.moveLocal(backPanel, new Vector3(-30f, 2000f, 2f), .5f).setDelay(0.3f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(clockButton, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(repeatButton, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(quitButton, new Vector3(0f, 0f, 0f), 0.3f);
       
        SteeringPanel.Instance.ShowSteeringPanel(.5f);
        backPanel.SetActive(false);
     

    }
}
