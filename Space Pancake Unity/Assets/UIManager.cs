using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    private GameObject mainUI;
    private GameObject UIPause;
    public Image fondu;

    private PlayerControls controls;

    [Header("Pause")]
    [HideInInspector] public bool pauseActive;
    private float timerSortiePause;
    public EventSystem eventSystem;


    [Header("Timer")] public TextMeshProUGUI timer;
    
    
    

    private void Awake()
    {
        controls = new PlayerControls();

        Instance = this;
    }

    private void OnEnable()
    {
        controls.Personnage.Enable();
    }

    private void OnDisable()
    {
        controls.Personnage.Disable();
    }


    private void Start()
    {
        mainUI = UIPrincipale.Instance.gameObject;
        UIPause = InstancePause.Instance.gameObject;

        pauseActive = false;

        timerSortiePause = 1;
    }


    void Update()
    {
        if(!ScriptFinDeJeu.Instance.endEvent)
            timer.text = Timer();
        
        if ((controls.Personnage.Pause.WasPerformedThisFrame() || controls.Personnage.Quitter.WasPerformedThisFrame()) && !MenuManager.Instance.ActivateOnThisScene && !EventManager.Instance.isDead
            && !ScriptFinDeJeu.Instance.endEvent)
        {
            if (!pauseActive && !controls.Personnage.Quitter.WasPerformedThisFrame())
            {                                                                                                                                                                                                                                                                   
                pauseActive = true;
                UIPause.SetActive(true);

                Time.timeScale = 0;
                Character.Instance.noControl = true;
            }

            else if (MenuManager.Instance.optionsActives)
            {
                MenuManager.Instance.QuitterOptions();
            }
            
            else
            {
                pauseActive = false;
                UIPause.SetActive(false);

                Time.timeScale = 1;
                Character.Instance.noControl = false;
                
                FirstSelected.Instance.FermetureMenu();

                timerSortiePause = 0;
            }
        }

        if (timerSortiePause < 0.1f)
        {
            timerSortiePause += Time.deltaTime;
            Character.Instance.jump = false;
            Character.Instance.noControl = true;
        }
        else
        {
            Character.Instance.noControl = false;
        }
    }

    
    public void Reprendre()
    {
        pauseActive = false;
        UIPause.SetActive(false);

        Time.timeScale = 1;

        FirstSelected.Instance.FermetureMenu();

        timerSortiePause = 0;
    }

    
    public void Restart()
    {
        pauseActive = false;
        UIPause.SetActive(false);
        
        FirstSelected.Instance.FermetureMenu();

        Time.timeScale = 1;

        EventManager.Instance.Death();
    }

    
    public void Menu()
    {
        pauseActive = false;
        UIPause.SetActive(false);
        
        FirstSelected.Instance.FermetureMenu();
        
        Time.timeScale = 1;

        EventManager.Instance.gameToMenu = true;
        Character.Instance.rb.bodyType = RigidbodyType2D.Static;
        
        eventSystem.SetSelectedGameObject(null);

        StartCoroutine(Fondu(1));
    }
    
    
    IEnumerator Fondu(float duree)
    {
        fondu.DOFade(1, duree);

        yield return new WaitForSeconds(duree);
        
        SceneManager.LoadScene("Menu Principal");

        if (SceneManager.GetActiveScene().name != "Menu Principal")
        {
            StartCoroutine(WaitForSceneLoad("Menu Principal"));
        }
    }


    public string Timer()
    {
        if (EventManager.Instance.CallTimerSeconds() < 10)
        {
            if (EventManager.Instance.CallTimerMinutes() < 10)
            {
                return "0" + EventManager.Instance.CallTimerMinutes() + ":0" + EventManager.Instance.CallTimerSeconds();
            }
            else
            {
                return EventManager.Instance.CallTimerMinutes() + ":0" + EventManager.Instance.CallTimerSeconds();
            }
        }

        
        else if(EventManager.Instance.CallTimerMinutes() < 10)
            return "0" + EventManager.Instance.CallTimerMinutes() + ":" + EventManager.Instance.CallTimerSeconds();
            
        else
            return EventManager.Instance.CallTimerMinutes() + ":" + EventManager.Instance.CallTimerSeconds();
    }
    
    
    IEnumerator WaitForSceneLoad(string sceneName)
    {
        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }
    }
}
