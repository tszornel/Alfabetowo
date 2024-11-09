using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class WalletController : MonoBehaviour
{
    //TMP_Text amountText;
    public WalletObject wallet;
    int coinAmount = 0;
    // Start is called before the first frame update
    public event EventHandler OnWalletChanged;
    public bool clearOnSceneLoad = false;
    private void Awake()
    {
        //coinAmount = PickupItemAssets.Instance.database.GetItem[monety.Id].amount;
        // wallet.amount;
        //OnWalletChanged?.Invoke(this, EventArgs.Empty);

       
    }

    public static WalletController Instance { get; private set; }


    private void OnEnable()
    {
        PickupItem.OnCollectCoin += AddCoinToWallet;
    }

    private void OnDisable()
    {
        PickupItem.OnCollectCoin -= AddCoinToWallet;
    }

    public bool AddCoinToWallet(Item item)
    {
        
        coinAmount = item.GetItemObject().amount;
        GameLog.LogMessage("Add coin to wallet" + item+" amount"+ coinAmount);
        if (coinAmount!=0) { 
            AddToWallet(coinAmount);
            return true;
        } 

        //for free (trzebab poprawic ze coint nie ma ustawionej ilosci zlota 
        return true;
    }


      public bool AddAmountToWallet(int coinAmount)
    {
        
        
        GameLog.LogMessage("Add  amount"+ coinAmount);
        if (coinAmount!=0) { 
            AddToWallet(coinAmount);
            return true;
        } 

        //for free (trzebab poprawic ze coint nie ma ustawionej ilosci zlota 
        return true;
    }
    public string DisplayWallet() {
        return wallet.GetAmount().ToString();
    }

    public float DisplayWalletValue()
    {
        return wallet.GetAmount();
    }
    public void AddToWallet(int addamount) {
       // coinAmount = PickupItemAssets.Instance.database.GetItem[monety.Id].amount;
        wallet.IncreaseAmount(addamount);
        //amountText.text = coinAmount.ToString();
        OnWalletChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool PayWithWallet(int payamount) {
        bool succesfullPayment = false;
        if (wallet.GetAmount() >= payamount)
        {
            wallet.DecreaseAmount(payamount);
            OnWalletChanged?.Invoke(this, EventArgs.Empty);
            succesfullPayment = true;
        }
        else {
            GameLog.LogMessage("Not enought money in wallet");
        }
        //amountText.text = coinAmount.ToString();
        return succesfullPayment;
    }
    public int GetAmount() {
        return wallet.GetAmount();
    }

    public void ClearWallet()
    {

        wallet.ClearWallet();       
       
    }
}
