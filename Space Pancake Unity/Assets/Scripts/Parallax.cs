using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    private float length;
    private float startPosX;

    public GameObject cam;
    public float parallaxEffect;

    private float xActuel;
    
    

    void Start()
    {
        startPosX = cam.transform.position.x;

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        if (CameraMovements.Instance.followPlayer)
        {
            xActuel = CameraMovements.Instance.avanceeX;

            float temp = (xActuel * (1 - parallaxEffect));
            float dist = (xActuel * parallaxEffect);

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
}
