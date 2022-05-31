using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using Random = System.Random;


public class AudioManager : MonoBehaviour
{
    public List<Sound> listClip;
    public AudioSource mainAudioSource;
    public AudioSource secondaryAudioSource;
    public static AudioManager instance;
    public int actualNeutralBackground;
    public int actualNeutralSoundEffect;

    [Header("Volume Sound Effects")] 
    public float playermastervolume;
    [SerializeField] [Range(0,1)] private float masterVolume = 1;
    [SerializeField] [Range(0,1)] private float neutralBackgroundVolume;
    [SerializeField] [Range(0,1)] private float neutralSoundEffectVolume;
    [SerializeField] [Range(0,1)] private float volumeMainTheme;
    [SerializeField] [Range(0,1)] private float volumeSurvivalTheme;
    [SerializeField] [Range(0,1)] private float volumeForestAmbiance;
    [SerializeField] [Range(0,1)] private float volumeBirds;
    [SerializeField] [Range(0,1)] private float volumeCollectibles;
    [SerializeField] [Range(0,1)] private float volumeSerpe;
    private int backGroundPlaying;
    private int soundEffectPlaying;


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
        mainAudioSource.clip = listClip[0].clip;
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

    public void SetBackGroundSound(int numberIntheList)
    {
        if (mainAudioSource.clip != listClip[numberIntheList].clip || !mainAudioSource.isPlaying)
        {
            mainAudioSource.volume = listClip[numberIntheList].volume * masterVolume * playermastervolume;
            mainAudioSource.Stop();
            mainAudioSource.clip = listClip[numberIntheList].clip;
            mainAudioSource.Play();
            backGroundPlaying = numberIntheList;
        }
    }

    public void SetSoundEffect1(int numberIntheList)
    {
        if (secondaryAudioSource.clip != listClip[numberIntheList].clip || !secondaryAudioSource.isPlaying)
        {
            secondaryAudioSource.volume = listClip[numberIntheList].volume * masterVolume * playermastervolume;
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
        secondaryAudioSource.volume = listClip[backGroundPlaying].volume * masterVolume * playermastervolume;
    }

    public void SetForestAmbiance()
    {
        SetBackGroundSound(2);
    }

    public void SetSurvivalTheme()
    {
        SetBackGroundSound(1);
    }

    public void LaunchBirdsSound()
    {
        PlayOneshotSoundEffect(3);
    }

    public void LaunchCollectibleSound()
    {
        PlayOneshotSoundEffect(4);
    }

    public void SetSerpeSound()
    {
        SetSoundEffect1(5);
    }
    
}
