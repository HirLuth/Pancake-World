using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldatFraise : MonoBehaviour
{
    [Header("Self Refernce")]
    [SerializeField] private Rigidbody2D rbSelf;
    [SerializeField] private SpriteRenderer spriteSelf;
    [Header("other référence")]
    [SerializeField] private EventManager eventManager;
    [Header("Variables modifiable")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float patrollingDistanceRight;
    [SerializeField] private float patrollingDistanceLeft;
    [SerializeField] private bool iaIsWalking;
    [SerializeField] private bool startingWalkingSideIsRight;

    [Header("Variables de fonctionnement")] 
    [SerializeField] private Vector2 startPostion;
    [SerializeField] private bool isGoingRight;

    private void Awake()
    {
        startPostion = transform.position;
        if (startingWalkingSideIsRight)
        {
            isGoingRight = true;
        }
        else
        {
            isGoingRight = false;
        }
    }

    void Update()
    {
        if (isGoingRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rbSelf.velocity = Vector2.right*movementSpeed;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rbSelf.velocity = Vector2.left*movementSpeed;
        }

        if (transform.position.x >= startPostion.x + patrollingDistanceRight )
        {
            isGoingRight = false;
        }
        if (transform.position.x <= startPostion.x - patrollingDistanceLeft )
        {
            isGoingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            
        }
    }
}
