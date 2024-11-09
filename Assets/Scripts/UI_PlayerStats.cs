using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text;
public class UI_PlayerStats : MonoBehaviour
{
    [SerializeField] TMP_Text textBox;
     public Player player;
    Dictionary<string, string> atributesNameToIcon = new Dictionary<string, string>();
    PlayerAttribute[] attributes;
    public RectTransform scrolViewPort;
    private bool statsShown = false;
    private StringBuilder sb = new StringBuilder();

    private void Awake()
    {
        Player.PlayerAttributesChangeAction += AttributesChanges;
        atributesNameToIcon.Add("Intelect", "<sprite name=Intelect>");
        atributesNameToIcon.Add("Power", "<sprite name=Sword>");
        atributesNameToIcon.Add("Protection", "<sprite name=Shield>");
        atributesNameToIcon.Add("Health", "<sprite name=Health>");
        atributesNameToIcon.Add("Speed", "<sprite name=Run>");
        atributesNameToIcon.Add("Agility", "<sprite name=Jump>");
        
        
    }
    private void Start()
    {
        AttributesChanges(player.playerData.playerAttributes);
       // textBox.text = "";
    }
    private void OnDestroy()
    {
        Player.PlayerAttributesChangeAction -= AttributesChanges;
    }
    public void AttributesChanges(PlayerAttribute[] attributes)
    {
        GameLog.LogMessage("Attributes Changes Entered");
        this.attributes = attributes;
        //string attributesString = "";
        //change to string builder
        if(sb!=null)
            sb.Clear();
        for (int i = 0; i < attributes.Length; i++)
        {
            GameLog.LogMessage("Type"+attributes[i].type.ToString());
            GameLog.LogMessage("Value"+attributes[i].value.ModifiedValue.ToString());
            sb.Append(atributesNameToIcon[attributes[i].type.ToString()]);
            sb.Append("     ");
            sb.Append(attributes[i].value.ModifiedValue);
            sb.Append("\n");
            //attributesString += atributesNameToIcon[attributes[i].type.ToString()] +" "+ attributes[i].value.ModifiedValue+"\n";
        }
        //read attributes and display on screen
        textBox.text = sb.ToString();
    }

    public void ToggleStats() {

        if (statsShown)
        {
            HideFullStats();



        }
        else {
            ShowFullStats();
        }
    }

    private void ShowFullStats() {
        TweenViewPort(0, 145, 0.5f, false);
       // TweenTextBox(0, 60, 0.5f, false);

        statsShown = true;
    }

    private void HideFullStats()
    {
        TweenViewPort(145, 0, 0.5f, false);
        //TweenTextBox(60, 0, 0.5f, false);
        statsShown = false;

    }


    private void TweenTextBox(float from, float to, float tweenTime, bool close)
    {

        LeanTween.value(textBox.gameObject, from, to, tweenTime).setOnUpdate((to) =>
        {
            scrolViewPort.anchoredPosition = new Vector2(0, to);
        }
       ).setEaseOutSine().setOnCompleteParam(close).setOnComplete(() => DisableTextBox(close));
    }
    public void DisableStatsBox(bool close)
    {
        GameLog.LogMessage("DisableStatsBox entered:" + close);
        if (close)
        {
            gameObject.SetActive(false);

        }
    }

    public void DisableTextBox(bool close)
    {
        GameLog.LogMessage("DisableTextBox entered:" + close);
        if (close)
        {
            textBox.gameObject.SetActive(false);

        }
    }

    private void TweenViewPort(float from, float to, float tweenTime, bool close) {
        GameLog.LogMessage("TweenViewPort entered" + to);
        LeanTween.value(scrolViewPort.gameObject, from, to, tweenTime).setOnUpdate((to) =>
        {
            scrolViewPort.offsetMax = new Vector2(scrolViewPort.offsetMax.x, to);
        }
       ).setEaseOutSine().setOnCompleteParam(close).setOnComplete(() => DisableStats(close));
    }
    public void DisableStats(bool close)
    {
        GameLog.LogMessage("DisableStatsBox entered:" + close);
        if (close)
        {
            gameObject.SetActive(false);

        }
    }
}
