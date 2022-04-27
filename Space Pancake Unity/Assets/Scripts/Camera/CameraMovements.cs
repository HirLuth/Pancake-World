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


    [Header("Tyrolienne")] 
    public float offsetTyrolienneMax;
    public float offsetTyrolienneSpeed;
    [HideInInspector] public bool tyrolienneCamera;
    private float offsetTyrolienne;


    [Header("Maïs")] 
    public float offsetMaïsMax;
    public float offsetSpeedMaïs;
    [HideInInspector] public bool maïsCamera;
    private float offsetMaïs;


    [Header("Rail")] 
    public bool isOnRail;
    public GameObject zoneMort;
    public float newPositionX;
    public float newPositionY;
    

    [Header("Autres")] 
    public float avanceeX;
    public static CameraMovements Instance;
    [HideInInspector] public bool followPlayer;




    private void Start()
    {
        Instance = this;
        
        camera = gameObject.GetComponent<Camera>();
        stockageSize = camera.orthographicSize;
        followPlayer = true;
    }


    void FixedUpdate()
    {
        zoneMort.SetActive(false);
            
        if (followPlayer)
        { 
            float stockage = targetPosition.x;
                
            targetPosition = Character.Instance.transform.position + offset;

            avanceeX += targetPosition.x - stockage;
        }
               

        float differenceX = transform.position.x - targetPosition.x;
        float differenceY = transform.position.y - targetPosition.y;


        // Déplacements camera sur l'axe x
        newPositionX = transform.position.x;
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
            newPositionY = transform.position.y;
            
            if (Mathf.Abs(differenceY) >= deadZoneY)
            {
                newPositionY = Mathf.Lerp(transform.position.y, targetPosition.y, Time.fixedDeltaTime * (Mathf.Abs(differenceY) - smoothFactorY));
            }
            
            
            // Partie tyrolienne
            if (tyrolienneCamera)
            {
                offsetTyrolienne += Time.deltaTime * offsetTyrolienneSpeed;
                
                offset.x += offsetTyrolienne;
                if (offset.x > offsetTyrolienneMax)
                {
                    offset.x = offsetTyrolienneMax;
                }
            }
            
            else
            {
                if (offsetTyrolienne > offsetTyrolienneMax)
                {
                    offsetTyrolienne = offsetTyrolienneMax - offset.x;
                }
                else if (offsetTyrolienne + offset.x > offsetTyrolienneMax)
                {
                    offsetTyrolienne -= offset.x;
                }
                
                if (offsetTyrolienne > 0)
                {
                    offsetTyrolienne -= Time.deltaTime * offsetTyrolienneSpeed * 2;
                    offset.x += offsetTyrolienne;
                }
            }
            
            
            // Partie maïs
            if (maïsCamera)
            {
                if (offsetMaïs < offsetMaïsMax)
                {
                    offsetMaïs += Time.deltaTime * offsetSpeedMaïs;
                }
                
                offset.y = offsetMaïs;
            }
            
            else
            {
                if (offsetMaïs > 0)
                {
                    offsetMaïs -= Time.deltaTime * offsetSpeedMaïs * 2;
                    offset.y = offsetMaïs;
                }
            }

        if (!isOnRail)
        {
            transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
        }
        else 
        { 
            zoneMort.SetActive(true);
        }
    }
}
