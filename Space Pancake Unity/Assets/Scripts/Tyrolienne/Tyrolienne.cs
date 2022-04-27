using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyrolienne : MonoBehaviour
{
    [Header("Paramètres Importants")] 
    private float speedTyrolienne;
    public bool usingTyrolienne;
    [SerializeField] float speedLimit;
    [SerializeField] float acceleration;
    [SerializeField] Vector2 direction;

    [SerializeField] GameObject poteau1;
    [SerializeField] GameObject poteau2;
    
    
    [Header("Player")]
    private GameObject player;
    private Rigidbody2D rb;
    private float stockageGravity;
    private PlayerControls controls;
    

    [Header("PlayerAirControl")]
    private float timer;

    [Header("Animations")] 
    private bool isUsingZipline;


    [Header("Autres")]
    private bool isOnThisZipline;
    public static Tyrolienne Instance;


    private void Awake()
    {
        controls = new PlayerControls();
        Instance = this;
    }
    
    private void OnEnable()
    {
        controls.Personnage.Enable();
    }

    private void OnDisable()
    {
        controls.Personnage.Disable();
    }



    private void Start()
    {
        player = Character.Instance.gameObject;
        
        // On recupère certains éléments
        rb = player.GetComponent<Rigidbody2D>();
        stockageGravity = rb.gravityScale;

        // On détermine la direction que va prendre la tyrolienne
        direction = poteau2.transform.position - poteau1.transform.position;
    }


    void Update()
    {

        if (isOnThisZipline)
        {
            UseZipline();
        }
    }


    void UseZipline()
    {
        if (usingTyrolienne)
        {
            // Si le personnage peut utiliser la tyrolienne
            if (usingTyrolienne && player.transform.position.x < poteau2.transform.position.x && player.transform.position.x > poteau1.transform.position.x && Detection.canUseZipline)
            {
                // Tout d'abord on retire la gravité du personnage 
                rb.gravityScale = 0;
                Character.Instance.noControl = true;
                CameraMovements.Instance.tyrolienneCamera = true;

                // Si le joueur décide de sauter sur la tyrolienne
                if (controls.Personnage.Sauter.WasPressedThisFrame() && rb.velocity.x > 0)
                {
                    // On le fait sauter 
                    timer = 0;
                    Character.Instance.Jump();
                    Character.Instance.noControl = false;
                    Detection.canUseZipline = false;
                    usingTyrolienne = false;
                    Character.Instance.noAirControl = true;
                    rb.gravityScale = stockageGravity;
                    CameraMovements.Instance.tyrolienneCamera = false;
                }

                // Gain de vitesse de la tyrolienne 
                else if (speedTyrolienne < speedLimit)
                {
                    if (rb.velocity.x < 0)
                    {
                        speedTyrolienne += Time.deltaTime * acceleration * 1.5f;
                    }

                    speedTyrolienne += Time.deltaTime * acceleration;
                }

                rb.velocity = direction.normalized * speedTyrolienne;
            }

            // Si le joueur n'est plus entre les deux poteaux ou si on n'utilise pas la tyrolienne
            else if (player.transform.position.x >= poteau2.transform.position.x || player.transform.position.x <= poteau1.transform.position.x)
            {
                isOnThisZipline = false;
                usingTyrolienne = false;
                Character.Instance.noControl = false;
                Character.Instance.noAirControl = false;
                Detection.canUseZipline = false;
                rb.gravityScale = stockageGravity;
                
                CameraMovements.Instance.tyrolienneCamera = false;
            }
        }
        
        else if (Character.Instance.noAirControl)
        {
            // Tout ce qui concerne l'absence d'air control 
            timer += Time.deltaTime;

            if (timer > 0.6f)
            {
                Detection.canUseZipline = false;
                isOnThisZipline = false;
                Character.Instance.noAirControl = false;
            }
        }

        else
        {
            isOnThisZipline = false;
        }
        
        
        Character.Instance.anim.SetBool("isOnTyroMaïs", usingTyrolienne);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character" && Detection.canUseZipline)
        {
            stockageGravity = rb.gravityScale;
            isOnThisZipline = true;
            usingTyrolienne = true;
            speedTyrolienne = rb.velocity.x;
        }
    }
}
