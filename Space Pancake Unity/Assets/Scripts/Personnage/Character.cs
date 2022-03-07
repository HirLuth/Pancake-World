using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    [Header("Inputs")]
    private PlayerControls controls;
    protected bool moveLeft;
    protected bool moveRight;
    private bool run;

    
    [Header("Physics")] 
    public Rigidbody2D rb;

    
    [Header("Déplacements")] 
    public AnimationCurve movementsCurve;
    public float vitesseMouvementsCurve;
    public float vitesseDecelerationCurve;
    public float vitesseDemiTourCurve;
    private float abscisseMovementsCurve;
    public float speed = 7f;
    private float direction;
    private bool stopDemiTourWalk;

    
    [Header("Course")] 
    public AnimationCurve runCurve;
    public float vitesseRunCurve;
    public float vitesseRunDecelerationCurve;
    private float abscisseRunCurve;
    private bool running;
    public float runSpeed = 10f;
    private bool demiTourRun;



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

    public void Start()
    {
        controls.Personnage.MoveLeft.started += ctx => moveLeft = true;
        controls.Personnage.MoveLeft.canceled += ctx => moveLeft = false;

        controls.Personnage.MoveRight.started += ctx => moveRight = true;
        controls.Personnage.MoveRight.canceled += ctx => moveRight = false;

        controls.Personnage.Run.performed += ctx => run = true;
        controls.Personnage.Run.canceled += ctx => run = false;

    }



    private void Update()
    {
        // Pour savoir la direction du personnage
        if (moveLeft)
        {
            direction = -1;
        }
        else if (moveRight)
        {
            direction = 1;
        }
       

        MoveCharacter();
    }
    


    void MoveCharacter()
    {
        // Si le joueur marche
        if (!run && !running)
        {
            // Si le joueur fait demi-tour en marchant 
            if (((moveLeft && rb.velocity.x > 0) || (moveRight && rb.velocity.x < 0)) && !stopDemiTourWalk)
            {
                abscisseMovementsCurve -= Time.deltaTime * vitesseDemiTourCurve;

                if (abscisseMovementsCurve <= 0)
                {
                    abscisseMovementsCurve = 1;
                    stopDemiTourWalk = true;
                }

                if (moveLeft)
                {
                    rb.velocity = new Vector2(movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);
                }
                else
                {
                    Debug.Log(moveRight);
                    rb.velocity = new Vector2(-1 * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);
                }
            }

            else
            {
                // Si le joueur appuie sur une touche de déplacement
                if ((moveLeft || moveRight) && abscisseMovementsCurve < 1)
                {
                    stopDemiTourWalk = false;
                    abscisseMovementsCurve += Time.deltaTime * vitesseMouvementsCurve;
                }

                // Si le joueur s'arrête de bouger
                else if (abscisseMovementsCurve > 0)
                {
                    stopDemiTourWalk = false;
                    abscisseMovementsCurve -= Time.deltaTime * vitesseDecelerationCurve;
                }

                rb.velocity = new Vector2(Mathf.Sign(direction) * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);

                abscisseRunCurve = abscisseMovementsCurve * 0.7f;  // Permet au personnage de pas ralentir quand il commence à courir
            }
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
            abscisseMovementsCurve = 1; // Pour garder la courbe de la marche à son max et donc avoir une transition entre les deux smooth

            // Si le joueur bouge
            if ((moveLeft || moveRight) && abscisseRunCurve < 1)
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
    }
}
