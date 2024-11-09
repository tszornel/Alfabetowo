using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "_HighScore", menuName = "Webaby/Unit/AbecadlowoHighScore")]
public class AbecadlowoHightScore : ScriptableObject
{
 
    public List<AbecadlowoScore> highScores = new List<AbecadlowoScore>();
    public void AddHighScore(AbecadlowoScore score) {
        highScores.Add(score);
        highScores.Sort();
        highScores.Reverse();
        foreach (var item in highScores)
        {
            GameLog.LogMessage("Score:" + item);

        }
    }

    public void SortScores() => highScores.Sort();
    public IEnumerable<AbecadlowoScore> GetHighScores()
    {
        highScores = highScores.AsEnumerable<AbecadlowoScore>().Take(10).ToList<AbecadlowoScore>();
        return Enumerable.Take<AbecadlowoScore>(highScores.AsEnumerable(), 10);
    }
}
