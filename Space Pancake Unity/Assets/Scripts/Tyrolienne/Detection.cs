using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public static bool canUseZipline;
    public static bool comeFromDown;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        
        
        if (other.gameObject.tag == "Character")
        {
            if (!comeFromDown)
            {
                canUseZipline = true;
            }
            
            else
            {
                comeFromDown = false;
            }
        }
    }
}
