using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlateformV2 : MonoBehaviour
{
   public GameObject player;
   public Collider2D colliderSelf;
   public Rigidbody2D rbSelf;
   [SerializeField] private bool playerAsSteppedOn;
   [SerializeField] private float fallingSpeed;
   [SerializeField] private float timerToFall;
   [SerializeField] private float timeBeforFalling;
   [SerializeField] private float margeDetection;

   private void Awake()
   {
      playerAsSteppedOn = false;
      rbSelf.bodyType = RigidbodyType2D.Static;
   }

   private void Update()
   {
      if (rbSelf.velocity.x <= -fallingSpeed)
      {
         rbSelf.velocity = Vector2.down*fallingSpeed;
      }
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
            rbSelf.bodyType = RigidbodyType2D.Dynamic;
            rbSelf.velocity = Vector2.down * fallingSpeed;
         }
      }
      
   }

   private void OnCollisionEnter2D(Collision2D other)
   {
      playerAsSteppedOn = true;
   }
}

