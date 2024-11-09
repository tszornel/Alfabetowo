using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
   public class ShowItemDescription : MonoBehaviour
   {
                // Start is called before the first frame update
    public TMP_Text descriptionTextBox;
    string descriptionText;
    private void Awake()
    {
        DescriptionDisplay.TryToBuyItemAction += ShowItemDescriptionMethod;
        InventoryDisplay.UseItemAction += ShowItemDescriptionMethod;
    }




    public bool ShowItemDescriptionMethod(Item item) 
    {
      //for now skip description but maybe should be added Talking for sellers 
        
        descriptionText = "";
                    if (item.buffs != null) {
                        if (item.buffs.Length > 0) {
                            for (int i = 0; i < item.buffs.Length; i++)
                            {
                                int value = item.buffs[i].value;
                                string name = item.buffs[i].attribute.ToString();
                                 GameLog.LogMessage("attribute value" + value+" i="+i);
                                 GameLog.LogMessage("attribute name" + name);
                                 descriptionText += item.buffs[i].attribute.ToString() + ":" + value +"\n";
                             }
                        }           
                    }
                    descriptionTextBox.text = descriptionText;
                   // descriptionTextBox.gameObject.SetActive(true);
        return true;        
    }
    private void OnDestroy()
    {
        InventoryDisplay.UseItemAction -= ShowItemDescriptionMethod;
        DescriptionDisplay.TryToBuyItemAction -= ShowItemDescriptionMethod;
    }
}
