using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : MonoBehaviour
{

    [Header("Inputs")]
    private PlayerControls controls;

    [Header("Detection")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color couleurDetection;
    [SerializeField] Color couleurNonDetection;
    private bool canUseSerpe;

    [Header("Autres")]
    public GameObject arrow;
    private Vector2 direction;
    public float force;
    [SerializeField] Rigidbody2D rb;
    public bool usingSerpe;


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

            if (controls.Personnage.Serpe.WasPerformedThisFrame())
            {
                Time.timeScale = 0;
                usingSerpe = true;
                arrow.SetActive(true);
            }

            else if (controls.Personnage.Serpe.WasReleasedThisFrame())
            {
                Time.timeScale = 1;
                arrow.SetActive(false);
                usingSerpe = false;

                direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") + 0.2f);
                Debug.Log(direction);
                rb.velocity = direction * force;
            }
        }


        else
        {
            spriteRenderer.color = couleurNonDetection;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        canUseSerpe = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canUseSerpe = false;
    }
}
