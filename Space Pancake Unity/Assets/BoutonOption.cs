using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonOption : MonoBehaviour
{
    public ParticleSystem VFX;
    public GameObject VFXGameObject;
    public bool launchVFX;

    public void Start()
    {
        VFX.Stop();
    }

    public void Update()
    {
        if (launchVFX)
        {
            VFX.gameObject.SetActive(true);
        }
        else
        {
            VFX.gameObject.SetActive(false);
        }
    }

    public void StartVFX()
    {
        VFX.Play();
    }

    public void StopVFX()
    {
        VFX.Stop();
    }
}
