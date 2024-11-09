using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Music : MonoBehaviour
{
    public static Music Instance;
    public AudioMixerGroup[] mixerGroups;
    public AudioMixerGroup musicGroup;
    public bool continueInTheNextScene;
    public AudioClip track1AudioClip;
    public AudioSource track1, track2;
    // public AudioClip deafultAmbience;
    private bool isPlayingTrack1;
    public AudioMixer mixer;
    // private bool mutedSound;
    string exposedParameter = "musicVolume";
    float duration = 0.5f;
    float durationSwitch = 0.5f;
    float currentMusicVolume;
    public AudioMixerSnapshot pauseMusic;
    /*  public AudioMixerSnapshot levelMusic;
      public AudioMixerSnapshot pitchMusic;*/
    private AudioMixerSnapshot current;
    public float TimeWarpingFactor;
    public float TransitionSeconds;
  //  private int numLocks = 0;
    public AudioClip uiClickSound;
    public AudioClip[] uiClips;
    public AudioClip soundProducedInLeanTween;
    public AudioMixerSnapshot musicSnapshot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (continueInTheNextScene)
                DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Set music volume to slider values
        InitializeSoundVolume();
        mixerGroups = mixer.FindMatchingGroups("Master/");
        uiClickSound = AddVibrato();
        soundProducedInLeanTween = soundFirstLeanTween();

    }

    public void ShotSound(AudioClip ac) {


        track2.PlayOneShot(ac);
      
    }

    IEnumerator PauseMusic(AudioSource toPause, AudioMixerSnapshot current, float time)
    {
        yield return new WaitForSeconds(time);

        if (this.current == current)
        {
            toPause.Pause();
        }
    }

    private void Pause()
    {
        Time.timeScale = TimeWarpingFactor;
        current = pauseMusic;
        track1.UnPause();
        current.TransitionTo(TransitionSeconds * TimeWarpingFactor);
        StartCoroutine(RepauseLater(TransitionSeconds * TimeWarpingFactor));
        StartCoroutine(PauseMusic(track1, current, TransitionSeconds * TimeWarpingFactor));
    }
    IEnumerator RepauseLater(float time)
    {
        yield return new WaitForSeconds(time);

        if (Time.timeScale <= TimeWarpingFactor) Time.timeScale = 0;
    }




    public void PlayAudio(AudioClip newAudio)
    {
        if (newAudio)
        {
            SwitchMusic(newAudio,musicSnapshot,0);
        }
    }
    private void InitializeSoundVolume()
    {
        float defaultAllVolume = 0.5f;
        defaultAllVolume = PlayerPrefs.GetFloat("allVolume", defaultAllVolume);
        ChangeVolume("allVolume", defaultAllVolume);
        float defaultMusicVolume = 0.5f;
        defaultMusicVolume = PlayerPrefs.GetFloat("musicVolume", defaultMusicVolume);
        ChangeVolume("musicVolume", defaultMusicVolume);
        float defaultEnvironmentSoundsVolume = 0.5f;
        defaultEnvironmentSoundsVolume = PlayerPrefs.GetFloat("allEnvironmentSoundsVolume", defaultEnvironmentSoundsVolume);
        ChangeVolume("environmentSoundsVolume", defaultEnvironmentSoundsVolume);
        float defaultPlayerSFXVolume = 0.5f;
        defaultPlayerSFXVolume = PlayerPrefs.GetFloat("playerSFXVolume", defaultPlayerSFXVolume);
        ChangeVolume("playerSFXVolume", defaultPlayerSFXVolume);
        float defaultDialogVolume = 0.5f;
        defaultDialogVolume = PlayerPrefs.GetFloat("dialogVolume", defaultDialogVolume);
        ChangeVolume("dialogVolume", defaultDialogVolume);

    }

 

        private void Start()
        {
        GameLog.LogMessage("Start Music entered");
        // track1 = gameObject.AddComponent<AudioSource>();
        //track2 = gameObject.AddComponent<AudioSource>();

        /*
        if (!musicGroup)
            musicGroup = Array.Find(mixerGroups, match: mixerGroup => mixerGroup.name == "Music");
        if (musicGroup)
        {
            GameLog.LogMessage("Found music group");
            track1.outputAudioMixerGroup = musicGroup;
            track2.outputAudioMixerGroup = musicGroup;
        }
        else
        {
            GameLog.LogMessage("Not Found music group");
        }
        */


        // track1.clip = track1AudioClip;
        // track1.Play();

        PlayAudio(track1AudioClip);

        isPlayingTrack1 = true;
        GameLog.LogMessage("Start Music left");
    }

    

    public float VolumeMusicDown(string exposedParameterName)
    {
        float musicVolume;
        mixer.GetFloat(exposedParameter, out musicVolume);
        StartCoroutine(FadeMixerGroup.StartFade(mixer, exposedParameterName, duration, -80f));
        return musicVolume;
    }

    public float VolumeMusicDown(SliderObject slider)
    {
        float volume;
        mixer.GetFloat(slider.key, out volume);
        volume = Mathf.Pow(10, volume / 20);
        float previousValue = slider.SliderBehaviour.value;
        slider.SliderBehaviour.value = 0;
        // StartCoroutine(FadeMixerGroup.StartFade(mixer, slider.key, duration, -80f));


        return previousValue;
    }

    public void VolumeMusicup(SliderObject slider, float value)
    {
        slider.SliderBehaviour.value = value;
        // StartCoroutine(FadeMixerGroup.StartFade(mixer, slider.key, duration, value));
    }

    public void ChangeVolume(string exposedParameterName, float value)
    {
        // mixer.SetFloat(exposedParameterName, Mathf.Log10(value) * 30);
        StartCoroutine(FadeMixerGroup.StartFade(mixer, exposedParameterName, duration, value));
        if (value == 0)
            mixer.SetFloat(exposedParameterName, -80f);

    }

    public bool ToggleMuteVolumeImediatelly(SliderObject s)
    {
        GameLog.LogMessage("Mute/unmute Volume" + currentMusicVolume);
        if (!s.muted)
        {

            GameLog.LogMessage("Mute Volume" + currentMusicVolume);
            StartCoroutine(FadeMixerGroup.StartFade(mixer, s.key, 0.5f, -80f));

        }
        else
            StartCoroutine(FadeMixerGroup.StartFade(mixer, s.key, 0.5f, 20f));

        s.muted = !s.muted;
        return s.muted;
    }

    public void VolumeMusicUp(string exposedParameterName, float value)
    {
        GameLog.LogMessage("Volume music up to: " + value);
        StartCoroutine(FadeMixerGroup.StartFade(mixer, exposedParameterName, duration, value));

    }

    
    public void SwitchTrack(AudioClip newClip,AudioMixerSnapshot _snapshot, float timeToReach)
    {

        // StartCoroutine(SwitchTrackRoutine(newClip));
        GameLog.LogMessage("switch track entered");
        float musicVolume;
        mixer.GetFloat(exposedParameter, out musicVolume);
        /*if (isPlayingTrack1)
        {*/
            GameLog.LogMessage("track 1 was playing ");
            track1.Stop();
            isPlayingTrack1 = false;
            track1.clip = newClip;
            GameLog.LogMessage("track 1  play again ");
            if (_snapshot != null)
            {

                current = _snapshot;
                current.TransitionTo(0.1f);
            }
           
            
            track1.Play();
            isPlayingTrack1 = true;

       // }
        GameLog.LogMessage("switch track left");


    }
    /// <summary>
    /// Stops to play Music from track1
    /// </summary>
    public void StopToPlayMusic()
    {
        GameLog.LogMessage("StopToPlayMusic Entered");
        track1.Stop();
        GameLog.LogMessage("StopToPlayMusic Left");
    }


/*private void OnGUI()
    {
        if (GUI.Button(new Rect(900, (Screen.height) - 25, 150, 20), "PLAY SOUND"))
        {


           // http://leanaudioplay.dentedpixel.com/?d=a:fvb:8,0,0.003005181,0,0,0.01507768,0.002227979,0,0,8~8,8.130963E-06,0.06526042,0,-1,0.0007692695,2.449077,9.078861,9.078861,0.01541314,0.9343268,-40,-40,0.05169491,0.03835937,-0.08621139,-0.08621139,8~0.1,0,0,~44100
            AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0.2f, 1f, 0.213005181f, -1f), new Keyframe(1f, 1f, -1f, 0f), new Keyframe(3f, 1f, -1f, 0f), new Keyframe(4f, 1f, -1f, 0f), new Keyframe(2f, 2f, -1f, 0f), new Keyframe(2f, 3f, -1f, 0f));
            AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0.2f, 1.03f, 2f, 1f), new Keyframe(1f, 1.73f, 0.5f, 0.3f), new Keyframe(3f, 1.03f, 0.1f, 0.2f), new Keyframe(4f, -17.26f, 0.5f, 0.6f), new Keyframe(2f, 1.75f, 0.4f, 0.3f), new Keyframe(4f, 1.73f, 0.1f, 0.6f));
           // 0: (1.03), 1: (1.73), 2: (-17.26), 3: (1.75), 4: (1.00), 5: (1.23), 6: (0.90), 7: (0.41), 8: (0.53), 9: (1.18), 10: (1.00), 11: (2.00), 12: (1.00), 13: (1.00), 14: (1.00), 15: (1.29), 16: (0.44), 17: (1.00)
            AudioClip boomAudioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve);

            //ShotSound(boomAudioClip);   

            LeanAudio.play(boomAudioClip, transform.position, 30 * 0.2f);
        }
    }*/



            

    public AudioClip AddVibrato()
    {
        AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));
        AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));
        AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato(new Vector3[] { new Vector3(0.32f, 0.3f, 0f) }).setFrequency(12100));
        return audioClip;
    }

    public AudioClip soundFirstLeanTween() {
        AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));
        AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, 0.3f, 1f, 0f), new Keyframe(1f, 0.003f, 1f, 0f));
        AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato(new Vector3[] { new Vector3(0.52f, 0f, 0f) }));
        return audioClip;
    }

    IEnumerator SwitchTrackRoutine(AudioClip newClip)
    {
        GameLog.LogMessage("switch track entered");
        float musicVolume;
        mixer.GetFloat(exposedParameter, out musicVolume);
        if (isPlayingTrack1)
        {
            GameLog.LogMessage("track 1 was playing ");
            StartCoroutine(FadeMixerGroup.StartFade(mixer, exposedParameter, durationSwitch, 0));
            //  yield return new WaitForSeconds(durationSwitch);
            track1.Stop();
            isPlayingTrack1 = false;
            track1.clip = newClip;
            GameLog.LogMessage("track 1 was playing ");
            track1.Play();
            StartCoroutine(FadeMixerGroup.StartFade(mixer, exposedParameter, durationSwitch, musicVolume));
            isPlayingTrack1 = true;
        }

        GameLog.LogMessage("switch track left");
        yield break;
    }

    public void SwitchMusic(AudioClip clip, AudioMixerSnapshot _snapshot,float timeToReach)
    {
        if (clip) { 
            GameLog.LogMessage("switch to audio clip: " + clip.name);
            float previousMusicVolume = VolumeMusicDown(SlidersUI.Instance.sliderDictionary["musicVolume"]);
            SwitchTrack(clip, _snapshot, timeToReach);
            VolumeMusicup(SlidersUI.Instance.sliderDictionary["musicVolume"], previousMusicVolume);
        }
        else
        {

            StopToPlayMusic();

        }
    }
}
