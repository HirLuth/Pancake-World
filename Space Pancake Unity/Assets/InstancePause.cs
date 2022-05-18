using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancePause : MonoBehaviour
{
    public static InstancePause Instance;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
