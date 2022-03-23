using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeQuiDisparait : MonoBehaviour
{
    public GameObject player;
    public Collider2D triggerZone;
    public Collider2D colliderSelf;
    [SerializeField] private bool steppedOn;
    [SerializeField] private float timerToDisapear;
    [SerializeField] private float timer;
    [SerializeField] private float margeDetection;

    private void Awake()
    {
        timer = 0;
        steppedOn = false;
    }

    void Update()
    {
        if (player.transform.position.y >= transform.position.y + (transform.localScale.y/2) + (player.transform.localScale.y/2) + margeDetection)
        {
            colliderSelf.enabled = true;
            triggerZone.enabled = true;
        }
        if (player.transform.position.y <= transform.position.y + (transform.localScale.y/2) + (player.transform.localScale.y/2))
        {
            colliderSelf.enabled = false;
            triggerZone.enabled = false;
        }
        if (steppedOn)
        {
            if (timer >= timerToDisapear)
            {
                timer = 0;
                Destroy(this.gameObject); 
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            steppedOn = true;
        }
    }
}
