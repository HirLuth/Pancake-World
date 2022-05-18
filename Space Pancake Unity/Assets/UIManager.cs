using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject mainUI;
    private GameObject UIPause;

    private PlayerControls controls;

    [Header("Pause")]
    private bool pauseActive;

    private void Awake()
    {
        controls = new PlayerControls();
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
            }

            else
            {
                pauseActive = false;
                UIPause.SetActive(false);

                Time.timeScale = 1;
                Character.Instance.noControl = false;
            }
        }
    }

    public void Reprendre()
    {
        pauseActive = false;
        UIPause.SetActive(false);

        Time.timeScale = 1;
        Character.Instance.noControl = false;
    }
}
