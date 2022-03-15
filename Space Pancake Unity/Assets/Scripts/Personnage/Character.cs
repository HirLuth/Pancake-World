using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using UnityEditorInternal;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Character: MonoBehaviour
{
    [Header("Inputs")]
    private PlayerControls controls;
    private bool moveLeft;
    private bool moveRight;
    private bool run;
    private bool jump;
    private bool wallJump;

    
    [Header("Déplacements")] 
    public AnimationCurve movementsCurve;  // Courbe de marche
    public float vitesseMouvementsCurve;   // Vitesse à laquelle on traverse la courbe (= acceleration)
    public float vitesseDecelerationCurve;   
    public float vitesseDemiTourCurve;   
    private float abscisseMovementsCurve;   // Abscisse utilisée pour lire cette courbe, elle évolue en fonction du temps et de la vitesse qu'on lui donne
    public float speed = 7f;   // Vitesse de déplacements
    private float direction;    // Direction que le joueur regarde
    private bool stopDemiTourWalk;    // Utilisé pour sortir de l'état de demi-tour

    
    [Header("Course")] 
    public AnimationCurve runCurve;   // Courbe de course
    public float vitesseRunCurve;   // Vitesse à laquelle on traverse la courbe (= acceleration)
    public float vitesseRunDecelerationCurve;
    public float vitesseRunDemiTourCurve;
    private float abscisseRunCurve;   // Abscisse utilisée pour lire la courbe de course, elle évolue en fonction du temps et de la vitesse qu'on lui donne
    private bool running;    // Utilisé pour éviter que le joueur sorte de l'état de course sans transition
    public float runSpeed;    // Vitesse de la course
    private bool stopDemiTourRun;    // Utilisé pour sortir de l'état de demi-tour
    private float stockageDemiTour;    // Permet de sortir de l'état de demi-tour avec une vitesse adaptée à l'inertie initale du pesonnage


    [Header("Saut")] 
    public AnimationCurve jumpCurve;
    public float vitesseJumpcurve;    // Vitesse à laquelle on parcourt cette courbe si on garde le bouton de saut enfoncé
    public float vitesseShortJumpAcceleration;    // Vitesse à laquelle on parcourt cette courbe si on lache le bouton de saut
    private float abscisseJumpCurve;    // Abscisse utilisée pour lire l'ordonnée actuelle de la courbe de saut
    private bool jumping;    // Permet de retourner dans la fonction saut
    public float jumpForce;    // Puissance du saut
    private bool onGround;    // Permet de vérifier si le personnage est au sol
    [SerializeField] float tailleRaycastGround;    // Longueur du raycast permettant de détecter le sol
    [SerializeField] LayerMask ground;


    [Header("AirControl")] 
    public float airControlForce;    // Puissance de l'air control
    public float noButtonForce;    // Resistance de l'air quand le joueur n'appuie sur aucune touche


    [Header("WallJump")] 
    public Vector2 directionWallJump;    // Direction dans laquelle va se faire le wall jump
    public float forceWallJump;    // Puissance du wall jump
    public float grabForceWall;    // Puissance de la résistence lorsque le personnage s'accorche aux murs
    private bool canWallJumpLeft;   // Un mur à gauche du personnage est détécté
    private bool canWallJumpRight;   // Un mur à droite du personnage est détécté
    [SerializeField] public float tailleRaycastWall;   // Longueur du raycast permettant de détecter le mur
    [SerializeField] public LayerMask wall;


    [Header("Animations")]
    public Animator anim;
    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    private bool isFalling;


    [Header("Autres")]
    public Rigidbody2D rb;


    // Tout ce qui concerne le controller
    private void Awake()
    {
        controls = new PlayerControls();
        controls.Personnage.MoveLeft.performed += ctx => moveLeft = true;
        controls.Personnage.MoveLeft.canceled += ctx => moveLeft = false;

        controls.Personnage.MoveRight.performed += ctx => moveRight = true;
        controls.Personnage.MoveRight.canceled += ctx => moveRight = false;

        controls.Personnage.Run.performed += ctx => run = true;
        controls.Personnage.Run.canceled += ctx => run = false;

        controls.Personnage.Sauter.started += ctx => jump = true;
        controls.Personnage.Sauter.canceled += ctx => jump = false;
        controls.Personnage.Sauter.canceled += ctx => wallJump = false;
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
        onGround = Physics2D.Raycast(transform.position, Vector2.down, tailleRaycastGround, ground);

        canWallJumpLeft = Physics2D.Raycast(transform.position, Vector2.left, tailleRaycastWall, ground);
        canWallJumpRight = Physics2D.Raycast(transform.position, Vector2.right, tailleRaycastWall, ground);


        if (controls.Personnage.MoveLeft.WasPressedThisFrame())
        {
            moveLeft = true;
            direction = -1;
        }

        if (controls.Personnage.MoveRight.WasPressedThisFrame())
        {
            moveRight = true;
            direction = 1;
        }

        if (controls.Personnage.Sauter.WasPressedThisFrame() && (canWallJumpLeft || canWallJumpRight))
        {
            wallJump = true;
        }


        rb.drag = 0;

        if (!Bash.usingSerpe && !Tyrolienne.usingTyrolienne)
        {
            // Lancement des différentes fonctions
            if (onGround)
            {
                isJumping = false;
                isFalling = false;
                MoveCharacter();
            }

            else
            {
                AirControl();
                if (canWallJumpLeft || canWallJumpRight)
                {
                    WallJump();
                }
            }

            if ((jump && onGround) || jumping)
            {
                Jump();
            }

            RotateCharacter();
        }
        else
        {
            jumping = false;
            abscisseJumpCurve = 0;
        }

        
        // Pour les animations
        if(rb.velocity.y < -0.1f)
        {
            isFalling = true;
            jumping = false;
        }
        else if (rb.velocity.y > 0.1f)
        {
            isFalling = false;
            isJumping = true;
        }

        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFalling", isFalling);
    }


    // Déplacements au sol du personnage
    void MoveCharacter()
    {
        // Si le joueur fait demi-tour...
        if (((moveLeft && rb.velocity.x > 0.1f) || (moveRight && rb.velocity.x < -0.1f)) && !stopDemiTourRun && !stopDemiTourWalk)
        {
            // ... en marchant 
            if (!running && !stopDemiTourWalk)
            {
                // On ralentir le personnage petit à petit
                abscisseMovementsCurve -= Time.deltaTime * vitesseDemiTourCurve;

                rb.velocity = new Vector2(-direction * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);

                // Si le personnage a fini de faire demi-tour
                if (abscisseMovementsCurve <= 0.1f)
                {
                    abscisseMovementsCurve = 0.5f;    // Pour ressortir du demi-tour avec la bonne vitesse
                    stopDemiTourWalk = true;
                }
            }

            // ... en courant 
            else
            {
                abscisseRunCurve -= Time.deltaTime * vitesseRunDemiTourCurve;    // On ralentir le personnage petit à petit
                stockageDemiTour += Time.deltaTime * vitesseRunDemiTourCurve;    // Pour adapter la vitesse du personnage lorsqu'il sort du demi-tour

                rb.velocity = new Vector2(-direction * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);

                // Lorsque le demi-tour est finit
                if(abscisseRunCurve <= 0.1f)
                {
                    abscisseRunCurve = stockageDemiTour;
                    stockageDemiTour = 0;
                    stopDemiTourRun = true;
                }
            }

        }

        // Si le joueur marche
        else if (!run && !running)
        {
            stopDemiTourWalk = false;
            
            // Si le joueur appuie sur une touche de déplacement
            if ((moveLeft || moveRight) && abscisseMovementsCurve < 0.5f)
            {
                abscisseMovementsCurve += Time.deltaTime * vitesseMouvementsCurve;
            }

            // Si le joueur s'arrête de bouger
            else if (abscisseMovementsCurve > 0)
            {
                abscisseMovementsCurve -= Time.deltaTime * vitesseDecelerationCurve;
            }

            rb.velocity = new Vector2(Mathf.Sign(direction) * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);

            abscisseRunCurve = abscisseMovementsCurve * 0.7f;  // Permet au personnage de pas ralentir quand il commence à courir
            
        }
        


        // Pour que la transition entre l'état de course et de marche soit smooth
        else if (!run && (moveLeft || moveRight))
        {
            // On baisse progressivement la velocity du joueur
            abscisseRunCurve -= Time.deltaTime * vitesseRunDecelerationCurve;

            rb.velocity = new Vector2(Mathf.Sign(direction) * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);
            
            // Si la velocity du personnage est inférieur ou égale à celle de la marche
            if (rb.velocity.x < speed)
            {
                running = false;
            }
        } 


        // Si le joueur court
        else
        {
            stopDemiTourRun = false;

            abscisseMovementsCurve = 0.5f; // Pour garder la courbe de la marche à son max et donc avoir une transition entre les deux smooth

            // Si le joueur bouge
            if ((moveLeft || moveRight) && abscisseRunCurve < 0.5f)
            {
                running = true;
                abscisseRunCurve += Time.deltaTime * vitesseRunCurve;
            }

            // Si le joueur arrête de courir sans enchaîner sur la marche
            else if (abscisseRunCurve > 0)
            {
                abscisseRunCurve -= Time.deltaTime * vitesseRunDecelerationCurve;
            }
            
            rb.velocity = new Vector2(Mathf.Sign(direction) * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);
        }


        // Pour les animations
        if(Mathf.Abs(rb.velocity.x) < 0.1f)
        {
            isWalking = false;
        }
        else
        {
            if(Mathf.Abs(rb.velocity.x) < speed)
            {
                isWalking = true;
                isRunning = false;
            }
            else
            {
                isRunning = true;
            }
        }
    }


    // Rotation du personnage (au sol)
    void RotateCharacter()
    {
        if ((canWallJumpLeft || canWallJumpRight) && !onGround)
        {
            transform.rotation = Quaternion.Euler(0, canWallJumpRight ? 180 : 0, 0);
        }
        else if (moveLeft || moveRight)
        {
            transform.rotation = Quaternion.Euler(0, moveLeft ? 180 : 0, 0);
        }
    }
    

    // Saut du personnage
    void Jump()
    {
        jumping = true;   // Pour retourner dans cette fonction à chaque update
        isJumping = true;    // Pour les animations
        
        // On test si le joueur continuer à appuyer sur la touche saut et donc le faire suater plus longtemps
        if (jump || abscisseJumpCurve > 0.6f)
        {
            abscisseJumpCurve += Time.deltaTime * vitesseJumpcurve;
        }
        
        // Si le joueur relâche
        else
        {
            abscisseJumpCurve += Time.deltaTime * vitesseShortJumpAcceleration;
        }

        // Si le saut est encore en cours
        if (abscisseJumpCurve < 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpCurve.Evaluate(abscisseJumpCurve) * jumpForce);
        }
        
        // Si le saut est terminé
        else
        {
            abscisseJumpCurve = 0;
            jump = false;
            jumping = false;
            
            // Pour les animations
            isFalling = true;
            isJumping = false;
        }
    }


    void AirControl()
    {
        if (moveLeft || moveRight)
        {
            // Si la vitesse du personnage ne doit pas dépasser celle de course
            if (running)
            { 
                // Si le joueur souhaite faire demi-tour à pleine vitesse (vers la gauche)
                if (moveLeft && rb.velocity.x >= 0)
                {
                    if (abscisseRunCurve > 0)
                    {
                        abscisseRunCurve -= Time.deltaTime;    // On dimine l'abscisse de la courbe de course pour évite que le personnage ne dérape trop à la fin si il a peu de vitesse
                    }
                    
                    direction = -1;
                    rb.AddForce(new Vector2(-1 * airControlForce, 0), ForceMode2D.Impulse);
                }

                // Si le joueur souhaite faire demi-tour à pleine vitesse (vers la droite)
                else if (moveRight && rb.velocity.x <= 0)
                {
                    if (abscisseRunCurve > 0)
                    {
                        abscisseRunCurve -= Time.deltaTime;    // On dimine l'abscisse de la courbe de course pour évite que le personnage ne dérape trop à la fin si il a peu de vitesse
                    }
                    
                    direction = 1;
                    rb.AddForce(new Vector2(airControlForce, 0), ForceMode2D.Impulse);
                }

                // Si le joueur n'est pas encore à pleine vitesse et va en ligne droite
                else if (Mathf.Abs(rb.velocity.x) < runSpeed)
                {
                    rb.AddForce(new Vector2(direction * airControlForce, 0), ForceMode2D.Impulse);
                }
            }

            // Si la vitesse du personnage ne doit pas dépasser celle de marche
            else
            {
                // Si le joueur n'est pas encore à pleine vitesse
                if (Mathf.Abs(rb.velocity.x) < speed)
                {
                    rb.AddForce(new Vector2(direction * airControlForce, 0), ForceMode2D.Impulse);
                }

                // Si le joueur souhaite changer de direction (gauche)
                else if (moveLeft && rb.velocity.x >= speed)
                {
                    direction = -1;
                    rb.AddForce(new Vector2(-1 * airControlForce, 0), ForceMode2D.Impulse);
                }

                // Si le joueur souhaite changer de direction (droite)
                else if (moveRight && rb.velocity.x <= -speed)
                {
                    direction = 1;
                    rb.AddForce(new Vector2(airControlForce, 0), ForceMode2D.Impulse);
                }
            }
        }
        else
        {
            rb.AddForce(new Vector2(Mathf.Sign(-rb.velocity.x) * noButtonForce, 0));
        }
    }

    
    void WallJump()
    {
        // si le joueur saute du mur
        if (wallJump)
        {
            // On arrête l'état de saut actuel
            jump = false;
            jumping = false;
            wallJump = false;
            rb.drag = 0;
            abscisseJumpCurve = 0;
            
            
            // Si le mur se trouve à gauche du personnage
            if (canWallJumpLeft)
            {
                direction = 1;
                rb.velocity = new Vector2(directionWallJump.x * forceWallJump, directionWallJump.y * forceWallJump);
            }
            
            // Si le mur se trouve à droite du personnage
            else
            {
                direction = -1;
                rb.velocity = new Vector2(-1 * directionWallJump.x * forceWallJump, directionWallJump.y * forceWallJump);
            }
        }
        
        // Si le joueur glisse sur le mur
        else
        {
            rb.drag = grabForceWall;
            //rb.AddForce(new Vector2(0, grabForceWall), ForceMode2D.Force);
        }
    }
}
