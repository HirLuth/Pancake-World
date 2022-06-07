using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonOption : MonoBehaviour
{
    public ParticleSystem VFX;
    public bool launchVFX;

    public void Start()
    {
        VFX.Stop();
    }

    public void Update()
    {
        if (launchVFX)
        {
            VFX.Play();
        }
        else
        {
            VFX.Stop();
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
