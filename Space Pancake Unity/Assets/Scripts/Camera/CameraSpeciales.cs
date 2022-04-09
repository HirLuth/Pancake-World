using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraSpeciales : MonoBehaviour
{
    [Header("Cocher Type De Camera")] 
    public bool cameraFixe;
    public bool cameraVerticale;


    [Header("Camera Fixe")] 
    public bool positionAutomatique;
    public Vector2 positionManuelle;
    public float newZoom;
    public float largeurTransition;


    [Header("Autres")] 
    public Color gizmosColor;
    public BoxCollider2D bc;
    private bool isAtRange;
    private float stockageZoom;
    


    private void Update()
    {
        if (isAtRange)
        {
            if (cameraFixe)
            {
                CameraFixe();
            }
        }
    }
    

    void CameraFixe()
    {
        if (positionAutomatique)
        {
            float xMin = (transform.position.x + bc.offset.x - bc.size.x / 2) + largeurTransition;
            float xMax = (transform.position.x + bc.offset.x + bc.size.x / 2) - largeurTransition;

            if (xMin > Character.Instance.transform.position.x || xMax < Character.Instance.transform.position.x)
            {
                float avancee;
                if (xMin > Character.Instance.transform.position.x)
                {
                    avancee = (Character.Instance.transform.position.x - xMin) / largeurTransition;
                }
                else
                {
                    avancee = (Character.Instance.transform.position.x - xMax) / largeurTransition;
                }
                

                CameraMovements.Instance.targetPosition = new Vector2(
                    Mathf.Lerp(transform.position.x + bc.offset.x, Character.Instance.transform.position.x, Mathf.Abs(avancee)),
                    Mathf.Lerp(transform.position.y + bc.offset.y, Character.Instance.transform.position.y, Mathf.Abs(avancee)));
                
                CameraMovements.Instance.camera.orthographicSize = Mathf.Lerp(newZoom, stockageZoom, Mathf.Abs(avancee));
                
                CameraMovements.Instance.transform.localScale = new Vector3(
                    Mathf.Lerp((newZoom / stockageZoom), 1, Mathf.Abs(avancee)), 
                    Mathf.Lerp((newZoom / stockageZoom), 1, Mathf.Abs(avancee)), 
                    1);
            }


            else
            {
                CameraMovements.Instance.targetPosition = new Vector2(transform.position.x, transform.position.y) + bc.offset;
                CameraMovements.Instance.camera.orthographicSize = newZoom;

                CameraMovements.Instance.transform.localScale = new Vector3(newZoom / stockageZoom, newZoom / stockageZoom, 1);
            }
        }
        
        else
        {
            CameraMovements.Instance.targetPosition = positionManuelle;
            CameraMovements.Instance.camera.orthographicSize = newZoom;

            CameraMovements.Instance.transform.localScale = new Vector3(newZoom / stockageZoom, newZoom / stockageZoom, 1);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            CameraMovements.Instance.followPlayer = false;
            isAtRange = true;
            
            stockageZoom = CameraMovements.Instance.camera.orthographicSize;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            CameraMovements.Instance.followPlayer = true;
            isAtRange = false;
            
            CameraMovements.Instance.transform.localScale = Vector3.one;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawCube(new Vector2(transform.position.x + bc.offset.x ,transform.position.y + bc.offset.y),new Vector2(bc.size.x,bc.size.y));
    }
}
