using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;
    
    [Range(0f,1f)]
    public float volume = 1;
    
    [Range(-3f,3f)]
    public float pitch = 1;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
