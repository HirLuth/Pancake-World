using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoldatFraise : MonoBehaviour
{
    [Header("Self Reference")]
    [SerializeField] private Rigidbody2D rbSelf;
    [SerializeField] private SpriteRenderer spriteSelf;
    [SerializeField] private Collider2D lanceCollider;
    [SerializeField] private Animator animatorSelf;
    [Header("other référence")]
    [SerializeField] private EventManager eventManager;
    [SerializeField] private Bash bash;
    [Header("Variables modifiable")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float patrollingDistanceRight;
    [SerializeField] private float patrollingDistanceLeft;
    [SerializeField] public bool iaIsWalking;
    [SerializeField] private bool startingWalkingSideIsRight;
    [SerializeField] private float timeStun;
    [SerializeField] private float timeToGrow;

    [Header("Variables de fonctionnement")] 
    [SerializeField] private Vector2 startPostion;
    [SerializeField] private bool isGoingRight;
    [SerializeField] private Color gizmoColor;
    [SerializeField] private bool grabLance;
    [SerializeField] private bool stunPeriod;
    [SerializeField] private float timer;
    [SerializeField] private bool freezeMovement;

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

    private void Start()
    {
        eventManager = EventManager.Instance;
    }

    void Update()
    {
        // On désactive la hitbox de la lance
        if (bash.usingSerpe)
        {
            lanceCollider.enabled = false;
            grabLance = true;
            freezeMovement = true;
            rbSelf.velocity = Vector2.zero;
        }

        // Si le joueur arrête d'utiliser la serpe
        if (grabLance)
        {
            if (!bash.usingSerpe)
            {
                stunPeriod = true;
                bash.soldatFraise = true;
                freezeMovement = false;
            }
        }

        if (stunPeriod)
        {
            grabLance = false;
            rbSelf.velocity = Vector2.zero;
            timer += Time.deltaTime;
            animatorSelf.SetBool("BriseLance", true);

            if (timer >= timeStun)
            {
                animatorSelf.SetBool("Repousse", true);
            }

            if (timer >= timeStun + timeToGrow)
            {
                bash.soldatFraise = false;
                animatorSelf.SetBool("BriseLance", false);
                animatorSelf.SetBool("Repousse", false);
                timer = 0;
                lanceCollider.enabled = true;
                stunPeriod = false;
            }
            return;
        }
        if (iaIsWalking && !freezeMovement)
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
