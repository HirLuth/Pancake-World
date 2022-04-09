using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    // Tout ce qui concerne le score
    private int pointsNumber;
    [SerializeField] private TextMeshProUGUI score;


    // Tout ce qui concerne la mort
    [SerializeField] private GameObject UI;
    private float timerAnimation;
    public float dureeAnimationMort;
    public bool isDead;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UI = UIPrincipale.Instance.gameObject;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Death()
    {
        isDead = true;

        // On immobilise le joueur
        Character.Instance.noControl = true;
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
        yield return new WaitForSeconds(duree);
        
        Character.Instance.gameObject.SetActive(false);
        
        UI.SetActive(true);
    }
}
