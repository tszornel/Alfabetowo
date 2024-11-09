using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;
public class meshTextAppearing : MonoBehaviour
{
    public static List<string> listStrLineElements = new List<string>() { };
    public float timeBeetwenLines = 1f; 
    public float timeTemp = 0.0f;
    public string word = "";
    public int len = 0;
    public float delay;
    public static TMP_Text textMeshPro;
    public TMP_Text textMesh;
    public int index;
    private String test1;
    private String test2;
    private static Queue<string> myQueueList;
    public int characterSkip = 10;
    TMP_TextInfo textInfo;
    // Start is called before the first frame update
    [ExecuteInEditMode]
    void Awake()
    {
        test1 = @"Działo się to dawno temu w krainie Abecadłowa, gdzie żyły dzieci razem ze swoimi literkami,  które były dla nich dobrymi duszkami. Litery wypełniały ich książki i uczyły  ich wszystkiego co potrzebne.
            Dzieci spędzały tak beztrosko cały czas na nauce, pracy i zabawie. Było tak do czasu gdy zły władca Sersenbaj -pan wszystkich wiatrów, zapragnął władzy nad całym światem.
            Postanowił przejąć na wyłączność wiedzę całego świata i zapanować nad nim.  ";
        textMeshPro = GetComponent<TMP_Text>();
        textInfo = textMeshPro.textInfo;
        String text = textMeshPro.GetParsedText();
        String text2 = textMeshPro.text;
        GameLog.LogMessage(text);
        GameLog.LogMessage("Pomiedzy ----------------------------------------------");
        GameLog.LogMessage(text2);
        listStrLineElements = test1.Split(',').ToList();
        myQueueList = new Queue<string>(listStrLineElements);
        textMeshPro.text = "";
      //  len = listStrLineElements.Count;
    }
    private IEnumerator FadeInLetters(TMP_CharacterInfo info)
    {
        for (int i = 0; i <= 255; i += characterSkip)
        {
            if (!info.isVisible) continue;
            int meshIndex = textInfo.characterInfo[info.index].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[info.index].vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;
            vertexColors[vertexIndex + 0].a = (byte)i;
            vertexColors[vertexIndex + 1].a = (byte)i;
            vertexColors[vertexIndex + 2].a = (byte)i;
            vertexColors[vertexIndex + 3].a = (byte)i;
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            yield return new WaitForSeconds(0.01f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (myQueueList.Count == 0)
        {
             return;
        }
        if (Time.time >= timeBeetwenLines)
            {
                test2 += myQueueList.Dequeue();
                GameLog.LogMessage("po dodaniu lini=" + test2);
            textMeshPro.color = new Color(120, 120, 120, 0);
            textMeshPro.text = test2;
            TMP_CharacterInfo[] info = textInfo.characterInfo;
            for (int i = 0; i < info.Length; i++)
            {
                StartCoroutine(FadeInLetters(info[i]));
            }
            // textMeshPro.SetText(test2);
            //textMeshPro.SetAllDirty();
            timeBeetwenLines = Time.time+ delay;
                GameLog.LogMessage("Text mesh pro text" + textMeshPro.text+ "delay "+delay+" timeBeertwenLines:"+ timeBeetwenLines);
               // Debug.Break();
            }
    }
}
