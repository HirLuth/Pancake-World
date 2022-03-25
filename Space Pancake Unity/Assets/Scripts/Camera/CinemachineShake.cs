using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    // Pour faire référence au script dans d'autres scripts facilement 
    public static CinemachineShake Instance
    {
        get;
        private set;
    }

    private CinemachineVirtualCamera camera;    // Référence de la virtual camera
    private CinemachineBasicMultiChannelPerlin shake;
    private float shakeTimer;    // Timer permettant de stopper le camera shake



    void Awake()
    {
        Instance = this;
        camera = GetComponent<CinemachineVirtualCamera>();
        shake = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();   // Référence de ce qui provoque le camera shake
    }


    public void Shake(float duration, float magnitude)
    {
        // On lance le camera shake
        shakeTimer = duration;
        shake.m_AmplitudeGain = magnitude;
    }


    private void Update()
    {
        // Boucle permettant de terminer le camera shake
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            // Si le camera shake doit se terminer
            if(shakeTimer <= 0)
            {
                shake.m_AmplitudeGain = 0f;
            }

        }
    }
}
