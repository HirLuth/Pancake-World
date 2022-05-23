using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    private GameObject mainUI;
    private GameObject UIPause;

    private PlayerControls controls;

    [Header("Pause")]
    [HideInInspector] public bool pauseActive;
    private float timerSortiePause;

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
        if (controls.Personnage.Pause.WasPerformedThisFrame())
        {
            if (!pauseActive)
            {
                pauseActive = true;
                UIPause.SetActive(true);

                Time.timeScale = 0;
                Character.Instance.noControl = true;
                
                Debug.Log(12);
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
}
