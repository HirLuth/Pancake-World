using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptRoulottePamplemousse : MonoBehaviour
{
    [SerializeField] private Animator animatorSelf;

    private void OnTriggerEnter2D(Collider2D other)
    {
        animatorSelf.SetBool("OuvertureRoulotte",true);
    }
}
