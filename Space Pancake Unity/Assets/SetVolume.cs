using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {
        AudioManager.instance.playermastervolume = sliderValue;
        
        AudioManager.instance.UpdateVolume();
    }
}
