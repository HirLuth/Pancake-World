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
   [SerializeField] private bool playerAsSteppedOn;
   [SerializeField] private float fallingSpeed;
   [SerializeField] private float timerToFall;
   [SerializeField] private float timeBeforFalling;
   [SerializeField] private float margeDetection;

   private void Awake()
   {
      playerAsSteppedOn = false;
   }

   private void Update()
   {
      if (player.transform.position.y >= transform.position.y + (transform.localScale.y/2) + (player.transform.localScale.y/2) + margeDetection)
      {
         colliderSelf.enabled = true;
      }
      if (player.transform.position.y <= transform.position.y + (transform.localScale.y/2) + (player.transform.localScale.y/2))
      {
         colliderSelf.enabled = false;
      }
      if (playerAsSteppedOn)
      {
         timerToFall += Time.deltaTime;
         if (timerToFall >= timeBeforFalling)
         {
            transform.Translate(Vector3.down*fallingSpeed);
         }
      }
   }

   private void OnCollisionEnter2D(Collision2D other)
   {
      playerAsSteppedOn = true;
   }
}


