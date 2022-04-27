using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rail rail;
    public static Mover Instance;

    public int currentSeg;
    private float transition;
    private bool isCompleted;

    public bool go;
    
    [Header("Vitesses")]
    public float vitesseNormal;
    public float vitesseRalenti;
    public float vitesseRapide;


    [Header("Ralenti")] 
    public float moinsViteX;
    public float moinsViteY;
    private float largeurTransitionX = 2.75f;
    private float largeurTransitionY = 2;


    [Header("Acceleration")] 
    public float plusViteX;
    public float plusViteY;

    public void Start()
    {
        Instance = this;
    }


    void FixedUpdate()
    {
        if (!isCompleted && go)
            Play();

        else
            CameraMovements.Instance.isOnRail = false;
    }

    private void Play()
    {
        CameraMovements.Instance.isOnRail = true;
        
        // Tout ce qui concerne le changement de rythme du scrolling
        Vector3 positionCamera = CameraMovements.Instance.transform.position;
        Vector3 positionCharacter = Character.Instance.transform.position;

        float differenceX;
        float differenceY;
        float differenceX2;
        float differenceY2;

        if (rail.nodes[currentSeg].position.x < rail.nodes[currentSeg + 1].position.x)
        {
            differenceX = positionCamera.x - positionCharacter.x;
            differenceX2 = positionCharacter.x - positionCamera.x;
        }
        else
        {
            differenceX = positionCharacter.x - positionCamera.x;
            differenceX2 = positionCamera.x - positionCharacter.x;
        }

        if (rail.nodes[currentSeg].position.y < rail.nodes[currentSeg + 1].position.y)
        {
            differenceY = positionCamera.y - positionCharacter.y;
            differenceY2 = positionCharacter.y - positionCamera.y;
        }
        else
        {
            differenceY2 = positionCamera.y - positionCharacter.y;
            differenceY = positionCharacter.y - positionCamera.y;
        }

        float distance = Mathf.Sqrt(Mathf.Pow(rail.nodes[currentSeg + 1].position.x - rail.nodes[currentSeg].position.x, 2) +
        Mathf.Pow(rail.nodes[currentSeg + 1].position.y - rail.nodes[currentSeg].position.y, 2));


        if (differenceX > moinsViteX)
        {
            float transitionVitesse = (differenceX - moinsViteX) / largeurTransitionX;
            float vitesseActuel = Mathf.Lerp(vitesseNormal, vitesseRalenti, transitionVitesse);
                
            transition += Time.deltaTime * vitesseActuel / distance;
        }
        
        else if (differenceY > moinsViteY)
        {
            float transitionVitesse = (differenceY - moinsViteY) / largeurTransitionY;
            float vitesseActuel = Mathf.Lerp(vitesseNormal, vitesseRalenti, transitionVitesse);
                
            transition += Time.deltaTime * vitesseActuel / distance;
        }
        
        
        else if (differenceX2 > plusViteX)
        {
            float transitionVitesse = (differenceX2 - plusViteX) / largeurTransitionX;
            float vitesseActuel = Mathf.Lerp(vitesseNormal, vitesseRapide, transitionVitesse);
                
            transition += Time.deltaTime * vitesseActuel / distance;
        }
        
        else if (differenceY2 > plusViteY)
        {
            float transitionVitesse = (differenceY2 - plusViteY) / largeurTransitionY;
            float vitesseActuel = Mathf.Lerp(vitesseNormal, vitesseRapide, transitionVitesse);
                
            transition += Time.deltaTime * vitesseActuel / distance;
        }
        
        
        else
        {
            transition += Time.deltaTime * vitesseNormal / distance;
        }
        

        if (transition > 1)
        {
            transition = 0;
            currentSeg++;
        }
        else if (transition < 0)
        {
            transition = 1;
            currentSeg--;
        }

        if(currentSeg < rail.nodes.Length - 1)
        {
            CameraMovements.Instance.transform.position = rail.CatmullPosition(currentSeg, transition);
            CameraMovements.Instance.transform.position = new Vector3(CameraMovements.Instance.transform.position.x, CameraMovements.Instance.transform.position.y, -10);
            CameraMovements.Instance.transform.transform.rotation = rail.Orientation(currentSeg, transition);
        }
        else
        {
            isCompleted = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        go = true;
    }
}
