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
    [SerializeField] private float volumeBirds;
    [SerializeField] private float volumeCollectibles;
    [SerializeField] private float volumeSerpe;


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
    }

    // Update is called once per frame
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
        if (mainAudioSource.clip != listClip[numberIntheList])
        {
            mainAudioSource.Stop();
            mainAudioSource.clip = listClip[numberIntheList];
            mainAudioSource.Play();
        }
    }

    public void SetSoundEffect1(int numberIntheList)
    {
        if (secondaryAudioSource.clip != listClip[numberIntheList])
        {
            secondaryAudioSource.Stop();
            secondaryAudioSource.clip = listClip[numberIntheList];
            secondaryAudioSource.Play();
        }
    }

    public void PlayOneshotSoundEffect(int numberIntheList, float volume)
    {
        mainAudioSource.PlayOneShot(listClip[numberIntheList], volume);
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
        PlayOneshotSoundEffect(3,volumeBirds);
    }

    public void LaunchCollectibleSound()
    {
        PlayOneshotSoundEffect(4, volumeCollectibles);
    }

    public void SetSerpeSound()
    {
        SetSoundEffect1(5);
    }
}
