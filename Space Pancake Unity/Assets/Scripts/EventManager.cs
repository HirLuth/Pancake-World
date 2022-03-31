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
    public bool isDead;


    private void Awake()
    {
        Instance = this;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isDead = false;
        Time.timeScale = 1;
        UI.SetActive(false);
    }


    public void Death()
    {
        isDead = true;
        Time.timeScale = 0;
        UI.SetActive(true);
    }


    public void AddPoints(int points)
    {
        pointsNumber += points;
        score.text = "SCORE : " + pointsNumber;
    }
}
