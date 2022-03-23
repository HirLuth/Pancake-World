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
    public GameObject player;
    public Character character;
    private Rigidbody2D rb;
    private float stockageGravity;
    private PlayerControls controls;
    
    [Header("PlayerAirControl")]
    public static bool noAirControl;
    private bool isJumping;
    private float timer;


    private void Awake()
    {
        controls = new PlayerControls();
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
        // On recupère certains éléments
        rb = player.GetComponent<Rigidbody2D>();
        stockageGravity = rb.gravityScale;
        
        // On détermine la direction que va prendre la tyrolienne
        direction = poteau2.transform.position - poteau1.transform.position;
    }


    void Update()
    {
        if (usingTyrolienne)
        {
            // Si le personnage peut utiliser la tyrolienne
            if (usingTyrolienne && player.transform.position.x < poteau2.transform.position.x && player.transform.position.x > poteau1.transform.position.x && Detection.canUseZipline)
            {
                // Tout d'abord on retire la gravité du personnage 
                rb.gravityScale = 0;
                character.noControl = true;

                // Si le joueur décide de sauter sur la tyrolienne
                if (controls.Personnage.Sauter.WasPressedThisFrame() && rb.velocity.x > 0)
                {
                    // On le fait sauter 
                    timer = 0;
                    character.Jump();
                    character.noControl = false;
                    usingTyrolienne = false;
                    noAirControl = true;
                    rb.gravityScale = stockageGravity;
                }

                // Gain de vitesse de la tyrolienne 
                else if (speedTyrolienne < speedLimit)
                {
                    if(rb.velocity.x < 0)
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
                usingTyrolienne = false;
                character.noControl = false;
                rb.gravityScale = stockageGravity;
            }
        }
        
        else if (noAirControl)
        {
            // Tout ce qui concerne l'absence d'air control 
            timer += Time.deltaTime;

            if (timer > 0.6f)
            {
                noAirControl = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        usingTyrolienne = true;
        speedTyrolienne = rb.velocity.x;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        usingTyrolienne = false;
        character.noControl = false;
        rb.gravityScale = stockageGravity;
    }
}
