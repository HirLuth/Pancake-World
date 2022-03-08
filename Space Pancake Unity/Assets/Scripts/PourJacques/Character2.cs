using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2 : MonoBehaviour
{
    [Header("Inputs")]
    private PlayerControls controls;
    private bool moveLeft;
    private bool moveRight;
    private bool run;
    private bool jump;


    [Header("Physics")]
    public Rigidbody2D rb;


    [Header("D�placements")]
    public AnimationCurve movementsCurve;  // Courbe de marche
    public float vitesseMouvementsCurve;   // Vitesse � laquelle on traverse la courbe (= acceleration)
    public float vitesseDecelerationCurve;
    public float vitesseDemiTourCurve;
    private float abscisseMovementsCurve;   // Abscisse utilis�e pour lire cette courbe, elle �volue en fonction du temps et de la vitesse qu'on lui donne
    public float speed = 7f;   // Vitesse de d�placements
    private float direction;    // Direction que le joueur regarde
    private bool stopDemiTourWalk;    // Utilis� pour sortir de l'�tat de demi-tour


    [Header("Course")]
    public AnimationCurve runCurve;   // Courbe de course
    public float vitesseRunCurve;   // Vitesse � laquelle on traverse la courbe (= acceleration)
    public float vitesseRunDecelerationCurve;
    public float vitesseRunDemiTourCurve;
    private float abscisseRunCurve;   // Abscisse utilis�e pour lire la courbe de course, elle �olue en fonction du temps et de la vitesse qu'on lui donne
    private bool running;    // Utilis� pour �viter que le joueur sorte de l'�tat de course sans transition
    public float runSpeed;    // Vitesse de la course
    private bool stopDemiTourRun;    // Utilis� pour sortir de l'�tat de demi-tour
    private float stockageDemiTour;    // Permet de sortir de l'�tat de demi-tour avec une vitesse adapt�e � l'inertie initale du pesonnage


    [Header("Saut")]
    public AnimationCurve jumpCurve;
    public float vitesseJumpcurve;
    public float vitesseShortJumpAcceleration;
    private float abscisseJumpCurve;
    private bool jumping;
    public float jumpForce;
    private bool onGround;
    [SerializeField] float tailleRaycastGround;
    [SerializeField] LayerMask ground;


    [Header("AirControl")]
    public float airControlForce;
    public bool walkSpeed;   //   Pour �viter que le personnage aille trop vite 



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
        if (moveLeft)
        {
            direction = -1;
            stockageDemiTour = 0; 
        }
        if (moveRight)
        {
            direction = 1;
            stockageDemiTour = 0; 
        } 

        MoveCharacter();
    }



    void MoveCharacter()
    {
        // Si le joueur fait demi-tour...
        if (((moveLeft && rb.velocity.x > 0) || (moveRight && rb.velocity.x < 0)) && !stopDemiTourRun && !stopDemiTourWalk)
        {

            // ... en marchant 
            if (!running && !stopDemiTourWalk)
            {

                abscisseMovementsCurve -= Time.deltaTime * vitesseDemiTourCurve;

                Debug.Log(moveLeft);

                // Pour que le personnage continue � glisser dans direction oppos�e � la touche press�e
                if (moveLeft)
                {
                    rb.velocity = new Vector2(1 * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-1 * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);
                }
               

                // Si le personnage a fini de faire demi-tour
                if (abscisseMovementsCurve <= 0.1f)
                {
                    abscisseMovementsCurve = 0.5f;   // On met place l'abscisse de la courbe sur 0.5 pour une transition plus smooth
                    stopDemiTourWalk = true;    // Pour sortir de l'�tat de demi-tour
                }
            }


            // ... en courant 
            else
            {
                abscisseRunCurve -= Time.deltaTime * vitesseRunDemiTourCurve;
                stockageDemiTour += Time.deltaTime * vitesseRunDemiTourCurve;

                rb.velocity = new Vector2(-direction * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);

                // Si le personnage a finit son demi-tour
                if (abscisseRunCurve <= 0.1f)
                {
                    abscisseRunCurve = stockageDemiTour;    // Pour �viter qu'il reparte � 100% si il fait un d�rapage qui n'est pas � pleine vitesse
                    stockageDemiTour = 0;
                    stopDemiTourRun = true;
                }
            }

        }

        // Si le joueur marche
        else if (!run && !running)
        {
            // Si le joueur appuie sur une touche de d�placement
            if ((moveLeft || moveRight) && abscisseMovementsCurve < 0.5f)
            {
                stopDemiTourWalk = false;
                abscisseMovementsCurve += Time.deltaTime * vitesseMouvementsCurve;
            }

            // Si le joueur s'arr�te de bouger
            else if (abscisseMovementsCurve > 0)
            {
                stopDemiTourWalk = false;
                abscisseMovementsCurve -= Time.deltaTime * vitesseDecelerationCurve;
            }

            rb.velocity = new Vector2(Mathf.Sign(direction) * movementsCurve.Evaluate(abscisseMovementsCurve) * speed, rb.velocity.y);

            abscisseRunCurve = abscisseMovementsCurve * 0.7f;  // Permet au personnage de pas ralentir quand il commence � courir

        }



        // Pour que la transition entre l'�tat de course et de marche soit smooth
        else if (!run && (moveLeft || moveRight))
        {
            // On baisse progressivement la velocity du joueur
            abscisseRunCurve -= Time.deltaTime * vitesseRunDecelerationCurve;

            rb.velocity = new Vector2(Mathf.Sign(direction) * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);

            // Si la velocity du personnage est inf�rieur ou �gale � celle de la marche
            if (rb.velocity.x < speed)
            {
                running = false;
            }
        }


        // Si le joueur court
        else
        {
            abscisseMovementsCurve = 0.5f; // Pour garder la courbe de la marche � son max et donc avoir une transition entre les deux smooth
            stopDemiTourRun = false;

            // Si le joueur bouge
            if ((moveLeft || moveRight) && abscisseRunCurve < 0.5f)
            {
                running = true;
                abscisseRunCurve += Time.deltaTime * vitesseRunCurve;
            }

            // Si le joueur arr�te de courir sans encha�ner sur la marche
            else if (abscisseRunCurve > 0)
            {
                abscisseRunCurve -= Time.deltaTime * vitesseRunDecelerationCurve;
            }

            rb.velocity = new Vector2(Mathf.Sign(direction) * runCurve.Evaluate(abscisseRunCurve) * runSpeed, rb.velocity.y);
        }
    }
}


