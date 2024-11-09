using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Webaby.Utils;

public class DialogManager : MonoBehaviour
{
    public Canvas dialogCanvas;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI portraitImages;
    public TextMeshProUGUI portraitName;
    public Animator dialogBoxAnimator;
    public PageTurn pageTurn;
    public GameObject continueButton;
    DialogSegment sentense;
    private Queue<DialogSegment> sentences;
    private AudioSource audioSource;
    private Dialog dialog;
    public bool dialogStop;
    private Animator characterAnim;
    private Animator playerAnim;
    public DialogDatabase dialogDatabase;
    private DialogLanguage language;
    public bool intro = false;
    private int indexOfLastFrame;
    private Coroutine talkSentence;

    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
       // sentencesArray = new DialogSegment[9];
       // DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        sentences = new Queue<DialogSegment>();
        dialogDatabase = Resources.Load<DialogDatabase>("DialogDatabase");
        language =  PickupItemAssets.Instance.language;
    }
   private void Start()
    {
        dialogStop = false;
      //  sentences = new Queue<DialogSegment>();
    }
    public void SetCharacterAnimator(Animator anim) {
        characterAnim = anim;
    }
    public void SetPlayerAnimator(Animator anim)
    {
        playerAnim = anim;
    }
    public Dialog GetDialog(string name)
    {

          return dialogDatabase.GetDialogByName(name);
         
        
    }


    private void SetDialogWindow(GameObject dialogCanvas, float from, float to,bool close) {


        GameLog.LogMessage("SetDialogWindow Entered "+to);
        float tweenTime = 0.5f;

        LeanTween.value(dialogCanvas, from, to, tweenTime).setOnUpdate((to) =>
        {
            dialogCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, to);
        }
        ).setEaseOutSine().setOnCompleteParam(close).setOnComplete(() => DisableDialogBox(close));




    }
    public void DisableDialogBox(bool close) {
        if (close)
            dialogCanvas.gameObject.SetActive(false);
    }


    public void StartDialog(Dialog dialog) {
        this.dialog = dialog;
        MusicControl.Instance.MuteMusicWhenDialog();
        if(dialog)
            dialog.dialogPlayed = false;
        dialogStop = false;
        dialogCanvas.gameObject.SetActive(true);
        //dialogBoxAnimator.SetBool("isOpen", true);
        SetDialogWindow(dialogCanvas.gameObject,200f, -100f,false);
        //dialogBoxAnimator.Play("DialogBoxOpen");
        sentences.Clear();
        GameLog.LogMessage("dialog.dialogSegments"+ dialog.dialogSegments);
        foreach (DialogSegment sentense in dialog.dialogSegments)
        {
            sentences.Enqueue(sentense);
        }
        DisplayNextSentence();
    }
    public void StartIntroDialog(Dialog dialog)
    {
        this.dialog = dialog;
        dialog.dialogPlayed = false;
        dialogStop = false;
        GameLog.LogMessage("count sentences"+dialog.dialogSegments.Count);
        //dialogCanvas.gameObject.SetActive(true);
       // dialogBoxAnimator.SetBool("isOpen", true);
        //dialogBoxAnimator.Play("DialogBoxOpen");
        sentences.Clear();
       // sentencesArray = dialog.dialogSegments.ToArray();
        foreach (DialogSegment sentense in dialog.dialogSegments)
        {
            
            sentences.Enqueue(sentense);
        }
        DisplayNextSentence();
    }

    

    private void StopTalking() {

        if (characterAnim && UtilsClass.AnimatorHasParameter(characterAnim,"isTalking"))
            characterAnim.SetBool("isTalking", false);

        if (playerAnim && UtilsClass.AnimatorHasParameter(playerAnim, "isTalking"))
            playerAnim.SetBool("isTalking", false);

        if (playerAnim && UtilsClass.AnimatorHasLayer(playerAnim,"isTalkingLayer"))
        {
            int layerIndex = playerAnim.GetLayerIndex("isTalkingLayer");
            playerAnim.SetLayerWeight(layerIndex, 0);
            // playerAnim.SetBool("isTalking", false);
        }

        if (characterAnim && UtilsClass.AnimatorHasLayer(characterAnim, "isTalkingLayer")) 
        { 
            int layerIndex = characterAnim.GetLayerIndex("isTalkingLayer");
            characterAnim.SetLayerWeight(layerIndex, 0);
            // playerAnim.SetBool("isTalking", false);
        }

    }


    private void Talk() {

        switch (sentense.characterImage.name)
        {
            case "hero-name":
                if (playerAnim && UtilsClass.AnimatorHasLayer(playerAnim, "isTalkingLayer"))
                {
                    int layerIndex = playerAnim.GetLayerIndex("isTalkingLayer");
                    playerAnim.SetLayerWeight(layerIndex, 1);
                }
                else if (playerAnim && UtilsClass.AnimatorHasParameter(playerAnim, "isTalking"))
                {
                    //Jesli nie ma warstwy to jedziemy w animatorze
                    playerAnim.SetBool("isTalking", true);
                }
                break;
            default:
                //Gada Friend i sprawdzamy czy jest warstwa
                if (characterAnim && UtilsClass.AnimatorHasLayer(characterAnim, "isTalkingLayer"))
                {
                    int layerIndex = characterAnim.GetLayerIndex("isTalkingLayer");
                    characterAnim.SetLayerWeight(layerIndex, 1);
                }
                else if (characterAnim && UtilsClass.AnimatorHasParameter(characterAnim, "isTalking"))
                {
                    //Jesli nie ma warstwy to jedziemy w animatorze
                    characterAnim.SetBool("isTalking", true);
                }
                break;
       }
    }

    public void DisplaySentence(int index,float delay)
    {
        StopTalking();
        GameLog.LogMessage("DISPLAY PREVIOUS SENTECE ENTERED");
        if (dialogStop)
        {
            EndDialog();
            return;
        }
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        //sentense = sentences.Dequeue();
        sentense = dialog.dialogSegments.ToArray()[index];
        if (talkSentence != null)
        {
            StopCoroutine(talkSentence);
        }

        dialogText.text = "";
        Talk();
        GameLog.LogMessage("Message To display"+ sentense.lineOfText);
        talkSentence = StartCoroutine(TypeSentense(sentense.lineOfText, sentense.dialogLineDisplayTime, sentense.audio,delay));
        if (portraitImages) {

            //dialogImage.sprite = sentense.characterImage;
            portraitImages.spriteAsset = sentense.characterImage;
          //  indexOfLastFrame = portraitImages.spriteAsset.spriteInfoList.Count -1;
            // portraitImages.text = "<sprite anim=\"0,{indexOfLastFrame},10\">";
            portraitImages.text = "<sprite anim=\"0,3,10\">";
        }
        if (portraitName)
            portraitName.text = sentense.characterName;
    }

    public void DisplayNextSentence()
    {
        if (characterAnim && UtilsClass.AnimatorHasParameter(playerAnim, "isTalking")) 
        {
             characterAnim.SetBool("isTalking", false);
        }
           
        GameLog.LogMessage("DisplayNextSentence entered");
        if (dialogStop) {
            EndDialog();
            return;
        }
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        sentense = sentences.Dequeue();
        if (talkSentence != null)
        {
            StopCoroutine(talkSentence);
        }
        
        dialogText.text = "";
        Talk();


           talkSentence =  StartCoroutine(TypeSentense(sentense.lineOfText, sentense.dialogLineDisplayTime, sentense.audio,0));
        if (portraitImages) {


            GameLog.LogMessage("Set character image sprite Aseet "+ sentense.characterImage);

            portraitImages.spriteAsset = sentense.characterImage;
            portraitImages.text = "<sprite anim=\"0,3,10\">";
        }
            
  
           // indexOfLastFrame = portraitImages.spriteAsset.spriteCharacterTable.Count  - 1;
          //  GameLog.LogMessage("Last index for sprite asset="+portraitImages.spriteAsset.name +" index="+ indexOfLastFrame);
           
        if (portraitName)
            portraitName.text = sentense.characterName;
    }
    public void EndDialog()
    {
       StopTalking();   
        SetDialogWindow(dialogCanvas.gameObject, -100f,200f,true);

        dialog.dialogPlayed = true;
        MusicControl.Instance.UnMuteMusicWhenDialog();
    }
    public void StopDialog() {
        GameLog.LogMessage("Stop Dialog !!!!!!!!!!!!!");
        SetDialogWindow(dialogCanvas.gameObject, -100f, 200f, true);
        dialogStop = true;
        if (talkSentence != null)
        {
            StopCoroutine(talkSentence);
        }

        //StopAllCoroutines();
        StopTalking();
        MusicControl.Instance.UnMuteMusicWhenDialog();
    }
    IEnumerator TypeSentense(string lineOfSentense, float seconds, AudioClip clip, float delay)
    {
       
        MusicControl.Instance.MuteMusicWhenDialog();
      //  GameLog.LogMessage("TypeSentense entered PLAY AUDIO:"+ lineOfSentense);
        audioSource.Stop();
        audioSource.clip = clip;
        float clipTime = clip.length;
        float charSentencesAmount = lineOfSentense.ToCharArray().Length;
        seconds = clipTime / charSentencesAmount;
       // GameLog.LogMessage("Seconds beetwen chars" + seconds);

        float maxDelay = 0.065f;
        if (seconds > maxDelay)
            seconds = maxDelay;

        yield return new WaitForSeconds(delay);
        audioSource.Play();
        foreach (char letter in lineOfSentense.ToCharArray())
        {
           // GameLog.LogMessage("Played letter:" + letter);
            dialogText.text += letter;
            yield return new WaitForSeconds(seconds);
        }
        
        if (clipTime > seconds * charSentencesAmount) 
            yield return new WaitForSeconds(clipTime- seconds * charSentencesAmount);
        if(portraitImages)
            portraitImages.text = "<sprite anim=\"0,0,1\">";
        StopTalking();
        yield return new WaitForSeconds(1);
        if (!intro)
        {
            DisplayNextSentence();
          
        }
        else if (pageTurn) {
            pageTurn.DisplayNextSentence();
            
         }
    
    }
    void Update()
    {
        if (dialogText.text == sentense.lineOfText) {
           if  (continueButton)
                continueButton.SetActive(true);
        }
    }

    private void OnApplicationQuit()
    {
        dialogDatabase.ResetDialogs();
    }
}
