using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneRepriseRail : MonoBehaviour
{
    public int pointReprise;
    public Mover mover;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!mover.go && other.tag == "Character")
        {
            mover.currentSeg = pointReprise;
            mover.go = true;
        }
    }
}
