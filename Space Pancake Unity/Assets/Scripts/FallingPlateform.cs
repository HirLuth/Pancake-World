using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class FallingPlateform : MonoBehaviour
{
   public GameObject player;
   public Collider2D colliderSelf;
   public Collider2D triggerZone;
   public Rigidbody2D rbSelf;
   [SerializeField] private bool playerAsSteppedOn;
   [SerializeField] private float fallingSpeed;
   [SerializeField] private float timerToFall;
   [SerializeField] private float timeBeforFalling;
   [SerializeField] private float margeDetection;
   [SerializeField] private float colliderSize;

   private void Awake()
   {
      playerAsSteppedOn = false;
      rbSelf.bodyType = RigidbodyType2D.Kinematic;
   }

   private void Update()
   {
      if (player.transform.position.y >= transform.position.y + (colliderSize/2) + (player.transform.localScale.y/2) + margeDetection)
      {
         colliderSelf.enabled = true;
         triggerZone.enabled = true;
      }
      if (player.transform.position.y <= transform.position.y + (colliderSize/2) + (player.transform.localScale.y/2))
      {
         colliderSelf.enabled = false;
         triggerZone.enabled = false;
      }
      if (playerAsSteppedOn)
      {
         timerToFall += Time.deltaTime;
         if (timerToFall >= timeBeforFalling)
         {
            rbSelf.velocity = Vector2.down * fallingSpeed;
         }
      }
      
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Character"))
      {
         playerAsSteppedOn = true;
      }
   }
}


