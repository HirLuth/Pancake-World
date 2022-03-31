using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MaïsAReaction : MonoBehaviour
{
    [Header("Player refrence")]
    [SerializeField] private PlayerControls controls;
    [SerializeField] private Character character;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private Rigidbody2D playerRB;

    [Header("Self refrence")]
    [SerializeField] private SpriteRenderer spriteSelf;
    [SerializeField] private Rigidbody2D rbSelf;
    [SerializeField] private float maxSpeedGoingUp;
    [SerializeField] private Animator animatorSelf;
    [Header("Variables modifiables")] 
    [SerializeField] private AnimationCurve courbeAccelerationMonté;
    [SerializeField] private float timerToExplode;
    [SerializeField] private float explosionAnimationTimer;
    [SerializeField] private float positionSetUpDistanceFromEdge;
    [SerializeField] private float multiplicatorTimeToGetInPosition;
    [SerializeField] private float directionnalModificator;
    [SerializeField] private float jumpOutForceMultiplicator;


    [Header("Variables de fonctionnement")] 
    [SerializeField] public bool isInDestroyingAnmation;
    [SerializeField] private bool playerIsAtRange;
    [SerializeField] private bool isOnTheRide;
    [SerializeField] private bool launchedWithoutPlayer;
    [SerializeField] private float timer;
    [SerializeField] private float timerExplosion;
    [SerializeField] private float stockageGravityScaleJoueur;
    [SerializeField] private float horizontaleSpeedSide;
    [SerializeField] private float stockageJumpForce;


    [Header("Variable UI")] 
    [SerializeField] private Color colorNotAtRange;
    [SerializeField] private Color colorAtRange;
    
    private void Awake()
    {
        controls = new PlayerControls();
        playerIsAtRange = false;
        stockageGravityScaleJoueur = playerRB.gravityScale;
        stockageJumpForce = character.jumpForce;
    }
    
    private void OnEnable()
    {
        controls.Personnage.Enable();
    }

    private void OnDisable()
    {
        controls.Personnage.Disable();
    }

    private void Update()
    {
        if (isInDestroyingAnmation)
        {
            Destruction();
            return;
        }
        if (playerIsAtRange && controls.Personnage.Serpe.WasPressedThisFrame())
        {
            isOnTheRide = true;
        }
        if (isOnTheRide)
        {
            spriteSelf.color = colorNotAtRange;
            animatorSelf.SetBool("maïsIsGoingUp", true);
            character.jumping = false;
            character.abscisseJumpCurve = 0;
            timer += Time.deltaTime;
            playerRB.gravityScale = 0;
            character.noControl = true;
            character.jumpForce = jumpOutForceMultiplicator*stockageJumpForce;
            
            
            if (controls.Personnage.MoveRight.WasPerformedThisFrame())
            {
                horizontaleSpeedSide = 1;
            }
            else if (controls.Personnage.MoveLeft.WasPerformedThisFrame())
            {
                horizontaleSpeedSide = -1;
            }
            
            if (controls.Personnage.Sauter.WasPerformedThisFrame())
            {
                //playerRB.velocity = new Vector2(horizontaleSpeedSide * jumpOutForceX+1, jumpOutForceY);
                
                character.Jump();
                ReintialiseWhenGetOut();
                launchedWithoutPlayer = true;
            }
            
            if (playerGameObject.transform.position == transform.position + Vector3.down * positionSetUpDistanceFromEdge)
            {
                
                playerRB.velocity = new Vector2(horizontaleSpeedSide*directionnalModificator, courbeAccelerationMonté.Evaluate(timer/timerToExplode) * maxSpeedGoingUp) ;
                rbSelf.velocity = new Vector2(horizontaleSpeedSide*directionnalModificator, courbeAccelerationMonté.Evaluate(timer/timerToExplode) * maxSpeedGoingUp) ;
                if (timer >= timerToExplode)
                {
                    ReintialiseWhenGetOut();
                    isInDestroyingAnmation = true;
                }
            }
            else if (!launchedWithoutPlayer)
            {
                playerGameObject.transform.position = Vector3.Lerp(playerGameObject.transform.position, transform.position + Vector3.down*positionSetUpDistanceFromEdge, timer*multiplicatorTimeToGetInPosition);
            }
        }

        if (launchedWithoutPlayer)
        {
            timer += Time.deltaTime;
            rbSelf.velocity = Vector2.up * courbeAccelerationMonté.Evaluate(timer/timerToExplode) * maxSpeedGoingUp;
            if (timer >= timerToExplode)
            {
                isInDestroyingAnmation = true;
            }
        }
    }

    public void ReintialiseWhenGetOut()
    {
        playerRB.gravityScale = stockageGravityScaleJoueur;
        character.noControl = false;
        isOnTheRide = false;
    }

    private void Destruction()
    {
        animatorSelf.SetBool("maïsIsExploding", true);
        timerExplosion += Time.deltaTime;
        rbSelf.velocity = Vector2.zero;
        if (timerExplosion >= explosionAnimationTimer)
        {
            Destroy(this.gameObject);
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerIsAtRange = true;
        spriteSelf.color = colorAtRange;
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerIsAtRange = false;
        spriteSelf.color = colorNotAtRange;
        character.jumpForce = stockageJumpForce;
    }
}
