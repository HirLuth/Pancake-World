using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBas : MonoBehaviour
{
    public static bool tropBas;
    private void OnTriggerEnter2D(Collider2D other)
    {
        tropBas = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        tropBas = false;
    }
}
    
