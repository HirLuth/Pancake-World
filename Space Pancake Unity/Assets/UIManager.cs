using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        if (controls.Personnage.Pause.WasPerformedThisFrame() && !MenuManager.Instance.ActivateOnThisScene && !EventManager.Instance.isDead)
        {
            if (!pauseActive)
            {
                pauseActive = true;
                UIPause.SetActive(true);

                Time.timeScale = 0;
                Character.Instance.noControl = true;
            }

            else
            {
                pauseActive = false;
                UIPause.SetActive(false);

                Time.timeScale = 1;
                Character.Instance.noControl = false;

                timerSortiePause = 0;
            }
        }

        if (timerSortiePause < 0.1f)
        {
            timerSortiePause += Time.deltaTime;
            Character.Instance.jump = false;
        }
    }

    
    public void Reprendre()
    {
        pauseActive = false;
        UIPause.SetActive(false);

        Time.timeScale = 1;
        Character.Instance.noControl = false;
        Character.Instance.jump = false;
        
        timerSortiePause = 0;
    }

    
    public void Restart()
    {
        pauseActive = false;
        UIPause.SetActive(false);

        Time.timeScale = 1;

        EventManager.Instance.Death();
    }

    
    public void Menu()
    {
        pauseActive = false;
        UIPause.SetActive(false);
        
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
    
    
    IEnumerator WaitForSceneLoad(string sceneName)
    {
        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }
    }
}
