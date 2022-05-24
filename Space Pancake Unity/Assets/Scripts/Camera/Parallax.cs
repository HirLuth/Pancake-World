using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPosX;

    public GameObject cam;
    public float parallaxEffect;
    

    private void Start()
    {
        startPosX = CameraMovements.Instance.transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        
        transform.position = new Vector3(startPosX, transform.position.y, transform.position.z);
    }


    void FixedUpdate()
    {
        float temp = (CameraMovements.Instance.transform.position.x * (1 - parallaxEffect));
        float dist = (CameraMovements.Instance.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);

        if (temp > startPosX + length)
        { 
            startPosX += length;
        }
        else if (temp < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
