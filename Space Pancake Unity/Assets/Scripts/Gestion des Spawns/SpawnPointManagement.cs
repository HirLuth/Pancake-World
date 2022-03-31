using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class SpawnPointManagement : MonoBehaviour
{
    public static SpawnPointManagement instance;
    public static Vector2 spawnPointLocation;
    public static bool spawnWasModifiedOnce;
    [SerializeField] private bool activerCheckpoint;
    
    private void Awake()
    {
        if (activerCheckpoint)
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(spawnPointLocation);
    }

    public void SetSpawnPoint(Vector2 newSpawnPointLocation)
    {
        if (activerCheckpoint)
        {
            spawnPointLocation = newSpawnPointLocation;
            spawnWasModifiedOnce = true;
        }
    }

}
