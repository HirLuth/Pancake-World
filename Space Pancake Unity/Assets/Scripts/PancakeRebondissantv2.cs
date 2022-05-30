using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeRebondissantv2 : MonoBehaviour
{
    public Rigidbody2D rbCharacter;
    public Animator animatorSelf;
    [SerializeField] float bounceForce;
    [SerializeField] float timerBounce;
    [SerializeField] float limitTimerBounce;
    [SerializeField] private float stockageVelocityX;
    [SerializeField] private bool isOnThePancake;
    [SerializeField] private LayerMask layerPlayer;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private bool canStopPlayer = true;


    private void Start()
    {
        rbCharacter = Character.Instance.rb;
    }

    private void Update()
    {
        if (isOnThePancake)
        {
            animatorSelf.SetBool("IsBouncing",true);
            timerBounce += Time.deltaTime;
            stockageVelocityX = rbCharacter.velocity.x;
            if (canStopPlayer)
            {
                rbCharacter.bodyType = RigidbodyType2D.Static;
                canStopPlayer = false;
            }
            

            if (timerBounce > limitTimerBounce)
            {
                Debug.Log("oui");
                rbCharacter.bodyType = RigidbodyType2D.Dynamic;
                var vector = new Vector2(stockageVelocityX, bounceForce);
                rbCharacter.velocity = vector;
                //Debug.Log(Vector2.Angle(vector.normalized, Vector2.down));
                //particleSystem.main.startRotation.constant = Vector2.Angle(vector.normalized, Vector2.down);
            }
            
        }
        else
        {
            animatorSelf.SetBool("IsBouncing",false);
            timerBounce = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            Character.Instance.particuleImpulsionPancake.Play();
            isOnThePancake = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character") && rbCharacter.bodyType != RigidbodyType2D.Static)
        {
            isOnThePancake = false;
            canStopPlayer = true;
        }
    }
}