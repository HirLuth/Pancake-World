using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointManagement : MonoBehaviour
{
    public static SpawnPointManagement instance;
    public static Vector2 spawnPointLocation;
    public  static bool spawnWasModifiedOnce;
    
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    /*private void OnEnable()
    {
        Debug.Log("Spawn !!!");
        if (spawnWasModifiedOnce)
        {
            playerGameObject.transform.position = spawnPointLocation;
        }
    }*/
    
    public void SetSpawnPoint(Vector2 newSpawnPointLocation)
    {
        spawnPointLocation = newSpawnPointLocation;
        spawnWasModifiedOnce = true;
    }

}
