using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FirstSelected : MonoBehaviour
{
    public GameObject menuPrincipale;
    public GameObject menuPause;

    public EventSystem eventSystem;


    [Header("Oscillation")] 
    public float speedOscillation;
    private bool goDown;
    private bool goUp;
    
    private GameObject bouton;
    private Vector3 originalPosition;
    private bool stop;

    private GameObject newBouton;
    private Vector3 newOriginalPosition;
    
    
    void Update()
    {
        if (Character.Instance.menuPrincipale && !eventSystem.currentSelectedGameObject)
        {
            eventSystem.SetSelectedGameObject(menuPrincipale);
        }

        else if (UIManager.Instance.pauseActive && !eventSystem.currentSelectedGameObject)
        {
            eventSystem.SetSelectedGameObject(menuPause);
        }
        
        else if(!Character.Instance.menuPrincipale && !UIManager.Instance.pauseActive)
        {
            eventSystem.SetSelectedGameObject(null);
        }


        
        if (eventSystem.currentSelectedGameObject != bouton && eventSystem.currentSelectedGameObject)
        {
            if (bouton != null)
            {
                StopCoroutine(BoutonUp());
                StopCoroutine(BoutonDown());
                
                bouton.transform.DOMoveY(originalPosition.y, speedOscillation);
            }

            goUp = true;
            goDown = false;
            
            bouton = eventSystem.currentSelectedGameObject;
            originalPosition = bouton.transform.position;
        }
        

        if (eventSystem.currentSelectedGameObject)
        {
            if (goUp)
            {
                goUp = false;
                bouton.transform.DOMoveY(originalPosition.y + 10, speedOscillation);

                StartCoroutine(BoutonUp());
            }
            
            if (goDown)
            {
                goDown = false;
                bouton.transform.DOMoveY(originalPosition.y - 10, speedOscillation);

                StartCoroutine(BoutonDown());
            }
        }
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
