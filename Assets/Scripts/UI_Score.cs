using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UI_Score : MonoBehaviour
{

    public TMP_Text PlaceNumber;
    public TMP_Text ScoreDate;
    public TMP_Text WordsAmount;
    public TMP_Text ScoreTime;
    public TMP_Text ScoreValue;
    public TMP_Text CoinsAmount;


    // Start is called before the first frame update
    private void Awake()
    {
        //if not set than find it
    }


    public void SetScore(int place,AbecadlowoScore score) {
        PlaceNumber.text = place.ToString();
        ScoreDate.text = score.date.ToString("yyyy MM dd HH:mm:ss");
        WordsAmount.text  = score.Words_Amount.ToString();      
        ScoreTime.text = score.Time_Amount.ToString();  
        CoinsAmount.text = score.Coins_Amount.ToString();   
        ScoreValue.text = score.value.ToString();   


    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
