using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    [Header("Suivi")] 
    public Vector3 offset;
    public float smoothFactorX;
    public float smoothFactorY;
    public float deadZoneX;
    public float deadZoneY;
    private float avanceeY;
    [HideInInspector] public Vector2 targetPosition;


    [Header("OffsetX")] 
    public float offsetMax;
    public float speedRattrapage;
    private float offsetActuel;
    bool goingLeft;
    bool goingRight;


    [Header("Dezoom")]
    public float vitesseDezoom;
    public float vitesseZoom;
    public float dezoomMax;
    [HideInInspector] public float stockageSize;
    [HideInInspector] public float dezoomActuel;
    [HideInInspector] public Camera camera;


    [Header("Autres")] 
    public static CameraMovements Instance;
    [HideInInspector] public bool followPlayer;


    private void Start()
    {
        Instance = this;
        
        camera = gameObject.GetComponent<Camera>();
        stockageSize = camera.orthographicSize;
    }


    void FixedUpdate()
    {
        if (followPlayer)
        {
           targetPosition = Character.Instance.transform.position + offset;
        }
           

        float differenceX = transform.position.x - targetPosition.x;
        float differenceY = transform.position.y - targetPosition.y;


        // Déplacements camera sur l'axe x
        float newPositionX = transform.position.x;
        if (Mathf.Abs(differenceX) >= deadZoneX)
        {
            newPositionX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.fixedDeltaTime * (Mathf.Abs(differenceX) - smoothFactorX));
        }


        // Rattrapage de la camera lorsque le joueur court vers la gauche + dezoom
        if (Character.Instance.moveLeft && Character.Instance.running && !goingRight)
        {
            goingLeft = true;
            
            if (offsetActuel < offsetMax)
            {
                offsetActuel += Time.deltaTime * speedRattrapage;
                offset.x = Mathf.Lerp(0, -offsetMax, Mathf.SmoothStep(0, 1, offsetActuel));
            }
            
            if (dezoomActuel < 1)
            {
                dezoomActuel += Time.deltaTime * vitesseDezoom;
            }
            
            camera.orthographicSize = stockageSize + Mathf.Lerp(0, dezoomMax, Mathf.SmoothStep(0, 1, dezoomActuel));
        }
        
        // Rattrapage de la camera lorsque le joueur court vers la droite + dezoom
        else if (Character.Instance.moveRight && Character.Instance.running && !goingLeft)
        {
            goingRight = true;

            if (offsetActuel < offsetMax)
            {
                offsetActuel += Time.deltaTime * speedRattrapage;
                offset.x = Mathf.Lerp(0, offsetMax, Mathf.SmoothStep(0, 1, offsetActuel));
            }

            if (dezoomActuel < 1)
            {
                dezoomActuel += Time.deltaTime * vitesseDezoom;
            }
            
            camera.orthographicSize = stockageSize + Mathf.Lerp(0, dezoomMax, Mathf.SmoothStep(0, 1, dezoomActuel));
        }

        // Quand le joueur s'arrête ou change de direction
        else
        {
            goingLeft = false;
            goingRight = false;
            
            offsetActuel = 0;
            offset.x = 0;

            
            if (dezoomActuel > 0)
            {
                dezoomActuel -= Time.deltaTime * vitesseZoom;
            }
            
            camera.orthographicSize = stockageSize + Mathf.Lerp(0, dezoomMax, Mathf.SmoothStep(0, 1, dezoomActuel));
        }


        // Déplacements camera sur l'axe y
        float newPositionY = transform.position.y;
        
        if (Mathf.Abs(differenceY) >= deadZoneY)
        {
            newPositionY = Mathf.Lerp(transform.position.y, targetPosition.y, Time.fixedDeltaTime * (Mathf.Abs(differenceY) - smoothFactorY));
        }
        
        transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
    }
}
