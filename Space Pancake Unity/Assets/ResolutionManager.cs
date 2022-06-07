using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public Toggle fullscreenTog;
    
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenTog.isOn;
    }
}
