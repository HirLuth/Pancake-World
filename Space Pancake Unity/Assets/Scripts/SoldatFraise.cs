using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoldatFraise : MonoBehaviour
{
    [Header("Self Refernce")]
    [SerializeField] private Rigidbody2D rbSelf;
    [SerializeField] private SpriteRenderer spriteSelf;
    [SerializeField] private Collider2D lanceCollider;
    [Header("other référence")]
    [SerializeField] private EventManager eventManager;
    [SerializeField] private Character characterScript;
    [Header("Variables modifiable")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float patrollingDistanceRight;
    [SerializeField] private float patrollingDistanceLeft;
    [SerializeField] private bool iaIsWalking;
    [SerializeField] private bool startingWalkingSideIsRight;

    [Header("Variables de fonctionnement")] 
    [SerializeField] private Vector2 startPostion;
    [SerializeField] private bool isGoingRight;
    [SerializeField] private Color gizmoColor;

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
        if (iaIsWalking)
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            eventManager.Death();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + patrollingDistanceRight, transform.position.y, 0 ));
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x - patrollingDistanceLeft, transform.position.y, 0 ));
    }
}
