using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using System;
public class ClockUI : MonoBehaviour
{
    private static float day;
    public float REAL_SECONDS_PER_INGAME_DAY = .5f;
    private Transform clockMinuteHandTransform;
    private Transform clockHourHandTransform;
    public Transform clock;
    private static TextMeshProUGUI timeText;
    private GameObject timeObject;
    private SceneTransitions sceneTransitions;
    public  tweenUI tweenUIinstance;
    public GameObject finishClock;
    public GameObject finishClockBackground;
    public Transform finishClockMinuteHandTransform;
    public Transform finishClockHourHandTransform;
    public Transform homeClockMinuteHandTransform;
    public Transform homeClockHourHandTransform;
    [SerializeField]TMP_Text finishClockTimeText;
    bool finished=false;
    private static bool stopedTime;
    UnitAudioBehaviour unitAudioBehaviour;  

    public static ClockUI Instance { get; private set; }


    void OnGUI()
    {
        if (GameHandler.Instance && !GameHandler.Instance.GameLogEnabled)
            return;
        /* GUILayout.Label("Pool size: ");
         if (GUILayout.Button("Create Particles"))
         {
             GameLog.LogMessage("clicked the BUTTON ");
             print("You clicked the button!");
             ResetClockTime();
         }*/
        /* if (GUI.Button(new Rect(400, (Screen.height) - 25, 150, 20), "RESET CLOCK"))
     {
         GameLog.LogMessage("clicked the BUTTON ");
       //  print("You clicked the button!");
         ResetClockTime();
       }*/
    }

    public void StopTime() 
    {
        Time.timeScale = 0;
        stopedTime = true; 
    
    }

    public void StartTime()
    {
        Time.timeScale = 1;
        stopedTime = false;

    }


    // Awake is called at the game start
    private void Awake()
    {
        stopedTime = false;
        clockMinuteHandTransform = clock.transform.Find("ClockMinuteHand");
        clockHourHandTransform = clock.transform.Find("ClockHourHand");
        if (clock.transform.Find("ClockTimeText")) 
        { 
            timeObject = clock.transform.Find("ClockTimeText").gameObject;
        // GameLog.LogMessage("Odczytana pole:"+timeText.text);
        // GameLog.LogMessage("clockHand object name:" + clockMinuteHandTransform.name);
        //clockHandTransform = GetComponent()<("ClockHand");
            timeText = timeObject.GetComponent<TextMeshProUGUI>();
        }
        sceneTransitions = FindObjectOfType<SceneTransitions>();
        // GameLog.LogMessage("Odczytana pole:"+ timeObject.name);
        tweenUIinstance = tweenUI.Instance;

        if (!tweenUIinstance)
            tweenUIinstance = GameObject.FindObjectOfType<tweenUI>(true);

        unitAudioBehaviour = GetComponent<UnitAudioBehaviour>();        
    }
    // Update is called once per frame
    void Update()
    {
        if (stopedTime) return;
        // GameLog.LogMessage("Update CLOCK"+ timeText.text);
        if (timeText && timeText.text.Equals("23:59"))
        {
            // Time for lesson finished
            if (!finished)
                StartCoroutine(TweenFinishClock(0,360));
            //sceneTransitions.LoadScene("NowaPrzygoda.GameOver",4);
            return;
        }
        UpdateClockTime();
        //  clockHandTransform.eulerAngles = new Vector3(0, 0, -Time.realtimeSinceStartup * 90f);
    }
    public 
    IEnumerator TweenBackHomeClock(float fromValue, float toValue)
    {
        GameLog.LogMessage("TweenBackHomeClock");
        finished = true;
        finishClock.SetActive(true);
       // float tweenTime = 2f;
       // LeanTween.value(HourHandTransform.gameObject, 0, toValue, tweenTime).setEasePunch().setOnUpdate(setClockHourHand);
       // LeanTween.value(MinuteHandTransform.gameObject, 0, toValue, tweenTime).setEasePunch().setOnUpdate(setClockMinuteHand);
        yield return new WaitForSeconds(3);
        LeanTween.cancelAll();
    }

    private void HideFinishClock() {
        if(finishClock && finishClock.activeSelf)
            finishClock.SetActive(false);

    }
    IEnumerator TweenFinishClock(float fromValue, float toValue)
    {
        GameLog.LogMessage("TweenFinishClock");
        finished = true;
        if (unitAudioBehaviour)
            unitAudioBehaviour.PlayClockTicking();

        if (finishClock) { 
            finishClock.SetActive(true);
            float tweenTime = 2f;
       // previousFinishClockColor = finishClockBackground.GetComponent<Image>().color;
         finishClockBackground.transform.localScale = Vector3.one *2;
            LeanTween.scale(finishClockBackground.gameObject, Vector3.one * 3, tweenTime).setEasePunch().setRepeat(-1);
       // LeanTween.value(finishClockTimeText.gameObject, 0, 24f, tweenTime).setEasePunch().setOnUpdate(setFinishClockText);
            LeanTween.value(finishClockHourHandTransform.gameObject, fromValue, toValue, tweenTime).setEasePunch().setOnUpdate(setClockHourHand);
            LeanTween.value(finishClockMinuteHandTransform.gameObject, fromValue, toValue, tweenTime).setEasePunch().setOnUpdate(setClockMinuteHand);
         var tempColor = finishClockBackground.GetComponent<Image>().color;
            LeanTween.value(finishClockBackground.gameObject, setColorCallback, tempColor, Color.red, 5f);
            yield return new WaitForSeconds(5);
            LeanTween.cancelAll();
            finishClockBackground.GetComponent<Image>().color = tempColor;
            GameLog.LogMessage("Show Success Panel from Clock");
            tweenUIinstance?.ShowSuccesPanel();
        }

        //sceneTransitions.LoadScene("NowaPrzygoda.GameOver");
        /*finishClocksetOnComplete(() =>
        {
            tempColor.a = 0f;
            finishClockBackground.GetComponent<Image>().color = tempColor;
            finishClock.SetActive(false);
            sceneTransitions.LoadScene("NowaPrzygoda.GameOver");
        });*/
    }
    private void setColorCallback(Color c)
    {
        finishClockBackground.GetComponent<Image>().color = c;
    }
    public void setClockHourHand(float value)
    {
        GameLog.LogMessage("setClockHourHand entered");
        if(finishClockHourHandTransform)
            finishClockHourHandTransform.eulerAngles = new Vector3(0, 0, value);
        if(clockHourHandTransform)
             clockHourHandTransform.eulerAngles = new Vector3(0, 0, value);
    }
    public void setClockMinuteHand(float value)
    {
        if (finishClockHourHandTransform)
            finishClockMinuteHandTransform.eulerAngles = new Vector3(0, 0, value);
        if (clockHourHandTransform)
            clockMinuteHandTransform.eulerAngles = new Vector3(0, 0, value);
    }
    public void resetTimeText() {
        timeText.text = "00:00";
    }
   /* public void setFinishClockText(float value)
    {
        string normalized = Mathf.Floor(value * 24f).ToString("00");
        finishClockTimeText.text = normalized;
    }*/
    private void UpdateClockTime()
    {
        day += Time.deltaTime / REAL_SECONDS_PER_INGAME_DAY;
       // GameLog.LogMessage("PRINT DAY:"+day);
        float dayNormalized = day % 1f;
        float rotationDegreesPeDay = 360f;
        //GameLog.LogMessage("Day normalized:" + dayNormalized);
        clockHourHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPeDay);
        float hoursPerDay = 24f;
        string hourString = Mathf.Floor(dayNormalized * 24f).ToString("00");
        float minutePerHour = 60f;
        string minuteString = Mathf.Floor(((dayNormalized * hoursPerDay) % 1) * minutePerHour).ToString("00");
        clockMinuteHandTransform.eulerAngles = new Vector3(0, 0, -dayNormalized * rotationDegreesPeDay * hoursPerDay);
        if(timeText)
            timeText.text = hourString + ":" + minuteString;
    }
    public static string GetTime()
    {
        GameLog.LogMessage("GetTime entered");
        return timeText.text;
    }
    public void ResetClockTime()
    {
        GameLog.LogMessage("ResetClockTime entered");
        if (unitAudioBehaviour)
            unitAudioBehaviour.PlayClockBack();
        setClockHourHand(0);
        setClockMinuteHand(0);
        day = 0;        
        if(timeText != null)    
            resetTimeText();
        finished = false;
        HideFinishClock();
    }
}
