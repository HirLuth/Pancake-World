using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneRepriseRail : MonoBehaviour
{
    public int pointReprise;

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!Mover.Instance.go && other.tag == "Character")
        {
            Mover.Instance.currentSeg = pointReprise;
            Mover.Instance.go = true;
        }
    }
}
