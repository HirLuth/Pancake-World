using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PassageTyrolienne : MonoBehaviour
{
    private bool triggerActivated;
    public GameObject cible;
    
    
    void FixedUpdate()
    {
        if (triggerActivated)
        {
            Debug.Log(12222);
            CameraMovements.Instance.offset.x += Mathf.Lerp(5, 0, Time.deltaTime);
            CameraMovements.Instance.camera.DOOrthoSize(11, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggerActivated && other.tag == "Character")
        {
            triggerActivated = true;
        }
    }
}
