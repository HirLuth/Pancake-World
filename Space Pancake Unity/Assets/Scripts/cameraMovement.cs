using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Transform characterPosition;

    public float speedX = 2.0f;
    public float speedY = 2.0f;

    void FixedUpdate()
    {
        float interpolationX = speedX * Time.deltaTime;
        float interpolationY = speedY * Time.deltaTime;

        Vector3 position = this.transform.position;

        position.x = Mathf.Lerp(transform.position.x, characterPosition.position.x, interpolationX);

        transform.position = position;
    }
}
