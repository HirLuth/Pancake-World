using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeQuiDisparait : MonoBehaviour
{
    public GameObject player;
    public Collider2D triggerZone;
    public Collider2D colliderSelf;
    [SerializeField] private Animator animatorSelf;
    [SerializeField] private bool steppedOn;
    [SerializeField] private float timerToDisapear;
    [SerializeField] private float timer;
    [SerializeField] private float margeDetection;
    [SerializeField] private float colliderSizeY;
    [SerializeField] private string vrai;
    [SerializeField] private string faux;

    private void Awake()
    {
        timer = 0;
        steppedOn = false;
    }

    void Update()
    {
        if (player.transform.position.y >= transform.position.y + (colliderSizeY/2) + (player.transform.localScale.y/2) + margeDetection)
        {
            //colliderSelf.enabled = true;
            triggerZone.enabled = true;
        }
        if (player.transform.position.y <= transform.position.y + (colliderSizeY/2) + (player.transform.localScale.y/2))
        {
            //colliderSelf.enabled = false;
            triggerZone.enabled = false;
        }
        
        if (steppedOn)
        {
            if (timer >= timerToDisapear)
            {
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
            animatorSelf.SetBool("sucreIsdisapearing",true);
            steppedOn = true;
        }
    }
}
