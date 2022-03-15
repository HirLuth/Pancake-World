using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FallingPlateform : MonoBehaviour
{
   public GameObject player;
   public Rigidbody2D rbPlayer;
   [SerializeField] private bool playerAsSteppedOn;
   [SerializeField] private float fallingSpeed;
   [SerializeField] private float timerToFall;
   [SerializeField] private float timeBeforFalling;

   private void Awake()
   {
      playerAsSteppedOn = false;
   }

   private void Update()
   {
      if (playerAsSteppedOn)
      {
         timerToFall += Time.deltaTime;
         if (timerToFall == timeBeforFalling)
         {
            
         }
      }
   }

   private void OnCollisionEnter2D(Collision2D other)
   {
      playerAsSteppedOn = true;
   }
}


