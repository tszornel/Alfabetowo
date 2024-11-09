using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class swipe_menu : MonoBehaviour
{
    float[] pos;
    //float distance;
    public GameObject scrollbar;
    float scroll_pos = 0;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
            GameLog.LogMessage("set pos i" + i + "cnew pos" + pos[i]);
        }
        if (Input.GetMouseButton(0))
        {
            GameLog.LogMessage("Mouse button get="+ scrollbar.GetComponent<Scrollbar>().value);
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else {
           // GameLog.LogMessage("else");
            for (int i = 0; i < pos.Length; i++)
            {
              //  Debu
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2)) {
                    GameLog.LogMessage("set bigger"+ transform.GetChild(i).name);
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                    for (int a = 0; a < pos.Length; a++)
                    {
                        if (a != i) {
                            GameLog.LogMessage("set smaller"+ transform.GetChild(a).name);
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                    }
                }
            }
        }
    }
}
