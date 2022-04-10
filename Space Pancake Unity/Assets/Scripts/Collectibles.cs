using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Collectibles : MonoBehaviour
{
    [SerializeField] private EventManager eventManager;
    [SerializeField] private Animator animatorSelf;
    [SerializeField] private int numberOfPointsGiven;
    [SerializeField] private Vector2 stockagePosition;
    [SerializeField] private float timer;
    [SerializeField] private float timeToGoUp;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float hauteurAnimation;
    [SerializeField] private bool asCollected;

    private void Start()
    {
        eventManager = EventManager.Instance;
        eventManager.UI = UIPrincipale.Instance.gameObject;
        eventManager.score = UIPrincipale.Instance.textScore;
        eventManager.AddPoints(0);
        stockagePosition = transform.position;
    }

    private void Update()
    {
        if (asCollected)
        {
            timer += Time.deltaTime;
            animatorSelf.SetBool("IsCroqued", true);
            transform.position = Vector3.Lerp(stockagePosition, new Vector2(stockagePosition.x,stockagePosition.y + hauteurAnimation), timer/timeToGoUp);
            if (timer >= timeToDestroy)
            {
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
                eventManager.AddPoints(numberOfPointsGiven);
                asCollected = true;
            }
        }
    }
}
