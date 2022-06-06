using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Random = System.Random;
using DG.Tweening;
using TMPro.EditorUtilities;


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

    public AnimationCurve curve, curveNeg;

    private float increment, graph, graph2;
    private bool canRunCurve, activateCurve, crossFadeFinished, crossFade2Finished = true;
    public bool x, x2;

    public bool isCompleted;


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
    }
    
    void Update()
    {
        if (CameraMovements.Instance != null)
        {
            if (CameraMovements.Instance.isOnRail)
            {
                if (!x)
                {
                    main3AudioSource.priority = 0;
                    main2AudioSource.priority = 128;
                    
                    main2AudioSource.DOFade(0f, 1f);
                    main3AudioSource.DOFade(1f, 1f);
                    
                    x = true;
                    x2 = false;
                    
                    Debug.Log("CrossFade1");
                }
            }
            if(isCompleted)
            {
                if (!x2)
                {
                    main2AudioSource.priority = 0;
                    main3AudioSource.priority = 128;
                    
                    main2AudioSource.DOFade(1f, 1f);
                    main3AudioSource.DOFade(0f, 1f);
                    
                    x = false;
                    x2 = true;
                    
                    isCompleted = false;
                    Debug.Log("CrossFade2");
                }
            }
        }
    }

    public void SetBackGroundSound(int numberIntheList)
    {
        if (mainAudioSource.clip != listClip[numberIntheList].clip || !mainAudioSource.isPlaying)
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
        if (main2AudioSource.clip != listClip[numberIntheList].clip || !main2AudioSource.isPlaying)
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
        mainAudioSource.PlayOneShot(listClip[numberIntheList].clip,  listClip[numberIntheList].volume * masterVolume * playermastervolume);
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
        PlayOneshotSoundEffect(12);
    }

    public void SetWalkingSound()
    {
        SetSoundEffect1(13);
    }
    
}
