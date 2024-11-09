using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPButtonView : MonoBehaviour
{
    [SerializeField]
    TMP_Text title;
    [SerializeField]
    TMP_Text price;

    public void OnProductFetched(Product product)
    {
        GameLog.LogMessage("OnProductFetched entered"+ product.metadata.localizedTitle+ " price"+price);
       /* if (title != null)
        {
            title.text = product.metadata.localizedTitle;
        }
        
        if (price != null)
        {
            price.text = product.metadata.localizedPriceString;
        }*/
    }
}