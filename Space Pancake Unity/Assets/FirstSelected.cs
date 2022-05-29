using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.InputSystem.UI;

public class FirstSelected : MonoBehaviour
{
    public static FirstSelected Instance;
    
    public GameObject menuPrincipale;
    public GameObject menuPause;
    public GameObject menuOptions;

    public InputSystemUIInputModule menuPrincipalControls;

    public EventSystem eventSystem;


    [Header("Oscillation")] 
    public float speedOscillation;
    public Vector3 newSize;
    public Vector3 normalSize;
    private bool goDown;
    private bool goUp;
    private float timer;
    
    private GameObject bouton;
    private Vector3 originalPosition;
    private bool stop;

    private GameObject newBouton;
    private Vector3 newOriginalPosition;


    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        if (MenuManager.Instance.ActivateOnThisScene && !eventSystem.currentSelectedGameObject && !MenuManager.Instance.optionsActives)
        {
            eventSystem.SetSelectedGameObject(menuPrincipale);
        }
        
        else if (MenuManager.Instance.optionsActives && !eventSystem.currentSelectedGameObject)
        {
            eventSystem.SetSelectedGameObject(menuOptions);
        }

        else if (UIManager.Instance.pauseActive && !eventSystem.currentSelectedGameObject)
        {
            eventSystem.SetSelectedGameObject(menuPause);
        }



        if (eventSystem.currentSelectedGameObject != bouton && eventSystem.currentSelectedGameObject)
        {
            if (bouton != null)
            {
                bouton.transform.DOMoveY(originalPosition.y, speedOscillation);
                bouton.transform.DOScale(normalSize, speedOscillation);
            }

            goUp = true;
            goDown = false;
            timer = 0;
            
            bouton = eventSystem.currentSelectedGameObject;
            originalPosition = bouton.transform.position;
        }
        

        if (eventSystem.currentSelectedGameObject)
        {
            if (goUp && eventSystem.currentSelectedGameObject.tag == "Button")
            {
                goUp = false;
                bouton.transform.DOMoveY(originalPosition.y + 4, speedOscillation);
                bouton.transform.DOScale(newSize, speedOscillation);

                timer += Time.deltaTime;

                if (timer >= speedOscillation)
                {
                    timer = 0;
                    goDown = true;
                }
            }
            
            if (goDown)
            {
                goDown = false;
                bouton.transform.DOMoveY(originalPosition.y - 4, speedOscillation);

                timer += Time.deltaTime;

                if (timer >= speedOscillation)
                {
                    timer = 0;
                    goUp = true;
                }
            }
        }
    }


    public void FermetureMenu()
    {
        eventSystem.SetSelectedGameObject(null);
    }
    

    public IEnumerator BoutonUp()
    {
        yield return new WaitForSeconds(speedOscillation);
        goDown = true;
    }

    public IEnumerator BoutonDown()
    {
        yield return new WaitForSeconds(speedOscillation);
        goUp = true;
    }

    public IEnumerator BoutonCanceled()
    {
        bouton.transform.DOMoveY(originalPosition.y, speedOscillation);
        yield return new WaitForSeconds(speedOscillation);
    }
}
