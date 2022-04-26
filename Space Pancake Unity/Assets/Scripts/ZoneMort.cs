using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneMort : MonoBehaviour
{

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Character"))
            {
                EventManager.Instance.Death(); 
            } 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Character"))
            {
                EventManager.Instance.Death(); 
            } 
        }
}
