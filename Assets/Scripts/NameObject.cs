using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
[System.Serializable]
public struct NameSegment
{
    public Boolean initialActive;
    public Boolean active;
    public Item nameLetter;
    public AudioClip letterSound;
    public void SetActive(bool active)
    {
        this.active = active;
    }
}
[CreateAssetMenu(fileName = "New Name ", menuName = "Name System/Name")]
public class NameObject : ScriptableObject
{
    //  public event EventHandler OnNameChanged;
    public List<NameSegment> nameLetters = new List<NameSegment>();
    public int Id;
    public string displayedName;
    public Sprite characterImage;
    public bool done;
    public DialogLanguage language;
    public string savePath;
    public Action<object, EventArgs> OnPlaySuccess { get; internal set; }
    private void OnEnable()
    {
    }
    public override string ToString() {
        string nameObjectString ="";
        foreach (NameSegment letter in nameLetters)
        {
           //  if (letter.active)
                nameObjectString += letter.nameLetter.Name;
        }
        return nameObjectString;
    }
    public bool CheckCorrect() {
        int successAmount = nameLetters.Count;
       GameLog.LogMessage("check correct " + successAmount);
        int verifyCount = 0;
        for (int i = 0; i < nameLetters.Count; i++) {
            if (nameLetters[i].active)
            {
                 verifyCount++;
            }
        }
        var condition = (verifyCount == successAmount);
        done = condition ? true : false;

        GameLog.LogMessage("SUCCESS=" + done);
        return done;
    }
    public void ResetNameObject() 
    {
        done = false;
        for (int i = 0; i < nameLetters.Count; i++)
        {
            NameSegment ns = nameLetters[i];
            ns.active = ns.initialActive;
            nameLetters[i] = ns;
        }
    }
}