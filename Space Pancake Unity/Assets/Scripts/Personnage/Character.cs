using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    [Header("Inputs")]
    private PlayerControls controls;
    private bool moveLeft;
    private bool moveRight;
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
    public float vitesseRunDemiTourCurve;
    private float abscisseRunCurve;
    private bool running;
    public float runSpeed = 10f;
    private bool stopDemiTourRun;



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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveLeft = true;
            moveRight = false;
            direction = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveRight = true;
            moveLeft = false;
            direction = 1;
        }

        /* if (moveLeft)
        {
            direction = -1;
        }
        if (moveRight)
        {
            direction = 1;
        } */
       
        MoveCharacter();
    }
    


    void MoveCharacter()
    {
        // Si le joueur fait demi-tour...
        if (((moveLeft && rb.velocity.x > 0) || (moveRight && rb.velocity.x < 0)) && !stopDemiTourRun && !stopDemiTourWalk)
        {

            // ... en marchant 
            if(!running && !stopDemiTourWalk)
            {
                abscisseMovementsCurve -= Time.deltaTime * vitesseDemiTourCurve;

                rb.velocity = new Vector2(-direction * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);

                // Si le personnage a fini de faire demi-tour
                if (abscisseMovementsCurve <= 0.1f)
                {
                    abscisseMovementsCurve = 1;
                    stopDemiTourWalk = true;
                }
            }


            // ... en courant 
            else 
            {
                abscisseRunCurve -= Time.deltaTime * vitesseRunDemiTourCurve;

                rb.velocity = new Vector2(-direction * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);

                if(abscisseRunCurve <= 0.1f)
                {
                    Debug.Log(12);
                    abscisseRunCurve = 1;
                    stopDemiTourRun = true;
                }
            }

        }

        // Si le joueur marche
        else if (!run && !running)
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
            stopDemiTourRun = false;

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
    
    void Jump()
    {

    }
}
