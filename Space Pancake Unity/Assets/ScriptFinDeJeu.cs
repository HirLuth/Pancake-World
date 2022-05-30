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

    [Header("Partie2")] 
    public Vector2 teleportation = new Vector2(1661, 80);
    public Vector2 posCam = new Vector2(1680, 85);
    public float zoom1 = 13;
    public float zoom2 = 10;

    public GameObject guimauve;
    private Vector2 originalPos;
    private float timerGuimauve;
    private bool stop;


    void Start()
    {
        originalPos = guimauve.transform.position;
    }
    
    
    
    void Update()
    {
        timerGuimauve += Time.deltaTime;

        if (timerGuimauve < 1 && !stop)
        {
            guimauve.transform.DOMoveY(originalPos.y + 0.3f, 1).SetEase(Ease.Flash);
        }
        else if (timerGuimauve < 2 && !stop)
        {
            guimauve.transform.DOMoveY(originalPos.y - 0.3f, 1).SetEase(Ease.Flash);
        }
        else
        {
            timerGuimauve = 0;
        }
        
        if (endEvent && Character.Instance.onGround)
        {
            timer += Time.deltaTime;

            if (timer < 2)
            {
                Character.Instance.rb.velocity = new Vector2(5, 0);
                
                Character.Instance.isWalking = true;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }
            
            else if (timer < 4)
            {
                fondu.DOFade(1.1f, 2);
                Character.Instance.rb.velocity = new Vector2(5, 0);
                
                Character.Instance.isWalking = true;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }

            else if (timer < 4.1f)
            {
                Character.Instance.transform.position = teleportation;

                CameraMovements.Instance.followPlayer = false;
                CameraMovements.Instance.targetPosition = posCam;
                CameraMovements.Instance.camera.DOOrthoSize(zoom1, 0);
            }
            
            else if (timer < 6.5f)
            {
                fondu.DOFade(0, 1.5f);
                Character.Instance.rb.velocity = new Vector2(5, 0);
                
                Character.Instance.isWalking = true;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }

            else if (timer < 8f)
            {
                Character.Instance.rb.velocity = new Vector2(0, 0);

                CameraMovements.Instance.camera.DOOrthoSize(zoom2, 3);
                
                Character.Instance.isWalking = false;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }
            
            else if(timer < 10.5f)
            {
                stop = true;
                guimauve.transform.DOMoveY(originalPos.y + 2, 2);
            }

            else
            {
                fondu.DOFade(1.1f, 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character.Instance.noControl = true;
        endEvent = true;
    }
}
