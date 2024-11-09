using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonStartPressed : MonoBehaviour
{
    [SerializeField] ParticleSystem startButtonPressed = null;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startButton();
        }
    }
    public void startButton()
    {
        startButtonPressed.Play();
    }
}
