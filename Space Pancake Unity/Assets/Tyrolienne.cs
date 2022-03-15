using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyrolienne : MonoBehaviour
{
    public Vector2 cible;
    public float speedTyrolienne;
    public static bool usingTyrolienne;

    [Header("Player")]
    public GameObject player;
    private Rigidbody2D rb;
    private float stockage;


    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        stockage = rb.gravityScale;
    }


    void FixedUpdate()
    {
        if (usingTyrolienne == true && player.transform.position.x < cible.x)
        {
            rb.gravityScale = 0;

            Vector2 direction = new Vector2(cible.x - player.transform.localPosition.x, cible.y - player.transform.localPosition.y);
            rb.velocity = direction.normalized * speedTyrolienne;
        }

        else
        {
            rb.gravityScale = stockage;
            usingTyrolienne = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        usingTyrolienne = true;
    }
}
