using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data_SoundEffects_", menuName = "Webaby/Unit/SoundEffects Data", order = 1)]
public class SoundEffectsScriptableObject : ScriptableObject
{
    // Start is called before the first frame update

        [Header("Sound Effects ")]
        public AudioClip[] audioClipsAttackHand;
        public AudioClip[] audioClipsAttackSword;
        public AudioClip[] audioClipsGetHit;
        public AudioClip[] audioClipsDeath;
        public AudioClip[] audioClipsWalkingOnGround;
        public AudioClip[] audioClipsWalkingOnWater;
        public AudioClip[] audioClipsPickupLetters;
        public AudioClip[] audioClipsPickupItems;
        public AudioClip[] audioClipsFightSword;
        public AudioClip[] audioClipsFightWithHands;
        public AudioClip[] audioClipsFullBag;
        public AudioClip[] audioClipsComeLater;
        public AudioClip[] audioClipsNotWorking;
        public AudioClip[] audioClipsJump;
        private AudioClip[] audioClipsAttack;
        public AudioClip[] spiderClip;
        public AudioClip[] switchFrameClip;
        public AudioClip[] introPageClips;
        public AudioClip[] drowningClips;
        public AudioClip[] successClips;
        public AudioClip[] randomSound;
        public AudioClip[] repaireClips;
        public AudioClip[] UILighterPanel;
        public AudioClip successPanelClip;
        public AudioClip leanTweenSound;
        public AudioClip vibratoSound;
        public AudioClip soundFirst;
        public AudioClip[] audioClipsPirateSaber;
        public AudioClip[] audioClipsSwatter;
        public AudioClip[] audioClipsStick;
        public AudioClip[] audioClipsBow;
        public AudioClip[] teleportSoundClip;
        public AudioClip[] noMoney;
        public AudioClip[] clockTicking;
        public AudioClip[] clockBack;
        public AudioClip[] audioClipsLash;

    public AudioClip GetSuccessPanelClip()
    {
        return successPanelClip;
    }
    public AudioClip GetClockBackClip()
    {
        return SelectRandomAudioClip(clockBack);
    }

    public AudioClip GetClockTickingClip()
    {
        return SelectRandomAudioClip(clockTicking);
    }


    public AudioClip GetNoMoneyClip()
    {
        return SelectRandomAudioClip(noMoney);
    }

    public AudioClip GetRandomSoundClip()
    {
        return SelectRandomAudioClip(randomSound);
    }

    public AudioClip GetLighterPanelSound()
    {
        return SelectRandomAudioClip(UILighterPanel);
    }


    public AudioClip GetTeleportSound() {

        return SelectRandomAudioClip(teleportSoundClip);



    }


    public AudioClip GetVibrato()
    {
        if (!vibratoSound) 
        { 
            AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));
            AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, 0.003f, 0f, 0f), new Keyframe(1f, 0.003f, 0f, 0f));
            AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato(new Vector3[] { new Vector3(0.32f, 0.3f, 0f) }).setFrequency(12100));
            vibratoSound = audioClip;
        }
        return vibratoSound;
    }

    public AudioClip GetsoundFirstLeanTween()
    {
        if (!soundFirst)
        {
            AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, -1f), new Keyframe(1f, 0f, -1f, 0f));
            AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, 0.3f, 1f, 0f), new Keyframe(1f, 0.003f, 1f, 0f));
            AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato(new Vector3[] { new Vector3(0.52f, 0f, 0f) }));
            soundFirst = audioClip; 


        }
        return soundFirst;
    }


    public AudioClip GetSuccessClip()
    {
        AudioClip audioClip = leanTweenSound;
        if (audioClip==null)
        {
            AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 0.005464481f, 1.83897f, 0f), new Keyframe(0.1114856f, 2.281785f, 0f, 0f), new Keyframe(0.2482903f, 2.271654f, 0f, 0f), new Keyframe(0.3f, 0.01670286f, 0f, 0f));
            AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, 0.00136725f, 0f, 0f), new Keyframe(0.1482391f, 0.005405405f, 0f, 0f), new Keyframe(0.2650336f, 0.002480127f, 0f, 0f));

             audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato(new Vector3[] { new Vector3(0.2f, 0.5f, 0f) }).setWaveNoise().setWaveNoiseScale(1000));
        }
       
        //
        // return SelectRandomAudioClip(rechotkaSuccess);

        return audioClip;
    }

    /*void playSwish()
    {
        AnimationCurve volumeCurve = new AnimationCurve(new Keyframe(0f, 0.005464481f, 1.83897f, 0f), new Keyframe(0.1114856f, 2.281785f, 0f, 0f), new Keyframe(0.2482903f, 2.271654f, 0f, 0f), new Keyframe(0.3f, 0.01670286f, 0f, 0f));
        AnimationCurve frequencyCurve = new AnimationCurve(new Keyframe(0f, 0.00136725f, 0f, 0f), new Keyframe(0.1482391f, 0.005405405f, 0f, 0f), new Keyframe(0.2650336f, 0.002480127f, 0f, 0f));

        AudioClip audioClip = LeanAudio.createAudio(volumeCurve, frequencyCurve, LeanAudio.options().setVibrato(new Vector3[] { new Vector3(0.2f, 0.5f, 0f) }).setWaveNoise().setWaveNoiseScale(1000));

        LeanAudio.play(audioClip); //a:fvb:8,,.00136725,,,.1482391,.005405405,,,.2650336,.002480127,,,8~8,,.005464481,1.83897,,.1114856,2.281785,,,.2482903,2.271654,,,.3,.01670286,,,8~.2,.5,,~~0~~3,1000,1
    }*/


 
    public AudioClip GetDrowningClip()
    {
        return SelectRandomAudioClip(drowningClips);
    }

    public AudioClip GetSwitchClip()
    {
        return SelectRandomAudioClip(switchFrameClip);
    }
    public AudioClip GetJumpingClip()
    {
        return SelectRandomAudioClip(audioClipsJump);
    }

    public AudioClip GetSpiderClip()
    {
        return SelectRandomAudioClip(spiderClip);
    }

    public AudioClip GetIntroPageClip(int pageId)
    {
        AudioClip introPageClip = introPageClips[pageId];
        return introPageClip;
    }

    public void SetIntroPageClips(AudioClip[] _introClips)
    {
        introPageClips = _introClips;
     }


    public AudioClip GetSuperAttackClip(Item weapon)
    {
        if (weapon != null)
        {
            switch (weapon.Name)
            {
                case "PirateSaber":
                    audioClipsAttack = audioClipsPirateSaber;
                    break;
                case "Swatter":
                    audioClipsAttack = audioClipsSwatter;
                    break;
                case "Lash":
                    audioClipsAttack = audioClipsStick;
                    break;
                case "Stick":
                    audioClipsAttack = audioClipsStick;
                    break;
                case "Bow":
                    audioClipsAttack = audioClipsBow;
                    break;
                case "Sword":
                case "Sting-Sword":
                    audioClipsAttack = audioClipsAttackSword;
                    break;
                default:
                    audioClipsAttack = audioClipsAttackHand;
                    break;
            }
        }
        else
            audioClipsAttack = audioClipsAttackHand;

        return SelectRandomAudioClip(audioClipsAttack);
    }

    public AudioClip GetAttackClip(Item weapon)
    {
        if (weapon != null) { 
            switch (weapon.Name)
            {
                case "PirateSaber":
                    audioClipsAttack = audioClipsPirateSaber;
                    break;
                case "Swatter":
                    audioClipsAttack = audioClipsSwatter;
                    break;
                case "Lash":
                    audioClipsAttack = audioClipsLash;
                    break;
                case "Stick":
                    audioClipsAttack = audioClipsStick;
                    break;
                case "Bow":
                    audioClipsAttack = audioClipsBow;
                    break;
                case "Sword":
                case "Sting-Sword":
                    audioClipsAttack = audioClipsAttackSword;
                    break;
                default:
                    audioClipsAttack = audioClipsAttackHand;
                    break;
            }
        }
        else
            audioClipsAttack = audioClipsAttackHand;

        return SelectRandomAudioClip(audioClipsAttack);
    }

    public AudioClip GetNotWorkingClip()
        {
            return SelectRandomAudioClip(audioClipsNotWorking);
        }
        public AudioClip GetComeLaterClip()
        {
            return SelectRandomAudioClip(audioClipsComeLater);
        }

        public AudioClip GetFullBagClip()
        {
            return SelectRandomAudioClip(audioClipsFullBag);
        }

        public AudioClip GetFindWithSwordClip()
        {
            return SelectRandomAudioClip(audioClipsFightSword);
        }

        public AudioClip GetWalkingOnGroundClip()
        {
            return SelectRandomAudioClip(audioClipsWalkingOnGround);
        }

        public AudioClip GetWalkingOnWaterClip()
        {
            return SelectRandomAudioClip(audioClipsWalkingOnWater);
        }

        public AudioClip GetLetterPickupClip()
        {
            return SelectRandomAudioClip(audioClipsPickupLetters);
        }

        public AudioClip GetItemsPickupClip()
        {
            return SelectRandomAudioClip(audioClipsPickupItems);
        }

        public AudioClip GetHitClip()
        {
            return SelectRandomAudioClip(audioClipsGetHit);
        }

        public AudioClip GetDeathClip()
        {
            return SelectRandomAudioClip(audioClipsDeath);
        }

        AudioClip SelectRandomAudioClip(AudioClip[] audioClipArray)
        {

            if (audioClipArray.Length <= 0)
            {
                return null;
            }

            int randomClipInt = Random.Range(0, audioClipArray.Length);
            return audioClipArray[randomClipInt];
        }

     
}


