using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PancakeTriggerHighBouncing : MonoBehaviour
{
    public KeyCode jumpButton = KeyCode.Space;
    public bool IsGoingToHighJump;
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("le joueur est dedans");
        if (Input.GetKeyDown(jumpButton))
        {
            IsGoingToHighJump = true;
        }

    }
    
}
