using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public static MenuManager Instance;

    private float timerSortieMenue;
    private bool timerActif;

    public bool ActivateOnThisScene;
    public Image fondu;


    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        if (ActivateOnThisScene)
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
        // On active le menu si il doit s'afficher
        if (ActivateOnThisScene)
        {
            menu.SetActive(true);
        }

        // Sinon on le retire
        else
        {
            menu.SetActive(false);
        }

        // Pour que le personnage ne saute pas au lancement de la partie
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
        StartCoroutine(Fondu(2));
    }

    public void Continuer()
    {
        SceneManager.LoadScene("Léontine");

        if (Character.Instance.activatespawnpoint && SpawnPointManagement.spawnWasModifiedOnce)
        {
            Character.Instance.transform.position = SpawnPointManagement.spawnPointLocation;
        }

        Character.Instance.menuPrincipale = false;
        menu.SetActive(false);
        Time.timeScale = 1;

        Character.Instance.noControl = true;
        timerSortieMenue = 0;
        timerActif = true;
    }


    IEnumerator WaitForSceneLoad(string sceneName)
    {
        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }
    }

    IEnumerator Fondu(float duree)
    {
        fondu.DOFade(1, duree);

        yield return new WaitForSeconds(duree);
        
        SceneManager.LoadScene("Léontine");

        if (SceneManager.GetActiveScene().name != "Léontine")
        {
            StartCoroutine(WaitForSceneLoad("Léontine"));
        }

        if (SceneManager.GetActiveScene().name == "Léontine")
        {
            Character.Instance.transform.position = Character.Instance.coordonnesApparition;
            SpawnPointManagement.spawnWasModifiedOnce = false;

            Character.Instance.menuPrincipale = false;
            menu.SetActive(false);
            Time.timeScale = 1;

            Character.Instance.noControl = true;
            timerSortieMenue = 0;
            timerActif = true;
        }
    }
}