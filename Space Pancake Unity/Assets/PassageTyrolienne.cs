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
        if (!EventManager.Instance.stopCamera)
        {
            if (triggerActivated && !EventManager.Instance.isDead && !stop)
            {
                CameraMovements.Instance.offset.x += Mathf.Lerp(6, 0, Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Character" && other.transform.position.x < 1100)
        {
            triggerActivated = true;
            CameraMovements.Instance.camera.DOOrthoSize(11.5f, 2f);
            CameraMovements.Instance.dezoomAuto = false;
        }
        
        else if (other.tag == "Character" && other.transform.position.x > 1100)
        {
            triggerActivated = false;
            CameraMovements.Instance.offset.x += Mathf.Lerp(0, 5, Time.deltaTime);
            CameraMovements.Instance.camera.DOOrthoSize(9 + CameraMovements.Instance.dezoomActuel, 2f);
            CameraMovements.Instance.dezoomAuto = true;
        }
    }
}
