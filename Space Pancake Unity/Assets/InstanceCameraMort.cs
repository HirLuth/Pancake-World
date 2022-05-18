using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceCameraMort : MonoBehaviour
{
    public static InstanceCameraMort Instance;

    public void Awake()
    {
        Instance = this;
    }
}
