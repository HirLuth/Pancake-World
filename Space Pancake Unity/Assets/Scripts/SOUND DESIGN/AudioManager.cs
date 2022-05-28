using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public List<AudioClip> listClip;
    public AudioSource mainAudioSource;
    public AudioSource secondaryAudioSource;
    public static AudioManager instance;
    public int actualNeutralBackground;
    public int actualNeutralSoundEffect;

    [Header("Volume Sound Effects")] 
    [SerializeField] [Range(0,1)] private float masterVolume = 1;
    [SerializeField] [Range(0,1)] private float neutralBackgroundVolume;
    [SerializeField] [Range(0,1)] private float neutralSoundEffectVolume;
    [SerializeField] [Range(0,1)] private float volumeMainTheme;
    [SerializeField] [Range(0,1)] private float volumeSurvivalTheme;
    [SerializeField] [Range(0,1)] private float volumeForestAmbiance;
    [SerializeField] [Range(0,1)] private float volumeBirds;
    [SerializeField] [Range(0,1)] private float volumeCollectibles;
    [SerializeField] [Range(0,1)] private float volumeSerpe;


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
        mainAudioSource.clip = listClip[0];
        mainAudioSource.Play();
        mainAudioSource.volume = volumeMainTheme;
        actualNeutralBackground = 1;
        actualNeutralSoundEffect = 0;
    }
    
    void Update()
    {
        if (CameraMovements.Instance != null)
        {
            if (CameraMovements.Instance.isOnRail)
            {
                SetSurvivalTheme();
            }
            else
            {
                SetForestAmbiance();
            }
        }
        
    }

    public void SetBackGroundSound(int numberIntheList, float volume)
    {
        if (mainAudioSource.clip != listClip[numberIntheList] || !mainAudioSource.isPlaying)
        {
            mainAudioSource.volume = volume * masterVolume;
            mainAudioSource.Stop();
            mainAudioSource.clip = listClip[numberIntheList];
            mainAudioSource.Play();
        }
    }

    public void SetSoundEffect1(int numberIntheList, float volume)
    {
        if (secondaryAudioSource.clip != listClip[numberIntheList] || !secondaryAudioSource.isPlaying)
        {
            secondaryAudioSource.volume = volume * masterVolume;
            secondaryAudioSource.Stop();
            secondaryAudioSource.clip = listClip[numberIntheList];
            secondaryAudioSource.Play();
        }
    }

    public void PlayOneshotSoundEffect(int numberIntheList, float volume)
    {
        mainAudioSource.PlayOneShot(listClip[numberIntheList], volume * masterVolume);
    }

    public void BackToNeutralMain()
    {
        if (actualNeutralBackground == 0 && mainAudioSource.isPlaying)
        {
            mainAudioSource.Stop();
        }
        else if (actualNeutralBackground != 0)
        {
            SetBackGroundSound(actualNeutralBackground, neutralBackgroundVolume); 
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
            SetSoundEffect1(actualNeutralSoundEffect, neutralSoundEffectVolume);
        }
    }

    public void SetForestAmbiance()
    {
        SetBackGroundSound(2,volumeForestAmbiance);
    }

    public void SetSurvivalTheme()
    {
        SetBackGroundSound(1, volumeSurvivalTheme);
    }

    public void LaunchBirdsSound()
    {
        PlayOneshotSoundEffect(3,volumeBirds);
    }

    public void LaunchCollectibleSound()
    {
        PlayOneshotSoundEffect(4, volumeCollectibles);
    }

    public void SetSerpeSound()
    {
        SetSoundEffect1(5, volumeSerpe);
    }
}
