using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [Header("Suivi")] 
    public Vector3 offset;
    public float smoothFactorX;
    public float smoothFactorY;
    public float deadZoneX;
    public float deadZoneY;
    private float avanceeY;


    [Header("OffsetX")] 
    public float offsetMax;
    public float speedRattrapage;
    private float offsetActuel;
    bool goingLeft;
    bool goingRight;


    [Header("Dezoom")] 
    public Camera camera;
    public float vitesseDezoom;
    public float dezoomMax;
    private float stockageSize;


    private void Start()
    {
        stockageSize = camera.orthographicSize;
    }


    void FixedUpdate()
    {
        Vector2 playerPosition = Character.Instance.transform.position + offset;

        float differenceX = transform.position.x - playerPosition.x;
        float differenceY = transform.position.y - playerPosition.y;


        // Déplacements camera sur l'axe x
        float newPositionX = transform.position.x;
        if (Mathf.Abs(differenceX) >= deadZoneX)
        {
            newPositionX = Mathf.Lerp(transform.position.x, playerPosition.x, Time.fixedDeltaTime * (Mathf.Abs(differenceX) - smoothFactorX));
        }
        

        if (Character.Instance.moveLeft && Character.Instance.running && !goingRight)
        {
            goingLeft = true;
            
            if (offsetActuel < offsetMax)
            {
                offsetActuel += Time.deltaTime * speedRattrapage;
                offset.x = Mathf.Lerp(0, -offsetMax, Mathf.SmoothStep(0, 1, offsetActuel));
            }
        }
        
        else if (Character.Instance.moveRight && Character.Instance.running && !goingLeft)
        {
            goingRight = true;

            if (offsetActuel < offsetMax)
            {
                offsetActuel += Time.deltaTime * speedRattrapage;
                offset.x = Mathf.Lerp(0, offsetMax, Mathf.SmoothStep(0, 1, offsetActuel));
            }
        }

        else
        {
            goingLeft = false;
            goingRight = false;
            
            offsetActuel = 0;
            offset.x = 0;
        }


        // Déplacements camera sur l'axe y
        float newPositionY = transform.position.y;
        if (Mathf.Abs(differenceY) >= deadZoneY)
        {
            newPositionY = Mathf.Lerp(transform.position.y, playerPosition.y, Time.fixedDeltaTime * (Mathf.Abs(differenceY) - smoothFactorY));
        }
        
        
        transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
    }
}
