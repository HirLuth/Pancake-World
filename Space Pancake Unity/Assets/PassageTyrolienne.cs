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
        Debug.Log(triggerActivated);
        
        if (triggerActivated && !EventManager.Instance.isDead)
        {
            CameraMovements.Instance.offset.x += Mathf.Lerp(5, 0, Time.deltaTime);
            CameraMovements.Instance.camera.DOOrthoSize(11, 2.5f);
        }
        else
        {
            Debug.Log("oui");
            triggerActivated = false;
            CameraMovements.Instance.offset.x += Mathf.Lerp(0, 5, Time.deltaTime);
            CameraMovements.Instance.camera.DOOrthoSize(9, 2.5f);
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
