using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera camera;
    private float multiplicateur;
    

    void Update()
    {
        multiplicateur = rb.velocity.x * 0.1f;
        camera.orthographicSize = 1;
        Debug.Log(camera.orthographicSize);
    }
}
