using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Bash : MonoBehaviour
{

    [Header("Inputs")]
    private PlayerControls controls;

    [Header("Detection")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color couleurDetection;
    [SerializeField] Color couleurNonDetection;
    private bool canUseSerpe;

    [Header("Camera Shake")]
    [SerializeField] private cameraShake camera;
    [SerializeField] private float duration;
    [SerializeField] private float amplitude;

    [Header("Autres")]
    public GameObject arrow;
    private Vector2 direction;
    public float force;
    [SerializeField] Rigidbody2D rb;
    public static bool usingSerpe;
    private float ralenti;
    
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
        if (canUseSerpe)
        {
            spriteRenderer.color = couleurDetection;
            

            // Si le joueur s'élance
            if (controls.Personnage.Serpe.WasReleasedThisFrame())
            {
                // On sort du ralenti
                ralenti = 1;
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;

                arrow.SetActive(false);   // On retire la flèche
                usingSerpe = false;    // On redonne le contrôle du personnage

                // On donne de l'élan au personnage
                direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") + 0.2f);
                rb.velocity = direction * force;
                
                CinemachineShake.Instance.Shake(duration, amplitude);   // Camera shake
            }
            
            
            // Si le joueur reste appuyé
            else if (controls.Personnage.Serpe.WasPerformedThisFrame() || usingSerpe)
            {
                if (ralenti > 0)
                {
                    ralenti -= Time.deltaTime * 20;
                }

                Time.timeScale = 0.05f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                usingSerpe = true;
                arrow.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        canUseSerpe = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canUseSerpe = false;

        // Tout ce qui concerne l'arrêt du ralenti
        ralenti = 1;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Tout le reste 
        arrow.SetActive(false);
        usingSerpe = false;
        spriteRenderer.color = couleurNonDetection;
    }
}
