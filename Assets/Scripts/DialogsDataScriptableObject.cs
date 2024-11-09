using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "_Data_Dialogs", menuName = "Webaby/Unit/Dialogs Data", order = 1)]
public class DialogsDataScriptableObject : ScriptableObject
{
    // Start is called before the first frame update
    [Header("Data")]
    public DialogDatabase dialogDatabase;
    [Header("Dialogs")]
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] haveNoMoney;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] doesNotFit;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] fullBag;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] comeBackLater;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] doesNotWork;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] whatNow;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] haveToFixIt;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] buyTime;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] hereYouAre;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] canBuySomething;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] friendDialogs;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] friendDialogsShorts;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string successFriendDialog;
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string introDialog;
   

    public AudioClip GetIntroPageDialogSegment(int pageId)
    {
        AudioClip introPageClip = GetSuccessDialog(introDialog).dialogSegments.ElementAt(pageId).audio;
        return introPageClip;
    }
    public AudioClip[] GetIntroPageClips()
    {
        List<AudioClip> introAudioClipList = new List<AudioClip>();
        foreach(DialogSegment introPageClipSegment in GetSuccessDialog(introDialog).dialogSegments) 
        {
            introAudioClipList.Add(introPageClipSegment.audio);
        }  
        return introAudioClipList.ToArray();
    }

    public Dialog GetFullBagClips()
    {
        return SelectRandomDialog(fullBag);
    }


    /*public void PlayAudioClip(AudioClip selectedAudioClip)
    {
        Music.Instance.SwitchMusic(selectedAudioClip);
    }*/
    private void Awake()
    {
        if(dialogDatabase==null)
            dialogDatabase = Resources.Load<DialogDatabase>("DialogDatabase");
    }
    public Dialog GetFriendDialog()
    {
        return SelectNextDialog(friendDialogs);
    }
    public Dialog[] GetAllFriendsDialogs()
    {
        return dialogDatabase.GetDialogsFromNameArray(friendDialogs);
    }
    public Dialog GetFriendDialogShorts()
    {
        return SelectRandomDialog(friendDialogsShorts);
    }
    public Dialog GetCanBuySomethingDialog()
    {
        return SelectRandomDialog(canBuySomething);
    }
    public Dialog GetDoesNotFitDialog()
    {
        return SelectRandomDialog(doesNotFit);
    }
    public Dialog GetWhatNowDialog()
    {
        return SelectRandomDialog(whatNow);
    }
    public Dialog GetBuyTimeDialog()
    {
        return SelectRandomDialog(buyTime);
    }
    public Dialog GetHaveToFixItDialog()
    {
        return SelectRandomDialog(haveToFixIt);
    }
    public Dialog GetHereYouAreDialog()
    {
        return SelectRandomDialog(hereYouAre);
    }
    public Dialog GetDoesNotWork()
    {
        return SelectRandomDialog(doesNotWork);
    }
    public Dialog GetComeBackLaterDialog()
    {
        return SelectRandomDialog(comeBackLater);
    }
    Dialog SelectRandomDialog(string[] dialogArray)
    {
        if (dialogArray.Length <= 0)
        {
            return null;
        }
        int randomClipInt = Random.Range(0, dialogArray.Length);
        return dialogDatabase.GetDialogByName(dialogArray[randomClipInt]);
    }
    Dialog SelectNextDialog(string[] dialogArray)
    {
        Dialog dialog=null;
        if (dialogArray.Length <= 0)
        {
            return null;
        }
        for (int i = 0; i < dialogArray.Length; i++)
        {
            dialog = dialogDatabase.GetDialogByName(dialogArray[i]);
            if (dialog.dialogPlayed)
                continue;
            else
                break;
        }
        return dialog;
    }
    public Dialog GetSuccessDialog(string DialogName)
    {
        return dialogDatabase.GetDialogByName(DialogName);
    }

    public Dialog GetIntroDialog()
    {
        return dialogDatabase.GetDialogByName(introDialog);
    }

    public Dialog GetHaveNoMoney()
    {
        return SelectRandomDialog(haveNoMoney);
    }
}
