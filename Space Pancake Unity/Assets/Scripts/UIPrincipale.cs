using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrincipale : MonoBehaviour
{
    public static UIPrincipale Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
