using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelected : MonoBehaviour
{
    public GameObject menuPrincipale;
    public GameObject menuPause;

    public EventSystem eventSystem;
    
    
    void LateUpdate()
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
    }
}
