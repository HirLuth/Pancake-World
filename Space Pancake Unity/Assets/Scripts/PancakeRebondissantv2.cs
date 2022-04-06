using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeRebondissantv2 : MonoBehaviour
{
    public Character character;
    public Rigidbody2D rbCharacter;
    public GameObject gameObjectCharacter;
    public Animator animatorSelf;
    [SerializeField] float bounceForce;
    [SerializeField] float timerBounce;
    [SerializeField] float limitTimerBounce;
    [SerializeField] private float stockageVelocityX;
    [SerializeField] private bool isOnThePancake;
    [SerializeField] private LayerMask layerPlayer;
    

    private void Update()
    {
        if (isOnThePancake)
        {
            animatorSelf.SetBool("IsBouncing",true);
            timerBounce += Time.deltaTime;
            stockageVelocityX = rbCharacter.velocity.x;
            rbCharacter.velocity = Vector2.zero;
            if (timerBounce > limitTimerBounce)
            {
                rbCharacter.velocity = new Vector2(stockageVelocityX, bounceForce);
            }
        }
        else
        {
            animatorSelf.SetBool("IsBouncing",false);
            timerBounce = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            isOnThePancake = true;
        }
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            isOnThePancake = false;
        }
    }
    
}