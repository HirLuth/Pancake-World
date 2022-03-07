using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControler : MonoBehaviour
{
    [Header("Physics")]
    public float detectionSol;    // Longueur du raycast permettant de détecter le sol
    public LayerMask ground;  
    private Rigidbody2D rb;


    [Header("Déplacements")]
    public float speed;
    public float vitesseDemiTour;    // Vitesse à laquelle le personnage réalise son mouvement de demi-tour
    public float vitesseDeceleration = 2.5f;   // Vitesse à laquelle le personnage réalise son mouvement d'arrêt
    public AnimationCurve acceleration;
    public AnimationCurve demiTour;
    private Vector2 direction = Vector2.zero;   // Va stocker la direction regardée par le personnage
    private float progression = 0.0f;   // Variable utilisée pour mesurer la vitesse du personnage en ligne droite
    private float timer = 0.0f;   // Variable utilisée pour mesurer un demi-tour
    private bool isStopping;   // Empêche le personnage d'arrêter son animation d'arrêt lorsqu'il court


    [Header("Course")]
    public float runSpeed;
    public float runVitesseDemiTour;    // Vitesse à laquelle le personnage réalise son mouvement de demi-tour(en course)
    public float runVitesseDeceleration = 2.5f;   // Vitesse à laquelle le personnage s'arrête (en course)
    private bool demiTourCourse;   // Permet d'éviter que le joueur n'arrête de courir lors d'un demi-tour


    [Header("Saut")]
    public float jumpForce;  // Et je parle pas du jeu ptdr XD lol mdr
    public float vitesseMontee;   // Vitesse à laquelle la courbe de saut est traversée
    public float vitesseMonteeRapide;    // Vitesse à laquelle la courbe de saut est traversée si le joueur relâche le bouton
    public AnimationCurve monteeDescente;
    [HideInInspector] public bool onGround;   // Détecte si le personnage est en contact avec le sol
    [HideInInspector] public bool jumping = false;
    [HideInInspector] public float timerJump = 0.0f;


    [Header("Ghost Jump + Jump Buffer")]
    public float ghostJumpDuree;
    private bool canJumpBuffer;
    private bool isJumpBuffering;
    private float timerGhostJump;
    
    
    [Header("Wall Jump")] 
    public float wallJumpForce = 5;
    public float distanceDetection = 1f;
    public AnimationCurve wallJump;
    public float vitesseCourbeWallJump;
    private float timerWallJump = 1;
    private bool onWallLeft;
    private bool onWallRight;
    private bool isWallJumping;
    private float signe;


    [Header("AirControl")]
    public float airControlForce;
    private float vitesseMax;
    private float vitesseMaxRun;


    [Header("Inputs")]
    private PlayerControls controls;
    private bool run;
    private bool jump;
    private bool longJump;
    private bool serpe;
    private bool moveLeft;
    private bool moveRight;


    [Header("Autres")] 
    public float duree = 0.1f;
    public float amplitude = 0.1f;
    public Branche branche;
    public cameraShake cameraShake;
    private bool checkForShake = true;


    // Toute la partie dédiée aux contrôles
    private void Awake()
    {
        controls = new PlayerControls();

        controls.Personnage.Sauter.started += ctx => jump = true;
        controls.Personnage.Sauter.performed += ctx => longJump = true;
        controls.Personnage.Sauter.canceled += ctx => longJump = false;
        
        
        controls.Personnage.Run.performed += ctx => run = false;
        controls.Personnage.Run.canceled += ctx => run = true;
        controls.Personnage.MoveLeft.performed += ctx => moveLeft = true;
        controls.Personnage.MoveLeft.canceled += ctx => moveLeft = false;
        controls.Personnage.MoveRight.performed += ctx => moveRight = true;
        controls.Personnage.MoveRight.canceled += ctx => moveRight = false;
    }
    
    private void OnEnable()
    {
        controls.Personnage.Enable();
    }
    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // Définie la direction du joueur
        if (moveLeft)
        {
            direction = new Vector2(-1, 0);
        }
        else if (moveRight)
        {
            direction = new Vector2(1, 0);
        }
        else
        {
            direction = new Vector2(0, 0);
        }
        

        // Raycasts du saut 
        onGround = Physics2D.Raycast(transform.position, Vector2.down, detectionSol, ground);
        canJumpBuffer = Physics2D.Raycast(transform.position, Vector2.down, detectionSol+ 0.2f, ground);
        
        // Raycast du Wall Jump
        onWallLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceDetection, ground);
        onWallRight = Physics2D.Raycast(transform.position, Vector2.right, distanceDetection, ground);
        
        
        // Pour activer le jump buffering
        if (canJumpBuffer && jump && !onGround && jumping == false)
        {
            isJumpBuffering = true; 
        }

        // Déclenche le Wall Jump
        if (onWallLeft || onWallRight || isWallJumping)
        {
            WallJump();
        }
        
        // Si le joueur est au sol (remise à 0 des timers)
        if (onGround)
        {
            timerJump = 0;
            timerGhostJump = 0;
        }

        
        if(branche.useSerpe == false)
        {
            MoveCharacter(direction);

            if (onGround && jump || jumping)
            {
                Jump();
            }

            else if (onGround && isJumpBuffering)
            {
                isJumpBuffering = false;
                Jump();
            }
            
            else if (timerGhostJump < ghostJumpDuree && jump && !jumping)
            {
                Jump();
            }
            
            else if (onGround == false && jumping == false && timerGhostJump < ghostJumpDuree)
            {
                timerGhostJump += Time.deltaTime;
            }
        }
        else
        {
            timerJump = 0;
            jumping = false;
        }
    }


    /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (detectionSol));
    } */


    // Fonction dans laquelle tous les déplacements au sol du personnage sont réalisés
    public void MoveCharacter(Vector2 dir)
    {
        if (onGround != true)
        {
            if(Mathf.Abs(rb.velocity.x) < speed)
            {
                if(moveLeft || moveRight)
                {
                    rb.AddForce(new Vector2(airControlForce * Mathf.Sign(dir.x), 0));
                }
            }

            else if(rb.velocity.x > speed & moveLeft)
            {
                rb.AddForce(new Vector2(airControlForce * Mathf.Sign(dir.x), 0));
            }

            else if (rb.velocity.x < speed & moveRight)
            {
                rb.AddForce(new Vector2(airControlForce * Mathf.Sign(dir.x), 0));
            }
        }


        // Si le personnage marche ou ne fait pas un demi-tour en pleine course
        else if (run & demiTourCourse == false & isStopping == false)
        {
            // Si le joueur avance dans une direction sans faire demi-tour 
            if ((moveLeft || moveRight) & progression < 0.4f & timer == 0.5f)
            {
                progression += Time.deltaTime;
                RotateCharacter(dir);
            }

            // Si le joueur ne bouge plus ou fait demo-tour
            else if (progression > 0.01f & timer > 0.3f)
            {
                progression -= (Time.deltaTime * vitesseDeceleration);
            }


            // Si le joueur change de direction (de la gauche vers la droite)
            if (rb.velocity.x > 0.5f & moveLeft & timer > 0)
            {
                RotateCharacter(dir);
                timer -= Time.deltaTime * vitesseDemiTour;
                rb.velocity = new Vector2(speed * demiTour.Evaluate(timer), rb.velocity.y);
            }

            // Si le joueur change de direction (de la droite vers la gauche)
            else if (rb.velocity.x < -0.5f & moveRight & timer > 0)
            {
                RotateCharacter(dir);
                timer -= Time.deltaTime * vitesseDemiTour;
                rb.velocity = new Vector2(-speed * demiTour.Evaluate(timer), rb.velocity.y);
            }

            // Si le joueur avance normalement sans faire de demi-tour
            else
            {
                timer = 0.5f;
                if (rb.velocity.x > 0 || dir.x > 0.1f)
                {
                    rb.velocity = new Vector2(Mathf.Sign(dir.x) * speed * acceleration.Evaluate(progression), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-speed * acceleration.Evaluate(progression), rb.velocity.y);
                }
            }
        }


        // Si le personnage court
        else
        {
            // Si le joueur avance dans une direction sans faire demi-tour 
            if ((moveLeft || moveRight) && progression < 0.4f & timer == 0.5f)
            {
                RotateCharacter(dir);
                isStopping = false;
                progression += Time.deltaTime;
            }

            // Si le joueur ne bouge plus ou fait demi-tour
            else if (progression > 0.01f & timer > 0.3f)
            {
                isStopping = true;
                progression -= (Time.deltaTime * runVitesseDeceleration);
            }


            // Si le joueur change de direction (de la gauche vers la droite)
            if (rb.velocity.x > 0.5f & moveLeft & timer > 0)
            {
                RotateCharacter(dir);
                demiTourCourse = true;
                timer -= Time.deltaTime * runVitesseDemiTour;
                rb.velocity = new Vector2(runSpeed * demiTour.Evaluate(timer), rb.velocity.y);
            }

            // Si le joueur change de direction (de la droite vers la gauche)
            else if (rb.velocity.x < -0.5f & moveRight & timer > 0)
            {
                RotateCharacter(dir);
                demiTourCourse = true;
                timer -= Time.deltaTime * runVitesseDemiTour;
                rb.velocity = new Vector2(-runSpeed * demiTour.Evaluate(timer), rb.velocity.y);
            }

            // Si le joueur avance normalement sans faire de demi-tour
            else
            {
                if (rb.velocity.x > 0 || dir.x > 0.1f)
                {
                    rb.velocity = new Vector2(Mathf.Sign(dir.x) * runSpeed * acceleration.Evaluate(progression), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-runSpeed * acceleration.Evaluate(progression), rb.velocity.y);
                }
                
                demiTourCourse = false;
                timer = 0.5f;
            }
        }
    }


    // Fonction dans laquelle on change la rotation du personnage en fonction de là où il regarde
    public void RotateCharacter(Vector2 dir)
    {
        bool lookLeft;

        if (dir.x < 0.01f)
        {
            lookLeft = false;
        }
        else
        {
            lookLeft = true;
        }

        transform.rotation = Quaternion.Euler(0, lookLeft ? 0 : 180, 0);
    }

    
    public void Jump()
    {
        jump = false;
        // Si le joueur reste appuyé sur le bouton de saut
        if (longJump)
        {
            // On fait augmenter timerJump
            timerJump += Time.deltaTime * vitesseMontee;
        }
        
        // Si il relâche le bouton de saut
        else
        {
            // On fait augmenter timerJump (plus qu'au dessus)
            timerJump += Time.deltaTime * vitesseMonteeRapide;
        }
        
        
        // Se fait tant que le saut n'est pas fini
        if(timerJump < 1.5f)
        {
            // On fait monter le personnage et on met jumping en true pour repasser dans cette fonction
            jumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * monteeDescente.Evaluate(timerJump));
        }
        
        // Si le saut est terminé 
        else
        {
            // On met jumping en false pour ne plus repasser dans cette fonction
            jumping = false;
            timerJump = 0;
        }
    }


    public void WallJump()
    {
        // Pour stopper l'état de saut 
        jumping = false;
        timerJump = 0;
        
        // Quand le personnage saute du mur
        if (jump || isWallJumping)
        {
            if (timerWallJump > 0.1f)
            {
                
                if (onWallRight)
                {
                    signe = -1;
                }
                else if (onWallLeft)
                {
                    signe = 1;
                }
                
                isWallJumping = true;
                timerWallJump -= Time.deltaTime * vitesseCourbeWallJump;
                rb.velocity = new Vector2(signe * wallJumpForce/1.5f , wallJumpForce * wallJump.Evaluate(timerWallJump));
            }
            
            // On stop l'état de wall jump
            else
            {
                timerWallJump = 1;
                isWallJumping = false;
            } 
        }

        // Quand le personnage glisse sur le mur
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -0.25f);
            
            // Pour rotate le personnage
            if (onWallLeft)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
