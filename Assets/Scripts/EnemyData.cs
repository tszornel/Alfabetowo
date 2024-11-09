using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;


[CreateAssetMenu(fileName = "_EnemyData", menuName = "Webaby/EnemyData")]
public class EnemyData : ScriptableObject
{
    StringBuilder sb;
    [Header("Main Info")]
    public string enemyName;
    public TMP_SpriteAsset spriteAsset;
    public string image;

    public override string ToString()
    {
        if(sb==null)
            sb = new StringBuilder();
        if (enemyName != null)
            sb.Append("EnemyName="+enemyName);
        if (spriteAsset != null)
            sb.Append("SpriteAsset="+spriteAsset);
        if (image != null)
            sb.Append("Obrazek=" + image);
        return sb.ToString();
    }
}