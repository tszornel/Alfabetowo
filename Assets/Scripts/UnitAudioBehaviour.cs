using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GroundType
{
    NormalGround,
    Stones,
    Leafs,
    Water,
    Home,
    None
}
public class UnitAudioBehaviour : MonoBehaviour
    {

        [Header("Data")]
        public SoundEffectsScriptableObject sfxData;

        [Header("Component Reference")]
        public AudioSource audioSource;

        [Header("SFX Volume Override")]
        public float sfxDeathVolume=1;

        public AudioClip audioTrackAfterDeadth;

    private void Awake()
    {
        if (!audioSource) {

            audioSource = GetComponent<AudioSource>();
        }
    }



    public void PlayTeleportSound()
    {
        PlayAudioClip(sfxData.GetTeleportSound());
    }
    public void PlayLighterPanelSound()
    {
        PlayAudioClip(sfxData.GetLighterPanelSound());
    }

    public void PlaySpiderWalking()
    {
        PlayAudioClip(sfxData.GetSpiderClip());
    }


    public void PlaySuccess()
    {
        PlayAudioClip(sfxData.GetSuccessClip());
    }

    public void PlayVibrato()
    {
        PlayAudioClip(sfxData.GetVibrato());
    }
    public void PlayDrowning()
    {
        PlayAudioClip(sfxData.GetDrowningClip());
    }

    public void PlaySwitchFrameSound()
    {
        GameLog.LogMessage("PlaySwitchFrameSound");
        PlayAudioClip(sfxData.GetSwitchClip());
    }

    public void PlaySFXDoeasNotFit()
        {
            PlayAudioClip(sfxData.GetNotWorkingClip());
        }

 
    public void PlaySFXComeBackLater()
        {
            PlayAudioClip(sfxData.GetComeLaterClip());
        }

    public void PlayJumpSound()
    {
        PlayAudioClip(sfxData.GetJumpingClip());
    }



    public void PlayIntroPageMusic(int index)
    {
        AudioClip pageClip = sfxData.GetIntroPageClip(index);
        GameLog.LogMessage("Page clip " + pageClip.name);
        PlayIntroAudioClip(pageClip);
    }

    public void PlayWalking(GroundType _groundType,Transform gamObject)
    {
        switch (_groundType)
        {
            case GroundType.NormalGround:
                PlaySFXWalkingOnGround();
                break;
            case GroundType.Stones:
                PlaySFXWalkingOnGround();
                break;
            case GroundType.Leafs:
                PlaySFXWalkingOnGround();
                break;
            case GroundType.Water:
                ObjectPoolerGeneric.Instance.SpawnFromPool("DrawningEffect", gamObject.transform.position, Quaternion.identity, null);
                PlaySFXWalkingOnWater();
                break;
            case GroundType.Home:
                PlaySFXWalkingOnGround();
                break;
            case GroundType.None:
                break;
            default:
                break;
        }
    }

        public void PlayLanding(GroundType _groundType)
        {
            switch (_groundType)
            {
                case GroundType.NormalGround:
                    PlaySFXWalkingOnGround();
                    break;
                case GroundType.Stones:
                    PlaySFXWalkingOnGround();
                    break;
                case GroundType.Leafs:
                    PlaySFXWalkingOnGround();
                    break;
                case GroundType.Water:
                    PlaySFXWalkingOnWater();
                    break;
                case GroundType.Home:
                    PlaySFXWalkingOnGround();
                    break;
                case GroundType.None:
                    break;
                default:
                    break;
            }



        }

    public void PlayUIPanelBounce()
    {
        PlayAudioClip(sfxData.GetSuccessPanelClip());
    }



    public void PlayAttack(Item item)
        {
             PlayAudioClip(sfxData.GetAttackClip(item));
        }

    public void PlayRandomSound()
    {
        PlayAudioClip(sfxData.GetRandomSoundClip());

    }

    public void PlaySFXWalkingOnGround()
        {
            PlayAudioClip(sfxData.GetWalkingOnGroundClip());
        }

        public void PlaySFXWalkingOnWater()
        {
            PlayAudioClip(sfxData.GetWalkingOnWaterClip());
        }

        public void PlaySFXPickupItem()
        {
            PlayAudioClip(sfxData.GetItemsPickupClip());
        }

        public void PlaySFXGetHit()
        {
            PlayAudioClip(sfxData.GetHitClip());
        }

        public void PlaySFXDeath()
        {
            GameLog.LogMessage("Play death Sound");
            PlayAudioClipStandard(sfxData.GetDeathClip());
        /* PlayAudioClip(sfxData.GetDeathClip());
        SetAudioSourceVolume(sfxDeathVolume);
       
        if (audioTrackAfterDeadth)
            Music.Instance.SwitchTrack(audioTrackAfterDeadth,null,0f);*/
            
        }

    public AudioClip GetDeathClip()
    {
       
        return sfxData.GetDeathClip();
        /* PlayAudioClip(sfxData.GetDeathClip());
        SetAudioSourceVolume(sfxDeathVolume);
       
        if (audioTrackAfterDeadth)
            Music.Instance.SwitchTrack(audioTrackAfterDeadth,null,0f);*/

    }


    public void PlaySFXDeath(AudioClip clip)
    {
        GameLog.LogMessage("Play death Sound");
        PlayAudioClip(clip);
        /* PlayAudioClip(sfxData.GetDeathClip());
        SetAudioSourceVolume(sfxDeathVolume);
       
        if (audioTrackAfterDeadth)
            Music.Instance.SwitchTrack(audioTrackAfterDeadth,null,0f);*/

    }

    public void PlayClockTicking()
    {
        GameLog.LogMessage("Play clock ticking Sound");
        PlayAudio(sfxData.GetClockTickingClip(),true);
        /* PlayAudioClip(sfxData.GetDeathClip());
        SetAudioSourceVolume(sfxDeathVolume);
       
        if (audioTrackAfterDeadth)
            Music.Instance.SwitchTrack(audioTrackAfterDeadth,null,0f);*/

    }

    public void PlayClockBack()
    {
        GameLog.LogMessage("Play clock back Sound");
        PlayAudioClip(sfxData.GetClockBackClip());
        /* PlayAudioClip(sfxData.GetDeathClip());
        SetAudioSourceVolume(sfxDeathVolume);
       
        if (audioTrackAfterDeadth)
            Music.Instance.SwitchTrack(audioTrackAfterDeadth,null,0f);*/

    }

    public void PlayNoMoney(AudioClip clip)
    {
        GameLog.LogMessage("Play no money Sound");
        PlayAudioClip(clip);
        /* PlayAudioClip(sfxData.GetDeathClip());
        SetAudioSourceVolume(sfxDeathVolume);
       
        if (audioTrackAfterDeadth)
            Music.Instance.SwitchTrack(audioTrackAfterDeadth,null,0f);*/

    }

    void SetAudioSourceVolume(float newVolume)
        {
            audioSource.volume = newVolume;
        }

        void PlayAudioClip(AudioClip selectedAudioClip)
        {

        if(audioSource == null)
        {
            GameLog.LogMessage("Missing oudio source on object", this);
            return;

        }
           

        if (selectedAudioClip && audioSource.isActiveAndEnabled)
        {

          //  GameLog.LogMessage("AudioBehaviour source  active play " + selectedAudioClip.name, transform);
            audioSource.PlayOneShot(selectedAudioClip);
        }
            
        else if (!audioSource.isActiveAndEnabled) { 
            GameLog.LogError("AudioBehaviour source not active on "+transform.name+"not playing audio clip"+ selectedAudioClip,transform);  
        
        }
        }



    void PlayAudio(AudioClip selectedAudioClip,bool loop)
    {
        if (selectedAudioClip && audioSource.isActiveAndEnabled)
        {

            GameLog.LogMessage("AudioBehaviour source  active play " + selectedAudioClip.name, transform);
            audioSource.clip = (selectedAudioClip);
            if (loop)
                audioSource.loop = true;
            audioSource.Play(); 
        }

        else if (!audioSource.isActiveAndEnabled)
        {
            GameLog.LogError("AudioBehaviour source not active on " + transform.name + "not playing audio clip" + selectedAudioClip, transform);

        }
    }
    IEnumerator PlaySoundRoutine(AudioClip selectedAudioClip) {

        GameLog.LogMessage("Courutine started to play +" + selectedAudioClip);
        audioSource.PlayOneShot(selectedAudioClip);
        yield return null;

    }


   

    void PlayAudioClipStandard(AudioClip selectedAudioClip)
    {
        if (selectedAudioClip && audioSource.isActiveAndEnabled)
        {
            SetAudioSourceVolume(1);
            GameLog.LogMessage("AudioBehaviour source  active play " + selectedAudioClip.name, transform);
            audioSource.clip = selectedAudioClip;
            audioSource.Play();
            GameLog.LogMessage("AudioBehaviour source  active play isPlaying"+audioSource.isPlaying, transform);    
            StartCoroutine(PlaySoundRoutine(selectedAudioClip));        
            
           
        }

        else if (!audioSource.isActiveAndEnabled)
        {
            GameLog.LogError("AudioBehaviour source not active on " + transform.name + "not playing audio clip" + selectedAudioClip, transform);

        }
    }

    void PlayIntroAudioClip(AudioClip selectedAudioClip)
    {
        Music.Instance.SwitchMusic(selectedAudioClip,null,0f);
       // audioSource.clip = selectedAudioClip;
       // audioSource.Play();
    }

    public void PlaySoundLetter(AudioClip letterSound)
    {
        audioSource.PlayOneShot(letterSound);
    }

    public void StopAudio()
    {
        audioSource.Stop();     
    }

    public void PlaySuperAttack(Item item)
    {
       
            PlayAudioClip(sfxData.GetAttackClip(item));
        
    }
}


