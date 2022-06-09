using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Collectibles : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    [SerializeField] private SpawnPointManagement spawnPointManagement;
    [SerializeField] private Animator animatorSelf;
    [SerializeField] private int numberOfPointsGiven;
    [SerializeField] private Vector2 stockagePosition;
    [SerializeField] private float timer;
    [SerializeField] private float timeToGoUp;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float hauteurAnimation;
    [SerializeField] private bool asCollected;
    private Vector2 locationToRemove;
    [SerializeField] private ParticleSystem vfxSelf;

    private void Start()
    {
        eventManager = EventManager.Instance;
        spawnPointManagement = SpawnPointManagement.instance;
        stockagePosition = transform.position;
        Debug.Log(SpawnPointManagement.instance.newGame);
        if (SpawnPointManagement.instance.newGame)
        {
            SpawnPointManagement.instance.locationCollectibleCollected = new List<Vector2>();
            SpawnPointManagement.instance.newGame = false;
        }
        foreach (var location in SpawnPointManagement.instance.locationCollectibleCollected)
        {
            if (location == stockagePosition)
            {
                Debug.Log(location);
                if (stockagePosition.x < SpawnPointManagement.spawnPointLocation.x)
                {
                    Destroy(this.gameObject);  
                }
                else
                {
                    eventManager.AddPoints(-numberOfPointsGiven);
                    locationToRemove = location;
                }
            }
        }

        if (locationToRemove != null)
        {
            spawnPointManagement.locationCollectibleCollected.Remove(locationToRemove);
        }
        eventManager.SavePoints();
    }

    private void Update()
    {
        if (asCollected)
        {
            timer += Time.deltaTime;
            animatorSelf.SetBool("IsCroqued", true);
            transform.position = Vector3.Lerp(stockagePosition, new Vector2(stockagePosition.x,stockagePosition.y + hauteurAnimation), timer/timeToGoUp);
            vfxSelf.Stop();
            if (timer >= timeToDestroy)
            {
                spawnPointManagement.IamCollected(stockagePosition);
                Destroy(this.gameObject);
            }
            
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            if (!asCollected)
            {
                AudioManager.instance.LaunchCollectibleSound();
                eventManager.AddPoints(numberOfPointsGiven);
                asCollected = true;
            }
        }
    }
}
