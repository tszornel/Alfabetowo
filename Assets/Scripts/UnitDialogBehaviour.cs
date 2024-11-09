using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitDialogBehaviour : MonoBehaviour
{
    DialogManager manager;
    [Header("Dialogs Data")]
    public DialogsDataScriptableObject dialogs;

    Queue<Dialog> unitDialogsTobePlayed;


    public void PlayFullBag()
    {

        PlayDialog(dialogs.GetFullBagClips());
    }



    public void PlayNoMoney()
    {

        PlayDialog(dialogs.GetHaveNoMoney());
    }


    public void PlayFixIt()
    {

        PlayDialog(dialogs.GetHaveToFixItDialog());
    }


    public void PlayWhatNow() {

        PlayDialog(dialogs.GetWhatNowDialog());
    }

    public bool PlaySuccesDialog(string DialogName) {

        Dialog dialog = dialogs.GetSuccessDialog(DialogName);
        if (!dialog.dialogPlayed)
        {
            PlayDialog(dialogs.GetSuccessDialog(DialogName));
            return true;
        }
        return false;
            

    }

    public bool CheckDialogPlayed(string DialogName)
    {

        return dialogs.GetSuccessDialog(DialogName).dialogPlayed;

    }

    public void PlayDoesNotFitDialog()
    {
        PlayDialog(dialogs.GetDoesNotFitDialog());
    }

    public void PlayCanBuySomething()
    {
        PlayDialog(dialogs.GetCanBuySomethingDialog());
    }

    public void PlayDialog(Dialog dialog)
    {
        if (manager == null)
        {
            manager = DialogManager.Instance;
        }
        manager.StartDialog(dialog);
       

    }

    public void PlayIntroDialog(Dialog dialog)
    {
        if (manager == null)
        {
            manager = DialogManager.Instance;
        }
        manager.StartIntroDialog(dialog);


    }




    public Dialog GetNextDialog()
    {
        Dialog playedDialog = null;
      if (unitDialogsTobePlayed.Count!=0)
        playedDialog = unitDialogsTobePlayed.Dequeue();  
        if (!playedDialog)
            return null;    

           
       // manager.StartDialog(playedDialog);
        return playedDialog;
    }

    public Dialog GetNextDialogOrShortifAlreadyPlayed()
    {
        Dialog playedDialog=null;
        if (unitDialogsTobePlayed.Count != 0)
            playedDialog = unitDialogsTobePlayed.Dequeue();
        if (!playedDialog)
            return null;

        if (playedDialog.dialogPlayed)
            playedDialog = manager.GetDialog(playedDialog.GetDialogShortName());
        

        // manager.StartDialog(playedDialog);
        return playedDialog;
    }



    public bool PlayDialogOrShort(string dialogName)
    {
        if (manager == null)
        {
            manager = DialogManager.Instance;
        }
        bool dialogAlreadyPlayed = true;
        GameLog.LogMessage("DIalog name=" + dialogName);
        Dialog dialog = manager.GetDialog(dialogName);
        if (dialog.dialogPlayed && dialog.hasShort)
        {
            dialog = manager.GetDialog(dialog.GetDialogShortName());
        }
        else if (!dialog.dialogPlayed) {

            dialogAlreadyPlayed = false;

        }

        PlayDialog(dialog);    
        return dialogAlreadyPlayed;
    }



    public void PlayDoesNotWork()
    {

        PlayDialog(dialogs?.GetDoesNotWork());

    }

    public void PlayComeBackLater()
    {
        GameLog.LogMessage("Play dialog" + dialogs.GetComeBackLaterDialog().name);
     
            PlayDialog(dialogs.GetComeBackLaterDialog());
      

    }

    private void Awake()
    {
        manager = DialogManager.Instance;
        unitDialogsTobePlayed = new Queue<Dialog>();
        if (!dialogs) 
        {
            GameLog.LogMessage("Brakuje ustawionych Dialogo w " + transform.name);
            return;
        }
       
        if(dialogs.GetAllFriendsDialogs().Length>0)
        foreach (Dialog dialog in dialogs.GetAllFriendsDialogs()) {

            unitDialogsTobePlayed.Enqueue(dialog);  


        }

    }

    public void GetDialogFromName(string dialogName) 
    {
        manager.GetDialog(dialogName);
    }


    public void PlayDialogFromName(string dialogName)
    {
       Dialog dialog  = manager.GetDialog(dialogName);
       dialog.PlayDialog();
       
    }

    public AudioClip[] GetIntroDialogAudio() 
    {

        return dialogs.GetIntroPageClips();
    }

    public void PlayIntroDialog()
    {

        PlayIntroDialog(dialogs.GetIntroDialog());
    }

  
}

