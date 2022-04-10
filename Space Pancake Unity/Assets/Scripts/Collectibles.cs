using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    [SerializeField] private int numberOfPointsGiven;

    private void Start()
    {
        eventManager = EventManager.Instance;
        eventManager.UI = UIPrincipale.Instance.gameObject;
        eventManager.score = UIPrincipale.Instance.textScore;
        eventManager.AddPoints(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            eventManager.AddPoints(numberOfPointsGiven);
            Destroy(this.gameObject);
        }
    }
}
