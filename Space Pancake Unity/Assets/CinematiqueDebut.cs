using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class CinematiqueDebut : MonoBehaviour
{
    [Header("Autres")] 
    public GameObject UI;
    public Camera camera;
    public GameObject intro;
    public GameObject fond;
    public GameObject fondAvant;
    private bool fadeOnce;
    private bool stop;
    public PlayerControls controls;

    [Header("Mouvement Parchemin")] 
    public float point1;
    public float point2;
    public float point3;
    public float point4;
    public float point5;

    [Header("CameraShake")] 
    public float shakeDuration;
    public float shakeStrenght;
    public int shakeVibrato;
    public float shakeRandomness;

    [Header("Gestion Temps")] 
    public float timer;
    public float time1;
    public float time2;
    public float time3;
    public float time4;
    public float dureeMouvements;


    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {

        controls.Disable();
    }


    void Start()
    {
        UI.SetActive(false);

        timer = 0;
        fondAvant.GetComponent<SpriteRenderer>().DOFade(0, 0);
        
        //camera.DOShakePosition(shakeDuration, shakeStrenght, shakeVibrato, shakeRandomness);
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        if (controls.Personnage.Pause.WasPerformedThisFrame() || stop)
        {
            if (!stop)
                timer = 0;
            
            stop = true;
            
            
            fondAvant.GetComponent<SpriteRenderer>().DOFade(1, 1);

            if (timer > 1 && !fadeOnce)
            {
                fondAvant.GetComponent<SpriteRenderer>().DOFade(0, 1);
                fond.GetComponent<SpriteRenderer>().DOFade(0, 1);
                gameObject.GetComponent<SpriteRenderer>().DOFade(0, 0);
                
                UI.SetActive(true);
                
                fadeOnce = true;
            }

            if (timer > 2)
            {
                intro.SetActive(false);
            }
        }
        
        else
        {
            if (timer < time1)
            {
                transform.DOMoveY(point1, dureeMouvements).SetEase(Ease.Flash);
            }

            else if (timer > time1 && timer < time2)
            {
                transform.DOMoveY(point2, dureeMouvements).SetEase(Ease.Flash);
            }
        
            else if (timer > time2 && timer < time3)
            {
                transform.DOMoveY(point3, dureeMouvements).SetEase(Ease.Flash);
            }
        
            else if (timer > time3 && timer < time4)
            {
                transform.DOMoveY(point4, dureeMouvements).SetEase(Ease.Flash);
            }
        
            else if (timer > time4 && timer < time4 + dureeMouvements)
            {
                fondAvant.GetComponent<SpriteRenderer>().DOFade(1, 1);

                if (timer > time4 + 1 && !fadeOnce)
                {
                    fondAvant.GetComponent<SpriteRenderer>().DOFade(0, 1);
                    fond.GetComponent<SpriteRenderer>().DOFade(0, 1);
                    gameObject.GetComponent<SpriteRenderer>().DOFade(0, 0);
                
                    UI.SetActive(true);

                    fadeOnce = true;
                }
            
                if (timer > time4 + 2)
                {
                    intro.SetActive(false);
                }
            }
        }
    }
}
