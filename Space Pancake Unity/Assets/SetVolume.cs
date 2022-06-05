using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volume", 1);
        
        AudioManager.instance.playermastervolume =  PlayerPrefs.GetFloat("volume", 1);;
        
        AudioManager.instance.UpdateVolume();
    }

    public void SetLevel(float sliderValue)
    {
        AudioManager.instance.playermastervolume = sliderValue;
        
        AudioManager.instance.UpdateVolume();
        
        PlayerPrefs.SetFloat("volume", sliderValue);
    }
}
