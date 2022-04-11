using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPrincipale : MonoBehaviour
{
    public static UIPrincipale Instance { get; private set; }
    public TextMeshProUGUI textScore;

    private void Awake()
    {
        Instance = this;
    }
}
