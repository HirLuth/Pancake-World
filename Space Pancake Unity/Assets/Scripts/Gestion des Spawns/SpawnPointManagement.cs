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
    
    public void SetSpawnPoint(Vector2 newSpawnPointLocation)
    {
        if (activerCheckpoint)
        {
            spawnPointLocation = newSpawnPointLocation;
            spawnPointLocation = spawnPointLocation + new Vector2(0, 1f);
            spawnWasModifiedOnce = true;
        }
    }

}
