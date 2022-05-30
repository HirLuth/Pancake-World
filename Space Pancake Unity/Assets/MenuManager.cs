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
    private bool stop;

    [Header("Options")] 
    public GameObject options;
    public bool optionsActives;


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
            
            if(!stop)
                StartCoroutine(DéFondu(1));
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
        // On modifie la sauvegarde actuelle
        SpawnPointManagement.spawnWasModifiedOnce = false;

        SpawnPointManagement.instance.RecordSpawn(new Vector2(-47.3f, -4.2f));
        
        // Transition de scene en scene
        EventManager.Instance.menuToGame = true;
        StartCoroutine(Fondu(2));
    }
    

    public void Continuer()
    {
        // Transition de scene en scene
        EventManager.Instance.menuToGame = true;
        StartCoroutine(Fondu(2));
    }


    public void LancerOptions()
    {
        optionsActives = true;
        
        FirstSelected.Instance.FermetureMenu();

        options.SetActive(true);
    }

    
    public void QuitterOptions()
    {
        optionsActives = false;
        
        FirstSelected.Instance.FermetureMenu();

        options.SetActive(false);
    }

    public void QuitterJeu()
    {
        Application.Quit();
    }

    
    public void WalkRun(bool inverse)
    {
        Character.Instance.inverseRun = inverse;
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

    IEnumerator DéFondu(float duree)
    {
        fondu.DOFade(0, duree);
        stop = true;
        
        yield return new WaitForSeconds(duree);
    }
}