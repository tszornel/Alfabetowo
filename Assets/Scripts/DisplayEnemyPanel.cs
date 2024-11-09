using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Webaby.Utils;


public class DisplayEnemyPanel : MonoBehaviour
{
    [SerializeField] private Transform descriptionBox;
    [SerializeField] private Transform DescImageTransform;
    private GameObject panel;
    [SerializeField] private TMP_Text descTextName;
    private TMP_Text descriptionTextBox;
    //string descriptionText;
    Dictionary<string, string> atributesNameToIcon = new Dictionary<string, string>();
    private StringBuilder sb = new StringBuilder();
    private Item item;
    public Enemy enemy;
    bool panelShown = false;

    UI_AbilityPanel abilityPanel;

    [Header("Unit Health")]
    public UnitHealthBehaviour healthBehaviour;

    [Header("UI References")]
    public UISliderBehaviour healthSlider;

    void SignUpToHealthChange()
    {
        healthBehaviour.HealthChangedEvent += UpdateHealthDisplay;
    }

    void UnsignToPreviousHealthChange()
    {
        healthBehaviour.HealthChangedEvent -= UpdateHealthDisplay;
    }


    public void UnsignToPreviousHealthChange(UnitHealthBehaviour hB)
    {
        hB.HealthChangedEvent -= UpdateHealthDisplay;
    }

    void Start()
    {
        if(enemy)
            SetupHealthDisplay(enemy.GetComponent<UnitHealthBehaviour>());
    }

    void SetupHealthDisplay(UnitHealthBehaviour _healthBehaviour)
    {
        if(healthBehaviour!=null)
            UnsignToPreviousHealthChange(); 

        healthBehaviour = _healthBehaviour; 
        int totalHealth = _healthBehaviour.GetCurrentHealth();
        int maxHealth = _healthBehaviour.GetMaxValue();
        GameLog.LogMessage("Enemy"+gameObject.name+" health="+totalHealth); 
        SignUpToHealthChange();
        healthSlider.SetupDisplay((float)maxHealth);
        UpdateHealthDisplay(totalHealth);
    }

    void UpdateHealthDisplay(int newHealthAmount)
    {
        healthSlider.SetCurrentValue((float)newHealthAmount);
    }

    private void Awake()
    {
        // Hide();
        // ShowEnemyDesription(GetComponent<Enemy>());

        if (descriptionBox == null)
            descriptionBox = transform;
        DescImageTransform = descriptionBox.Find("DescImage");
        descTextName = DescImageTransform.Find("EnemyName").GetComponent<TMP_Text>();
        abilityPanel = transform.Find("UI_Unit_Display_Enemy").GetComponent<UI_AbilityPanel>();
        descriptionTextBox = descriptionBox.Find("TextBox").GetComponent<TMP_Text>();
        atributesNameToIcon.Add("Power", "<sprite name=Sword>");
        atributesNameToIcon.Add("Health", "<sprite name=Heart>");
        atributesNameToIcon.Add("Intelect", "<sprite name=Intelect>");
        atributesNameToIcon.Add("Agility", "<sprite name=Jump>");
        atributesNameToIcon.Add("Speed", "<sprite name=Speed>");
        atributesNameToIcon.Add("Protection", "<sprite name=Shield>");
    }


    private void UpdateEnemyData(Enemy _enemy)
    {
        
        if (gameObject.activeSelf)
            gameObject.SetActive(true);
        this.enemy = _enemy;
        SetupHealthDisplay(_enemy.healthBehaviour);
        
        GameLog.LogMessage("Update enemy" + _enemy.enemyData, _enemy);
        descTextName.text = _enemy.enemyData.enemyName;
        if (sb != null)
            sb.Clear();
        if (enemy.buffs != null)
        {
            if (enemy.buffs.Length > 0)
            {
                for (int i = 0; i < enemy.buffs.Length; i++)
                {
                    int value = enemy.buffs[i].value;
                    string name = enemy.buffs[i].attribute.ToString();
                    GameLog.LogMessage("attribute value" + value + " i=" + i);
                    GameLog.LogMessage("attribute name" + name);

                    sb.Append(atributesNameToIcon[enemy.buffs[i].attribute.ToString()]);
                    sb.Append(" ");
                    sb.Append(value);
                    sb.Append("\n");

                    // descriptionText += atributesNameToIcon[item.buffs[i].attribute.ToString()] + " " + value + "\n";
                }
            }
        }
        descriptionTextBox.text = sb.ToString();
        abilityPanel.SwitchPortraitImage(enemy.enemyData.image, enemy.enemyData.spriteAsset);

    }


    public void ShowEnemyDesription(Enemy enemy)
    {
        UpdateEnemyData(enemy);

        Show();
    }

    public void ShowEnemyDesriptionForSeconds(Enemy enemy, float seconds)
    {
        UpdateEnemyData(enemy);
        descriptionTextBox.gameObject.SetActive(true);

        ShowForSeconds(seconds);
    }

    public void ShowEnemyDesriptionForOneSecond(Enemy enemy)
    {
        UpdateEnemyData(enemy);
        descriptionTextBox.gameObject.SetActive(true);

        ShowForSeconds(1f);
    }

    public void Hide()
    {
        if (panelShown) 
        {
            panelShown = false;     
            Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
            SetDescriptionWindow(gameObject, 0f, 350f, true);
        }



        //animator.SetBool("showDescription", false);
    }

    public void ShowForSeconds(float seconds)
    {
        Show();

        StartCoroutine(ShowCourutine(seconds));

        //animator.SetBool("showDescription", false);
    }

    IEnumerator ShowCourutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Hide();
        UnsignToPreviousHealthChange(healthBehaviour);
        gameObject.SetActive(false);
    }


    public void Show()
    {
        panelShown = true;
        if(!abilityPanel.gameObject.activeSelf)
            abilityPanel.gameObject.SetActive(true);   
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        Music.Instance.track2.PlayOneShot(Music.Instance.uiClickSound);
        SetDescriptionWindow(gameObject, 0f, 0f, false);

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
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, to);
            //  GameLog.LogMessage("POSITION x:" + gameObject.GetComponent<RectTransform>().anchoredPosition.x);
        }
        ).setEaseOutElastic().setOnCompleteParam(close).setOnComplete(() => DisableDescriptionBox(close));




    }

    private void OnDestroy()
    {
        if (healthBehaviour != null)
            UnsignToPreviousHealthChange();
    }
    public void DisableDescriptionBox(bool close)
    {
        if (close)
            gameObject.SetActive(false);
    }
}

