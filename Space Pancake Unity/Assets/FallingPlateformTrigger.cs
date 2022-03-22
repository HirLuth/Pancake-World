using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlateformTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            
        }
    }
}
