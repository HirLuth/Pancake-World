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
   [SerializeField] private SpriteRenderer spriteRendererSelf;
   [SerializeField] private Animator animatorself;
   [SerializeField] private Vector2 stockagePosition;
   [SerializeField] private bool playerAsSteppedOn;
   [SerializeField] private bool isDestroying;
   [SerializeField] private float fallingSpeed;
   [SerializeField] private float timerToFall;
   [SerializeField] private float timeBeforFalling;
   [SerializeField] private float margeDetection;
   [SerializeField] private float colliderSize;
   [SerializeField] private float timerToDestroy;
   [SerializeField] private float timeBeforDestroy;
   [SerializeField] private float timeToRespawn;

   private void Awake()
   {
      playerAsSteppedOn = false;
      rbSelf.bodyType = RigidbodyType2D.Kinematic;
   }

   private void Start()
   {
      player = Character.Instance.gameObject;
      stockagePosition = transform.position;

   }

   private void Update()
   {
      if (isDestroying)
      {
         colliderSelf.enabled = false;
         rbSelf.velocity = Vector2.zero;
         animatorself.SetBool("IsDestroying",true);
         timerToDestroy += Time.deltaTime;
         if (timerToDestroy >= timeBeforDestroy)
         {
            spriteRendererSelf.enabled = false;
         }

         if (timerToDestroy >= timeToRespawn - timeBeforDestroy)
         {
            transform.position = stockagePosition;
            spriteRendererSelf.enabled = true;
            animatorself.SetBool("IsRespawning", true);
            animatorself.SetBool("IsDestroying",false);
            playerAsSteppedOn = false;
            if (timerToDestroy >= timeToRespawn)
            {
               animatorself.SetBool("IsRespawning", false);
               colliderSelf.enabled = true;
               isDestroying = false;
            }
         }
      }
      else
      {
         if (player.transform.position.y >= transform.position.y + (colliderSize/2) + (player.transform.localScale.y/2) + margeDetection)
         {
            triggerZone.enabled = true;
         }
         if (player.transform.position.y <= transform.position.y + (colliderSize/2) + (player.transform.localScale.y/2))
         {
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
      
      
      
      
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Character"))
      {
         playerAsSteppedOn = true;
         timerToDestroy = 0;
      }
   }

   public void Destruction()
   {
      isDestroying = true;
   }
}


