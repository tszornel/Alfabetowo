using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PageTurn : MonoBehaviour
{
    public DialogManager dialogManager;
    public GameObject[] pages;
    private int index = 0;
    public TextMeshProUGUI text;
    private Animator flipAnim;
    public SceneTransitions sceneManager;
    private UnitAudioBehaviour audioBehaviour;
    public GameObject buttonBack;
    public GameObject nextButton;
   // UnitDialogBehaviour dialogBehaviour;
    public void StopAnim() {
        if (index<= pages.Length - 1)
           // pages[index].SetActive(false);
        //clear previous text
        text.text = "";
    }

    public void DisplayFirstSentence()
    {
                  DisplaySentence(0, 0);
        
    }

    public void DisplayNextSentence() {
        GameLog.LogMessage("Display net sentence index:" + index + " pages lenght" + pages.Length);
        if (index == pages.Length - 1)
        {
            nextButton.SetActive(false);
            buttonBack.SetActive(false);
            sceneManager.StartGame();
            return;
        }
        else 
        {
            flipAnim.Play("pageFlipNew");
            buttonBack.SetActive(true);
            if (index == pages.Length - 2)
                nextButton.SetActive(false);
            SetupSentence();
            index++;
            DisplaySentence(index, 1.5f);
        }
    }
    public void DisplayPreviousSentence()
    {
        flipAnim.Play("PageFlipBackNew");
        if(index ==1)
            buttonBack.SetActive(false);
        if(!nextButton.activeSelf)
            nextButton.SetActive(true);
        SetupSentence();
        index--;
        DisplaySentence(index,1.5f);
    }
    private void SetupSentence() {
        if (index < 0)
            return;
            GameLog.LogMessage("Display next index=" + flipAnim.GetBool("turn"));
        if (dialogManager.dialogStop)
        {
            dialogManager.EndDialog();
            return;
        }
        //text.text = "";
        pages[index].SetActive(false);
       // flipAnim.Play("pageFlip");
       // dialogManager.DisplaySentence(index);
    }
    private void DisplaySentence(int index, float delay)
    {
        if (index >= 0)
            dialogManager.DisplaySentence(index,delay);
        else 
        {
            //zamknij ksiazke
            return;
        }
        GameLog.LogMessage("displey before index=" + index + " pages lenght"+ pages.Length);
        if (index <= pages.Length)
        {
            GameLog.LogMessage("Page name=" + pages[index].name + " " + pages[index].activeSelf);
            pages[index].SetActive(true);
            GameLog.LogMessage("Page name=" + pages[index].name + " " + pages[index].activeSelf);
            ///flipAnim.SetBool("turn", false);
            if (audioBehaviour)
                audioBehaviour.PlayIntroPageMusic(index);
        }
    } 
    public void Awake()
    {
        flipAnim = GetComponent<Animator>();
        sceneManager = FindObjectOfType<SceneTransitions>();
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
       // dialogBehaviour = GetComponent<UnitDialogBehaviour>();
       
    }
}
