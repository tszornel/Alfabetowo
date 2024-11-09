using System.Collections;
using TMPro;
using UnityEngine;


public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    public TMP_Text Text;
    Transform TextTransform;
    TextMeshProEffect Effect;
    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        if(!TextTransform)
        TextTransform = transform.Find("Text");
        Effect = TextTransform?.GetComponent<TextMeshProEffect>();
    }

    private void SetText(string message)
    {
        if(Text)
            Text.text = message;
    }

    public void ActivateText(string message)
    {
        SetText(message);
        TextTransform.gameObject.SetActive(true);
        Effect.Play();
        StartCoroutine(WaitForText());
        
    }

    IEnumerator WaitForText()
    {
        GameLog.LogMessage("Checking text played");
        yield return new WaitUntil(() => Effect.IsFinished);
        TextTransform.gameObject.SetActive(false);

    }

}
