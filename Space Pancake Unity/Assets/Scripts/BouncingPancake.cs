using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BouncingPancake : MonoBehaviour
{
    public float lowBouncingForce = 1;
    public float highBouncingForce = 20;
    public Rigidbody2D rbEnteringTrigger;
    public PancakeTriggerHighBouncing pancakeTriggerHighBouncing;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        rbEnteringTrigger=other.GetComponent<Rigidbody2D>();
        if (pancakeTriggerHighBouncing.IsGoingToHighJump == true)
        {
            rbEnteringTrigger.velocity = new Vector2(rbEnteringTrigger.velocity.x, highBouncingForce);
            pancakeTriggerHighBouncing.IsGoingToHighJump = false;
        }
        else
        {
            rbEnteringTrigger.velocity = new Vector2(rbEnteringTrigger.velocity.x, lowBouncingForce); 
        }
        
    }
}
