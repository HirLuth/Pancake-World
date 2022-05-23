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


        
        if (eventSystem.currentSelectedGameObject != bouton)
        {
            if (bouton != null)
            {
                StopCoroutines(BoutonSelected());
                bouton.transform.DOMoveY(originalPosition.y, speedOscillation);
            }

            stop = false;
            bouton = eventSystem.currentSelectedGameObject;
            originalPosition = bouton.transform.position;
            
            Debug.Log(12);
        }
        

        if (eventSystem.currentSelectedGameObject && !stop)
        {
            stop = true;
            StartCoroutine(BoutonSelected());
        }
    }


    public IEnumerator BoutonSelected()
    {
        bouton.transform.DOMoveY(originalPosition.y + 10, speedOscillation);
        yield return new WaitForSeconds(speedOscillation);
        
        bouton.transform.DOMoveY(originalPosition.y - 10, speedOscillation);
        yield return new WaitForSeconds(speedOscillation);
        
        stop = false;
    }

    public IEnumerator BoutonCanceled()
    {
        bouton.transform.DOMoveY(originalPosition.y, speedOscillation);
        yield return new WaitForSeconds(speedOscillation);
    }
}
