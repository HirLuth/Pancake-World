using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;

    void Start()
    {
        if (Character.Instance.menuPrincipale)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }

        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
        }
    }
    
    void Update()
    {
        if (Character.Instance.menuPrincipale)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }

        else
        {
            menu.SetActive(false);
            Time.timeScale = 1;
        }
    }


    public void NouvellePartie()
    {
        Character.Instance.menuPrincipale = false;
        menu.SetActive(false);
        Time.timeScale = 1;
    }
}
