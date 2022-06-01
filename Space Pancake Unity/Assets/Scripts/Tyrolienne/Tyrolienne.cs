using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Tyrolienne : MonoBehaviour
{
    [Header("Paramètres Importants")] 
    private float speedTyrolienne;
    public bool usingTyrolienne;
    [SerializeField] float speedLimit;
    [SerializeField] float acceleration;
    [SerializeField] Vector2 direction;
    
    [Header("Poteaux")]
    [SerializeField] GameObject poteau1;
    [SerializeField] GameObject poteau2;
    private Vector2 pos1;
    private Vector2 pos2;
    private Vector2 posActuel;
    
    
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
    
    bool oui = true;


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
        stockageGravity = 7.3f;

        // On détermine la direction que va prendre la tyrolienne
        direction = poteau2.transform.position - poteau1.transform.position;

        pos1 = poteau1.transform.position;
        pos2 = poteau2.transform.position;
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
                
                if (!Character.Instance.particuleVitesse.isPlaying)
                {
                    Character.Instance.particuleVitesse.Play();
                }
                
                // Si le joueur décide de sauter sur la tyrolienne
                if (controls.Personnage.Sauter.WasPressedThisFrame() && rb.velocity.x > 0 || Character.Instance.wantsToJump && rb.velocity.x > 0)
                {
                    // On le fait sauter 
                    Character.Instance.Jump();
                    
                    timer = 0;
                    Character.Instance.noControl = false;
                    Detection.canUseZipline = false;
                    usingTyrolienne = false;
                    Character.Instance.noAirControl = true;
                    rb.gravityScale = stockageGravity;
                    CameraMovements.Instance.tyrolienneCamera = false;
                    Character.Instance.wantsToJump = false;
                    
                    Character.Instance.particuleVitesse.Stop();
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

                posActuel.x = Mathf.Abs(pos1.x - Character.Instance.transform.position.x + Time.deltaTime);
                posActuel.y = Mathf.Abs(Character.Instance.transform.position.y - pos1.y + Time.deltaTime);

                Character.Instance.transform.position = new Vector2(Character.Instance.transform.position.x, 
                    Mathf.Lerp(pos1.y - 0.6f, pos2.y - 0.6f, posActuel.x / (pos2.x - pos1.x)));

                rb.velocity = direction.normalized * speedTyrolienne;
            }

            // Si le joueur n'est plus entre les deux poteaux ou si on n'utilise pas la tyrolienne
            else if (player.transform.position.x >= poteau2.transform.position.x || player.transform.position.x <= poteau1.transform.position.x)
            {
                Debug.Log(stockageGravity);
                
                isOnThisZipline = false;
                usingTyrolienne = false;
                Character.Instance.noControl = false;
                Character.Instance.noAirControl = false;
                Detection.canUseZipline = false;
                rb.gravityScale = stockageGravity;
                
                CameraMovements.Instance.tyrolienneCamera = false;
                Character.Instance.particuleVitesse.Stop();
                
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
            stockageGravity = 7.3f;
            isOnThisZipline = true;
            usingTyrolienne = true;
            speedTyrolienne = rb.velocity.x;
            Character.Instance.noJump = false;

            posActuel.x = pos2.x - collision.transform.position.x;
            posActuel.y = pos2.y - collision.transform.position.y;
        }
    }
}
