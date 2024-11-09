using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DescriptionDisplay : MonoBehaviour
{
    [SerializeField] private Transform descriptionBox;
    [SerializeField] private Transform DescImageTransform;
    private Image descImage;
    private TMP_Text letterText;
    private TMP_Text descTextName;
    private TMP_Text descriptionTextBox;
    //string descriptionText;
    Dictionary<string, string> atributesNameToIcon = new Dictionary<string, string>();
    private StringBuilder sb = new StringBuilder();
    private Item item;
    public static Func<Item, Boolean> TryToBuyItemAction;
    public static Action ItemBoughtAction;

    public static bool TryToBuyItem(Item item)
    {
        bool success  = TryToBuyItemAction(item);
        return success;
    }

   
    private void Awake()
    {
       Hide();  
        if (descriptionBox == null)
            descriptionBox = transform;
        DescImageTransform = descriptionBox.Find("DescImage");
        descImage = DescImageTransform.Find("Sprite").GetComponent<Image>();
        letterText = DescImageTransform.Find("LetterText").GetComponent<TMP_Text>();
        descTextName = DescImageTransform.Find("ItemName").GetComponent<TMP_Text>();
        descriptionTextBox = descriptionBox.Find("TextBox").GetComponent<TMP_Text>();
        atributesNameToIcon.Add("Power", "<sprite name=Sword>");
        atributesNameToIcon.Add("Health", "<sprite name=Heart>");
        atributesNameToIcon.Add("Intelect", "<sprite name=Intelect>");
        atributesNameToIcon.Add("Agility", "<sprite name=Jump>");
        atributesNameToIcon.Add("Speed", "<sprite name=Speed>");
        atributesNameToIcon.Add("Protection", "<sprite name=Shield>");

    }



    public void TryToBuyItem() {

        bool success = TryToBuyItem(item);
        if (success)
        {
            Hide();
            ItemBoughtAction.Invoke();
        }
    }
    public void ShowEnemyDesription(Enemy enemy) 
    { 
    //DIsplay Enemy properties Transform descri
    } 
    public void ShowItemDescription(Item _item) 
    {

        this.item = _item;
        //animator.SetBool("showDescription", true);
        ItemObject itemObject = item.GetItemObject();


        if (item.Type != ItemObjectType.Letter)
        {
            letterText.gameObject.SetActive(false);
            descImage.gameObject.SetActive(true);
            if (itemObject.itemIcon)
                descImage.sprite = itemObject.itemIcon;
            else
                descImage.sprite = itemObject.itemSprite;
        }
        else 
        {
            descImage.gameObject.SetActive(false);
            letterText.gameObject.SetActive(true);
            letterText.text = ((LetterObject)itemObject).text;  

        }


        descTextName.text = itemObject.displayName;
       // descriptionText = "";
        if (sb != null)
            sb.Clear();
        if (item.buffs != null)
        {
            if (item.buffs.Length > 0)
            {
                for (int i = 0; i < item.buffs.Length; i++)
                {
                    int value = item.buffs[i].value;
                    string name = item.buffs[i].attribute.ToString();
                    GameLog.LogMessage("attribute value" + value + " i=" + i);
                    GameLog.LogMessage("attribute name" + name);

                    sb.Append(atributesNameToIcon[item.buffs[i].attribute.ToString()]);
                    sb.Append(" ");
                    sb.Append(value);
                    sb.Append("\n");

                   // descriptionText += atributesNameToIcon[item.buffs[i].attribute.ToString()] + " " + value + "\n";
                }
            }
        }
        sb.Append("<sprite name=Coin> ");
        sb.Append(itemObject.price);
        //descriptionText += "<sprite name=Coin> " + itemObject.price;
        descriptionTextBox.text = sb.ToString();
        descriptionTextBox.gameObject.SetActive(true);
        if (!gameObject.activeSelf || gameObject.GetComponent<RectTransform>().anchoredPosition.x != 0)
            Show();
    }
    public void Hide() {
        //if(gameobject)
            SetDescriptionWindow(gameObject ,0f, 600f, true);
            
        //animator.SetBool("showDescription", false);
    }

    public void Show()
    {
        //if(gameobject)
        SetDescriptionWindow(gameObject, 600f, 0f, false);

        //animator.SetBool("showDescription", false);
    }


    private void SetDescriptionWindow(GameObject gameObject, float from, float to, bool close)
    {

        gameObject.SetActive(true);
        GameLog.LogMessage("SetDescriptionWindow Entered " + to);
        float tweenTime = 0.5f;
       // LeanTween.moveLocalX(gameObject, to, 0.5f);
        LeanTween.value(gameObject, from, to, tweenTime).setOnUpdate((to) =>
        {
           // GameLog.LogMessage("POSITION x: to " + to);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(to, 30);
          //  GameLog.LogMessage("POSITION x:" + gameObject.GetComponent<RectTransform>().anchoredPosition.x);
        }
        ).setEaseOutElastic().setOnCompleteParam(close).setOnComplete(() => DisableDescriptionBox(close));




    }
    public void DisableDescriptionBox(bool close)
    {
        if (close)
            gameObject.SetActive(false);
    }
}
