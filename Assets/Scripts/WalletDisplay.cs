using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WalletDisplay : MonoBehaviour
{
    [SerializeField]TMP_Text amountText;
    WalletController wallet;
    [SerializeField] GameObject Background;
    public float backgroundShakeRate = 2.0f;
    Vector2 previousPosition;
    public bool showZero = false;

    public void SetWallet(WalletController wallet)
    {
        this.wallet = wallet;
        wallet.OnWalletChanged += Wallet_OnWalletChanged;
        if (Background)
            previousPosition = Background.gameObject.GetComponent<RectTransform>().anchoredPosition;
        IncreaseScore(wallet.DisplayWalletValue());
    }

    public void IncreaseScore(float value)
    {
        GameLog.LogMessage("IncreaseScore Entered");
        if (!showZero)
            if (value == -1 || value == 0)
                return;
        LeanTween.cancel(amountText.gameObject);
       // LeanTween.cancel(Background.gameObject);
        GameLog.LogMessage("Increace coins:" + value);
        float tweenTime = 0.5f;
        
        LeanTween.move(Background.gameObject, Random.insideUnitCircle * backgroundShakeRate, tweenTime).setEasePunch().setOnComplete(() =>
        {
            Background.gameObject.GetComponent<RectTransform>().anchoredPosition = previousPosition;
        }); 
       // LeanTween.move(Background.gameObject, previousPosition,0.1f);
        amountText.transform.localScale = Vector3.one;
        LeanTween.scale(amountText.gameObject, Vector3.one * 2, tweenTime).setEasePunch();
        LeanTween.value(amountText.gameObject, value, value, tweenTime).setEasePunch().setOnUpdate(setCoinAmount);
        Background.LeanColor(Color.red, 0.3f).setEasePunch().setOnComplete(() =>
        {
            Background.GetComponent<Image>().color = Color.white;
            Background.gameObject.GetComponent<RectTransform>().anchoredPosition = previousPosition;
        });
    }

    public void setCoinAmount(float value) 
    {
        amountText.text = value.ToString();
    }

    public void UnSetWallet(WalletController walletController)
    {
        walletController.OnWalletChanged -= Wallet_OnWalletChanged;
    }
    private void Wallet_OnWalletChanged(object sender, System.EventArgs e)
    {
        IncreaseScore(wallet.DisplayWalletValue());
    }



}