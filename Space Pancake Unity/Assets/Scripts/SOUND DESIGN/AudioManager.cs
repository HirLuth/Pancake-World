using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public List<AudioClip> listClip;
    public AudioSource mainAudioSource;
    public AudioSource secondaryAudioSource;
    
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
    }

    public void SetForestAmbiance()
    {
        secondaryAudioSource.clip = listClip[2];
    }

    public void SetSurvivalTheme()
    {
        mainAudioSource.Stop();
        mainAudioSource.PlayOneShot(listClip[1]);
    }
}
