using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScriptFinDeJeu : MonoBehaviour
{
    [SerializeField] private bool endEvent;
    
    [Header("Partie1")]
    public Image fondu;
    private float timer;

    
    
    void Update()
    {
        if (endEvent && Character.Instance.onGround)
        {
            Character.Instance.rb.velocity = new Vector2(5, 0);
            
            Character.Instance.isWalking = true;
            Character.Instance.isRunning = false;
            Character.Instance.isJumping = false;
            Character.Instance.isFalling = false;

            timer += Time.deltaTime;

            if (timer > 1)
            {
                fondu.DOFade(1.2f, 2);
            }

            if (timer > 5)
            {
                fondu.DOFade(0, 2);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character.Instance.noControl = true;
        endEvent = true;
    }
}
