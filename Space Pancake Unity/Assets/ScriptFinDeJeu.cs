using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScriptFinDeJeu : MonoBehaviour
{
    [SerializeField] private bool endEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endEvent && Character.Instance.onGround)
        {
            Character.Instance.rb.velocity = new Vector2(5, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character.Instance.noControl = true;
        endEvent = true;
    }
}
