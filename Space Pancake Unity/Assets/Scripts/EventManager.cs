using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.PlayerLoop;
using Color = System.Drawing.Color;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // Tout ce qui concerne le score
    [SerializeField] private int pointsNumber;
    [SerializeField] public TextMeshProUGUI score;


    // Tout ce qui concerne la mort
    [Header("La mooort")]
    [SerializeField] public GameObject UI;
    private float timerAnimation;
    public float dureeAnimationMort;
    public bool isDead;
    
    public float newZoom;
    public float stockageZoom;
    public float dureeZoom;

    public Vector3 cameraPos;
    public bool dieOnce;

    private Camera camera;


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
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Death()
    {
        if (!dieOnce)
        {
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

            // Attente de la fin de l'animation
            StartCoroutine(WaitAnimation(dureeAnimationMort));
        }
    }
    
    public void AddPoints(int points)
    {
        pointsNumber += points;
        score.text = "SCORE : " + pointsNumber;
    }


    public IEnumerator WaitAnimation(float duree)
    {
        CameraMovements.Instance.camera.DOOrthoSize(newZoom, dureeZoom);
        CameraMovements.Instance.transform.DOMove(Character.Instance.transform.position + new Vector3(0, 0, -10), dureeZoom);
        CameraMovements.Instance.fondu.GetComponent<SpriteRenderer>().DOFade(1, dureeZoom);
        
        yield return new WaitForSeconds(dureeZoom);
        
        if (Character.Instance.activatespawnpoint && SpawnPointManagement.spawnWasModifiedOnce)
        {
            Character.Instance.transform.position = SpawnPointManagement.spawnPointLocation;
        }
        else
        {
            Character.Instance.transform.position = Character.Instance.coordonnesApparition;
        }

        CameraMovements.Instance.transform.position = new Vector3(Character.Instance.transform.position.x,
            Character.Instance.transform.position.y, -10);

        cameraPos = CameraMovements.Instance.transform.position;
        
        //Character.Instance.transform.position = SpawnPointManagement.spawnPointLocation;

        Character.Instance.isSpawning = true;
        isDead = false;
        Restart();
    }
    
    
}
