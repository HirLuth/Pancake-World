using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] private Vector2 stockagePosition;
    [SerializeField] public bool isInDestroyingAnmation;
    [SerializeField] private bool playerIsAtRange;
    [SerializeField] private bool isOnTheRide;
    [SerializeField] private bool launchedWithoutPlayer;
    [SerializeField] private float timer;
    [SerializeField] private float timerExplosion;
    [SerializeField] private float horizontaleSpeedSide;
    [SerializeField] private bool grabKeyPressed;


    [Header("Variable UI")] 
    [SerializeField] private Color colorNotAtRange;
    [SerializeField] private Color colorAtRange;
    
    private void Awake()
    {
        controls = new PlayerControls();
        playerIsAtRange = false;
        stockagePosition = transform.position;
    }

    private void Start()
    {
        playerGameObject = Character.Instance.gameObject;
        character = Character.Instance;
        playerRB = Character.Instance.rb;
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
        if (playerIsAtRange && controls.Personnage.Serpe.WasPerformedThisFrame())
        {
            isOnTheRide = true;
        }

        if (controls.Personnage.MoveRight.WasPerformedThisFrame())
        {
            horizontaleSpeedSide = 1;
        }
        else if (controls.Personnage.MoveLeft.WasPerformedThisFrame())
        {
            horizontaleSpeedSide = -1;
        }

        else if (controls.Personnage.MoveLeft.WasReleasedThisFrame() || controls.Personnage.MoveRight.WasReleasedThisFrame())
        {
            horizontaleSpeedSide = 0;
        }

        if (isOnTheRide)
        {
            CameraMovements.Instance.maïsCamera = true;
            
            Character.Instance.anim.SetBool("isOnTyroMaïs", true);
            Character.Instance.GetComponent<SpriteRenderer>().sortingOrder = -1;
            
            spriteSelf.color = colorNotAtRange;
            animatorSelf.SetBool("maïsIsGoingUp", true);
            character.jumping = false;
            character.abscisseJumpCurve = 0;
            timer += Time.deltaTime;
            playerRB.gravityScale = 0;
            character.noControl = true;
            character.jumpForce = jumpOutForceMultiplicator*character.stockageJumpForce;

            if (controls.Personnage.Sauter.WasPerformedThisFrame())
            {
                character.Jump();
                ReintialiseWhenGetOut();
                launchedWithoutPlayer = true;
                
                Character.Instance.anim.SetBool("isOnTyroMaïs", false);
                Character.Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
            
            if (playerGameObject.transform.position == transform.position + new Vector3(-0.40f, -1, 0) * positionSetUpDistanceFromEdge || 
                playerGameObject.transform.position == transform.position + new Vector3(0.30f, -1, 0) * positionSetUpDistanceFromEdge)
            {
                
                playerRB.velocity = new Vector2(horizontaleSpeedSide*directionnalModificator, courbeAccelerationMonté.Evaluate(timer/timerToExplode) * maxSpeedGoingUp) ;
                rbSelf.velocity = new Vector2(horizontaleSpeedSide*directionnalModificator, courbeAccelerationMonté.Evaluate(timer/timerToExplode) * maxSpeedGoingUp) ;
                if (timer >= timerToExplode)
                {
                    ReintialiseWhenGetOut();
                    isInDestroyingAnmation = true;
                }
            }
            else if (!launchedWithoutPlayer && Character.Instance.transform.rotation.y != 0)
            {
                playerGameObject.transform.position = Vector3.Lerp(playerGameObject.transform.position, transform.position + new Vector3(0.30f, -1, 0) * positionSetUpDistanceFromEdge, 
                    timer*multiplicatorTimeToGetInPosition);
            }
            
            else if (!launchedWithoutPlayer && Character.Instance.transform.rotation.y == 0)
            {
                playerGameObject.transform.position = Vector3.Lerp(playerGameObject.transform.position, transform.position + new Vector3(-0.40f, -1, 0) * positionSetUpDistanceFromEdge, 
                    timer*multiplicatorTimeToGetInPosition);
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
        playerRB.gravityScale = character.stockageGravityScale;
        character.noControl = false;
        isOnTheRide = false;
    }

    private void Destruction()
    {
        CameraMovements.Instance.maïsCamera = false;
        animatorSelf.SetBool("maïsIsExploding", true);
        
        Character.Instance.anim.SetBool("isOnTyroMaïs", false);
        Character.Instance.GetComponent<SpriteRenderer>().sortingOrder = 3;
        
        timerExplosion += Time.deltaTime;
        rbSelf.velocity = Vector2.zero;
        if (timerExplosion >= explosionAnimationTimer)
        {
            Destroy(this.gameObject);
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            playerIsAtRange = true;
            spriteSelf.color = colorAtRange;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CameraMovements.Instance.maïsCamera = false;
        playerIsAtRange = false;
        spriteSelf.color = colorNotAtRange;
        character.jumpForce = character.stockageJumpForce;
    }
}
