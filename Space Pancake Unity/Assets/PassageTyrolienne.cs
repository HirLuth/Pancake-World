using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PassageTyrolienne : MonoBehaviour
{
    private bool triggerActivated;
    public GameObject cible;
    private bool stop;
    
    
    void FixedUpdate()
    {
        if (triggerActivated && !EventManager.Instance.isDead && !Character.Instance.isSpawning && !stop)
        {
            CameraMovements.Instance.offset.x += Mathf.Lerp(5, 0, Time.deltaTime);
            CameraMovements.Instance.camera.DOOrthoSize(11, 2.5f);
        }
        else if(!EventManager.Instance.isDead && !Character.Instance.isSpawning && !stop)
        {
            triggerActivated = false;
            CameraMovements.Instance.offset.x += Mathf.Lerp(0, 5, Time.deltaTime);
            CameraMovements.Instance.camera.DOOrthoSize(9, 2.5f);
        }
        else
        {
            stop = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Character" && other.transform.position.x < 1100)
        {
            triggerActivated = true;
        }
        
        else if (other.tag == "Character" && other.transform.position.x > 1100)
        {
            triggerActivated = false;
            CameraMovements.Instance.camera.DOOrthoSize(9, 2.5f);
        }
    }
}
