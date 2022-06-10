using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // Tout ce qui concerne le score
    public int pointsNumber;
    [SerializeField] public TextMeshProUGUI score;


    // Tout ce qui concerne la mort
    [Header("La mooort")]
    [SerializeField] public GameObject UI;
    private float timerAnimation;
    public float dureeAnimationMort;
    public bool isDead;
    public bool stopCamera;
    
    public float newZoom;
    public float stockageZoom;
    public float dureeZoom;

    public Vector3 cameraPos;
    public bool dieOnce;

    private Camera camera;

    [Header("Transitions")]
    public bool menuToGame;
    public bool gameToMenu;
    public Image fondu;
    
    [Header("Timer")] 
    public float timerGame;
    
    [Header("MeilleurScore")] 
    private int meilleurScore;

    [Header("CinématiqueDébut")] 
    public bool playedOnce;


    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        UI = UIPrincipale.Instance.gameObject;
        score = UIPrincipale.Instance.textScore;

        timerGame = PlayerPrefs.GetFloat("timer", 0);
        pointsNumber = PlayerPrefs.GetInt("coins", 0);
    }

    
    void Update()
    {
        if(!MenuManager.Instance.ActivateOnThisScene && !Character.Instance.isSpawning && !ScriptFinDeJeu.Instance.endEvent)
            Timer();

        // Transition lorsque l'on passe de la scene de menue à la scene de jeu
        if (menuToGame)
        {
            if (SceneManager.GetActiveScene().name == "Léontine")
            {
                fondu = MenuManager.Instance.fondu;
                
                fondu.DOFade(1, 0);
                fondu.DOFade(0, 1);

                Character.Instance.isSpawning = true;
                menuToGame = false;
                
                Character.Instance.rb.bodyType = RigidbodyType2D.Dynamic;
                AudioManager.instance.SetForestAmbiance();
            }
        }
        
        // Transition lorsque l'on passe de la scene de jeu à la scene de menu
        else if (gameToMenu)
        {
            if (SceneManager.GetActiveScene().name == "Menu Principal")
            {
                AudioManager.instance.SetMenuTheme();
                fondu = MenuManager.Instance.fondu;
                
                fondu.DOFade(1, 0);
                fondu.DOFade(0, 1);

                Character.Instance.isSpawning = true;
                gameToMenu = false;
            }
        }
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
    public void Death()
    {
        if (!dieOnce)
        {
            stopCamera = true;
            dieOnce = true;
            Time.timeScale = 1;

            if (CameraMovements.Instance.isOnRail)
            {
                CameraMovements.Instance.isOnRail = false;
                Mover.Instance.go = false;
            }
        
            isDead = true;

            CameraMovements.Instance.zoneMort.SetActive(false);

            // On immobilise le joueur
            Character.Instance.noControl = true;
            Character.Instance.apparition = true;
            Character.Instance.rb.gravityScale = 0;
            Character.Instance.rb.velocity = new Vector2(0, 0);
            Character.Instance.rb.bodyType = RigidbodyType2D.Static;
            Character.Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;

            // On lance son animation
            Character.Instance.anim.SetTrigger("isDead");
            AudioManager.instance.PlayOneshotSoundEffect(17);

            // Attente de la fin de l'animation
            StartCoroutine(WaitAnimation(dureeAnimationMort));
        }
    }

    public void Timer()
    {
        timerGame += Time.deltaTime;
        PlayerPrefs.SetFloat("timer", timerGame);
    }

    public int CallTimerMinutes()
    {
        return (int) PlayerPrefs.GetFloat("timer", 0) / 60;
    }

    public int CallTimerSeconds()
    {
        int minutes = (int) PlayerPrefs.GetFloat("timer", 0) / 60;
        
        return (int) PlayerPrefs.GetFloat("timer", 0) - minutes * 60;
    }
    
    
    public void AddPoints(int points)
    {
        pointsNumber += points;
        score.text = "" + pointsNumber;
    }

    public void SavePoints()
    {
        PlayerPrefs.SetInt("score",pointsNumber);
    }


    public IEnumerator WaitAnimation(float duree)
    {
        // Zoom vers le personnage
        CameraMovements.Instance.camera.DOOrthoSize(newZoom, dureeZoom);
        CameraMovements.Instance.transform.DOMove(Character.Instance.transform.position + new Vector3(0, 0, -10), dureeZoom);
        CameraMovements.Instance.fondu.GetComponent<SpriteRenderer>().DOFade(1, dureeZoom);
        
        // On attend que ça soit finit
        yield return new WaitForSeconds(dureeZoom);
        
        // Respawn du joueur
        if (Character.Instance.activatespawnpoint)
        {
            Character.Instance.transform.position = SpawnPointManagement.instance.GetSpawn();
        }
        else
        {
            Character.Instance.transform.position = Character.Instance.coordonnesApparition;
        }

        // On replace la camera
        CameraMovements.Instance.transform.position = new Vector3(Character.Instance.transform.position.x,
            Character.Instance.transform.position.y, -10);

        cameraPos = CameraMovements.Instance.transform.position;
        
        //Character.Instance.transform.position = SpawnPointManagement.spawnPointLocation;

        Character.Instance.isSpawning = true;
        isDead = false;
        Restart();
    }
}
