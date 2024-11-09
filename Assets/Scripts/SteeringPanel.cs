using System.Collections;
using UnityEngine;


public class SteeringPanel : MonoBehaviour
{

    private bool steeringPanelHiden;
    public static SteeringPanel Instance { get; private set; }


    [SerializeField] private Transform abilityFVX;
    [SerializeField] private Transform attackButton;
    private void Awake()
    {
        steeringPanelHiden = true;
        // sentencesArray = new DialogSegment[9];
        // DontDestroyOnLoad(gameObject);
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        if (!abilityFVX)
            abilityFVX = transform.Find("P_VFX_UI_AbilityCharged");

    }

    public void ActivateAbilityFVX()
    {
        StartCoroutine(PlayAbilityEnumerator());
    }

    private IEnumerator PlayAbilityEnumerator()
    {
        GameLog.LogMessage("PlayAbility entered");
        abilityFVX.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameLog.LogMessage("PlayAbility entered after yield");
        abilityFVX.gameObject.SetActive(false);
        yield return null;
    }


    public void ShowSteeringPanel(float time)
    {
        GameLog.LogMessage("Show steering Panel entered !!");
        if (steeringPanelHiden)
        {
            //Show steering Panel
            gameObject.SetActive(true);
            LeanTween.value(gameObject, transform.position.y, 0, time).setOnUpdate(MoveSteeringPanel).setEaseOutBounce();
            steeringPanelHiden = false;
        }
    }

    public void HideSteeringPanel()
    {
        HideSteeringPanel(0.2f);
    }

    public void TweenSteeringPanel()
    {
        GameLog.LogMessage("TweenSteeringPanel entered");
        float time = 0.5f;
        if (steeringPanelHiden)
        {
            //Show steering Panel

            ShowSteeringPanel(time);

        }
        else
        {
            //Hide steering panel
            HideSteeringPanel(time);

        }
    }
    public void HideSteeringPanel(float time)
    {
        GameLog.LogMessage("Hide steering Panel entered");
        if (!steeringPanelHiden)
        {
            LeanTween.value(gameObject, transform.position.y, 500, time).setOnUpdate(MoveSteeringPanel).setEaseOutBounce();
            steeringPanelHiden = true;
            //gameObject.SetActive(false);
        }
    }

    private void MoveSteeringPanel(float value)
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, value);
        gameObject.SetActive(true);

    }


    public void PressAttackButton()
    {
        if (attackButton)
        {
            LeanTween.cancel(attackButton.gameObject);
            GameLog.LogMessage("Przess attack button entered");
            LeanTween.scale(attackButton.gameObject, new Vector3(1.6f, 1.6f, 1.6f), .05f).setEase(LeanTweenType.linear).setOnComplete(MakeNormalAttackButton);
            //  LeanTween.scale(attackButton.gameObject, new Vector3(1f, 1f, 1f), .2f).setEase(LeanTweenType.easeOutBounce);
        }


    }

    private void MakeNormalAttackButton()
    {
        GameLog.LogMessage("MakeNormalAttackButton entered");
        LeanTween.scale(attackButton.gameObject, new Vector3(1f, 1f, 1f), .1f).setEase(LeanTweenType.easeInBounce);
    }


}

