using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaïsWallDetector : MonoBehaviour
{
    [SerializeField] private MaïsAReaction maïsAReaction;
    [SerializeField] private GameObject maïsParent;
    private void OnTriggerEnter2D(Collider2D other)
    {
        maïsAReaction.ReintialiseWhenGetOut();
        maïsAReaction.isInDestroyingAnmation = true;
    }
}
