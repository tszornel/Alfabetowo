using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Purchasing;





public class ShopController : MonoBehaviour, IStoreListener
{

    IStoreController storeController;
    private ConsumableItem cItem;

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new System.NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {

        throw new System.NotImplementedException();

    }

    // Start is called before the first frame update
    void Start()
    {
        int coins = PlayerPrefs.GetInt("coins");
        SetupBuilder();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void SetupBuilder() {

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
       // builder.AddProduct(cItem.ID,ProductType.Consumable);
    
    }

    public void Consumable_Btn_Pressed() {
        AddCoins(5);

    
    }

    private void AddCoins(int v)
    {
       
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
