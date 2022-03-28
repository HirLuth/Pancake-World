using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    [SerializeField] private int numberOfPointsGiven;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            eventManager.AddPoints(numberOfPointsGiven);
            Destroy(this.gameObject);
        }
    }
}
