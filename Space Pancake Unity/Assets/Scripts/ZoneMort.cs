using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneMort : MonoBehaviour
{
    [SerializeField] private bool isWater;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Character") && !EventManager.Instance.isDead)
            {
                if (isWater)
                {
                    EventManager.Instance.Death(true);  
                }
                else
                {
                    EventManager.Instance.Death(false);
                }
                
            } 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Character") && !EventManager.Instance.isDead)
            {
                if (isWater)
                {
                    EventManager.Instance.Death(true);  
                }
                else
                {
                    EventManager.Instance.Death(false);
                }
            } 
        }
}
