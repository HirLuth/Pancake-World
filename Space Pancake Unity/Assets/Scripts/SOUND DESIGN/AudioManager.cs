using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Random = System.Random;
using DG.Tweening;


public class AudioManager : MonoBehaviour
{
    public List<Sound> listClip;
    public AudioSource mainAudioSource, main2AudioSource, main3AudioSource;
    public AudioSource secondaryAudioSource;
    public static AudioManager instance;
    public int actualNeutralBackground;
    public int actualNeutralSoundEffect;

    [Header("Volume Sound Effects")] 
    [Range(0,3)] public float playermastervolume;
    [SerializeField] [Range(0,3)] private float masterVolume = 3;
    private int backGroundPlaying;
    private int soundEffectPlaying;
    [Range(6,10)]private int nextCollectibleSound = 6;

    private bool isSFXfinished;


    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SetMenuTheme();
        isSFXfinished = true;
    }
    
    void Update()
    {
        
    }

    public void RailEnter()
    {
        main3AudioSource.Play();
        main3AudioSource.priority = 0;
        main2AudioSource.priority = 128;
                    
        main2AudioSource.DOFade(0f, 1f);
        main3AudioSource.DOFade(1f, 1f);
        Debug.Log("Enter");
        
        
    }

    public void RailExit()
    {
        main2AudioSource.Play();
        main2AudioSource.priority = 0;
        main3AudioSource.priority = 128;
        
        main2AudioSource.DOKill();
        main3AudioSource.DOKill();
        Debug.Log(main2AudioSource.clip.name);
        Debug.Log(main3AudioSource.clip.name);

        main2AudioSource.DOFade(1f, 1f);
        main3AudioSource.DOFade(0f, 1f);
        Debug.Log("Exit");
    }
    public void SetBackGroundSound(int numberIntheList)
    {
        if (main2AudioSource.clip != listClip[numberIntheList].clip || !main2AudioSource.isPlaying)
        {
            main2AudioSource.volume = listClip[numberIntheList].volume * masterVolume * playermastervolume;
            main2AudioSource.pitch = listClip[numberIntheList].pitch;
            main2AudioSource.Stop();
            main2AudioSource.clip = listClip[numberIntheList].clip;
            main2AudioSource.Play();
            backGroundPlaying = numberIntheList;
        }
    }
    
    public void SetBackGroundSound2(int numberIntheList)
    {
        if (main3AudioSource.clip != listClip[numberIntheList].clip || !main3AudioSource.isPlaying)
        {
            main3AudioSource.volume = 0f;
            main3AudioSource.pitch = listClip[numberIntheList].pitch;
            main3AudioSource.Stop();
            main3AudioSource.clip = listClip[numberIntheList].clip;
            main3AudioSource.Play();
        }
    }

    public void SetSoundEffect1(int numberIntheList)
    {
        if (secondaryAudioSource.clip != listClip[numberIntheList].clip || !secondaryAudioSource.isPlaying)
        {
            secondaryAudioSource.volume = listClip[numberIntheList].volume * masterVolume * playermastervolume;
            secondaryAudioSource.pitch = listClip[numberIntheList].pitch;
            secondaryAudioSource.Stop();
            secondaryAudioSource.clip = listClip[numberIntheList].clip;
            secondaryAudioSource.Play();
            soundEffectPlaying = numberIntheList;
        }
    }

    public void PlayOneshotSoundEffect(int numberIntheList)
    {
        if (numberIntheList == 12)
        {
            mainAudioSource.clip = listClip[numberIntheList].clip;
            mainAudioSource.volume = listClip[numberIntheList].volume * masterVolume * playermastervolume;

            StartCoroutine(RunSoundEffect());
        }
        else
        {
            mainAudioSource.PlayOneShot(listClip[numberIntheList].clip,  listClip[numberIntheList].volume * masterVolume * playermastervolume);
        }
    }

    IEnumerator RunSoundEffect()
    {
        mainAudioSource.Play();
        isSFXfinished = false;
        yield return new WaitForSeconds(mainAudioSource.clip.length);
        mainAudioSource.Stop();
        isSFXfinished = true;
    }

    public void BackToNeutralMain()
    {
        if (actualNeutralBackground == 0 && mainAudioSource.isPlaying)
        {
            mainAudioSource.Stop();
        }
        else if (actualNeutralBackground != 0)
        {
            SetBackGroundSound(actualNeutralBackground); 
        }
        
    }

    public void BackToNeutralSoundEffect1()
    {
        if (actualNeutralSoundEffect == 0 && secondaryAudioSource.isPlaying)
        {
           secondaryAudioSource.Stop(); 
        }
        else if (actualNeutralSoundEffect != 0)
        {
            SetSoundEffect1(actualNeutralSoundEffect);
        }
    }

    public void UpdateVolume()
    {
        mainAudioSource.volume = listClip[backGroundPlaying].volume * masterVolume * playermastervolume;
        main2AudioSource.volume = listClip[backGroundPlaying].volume * masterVolume * playermastervolume;
        secondaryAudioSource.volume = listClip[backGroundPlaying].volume * masterVolume * playermastervolume;
    }


    public void SetMenuTheme()
    {
        SetBackGroundSound(0);
        SetSurvivalTheme();
    }
    
    public void SetForestAmbiance()
    {
        SetBackGroundSound(2);
    }

    public void SetSurvivalTheme()
    {
        SetBackGroundSound2(1);
    }

    public void LaunchBirdsSound()
    {
        PlayOneshotSoundEffect(3);
    }

    public void LaunchCollectibleSound()
    {
        PlayOneshotSoundEffect(nextCollectibleSound);
        if (nextCollectibleSound == 10)
        {
            PlayOneshotSoundEffect(11);
            nextCollectibleSound = 6;
        }
        else
        {
            nextCollectibleSound += 1; 
        }
        
    }

    public void LaunchPancakeSoundEffect()
    {
        PlayOneshotSoundEffect(4);
    }

    public void SetSerpeSound()
    {
        SetSoundEffect1(5);
    }

    public void LaunchOutOfSerpeSoundEffect()
    {
        PlayOneshotSoundEffect(14);
    }

    public void LaunchSucreSoundEffect()
    {
        if (isSFXfinished)
        {
            PlayOneshotSoundEffect(12);
        }
    }

    public void SetWalkingSound()
    {
        SetSoundEffect1(13);
    }
    
}
