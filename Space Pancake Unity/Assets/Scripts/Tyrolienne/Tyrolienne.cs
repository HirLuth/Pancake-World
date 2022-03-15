using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tyrolienne : MonoBehaviour
{
    [Header("Paramètres Importants")] 
    public float start;
    
    public Vector2 cible;
    private float speedTyrolienne;
    public static bool usingTyrolienne;
    public float speedLimit;
    public float acceleration;

    [Header("Player")]
    public GameObject player;
    private Rigidbody2D rb;
    private float stockageGravity;


    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        stockageGravity = rb.gravityScale;
    }


    void Update()
    {
        if (usingTyrolienne && (player.transform.position.x < cible.x && player.transform.position.x > start) && Detection.canUseZipline)
        {
            rb.gravityScale = 0;

            if (speedTyrolienne < speedLimit)
            {
                speedTyrolienne += Time.deltaTime * acceleration;
            }

            Vector2 direction = new Vector2(cible.x - player.transform.localPosition.x, cible.y - player.transform.localPosition.y);
            rb.velocity = direction.normalized * speedTyrolienne;
        }

        else if (player.transform.position.x >= cible.x || player.transform.position.x <= start)
        {
            rb.gravityScale = stockageGravity;
            Detection.canUseZipline = false;
            usingTyrolienne = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        usingTyrolienne = true;

        speedTyrolienne = rb.velocity.x;
    }
}
