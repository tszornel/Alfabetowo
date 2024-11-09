using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]    
    Transform HighScoresContainer, HighScoreTemplate;
    [SerializeField]
    AbecadlowoHightScore highScores;
    [SerializeField]
    protected ScrollRect scrollRect;
    [SerializeField]
    protected RectTransform contentPanel;

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        contentPanel.anchoredPosition =
                (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    }

    private void Awake()
    {
        ClearHighScoreDisplay();
        UpdateHighScoreDisplay();
    }


    void ClearHighScoreDisplay()
    {
        GameLog.LogMessage("Clear HighScores");
        int x = 0;

        foreach (Transform child in HighScoresContainer)
        {
            //  GameLog.LogMessage("Widze " + x + " child" + child.name);
            x++;
            if (child == HighScoreTemplate) continue;
            Destroy(child.gameObject);
        }
    }



    void UpdateHighScoreDisplay()
    {

        GameLog.LogMessage("UpdateHighScoreDisplay entered");
        int index = 0;
        RectTransform snapedItem=null;
        foreach (AbecadlowoScore score in highScores.GetHighScores())
        {
           
            GameLog.LogMessage("Add score = "+score);
            index++;
            RectTransform itemSlotRectTransform = Instantiate(HighScoreTemplate, HighScoresContainer).GetComponent<RectTransform>();
      
            itemSlotRectTransform.SetAsLastSibling();
            itemSlotRectTransform.name = "BestScore";
            UI_Score scoreDisplay = itemSlotRectTransform.GetComponent<UI_Score>();
            scoreDisplay.SetScore(index, score);
            scoreDisplay.Show();
            if (index == 1)
                snapedItem = itemSlotRectTransform;
        }
        if(snapedItem)
            SnapTo(snapedItem); 

   

    }


}
