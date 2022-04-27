using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionDetectorFallingPlateforme : MonoBehaviour
{
    [SerializeField] private int layerThatDestroy;
    [SerializeField] private FallingPlateform fallingPlateform;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == layerThatDestroy)
        {
            fallingPlateform.Destruction();
        }
    }
}
