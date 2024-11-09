
using UnityEngine.Purchasing;


public class MyIAPManager : IAPListener
{

    private IStoreController controller;
    private IExtensionProvider extensions;

    public string coinsProductId = "com.webaby.abecadlowo.5_coins";
    public string bunchofcoinsProductId = "com.webaby.abecadlowo.10_coins";
    public string chestofcoinsProductId = "com.webaby.abecadlowo.20_coins";

    private string purchasedProduct="";

    public WalletController wallet;

    public UnitDialogBehaviour dialogBehaviour;


    private void Awake()
    {
        dialogBehaviour = GetComponent<UnitDialogBehaviour>();
    }

    /*  public void Buy(string productid)
      {
          purchasedProduct = productid;
          controller.InitiatePurchase(productid);
      }

      public void Buy5coins()
      {
          purchasedProduct = coinsProductId;
          controller.InitiatePurchase(coinsProductId);
      }

      public void Buy10coins()
      {
          purchasedProduct = bunchofcoinsProductId;
          controller.InitiatePurchase(bunchofcoinsProductId);
      }

      public void Buy20coins()
      {   
          purchasedProduct = chestofcoinsProductId;  
          controller.InitiatePurchase(chestofcoinsProductId);
      }*/



    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        foreach (var product in controller.products.all)
        {
            print($"Initialize product: '{product.metadata.localizedTitle}'");
            GameLog.LogMessage(product.metadata.localizedTitle);
            GameLog.LogMessage(product.metadata.localizedDescription);
            GameLog.LogMessage(product.metadata.localizedPriceString);
        }
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        GameLog.LogMessage("Initialization Failed");
        
    }



    public void  ProcessPurchaseProduct(Product product)
    {
       // print($"Purchase success - Product: '{product.definition.id}'");
        dialogBehaviour.PlayWhatNow();
        //Retrieve the purchased product
        //var product = args.purchasedProduct;

        //Add the purchased product to the players inventory
        GameLog.LogMessage($"Purchase Complete entered");
        if (product.definition.id == coinsProductId || coinsProductId.Equals((string)product.definition.id))
        {
            GameLog.LogMessage("Addcoins =" + 5);
            AddCoins(5);
        }
        else if (product.definition.id == bunchofcoinsProductId || bunchofcoinsProductId.Equals((string)product.definition.id))
        {
            GameLog.LogMessage("Addcoins =" + 10);
            AddCoins(10);
        }
        else if (product.definition.id == chestofcoinsProductId || chestofcoinsProductId.Equals((string)product.definition.id))
        {
            GameLog.LogMessage("Addcoins =" + 20);
            AddCoins(20);
        }

        GameLog.LogMessage($"Purchase Complete - Product: {product}"+" definition id="+ product.definition.id);

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>

    /*public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        //var product = args.purchasedProduct;

        //Add the purchased product to the players inventory
        if (purchasedProduct == coinsProductId)
        {
            AddCoins(5);
        }
        else if (purchasedProduct == bunchofcoinsProductId)
        {
            AddCoins(10);
        }
        else if (purchasedProduct == chestofcoinsProductId)
        {
            AddCoins(20);
        }

        GameLog.LogMessage($"Purchase Complete - Product: {purchasedProduct}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }*/

    public void AddCoins(int v)
    {
        GameLog.LogMessage("Addcoins entered" + v);
        wallet.AddAmountToWallet(v);
    }

   /* public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        GameLog.LogMessage($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }*/

    public void OnPurchaseFailed(Product product)
    {
        dialogBehaviour.PlayDoesNotWork();
        print($"Purchase failed - Product: '{product.definition.id}'");
        GameLog.LogMessage($"Purchase failed - Product: '{product.definition.id}'");
    }

   /* public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        GameLog.LogMessage($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }*/
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        dialogBehaviour.PlayDoesNotWork();
        GameLog.LogMessage("Purchase failed -"+error +" message"+message);
    }
}