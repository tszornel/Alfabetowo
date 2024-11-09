using UnityEngine;
using UnityEngine.UI;
public class UIHeartController : MonoBehaviour
{
    private Animator hurtAnim;
    private SceneTransitions sceneTransitions;
    public GameObject hurtPanel;
    public Sprite fullheart;
    public Sprite emptyheart;
    public Image[] hearts;
    public UISliderBehaviour[] sliders;
    private Health currentHealth;
    public float backgroundShakeRate = 2.0f;
    public float tweenTime = 1f;
    //Image currentHeart;
    UISliderBehaviour currentSlider;
    Image currentHeart;
    Vector2 middle = new Vector2(Screen.width / 2, Screen.height / 2);
    void OnGUI()
    {
        if (GameHandler.Instance && !GameHandler.Instance.GameLogEnabled)
            return;
        if (GUI.Button(new Rect(600, (Screen.height) - 25, 150, 20), "Refil life"))
        {
            GameLog.LogMessage("clicked the BUTTON ");
            currentHealth.AddLife();
            currentHealth.RefillLife();
        }
    }
    public void SetHealth(Health health)
    {
        GameLog.LogMessage("Set inventory OnItemListChanged");
        this.currentHealth = health;
        currentHealth.UpdateHealthAction += UpdateHearts;
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].gameObject.SetActive(true);
            
            sliders[i].SetupDisplay(health.lives[i].lifeTotalAmount);
        }
        UpdateHearts(health);
    }

    public void UnSetHealth()
    {
        GameLog.LogMessage("UnSet health OnItemListChanged");
        
        currentHealth.UpdateHealthAction -= UpdateHearts;
      
    }
    public void UpdateHearts(Health _health)
    {
        int amount = _health.lives.Length;
        GameLog.LogMessage("UpdateHearts entered:"+ amount);
        if (amount != hearts.Length)
            GameLog.LogError("Zla ilosc zyc ustawiona w panelu :" + hearts.Length, transform);
        else
            updateHeartUI(_health);
     }
    public void ShakeHearts(int _currenthealth)
    {
        GameLog.LogMessage("Current health:" + _currenthealth);
        LeanTween.cancel(hearts[_currenthealth].gameObject);
        hearts[_currenthealth].transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        hearts[_currenthealth].transform.localScale = Vector3.one;
        LeanTween.rotateZ(hearts[_currenthealth].gameObject, 15.0f, 0.5f).setEasePunch();
        LeanTween.scaleX(hearts[_currenthealth].gameObject, 1.5f, 0.5f).setEasePunch();
        // 1
        hurtPanel.gameObject.SetActive(true);
        //LeanTween.move(hurtPanel.gameObject, Random.insideUnitCircle* middle * backgroundShakeRate, 0.5f).setEasePunch();
        Color fromColor = Color.white;
        fromColor.a = 1f;
        Color toColor = Color.red;
        toColor.a = 1f;
        LeanTween.value(hurtPanel.gameObject, setColorCallback, fromColor, toColor, 2f).setOnComplete(() =>
        {
            var tempColor = hurtPanel.GetComponent<Image>().color;
            tempColor.a = 0f;
            hurtPanel.GetComponent<Image>().color = tempColor;
            hurtPanel.gameObject.SetActive(false);
        });
    }
    private void setColorCallback(Color c)
    {
        hurtPanel.GetComponent<Image>().color = c;
    }
    void updateHeartUI(Health _currenthealth)
    {
        GameLog.LogMessage("Update hearts" + _currenthealth);


        int currentHearthindex = _currenthealth.GetCurrentLiveIndex();

       
       // sliders[i].SetCurrentValue
       for (int i = 0; i < _currenthealth.lives.Length; i++)
        {
            //Set Panel active
            // hearts[i].transform.Find("Panel").GetComponent<Image>().fillAmount = 1;
           // sliders[i].SetCurrentValue(_currenthealth.lives[i].lifeTotalAmount);
            bool test = _currenthealth.lives[i].lifePercentage < _currenthealth.lives[i].lifeTotalAmount;
            GameLog.LogMessage(" test=" + test +" current health:"+ _currenthealth);
            if (test)
            {
                ShakeHearts(i);
                if (currentHealth.lives[i].lifePercentage <= 0)
                {
                    GameLog.LogMessage("set empty heart=" );
                    hearts[i].sprite = emptyheart;
                    sliders[i].SetCurrentValue(currentHealth.lives[i].lifePercentage);
                   
                }
                else
                {
                   // hearts[i].sprite = fullheart;
                   // float percent = (float)currentHealth.lives[i].lifePercentage / (float)currentHealth.lives[i].lifeTotalAmount;
                   // GameLog.LogMessage("Fill Percent=" + percent);
                    //leanTween value fill amount to percent
                    currentSlider = sliders[i];
                    currentHeart = hearts[i];
                    float previousValue = sliders[i].GetSliderValue();
                    GameLog.LogMessage("Previous value=" + previousValue);
                    LeanTween.value(hearts[i].gameObject, previousValue, currentHealth.lives[i].lifePercentage, tweenTime).setEaseLinear().setOnUpdate(setHeartFillAmount).setLoopOnce();
                    // hearts[i].fillAmount = percent;
                }
            } else 
            {

                GameLog.LogMessage("Setup full life");
                hearts[i].sprite = fullheart;
                sliders[i].SetCurrentValue(_currenthealth.lives[i].lifeTotalAmount); 
                //hearts[i].fillAmount = 1;
            }
        }
    }
    private void setHeartFillAmount(float value)
    {
        currentSlider.SetCurrentValue(value);
        GameLog.LogMessage("cURERNT fill Percent=" + currentHeart.fillAmount);
    }
    private void Awake()
    {
        // River.OnPlayerDrowning += LostLife;
        sceneTransitions = FindObjectOfType<SceneTransitions>();
        if (hurtPanel)
            hurtAnim = hurtPanel.GetComponent<Animator>();

       
           

        if (hearts != null)
        {
            if (sliders == null) { 
            sliders = new UISliderBehaviour[hearts.Length];     
            for (int i = 0; i < hearts.Length; i++)
            {
                sliders[i] = hearts[i].gameObject.GetComponentInParent<UISliderBehaviour>();
            }
            }

           

        }
            

    }
    //Changes UI health objects
}
