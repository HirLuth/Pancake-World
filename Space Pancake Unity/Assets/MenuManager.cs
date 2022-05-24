using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;

    private float timerSortieMenue;
    private bool timerActif;

    void Start()
    {
        if (Character.Instance.menuPrincipale)
        {
            menu.SetActive(true);
        }

        else
        {
            menu.SetActive(false);
        }
    }
    
    void Update()
    {
        if (Character.Instance.menuPrincipale)
        {
            menu.SetActive(true);
            Character.Instance.noControl = true;
        }

        else 
        {
            menu.SetActive(false);
        }

        if (timerActif)
        {
            timerSortieMenue += Time.deltaTime;
            Character.Instance.jump = false;

            if (timerSortieMenue >= 0.05f)
            {
                Character.Instance.noControl = false;
                timerActif = false;
            } 
        }
        
    }


    public void NouvellePartie()
    {
        Character.Instance.menuPrincipale = false;
        menu.SetActive(false);
        Time.timeScale = 1;

        Character.Instance.noControl = true;
        timerSortieMenue = 0;
        timerActif = true;
    }
}
