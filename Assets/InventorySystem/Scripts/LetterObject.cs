using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[CreateAssetMenu(fileName = "New Letter Object", menuName = "Inventory System/Items/Letter")]
public class LetterObject : ItemObject
{
    public string text;
    private void Awake()
    {
        type = ItemObjectType.Letter;
    }
    public void SetText(string _text) {
        text = _text;
    }
    public string GetText()
    {
        return text;
    }
    public override string ToString()
    {
        return "text:"+text+" type:"+type+ " "+base.ToString();
    }
}
