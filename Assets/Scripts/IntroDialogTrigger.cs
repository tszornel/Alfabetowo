using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;


public class IntroDialogTrigger : MonoBehaviour
{
    [StringInList(typeof(PropertyDrawerHelper), "AllDialogs")]
    public string[] introDialogs;
    private Dialog dialog;
   // public static DialogLanguage language;
   // private DialogDatabase dialogDatabase;
    public GameObject page1;
    private UnitAudioBehaviour audioBehaviour;

    private void Start()
    {
       // language = UtilsClass.GetLanguageFromPrefs(PlayerPrefs.GetString("language", "Polski"));
      //  TriggerDialog();
    }
    public void TriggerDialog()
    {
        Dialog dialog1 = DialogManager.Instance.GetDialog(introDialogs[0]);
        dialog = dialog1;
    
        GameLog.LogMessage("Dialog Name Start DIALOG" + dialog.name);
       
        DialogManager.Instance.StartIntroDialog(dialog);
        if (page1)
            page1.SetActive(true);
        if (audioBehaviour)
        {
            GameLog.LogMessage("Play audio for page 1");
             audioBehaviour.PlayIntroPageMusic(0);
        }
            
    }
    
    void Awake()
    {
      //  dialogDatabase = Resources.Load<DialogDatabase>("DialogDatabase");
        audioBehaviour = GetComponent<UnitAudioBehaviour>();
    }
}
