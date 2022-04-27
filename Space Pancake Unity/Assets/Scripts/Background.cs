using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background Instance;

    private void Awake()
    {
        Instance = this;
    }
}
