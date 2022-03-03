using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    [Header("Physics")]
    public float detectionSol;
    public LayerMask ground;
    private Rigidbody2D rb;


    [Header("Déplacements")]
    public float speed;
    public float vitesseDemiTour;    // Vitesse à laquelle le personnage fait un demi tour
    public float vitesseDeceleration = 2.5f;   // Vitesse à laquelle le personnage s'arrête
    public AnimationCurve acceleration;
    public AnimationCurve demiTour;
    private Vector2 direction = Vector2.zero;
    private float progression = 0.0f;   // Variable utilisée pour mesurer la vitesse du personnage en ligne droite
    private float timer = 0.0f;   // Variable utilisée pour mesurer un demi-tour


    [Header("Course")]
    public float runSpeed;
    public float runVitesseDemiTour;    // Vitesse à laquelle le personnage fait un demi tour (en course)
    public float runVitesseDeceleration = 2.5f;   // Vitesse à laquelle le personnage s'arrête (en course)
    private bool demiTourCourse;   // Permet d'éviter que le joueur n'arrête de courir lors d'un demi-tour


    [Header("Saut")]
    public float jumpForce; // Et je parle pas du jeu ptdr XD lol mdr
    public float vitesseMontee;
    public float vitesseMonteeRapide;
    public AnimationCurve monteeDescente;
    public float ghostJumpDuree;
    private bool onGround;
    private bool jumping = false;
    private bool isStopping;
    public float timerJump = 0.0f;
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
    private KeyCode left = KeyCode.LeftArrow;
    private KeyCode right = KeyCode.RightArrow;
    private KeyCode course = KeyCode.LeftShift;
    private KeyCode jump = KeyCode.Space;


    [Header("Autres")] 
    public float duree = 0.1f;
    public float amplitude = 0.1f;
    public Branche branche;
    public cameraShake cameraShake;
    private bool checkForShake = true;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), 0);
        onGround = Physics2D.Raycast(transform.position, Vector2.down, detectionSol, ground);
        
        onWallLeft = Physics2D.Raycast(transform.position, Vector2.left, distanceDetection, ground);
        onWallRight = Physics2D.Raycast(transform.position, Vector2.right, distanceDetection, ground);


        if ((onWallLeft || onWallRight) || isWallJumping)
        {
            WallJump();
        }

        if (onGround)
        {
            timerGhostJump = 0;
        }

        if (branche.useSerpe)
        {
            timerJump = 0;
            jumping = false;
        }

        if(branche.useSerpe == false)
        {
            MoveCharacter(direction);

            if (onGround && Input.GetKeyDown(jump) || jumping)
            {
                Jump();
            }
            
            else if (timerGhostJump < ghostJumpDuree && Input.GetKeyDown(jump))
            {
                Jump();
            }
            
            else if (onGround == false && jumping == false && timerGhostJump < ghostJumpDuree)
            {
                timerGhostJump += Time.deltaTime;
            }
        }
    }



    // Fonction dans laquelle tous les déplacements au sol du personnage sont réalisés
    public void MoveCharacter(Vector2 dir)
    {
        if (onGround != true)
        {
            if(Mathf.Abs(rb.velocity.x) < speed)
            {
                if(Input.GetKey(left) || Input.GetKey(right))
                {
                    rb.AddForce(new Vector2(airControlForce * Mathf.Sign(dir.x), 0));
                }
            }

            else if(rb.velocity.x > speed & Input.GetKey(left))
            {
                rb.AddForce(new Vector2(airControlForce * Mathf.Sign(dir.x), 0));
            }

            else if (rb.velocity.x < speed & Input.GetKey(right))
            {
                rb.AddForce(new Vector2(airControlForce * Mathf.Sign(dir.x), 0));
            }
        }


        // Si le personnage ne court pas ou ne fait pas un demi-tour en pleine course
        else if (!Input.GetKey(course) & demiTourCourse == false & isStopping == false)
        {
            // Si le joueur avance dans une direction sans faire demi-tour 
            if ((Input.GetKey(left) || Input.GetKey(right)) & progression < 0.4f & timer == 0.5f)
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
            if (rb.velocity.x > 0.5f & Input.GetKey(left) & timer > 0)
            {
                RotateCharacter(dir);
                timer -= Time.deltaTime * vitesseDemiTour;
                rb.velocity = new Vector2(speed * demiTour.Evaluate(timer), rb.velocity.y);
            }

            // Si le joueur change de direction (de la droite vers la gauche)
            else if (rb.velocity.x < -0.5f & Input.GetKey(right) & timer > 0)
            {
                RotateCharacter(dir);
                timer -= Time.deltaTime * vitesseDemiTour;
                rb.velocity = new Vector2(-speed * demiTour.Evaluate(timer), rb.velocity.y);
            }

            // Si le joueur avance normalement sans faire de demi-tour
            else
            {
                timer = 0.5f;
                rb.velocity = new Vector2(Mathf.Sign(dir.x) * speed * acceleration.Evaluate(progression), rb.velocity.y);
            }
        }


        // Si le personnage court
        else
        {
            // Si le joueur avance dans une direction sans faire demi-tour 
            if ((Input.GetKey(left) || Input.GetKey(right)) && progression < 0.4f & timer == 0.5f)
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
            if (rb.velocity.x > 0.5f & Input.GetKey(left) & timer > 0)
            {
                RotateCharacter(dir);
                demiTourCourse = true;
                timer -= Time.deltaTime * runVitesseDemiTour;
                rb.velocity = new Vector2(runSpeed * demiTour.Evaluate(timer), rb.velocity.y);
            }

            // Si le joueur change de direction (de la droite vers la gauche)
            else if (rb.velocity.x < -0.5f & Input.GetKey(right) & timer > 0)
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
        if (Input.GetKey(jump))
        {
            timerJump += Time.deltaTime * vitesseMontee;
        }
        else
        {
            timerJump += Time.deltaTime * vitesseMonteeRapide;
        }

        if(timerJump < 1.5f)
        {
            jumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * monteeDescente.Evaluate(timerJump));
        }
        else
        {
            jumping = false;
            timerJump = 0;
        }
    }


    public void WallJump()
    {
        if (Input.GetKeyDown(jump) || isWallJumping)
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
            
            else
            {
                timerWallJump = 1;
                isWallJumping = false;
            } 
        }

        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -0.25f);
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
