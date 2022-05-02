using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.PlayerLoop;

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
    public float dureeZoom;
    private float stockageZoom;
    
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
        stockageZoom = CameraMovements.Instance.camera.orthographicSize;
    }
    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Death()
    {
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

        // On lance son animation
        Character.Instance.anim.SetTrigger("isDead");

        // Attente de la fin de l'animation
        StartCoroutine(WaitAnimation(dureeAnimationMort));
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
        
        yield return new WaitForSeconds(dureeZoom );
        
        
        //Character.Instance.transform.position = SpawnPointManagement.spawnPointLocation;

        Character.Instance.isSpawning = true;
        //Character.Instance.transform.position;
        isDead = false;
        Restart();
    }
    
    
}
