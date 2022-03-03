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

        position.x = Mathf.Lerp(this.transform.position.x, characterPosition.position.x + 3, interpolationX);
        
        if (transform.position.x > 45)
        {
            position.y = Mathf.Lerp(this.transform.position.y, 4.5f, interpolationY);
        }
        else
        {
            position.y = Mathf.Lerp(this.transform.position.y, 1.53f, interpolationY);
        }

        this.transform.position = position;
    }
}
