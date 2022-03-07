using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branche : MonoBehaviour
{
    [Header("Autres")]
    public GameObject character;
    public GameObject branche;
    public CharacterControler characterControler;
    public Rigidbody2D rbCharacter;

    [Header("Variables")] 
    public float dureeNoAirControl = 0.32f;
    private float timerAirControl;
    public float force = 20f;
    private Vector2 direction;
    private Vector2 posBranche;
    public bool useSerpe;
    private bool canUseSerpe;


    [Header("Camera Shake")]
    public cameraShake cameraShake;
    public float duration = 0.5f;
    public float magnitude = 0.05f;
    
    
    [Header("Inputs")]
    private PlayerControls controls;
    private bool serpe;
    
    private void Awake()
    {
        controls = new PlayerControls();

        controls.Personnage.Serpe.started += ctx => serpe = true;
        controls.Personnage.Serpe.canceled += ctx => serpe = false;
    }
    
    private void OnEnable()
    {
        controls.Personnage.Enable();
    }


    public void Start()
    {
        rbCharacter = character.GetComponent<Rigidbody2D>();
        posBranche = branche.transform.position;
    }

    public void Update()
    {
        if (characterControler.onGround)
        {
            canUseSerpe = false;
        }
        
        if (canUseSerpe & serpe & useSerpe == false) 
        {
            Vector2 posChara = character.transform.position;
            direction = new Vector2(Mathf.Abs(posBranche.x) - Mathf.Abs(posChara.x), Mathf.Abs(posBranche.y) - Mathf.Abs(posChara.y));
            useSerpe = true;

            characterControler.jumping = false;
            characterControler.timerJump = 0;
            timerAirControl = 0;
            
            rbCharacter.velocity = new Vector2(direction.normalized.x, direction.normalized.y + 0.5f) * force;

            StartCoroutine(cameraShake.Shake(duration, magnitude));
            serpe = false;
        }
        
        if (dureeNoAirControl > timerAirControl & useSerpe == true)
        {
            timerAirControl += Time.deltaTime;
        }
        else if (dureeNoAirControl <= timerAirControl & useSerpe == true)
        {
            useSerpe = false;
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
