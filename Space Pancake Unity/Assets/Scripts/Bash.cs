using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : MonoBehaviour
{

    [Header("Detection")]
    private bool canUseSerpe;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color couleurDetection;
    [SerializeField] Color couleurNonDetection;


    void Update()
    {
        if (canUseSerpe)
        {
            spriteRenderer.color = couleurDetection;
        }
        else
        {
            spriteRenderer.color = couleurNonDetection;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        canUseSerpe = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canUseSerpe = false;
    }
}
