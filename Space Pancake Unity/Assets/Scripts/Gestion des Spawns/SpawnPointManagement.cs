using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class SpawnPointManagement : MonoBehaviour
{
    public static SpawnPointManagement instance;
    public static Vector2 spawnPointLocation = Vector2.zero;
    public static bool spawnWasModifiedOnce;
    public bool activerCheckpoint;
    public List<Vector2> locationCollectibleCollected;

    public Vector3 originalPosition;

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

#if !UNITY_EDITOR
    private void Start()
    {
        if (GetSpawn() != Vector2.zero)
        {
            Character.Instance.transform.position = GetSpawn();
        }
    }  
#endif


    private void Start()
    {
        //originalPosition = Character.Instance.transform.position;
    }


    public void SetSpawnPoint(Vector2 newSpawnPointLocation)
    {
        if (activerCheckpoint)
        {
            spawnPointLocation = newSpawnPointLocation;
            spawnWasModifiedOnce = true;
            RecordSpawn(newSpawnPointLocation);
        }
    }

    public void IamCollected(Vector2 locationToAdd)
    {
        locationCollectibleCollected.Add(locationToAdd);
    }

    public Vector2 GetSpawn()
    {
        Vector2 position = new Vector2();
        position.x = PlayerPrefs.GetFloat("posX", 0);
        position.y = PlayerPrefs.GetFloat("posY", 0);
        return position;
    }

    public void RecordSpawn(Vector2 spawnLocation)
    {
        PlayerPrefs.SetFloat("posX", spawnLocation.x);
        PlayerPrefs.SetFloat("posY", spawnLocation.y);
    }

}
