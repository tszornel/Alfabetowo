using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Webaby.Utils;
public class Moon : MonoBehaviour
{
    Button_Sprite moonButton;
    private void Awake()
    {
        moonButton = transform.GetComponent<Button_Sprite>();
    }
    // Start is called before the first frame update
    void Start()
    {
        moonButton.ClickFunc = () =>
        {
        };
   }
}
