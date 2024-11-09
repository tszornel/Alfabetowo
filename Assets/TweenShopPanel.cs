using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class ConsumableItem
{

    public string ID;
    public string name;
    public string desc;
    public float price;


}
public class TweenShopPanel : MonoBehaviour
{
    [SerializeField]
    GameObject backPanel, coins, BunchOfCoins, CoinChest, closeBtn,buyButton;
    public WalletObject wallet;
    IStoreController storeController;
    private MyIAPManager IAPManager;
    private ConsumableItem cItem;
    public static TweenShopPanel Instance { get; private set; }
    string clickedProduct;


    private void Awake()
    {
        IAPManager = GetComponent<MyIAPManager>();
        SetupBuilder();
    }

    public void ShowSuccesPanel()
    {
        LeanTween.scale(coins, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(BunchOfCoins, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(CoinChest, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(buyButton, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(closeBtn, new Vector3(0f, 0f, 0f), 0.3f);
        backPanel.SetActive(true);
        LeanTween.scale(backPanel, new Vector3(0.5f, 0.5f, 0.5f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setOnComplete(Complete);
        LeanTween.moveLocal(backPanel, new Vector3(-30f, 300f, 2f), .7f).setDelay(.3f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(PlaySoundForPanel);
        LeanTween.scale(backPanel, new Vector3(1f, 1f, 1f), 2f).setDelay(.4f).setEase(LeanTweenType.easeInOutCubic);

        
     }

    void Complete()
    {
        LeanTween.scale(coins, new Vector3(1f, 1f, 1f), 0.3f);
        LeanTween.scale(BunchOfCoins, new Vector3(1f, 1f, 1f), 0.3f).setDelay(.3f);
        LeanTween.scale(CoinChest, new Vector3(1f, 1f, 1f), 0.3f).setDelay(.6f).setOnComplete(SetupProduct);
       // LeanTween.scale(buyButton, new Vector3(1f, 1f, 1f), 0.3f).setDelay(.9f);
        LeanTween.scale(buyButton, new Vector3(1.2f, 1.2f, 1.2f), 1.5f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setLoopPingPong();
        LeanTween.scale(closeBtn, new Vector3(1f, 1f, 1f), 0.3f).setDelay(.9f); ;
        LeanTween.moveLocal(backPanel, new Vector3(0f, 0f, 0f), 0.7f).setEaseOutBounce();
        LeanTween.alpha(coins.GetComponent<RectTransform>(), 1f, .5f).setDelay(1.1f).setLoopPingPong();
    }


    void SetupBuilder()
    {

       // IAPManager.InitializePurchasing();

    }


    public void SetupProduct() {
        ClickCoins(BunchOfCoins);
    }

    public void ClickBuyButton()
    {
       
        LeanTween.scale(buyButton, new Vector3(0.5f, 0.5f, 0.5f), .3f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic).setLoopOnce();
       
    }


    public void ClickCoins(GameObject _clickedProduct)
    {
        clickedProduct = _clickedProduct.name;
        LeanTween.cancel(coins);
        LeanTween.cancel(BunchOfCoins);
        LeanTween.cancel(CoinChest);
        LeanTween.scale(_clickedProduct, new Vector3(1.2f, 1.2f, 1.2f), 0.8f).setEase(LeanTweenType.easeInBounce).setLoopPingPong();

    }

  /*  public void Buy() {

        IAPManager.Buy(clickedProduct);
    }*/

    void PlaySoundForPanel()
    {
        GetComponent<UnitAudioBehaviour>()?.PlayUIPanelBounce();
    }
    /*void ClickedCoinsAnim(GameObject coin)
    {
    
        LeanTween.scale(coin, new Vector3(1.5f, 1.5f, 1.5f), 2f).setEase(LeanTweenType.easeOutElastic).setLoopPingPong();
      
    }*/

    public void CloseSuccesPanel(bool quitGame)
    {
        LeanTween.cancelAll();
        LeanTween.moveLocal(backPanel, new Vector3(0f, 1450f, 2f), .5f).setDelay(0.3f).setEase(LeanTweenType.easeInOutCubic).setOnComplete(DeactivatePanel);
        LeanTween.scale(coins, new Vector3(0f, 0f, 0f), 0.3f);
        LeanTween.scale(BunchOfCoins, new Vector3(0f, 0f, 0f), 0.7f);
        LeanTween.scale(CoinChest, new Vector3(0f, 0f, 0f), 0.7f);
        LeanTween.scale(buyButton, new Vector3(0f, 0f, 0f), 0.7f);
        LeanTween.scale(closeBtn, new Vector3(0f, 0f, 0f), 0.7f);
        LeanTween.scale(backPanel, new Vector3(0f, 0f, 0f), 0.7f).setDelay(.7f);
       
       
      
    }

    private void DeactivatePanel()
    {
        backPanel.SetActive(false);
    }

   
}
