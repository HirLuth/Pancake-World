using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using XInputDotNetPure;
using DG.Tweening;

public class Bash : MonoBehaviour
{

    [Header("Inputs")]
    private PlayerControls controls;
    public bool wantsToUseSerpe;


    [Header("Detection")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color couleurDetection;
    [SerializeField] Color couleurNonDetection;
    private bool canUseSerpe;


    [Header("Camera Shake")]
    [SerializeField] private float duration;
    [SerializeField] private float amplitude;

    [Header("Soldat Fraise")] 
    [HideInInspector] public bool soldatFraise;
    
    [Header("VFX")]
    [SerializeField] public GameObject vfx;
    [SerializeField] private ParticleSystem particleBash;


    [Header("Autres")]
    public GameObject arrow;
    private Vector2 direction;
    public float force;
    [SerializeField] Rigidbody2D rb;
    [HideInInspector] public bool usingSerpe;
    private float ralenti;
    private bool isOnGrab;


    [Header("Effets")]
    public Volume volume;
    private float distance;
    private float timerEffets;
    private float puissanceEffets;
    public bool exitEffects;
    private float debutRalenti = 1f;
    

    [Header("Vibrations")]
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;


    public void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        rb = Character.Instance.rb;
        particleBash.Stop();
    }

    private void OnEnable()
    {
        controls.Personnage.Enable();
    }

    private void OnDisable()
    {
        controls.Personnage.Disable();
    }



    void Update()
    {
        //Détection des contrôles
        if (controls.Personnage.Serpe.WasPerformedThisFrame())
        {
            wantsToUseSerpe = true;
        }
        
        else if(controls.Personnage.Serpe.WasReleasedThisFrame())
        {
            wantsToUseSerpe = false;
        }
        
        // Tout de qui concerne les effets spéciaux (le future)
        if (usingSerpe)
        {
            AudioManager.instance.SetSerpeSound();
            if (timerEffets <= 1)
            {
                timerEffets += Time.fixedDeltaTime * 5;
            }

            puissanceEffets = Mathf.Lerp(0, 1, timerEffets);
        }
        
        else if (exitEffects)
        {
            debutRalenti = 1;
            
            if (timerEffets > 0)
            {
                timerEffets -= Time.deltaTime * 2;
            }
            else
            {
                exitEffects = false;
            }

            puissanceEffets = Mathf.Lerp(0, 1, timerEffets);
            volume.weight = puissanceEffets;
            
        }

        if (soldatFraise && vfx != null)
            vfx.SetActive(false);

        else
            vfx.SetActive(true);

        
        // Fonctionnement pur de la serpe
        if ((canUseSerpe && wantsToUseSerpe || usingSerpe) && !soldatFraise)
        {
            UseSerpe();
        }
    }


    void UseSerpe()
    {
        if (!EventManager.Instance.isDead)
        {
            spriteRenderer.color = couleurDetection;

            // Si le joueur s'élance
            if (!wantsToUseSerpe)
            {
                // On sort du ralenti
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                exitEffects = true;
                timerEffets = 1;

                arrow.SetActive(false);   // On retire la fl�che
                usingSerpe = false;    // On redonne le contr�le du personnage
                Character.Instance.noControl = false;
                
                // Camera shake
                Camera.main.DOShakePosition(duration, amplitude);

                // On donne de l'�lan au personnage
                direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") + 0.23f);
                rb.velocity = direction.normalized * force;
                
                // Animations
                Character.Instance.anim.SetBool("isUsingSerpe2", true);
                Character.Instance.anim.SetBool("isGrabbing", false);
                
                //VFX
                particleBash.Play();
            }


            // Si le joueur reste appuy�
            else
            {
                distance = Mathf.Sqrt((Mathf.Pow(transform.position.x - Character.Instance.transform.position.x, 2)) + Mathf.Pow(transform.position.y - Character.Instance.transform.position.y, 2));
                
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                if (distance < 1)
                {
                    volume.weight = 0.5f * puissanceEffets;
                    Time.timeScale = 0.035f;
                }
                else
                {
                    volume.weight = 1f - distance * 0.5f * puissanceEffets;
                    Time.timeScale = 0.3f * distance / 10;
                }

                if (debutRalenti > puissanceEffets)
                {
                    debutRalenti -= Time.deltaTime * 25;
                    volume.weight = debutRalenti;
                }

                Character.Instance.noControl = true;
                usingSerpe = true;
                arrow.SetActive(true);

                Character.Instance.anim.SetBool("isGrabbing", true);
                Character.Instance.anim.SetBool("isUsingSerpe2", false);
            }
        }
        else
        {
            usingSerpe = false;
            timerEffets = 0;
            Character.Instance.noControl = false;
            canUseSerpe = false;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Character")
        {
            canUseSerpe = true;
            spriteRenderer.color = couleurDetection;
        }
    }

    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Character")
        {
            canUseSerpe = false;

            // Tout ce qui concerne l'arr�t du ralenti
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            volume.weight = 0;

            // Tout le reste 
            arrow.SetActive(false);
            Character.Instance.noControl = false;
            usingSerpe = false;
            spriteRenderer.color = couleurNonDetection;
            AudioManager.instance.BackToNeutralSoundEffect1();

            if (!exitEffects)
            {
                timerEffets = 0;
            }

            Character.Instance.anim.SetBool("isGrabbing", false);
        }
    }
}
