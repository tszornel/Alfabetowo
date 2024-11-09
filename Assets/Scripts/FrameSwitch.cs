using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FrameSwitch : MonoBehaviour
{
    public GameObject frame1;
    public GameObject frame2;
    private GameObject player;
    private PlayerPlatformerController playerController;
    public CutsceneMode cutScene;
    public CutsceneTimelineBehaviour cutsceneBehaviour;
    public bool playOnce;
    private bool firstEnter;

    public AudioClip frame1Audio;
    public AudioClip frame2Audio;
    public bool movePlayer = false;


    public AudioMixerSnapshot _snapshot;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cutScene = CutsceneMode.Play;
    }
    private void Start()
    {
        playerController = player.GetComponent<PlayerPlatformerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
    void PlayCutscene() {
        switch (cutScene)
        {
            case CutsceneMode.Play:
                StartCutScene();
                break;
            case CutsceneMode.None:
                break;
        }
    }   

    void StartCutScene() {
        if (cutsceneBehaviour)
        {
            cutsceneBehaviour.StartTimeline();
            if (playOnce)
                cutScene = CutsceneMode.None;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            GameLog.LogMessage("Frame Edge collider leaved");
            if (frame1.activeSelf)
            {
                if (playerController.faceRightCheck())
                {
                    GameLog.LogMessage("Frame 1 was active");
                    frame1.SetActive(false);
                    
                    frame2.SetActive(true);
                    
                    if(movePlayer)
                        player.transform.SetParent(frame2.transform, true);
                  //  playerController.SetActiveFrame(frame2);
                    PlayCutscene();
                    PlayAudio(frame2Audio);
                    
                }
            }
            else if (frame1.activeSelf == false)
            {
                if (!playerController.faceRightCheck())
                {
                    GameLog.LogMessage("Frame 1 wasn't active");
                    frame2.SetActive(false);
                    frame1.SetActive(true);

                    if (movePlayer)
                        player.transform.SetParent(frame1.transform,true);
                    PlayCutscene();
                    PlayAudio(frame1Audio);
                }
            }
        }
    }

    public void PlayAudio(AudioClip newAudio)
    {
        if (newAudio) 
        {
              Music.Instance.SwitchMusic(newAudio, _snapshot, 0f);
        }
    }
}
