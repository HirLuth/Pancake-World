using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using XInputDotNetPure;

public class Bash : MonoBehaviour
{

    [Header("Inputs")]
    private PlayerControls controls;
    private bool wantsToUseSerpe;


    [Header("Detection")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color couleurDetection;
    [SerializeField] Color couleurNonDetection;
    private bool canUseSerpe;


    [Header("Camera Shake")]
    [SerializeField] private float duration;
    [SerializeField] private float amplitude;


    [Header("Autres")]
    public GameObject arrow;
    private Vector2 direction;
    public float force;
    [SerializeField] Rigidbody2D rb;
    [HideInInspector] public bool usingSerpe;
    private float ralenti;

    [Header("Effets")]
    public Volume volume;
    private float distance;
    private float timerEffets;
    private float puissanceEffets;
    private bool exitEffects;
    

    [Header("Vibrations")]
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;


    public void Awake()
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



    void Update()
    {
        if (controls.Personnage.Serpe.WasPerformedThisFrame())
        {
            wantsToUseSerpe = true;
        }
        else if(controls.Personnage.Serpe.WasReleasedThisFrame())
        {
            wantsToUseSerpe = false;
        }

        if (usingSerpe)
        {
            if (timerEffets <= 1)
            {
                timerEffets += Time.fixedDeltaTime * 5;
            }

            puissanceEffets = Mathf.Lerp(0, 1, timerEffets);
        }
        
        else if (exitEffects)
        {
            if (timerEffets > 0)
            {
                timerEffets -= Time.deltaTime * 2;
            }
            else
            {
                exitEffects = false;
            }

            puissanceEffets = Mathf.Lerp(0, 1, timerEffets);
            volume.weight = timerEffets;
        }

        if ((canUseSerpe && wantsToUseSerpe) || usingSerpe)
        {
            UseSerpe();
        }
    }


    void UseSerpe()
    {
        if (!EventManager.Instance.isDead)
        {
            spriteRenderer.color = couleurDetection;

            // Si le joueur s'�lance
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

                // On donne de l'�lan au personnage
                direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") + 0.23f);
                rb.velocity = direction * force;

                //CinemachineShake.Instance.Shake(duration, amplitude);   // Camera shake
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
                    Time.timeScale = distance * 0.035f;
                }
                
                Character.Instance.noControl = true;
                usingSerpe = true;
                arrow.SetActive(true);
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

        if (!exitEffects)
        {
            timerEffets = 0;
        }
    }
}
