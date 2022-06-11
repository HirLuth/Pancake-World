using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScriptFinDeJeu : MonoBehaviour
{
    [HideInInspector] public bool endEvent;
    public static ScriptFinDeJeu Instance;
    
    
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
    public ParticleSystem particule;
    private bool particuleActives;

    public Animator dragon;

    [Header("Score")] 
    public GameObject UIScore;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textCoins;
    public TextMeshProUGUI textTime;


    [Header("Crédits")] public GameObject fondAvant;
    public bool launchCredits;
    private float timerCredits;
    public float finCredits;
    public GameObject fondAvant2;
    public bool stopCredits;


    void Start()
    {
        originalPos = guimauve.transform.position;
        
        particule.Stop(true);

        Instance = this;
        
        fondAvant.GetComponent<Image>().DOFade(0,0);
        fondAvant2.GetComponent<Image>().DOFade(0,0);
    }
    
    
    
    void Update()
    {
        timerGuimauve += Time.deltaTime;

        // Balancement du guimauve
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
            dragon.SetBool("YeuxDragon", true);

            if (timer < 1)
            {
                Character.Instance.rb.velocity = new Vector2(5, 0);
                
                Character.Instance.isWalking = true;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }
            
            else if (timer < 3)
            {
                fondu.DOFade(1.1f, 2);
                Character.Instance.rb.velocity = new Vector2(5, 0);
                
                Character.Instance.isWalking = true;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }

            else if (timer < 3.1f)
            {
                Character.Instance.transform.position = teleportation;

                CameraMovements.Instance.followPlayer = false;
                CameraMovements.Instance.targetPosition = posCam;
                CameraMovements.Instance.camera.DOOrthoSize(zoom1, 0);
            }
            
            else if (timer < 5.5f)
            {
                fondu.DOFade(0, 1.5f);
                Character.Instance.rb.velocity = new Vector2(5, 0);
                
                Character.Instance.isWalking = true;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }

            else if (timer < 7f)
            {
                Character.Instance.rb.velocity = new Vector2(0, 0);

                CameraMovements.Instance.camera.DOOrthoSize(zoom2, 3);
                
                Character.Instance.isWalking = false;
                Character.Instance.isRunning = false;
                Character.Instance.isJumping = false;
                Character.Instance.isFalling = false;
            }
            
            else if (timer < 9.5f)
            {
                stop = true;
                guimauve.transform.DOMoveY(originalPos.y + 2, 2);
                
                CameraMovements.Instance.camera.DOOrthoSize(zoom2, 3);

                if (!particule.isPlaying)
                {
                    particule.Play(true);
                }
            }

            else if (!launchCredits)
            {
                fondu.DOFade(1.2f, 2);

                UIScore.SetActive(true);

                textScore.text = "Score : " + CalculScore() + " pts";
                textTime.text = UIManager.Instance.Timer() + " = " + CalculPointsTime() + " pts";
                textCoins.text = EventManager.Instance.pointsNumber + " = " + CalculPointsCoins() + " pts";
                
                if(PlayerPrefs.GetFloat("meilleurScore", 0) < CalculScore())
                    PlayerPrefs.SetFloat("meilleurScore", CalculScore());

                if (Character.Instance.controls.Personnage.Sauter.WasPerformedThisFrame())
                {
                    CameraMovements.Instance.camera.DOOrthoSize(zoom2, 3);

                    fondAvant.GetComponent<Image>().DOFade(1, 1);

                    launchCredits = true;

                    /*SpawnPointManagement.instance.RecordSpawn(new Vector2(-90.4f, -4.2f));
                    SpawnPointManagement.spawnWasModifiedOnce = false;
                
                    UIManager.Instance.Menu();*/
                }
            }
            
            else if (launchCredits)
            {
                timerCredits += Time.deltaTime;
                
                if (Character.Instance.controls.Personnage.Pause.WasPerformedThisFrame())
                {
                    SpawnPointManagement.instance.RecordSpawn(new Vector2(-90.4f, -4.2f));
                    SpawnPointManagement.spawnWasModifiedOnce = false;
                    
                    fondAvant2.GetComponent<Image>().DOFade(1, 0.5f);
                
                    UIManager.Instance.Menu();
                }
                
                else if (timerCredits > finCredits)
                {
                    SpawnPointManagement.instance.RecordSpawn(new Vector2(-90.4f, -4.2f));
                    SpawnPointManagement.spawnWasModifiedOnce = false;
                    
                    fondAvant2.GetComponent<Image>().DOFade(1, 0.5f);

                    stopCredits = true;

                    if (timerCredits > finCredits + 1)
                    {
                        UIManager.Instance.Menu();
                    }
                }
            }
        }
    }


    private float CalculScore()
    {
        float time = PlayerPrefs.GetFloat("timer", 0);
        int coins = EventManager.Instance.pointsNumber;

        float arrondi = Mathf.Round(Mathf.Pow((1 / time * 400000), 1.125f) / 10) * 10;

        return 15 * coins + arrondi;
    }

    private float CalculPointsTime()
    {
        float time = PlayerPrefs.GetFloat("timer", 0);

        return Mathf.Round(Mathf.Pow((1 / time * 400000), 1.125f) / 10) * 10;
    }

    private float CalculPointsCoins()
    {
        float coins = EventManager.Instance.pointsNumber;

        return 15 * coins;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character.Instance.noControl = true;
        endEvent = true;
    }
}
