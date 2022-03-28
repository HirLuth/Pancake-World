using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldatFraise : MonoBehaviour
{
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            Debug.Log("mort");  
        }
    }
}
