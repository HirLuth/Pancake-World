using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionHaut : MonoBehaviour
{
    public static bool tropHaut;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        tropHaut = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        tropHaut = false;
    }
}
