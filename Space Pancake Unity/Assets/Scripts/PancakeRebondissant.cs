using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PancakeRebondissant : MonoBehaviour
{
    public Character character;
    public Rigidbody2D rbCharacter;
    public GameObject gameObjectCharacter;
    public Animator animatorSelf;
    [SerializeField] float smallBounceForce;
    [SerializeField] float highBounceMultiplicator;
    [SerializeField] float timerForLowBounce;
    [SerializeField] float limitTimerForLowBounce;
    [SerializeField] private float margeDetectionCoté;
    [SerializeField] private bool isOnThePancake;
    [SerializeField] private LayerMask layerPlayer;
    

    private void Update()
    {
        if (isOnThePancake)
        {
            animatorSelf.SetBool("IsBouncing",true);
            timerForLowBounce += Time.deltaTime;
            if (timerForLowBounce > limitTimerForLowBounce)
            {
                rbCharacter.velocity = Vector2.up * smallBounceForce;
            }
        }
        else
        {
            animatorSelf.SetBool("IsBouncing",false);
            timerForLowBounce = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            isOnThePancake = true; 
            character.jumpForce *= highBounceMultiplicator;
        }
        
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            character.jumpForce /= highBounceMultiplicator;
            isOnThePancake = false;
        }
    }
    
}
    
    

