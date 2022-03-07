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
    private float abscisseMovementsCurve;
    public float speed = 7f;
    private float direction;
    private float vitesseMaxMarche;

    
    [Header("Course")] 
    public AnimationCurve runCurve;
    public float vitesseRunCurve;
    public float vitesseRunDecelerationCurve;
    private float abscisseRunCurve;
    private bool running;
    public float runSpeed = 10f;



    private void Awake()
    {
        controls = new PlayerControls();
        controls.Personnage.MoveLeft.performed += ctx => moveLeft = true;
        controls.Personnage.MoveLeft.canceled += ctx => moveLeft = false;
        controls.Personnage.MoveRight.performed += ctx => moveRight = true;
        controls.Personnage.MoveRight.canceled += ctx => moveRight = false;
        controls.Personnage.Run.performed += ctx => run = true;
        controls.Personnage.Run.canceled += ctx => run = false;
    }
    
    private void OnEnable()
    {
        controls.Personnage.Enable();
    }


    private void Start()
    {
        vitesseMaxMarche = speed * runCurve.Evaluate(1);
    }

    private void Update()
    {
        // Pour savoir la direction du personnage
        if (moveLeft)
        {
            direction = -1;
        }
        if (moveRight)
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
            abscisseRunCurve = 0;
            if ((moveLeft || moveRight) && abscisseMovementsCurve < 1)
            {
                abscisseMovementsCurve += Time.deltaTime * vitesseMouvementsCurve;
            }
            else if (abscisseMovementsCurve > 0)
            {
                abscisseMovementsCurve -= Time.deltaTime * vitesseDecelerationCurve;
            }
        
            rb.velocity = new Vector2(Mathf.Sign(direction) * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);
        }
        

        else if (!run && (moveLeft || moveRight))
        {
            abscisseMovementsCurve = 1;
            abscisseRunCurve -= Time.deltaTime * vitesseRunDecelerationCurve;

            rb.velocity = new Vector2(Mathf.Sign(direction) * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);

            if (rb.velocity.x < speed)
            {
                running = false;
            }
        }

        // Si le joueur court
        else
        {
            abscisseMovementsCurve = 0;
            if ((moveLeft || moveRight) && abscisseRunCurve < 1)
            {
                running = true;
                abscisseRunCurve += Time.deltaTime * vitesseRunCurve;
            }
            else if (abscisseRunCurve > 0)
            {
                abscisseRunCurve -= Time.deltaTime * vitesseRunDecelerationCurve;
            }
            
            rb.velocity = new Vector2(Mathf.Sign(direction) * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);
        }
    }
}
