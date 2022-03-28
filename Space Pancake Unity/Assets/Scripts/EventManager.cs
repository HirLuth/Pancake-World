using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField] private GameObject UI;

    private int points;
    [SerializeField] private TextMeshProUGUI score;


    private void Awake()
    {
        Instance = this;
    }


    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UI.SetActive(false);
    }


    public void AddPoints()
    {
        points += 1;
        score.text = "SCORE : " + points;
    }
}
