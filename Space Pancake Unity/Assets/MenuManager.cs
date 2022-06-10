using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
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
    public InputSystemUIInputModule controls;
    private bool stop;

    public Animator anim;

    public TextMeshProUGUI highScore;

    [Header("Options")] 
    public GameObject options;
    public bool optionsActives;

    [Header("Objects To Destroy")] 
    public GameObject audioManager;
    public GameObject spawnPointManager;
    public GameObject character;


    private void Awake()
    {
        Instance = this;
    }


    void OnEnable()
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

            highScore.text = "Meilleur Score : " + PlayerPrefs.GetFloat("meilleurScore", 0) + " pts";
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
        SpawnPointManagement.instance.RecordSpawn(new Vector2(-90.4f, -4.2f));
        
        // Remise à zero des éléments du score
        PlayerPrefs.SetFloat("timer", 0);
        EventManager.Instance.timerGame = 0;
        
        PlayerPrefs.SetInt("coins", 0);
        EventManager.Instance.pointsNumber = 0;
        SpawnPointManagement.instance.locationCollectibleCollected = new List<Vector2>();
        
        Destroy(audioManager);
        //Destroy(spawnPointManager);
        Destroy(character);

        // Transition de scene en scene
        EventManager.Instance.menuToGame = true;
        anim.SetTrigger("Balade");

        controls.enabled = false;
        
        StartCoroutine(Fondu(1.3f));
    }
    

    public void Continuer()
    {
        // Transition de scene en scene
        EventManager.Instance.menuToGame = true;
        anim.SetTrigger("Balade");
        
        controls.enabled = false;
        
        StartCoroutine(Fondu(1.3f));
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
        Character.Instance.run = false;
    }

    public void FullScreen(bool fenetre)
    {
        Screen.fullScreen = fenetre;
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
        yield return new WaitForSeconds(1);
        
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