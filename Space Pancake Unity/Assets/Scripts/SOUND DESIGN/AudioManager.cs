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
        if (CameraMovements.Instance.isOnRail)
        {
            SetSurvivalTheme();
        }
        
        Debug.Log(mainAudioSource.isPlaying);
    }

    public void SetForestAmbiance()
    {
        mainAudioSource.Stop();
        secondaryAudioSource.clip = listClip[2];
        mainAudioSource.Play();
    }

    public void SetSurvivalTheme()
    {
        if (mainAudioSource.clip != listClip[1])
        {
            mainAudioSource.Stop();
            mainAudioSource.clip = listClip[1];
            mainAudioSource.Play();  
        }
        
    }
}
