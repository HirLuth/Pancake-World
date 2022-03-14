using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeRebondissant : MonoBehaviour
{
    public Character character;
    public Rigidbody2D rbCharacter;
    [SerializeField] float smallBounceForce;
    [SerializeField] float highBounceMultiplicator;
    [SerializeField] float timerForLowBounce;
    [SerializeField] float limitTimerForLowBounce;
    [SerializeField] private bool isOnThePancake;
    

    private void Update()
    {
        if (isOnThePancake)
        {
            timerForLowBounce += Time.deltaTime;
            if (timerForLowBounce > limitTimerForLowBounce)
            {
                rbCharacter.velocity = Vector2.up * smallBounceForce;
            }
        }
        else
        {
            timerForLowBounce = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        isOnThePancake = true; 
        character.jumpForce *= highBounceMultiplicator;
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        character.jumpForce /= highBounceMultiplicator;
        isOnThePancake = false;
    }
    
}
    
    

