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
    public float largeurTransitionX;
    public float largeurTransitionX2;
    public float largeurTransitionY;
    public float largeurTransitionY2;


    [Header("Autres")] 
    public Color gizmosColor;
    public BoxCollider2D bc;
    private bool isAtRange;
    private float zoomActuel;
    private float zoomActuelFond;
    private float offsetActuelX;
    private float offsetActuelY;
    


    private void Update()
    {
        zoomActuel = CameraMovements.Instance.stockageSize + Mathf.Lerp(0, CameraMovements.Instance.dezoomMax, Mathf.SmoothStep(0, 1, CameraMovements.Instance.dezoomActuel));
        zoomActuelFond = newZoom / CameraMovements.Instance.stockageSize;
        offsetActuelX = CameraMovements.Instance.offset.x;
        offsetActuelY = CameraMovements.Instance.offset.y;
        
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
            float xMin = (transform.position.x + bc.offset.x - bc.size.x / 2) + largeurTransitionX;
            float xMax = (transform.position.x + bc.offset.x + bc.size.x / 2) - largeurTransitionX2;

            float yMax = (transform.position.y + bc.offset.y + bc.size.y / 2) - largeurTransitionY;
            float yMin = (transform.position.y + bc.offset.y - bc.size.y / 2) + largeurTransitionY2;

            if (xMin > Character.Instance.transform.position.x || xMax < Character.Instance.transform.position.x || 
                yMin > Character.Instance.transform.position.y || yMax < Character.Instance.transform.position.y)
            {
                float avanceeX;
                if (xMin > Character.Instance.transform.position.x)
                {
                    avanceeX = (Character.Instance.transform.position.x - xMin) / largeurTransitionX;
                }
                else if(xMax < Character.Instance.transform.position.x)
                {
                    avanceeX = (Character.Instance.transform.position.x - xMax) / largeurTransitionX2;
                }
                else
                {
                    avanceeX = 0;
                }

                float avanceeY;
                if (yMin > Character.Instance.transform.position.y)
                {
                    avanceeY = (yMin - Character.Instance.transform.position.y) / largeurTransitionY;
                }
                else if (yMax < Character.Instance.transform.position.y)
                {
                    avanceeY = (yMax - Character.Instance.transform.position.y) / largeurTransitionY2;
                }
                else
                {
                    avanceeY = 0;
                }
                
                Debug.Log(avanceeY);
                

                CameraMovements.Instance.targetPosition = new Vector2(
                    Mathf.Lerp(transform.position.x + bc.offset.x, Character.Instance.transform.position.x + offsetActuelX, Mathf.Abs(avanceeX)),
                    Mathf.Lerp(transform.position.y + bc.offset.y, Character.Instance.transform.position.y, Mathf.Abs(avanceeY)));
                
                CameraMovements.Instance.camera.orthographicSize = Mathf.Lerp(newZoom, zoomActuel, Mathf.Abs(avanceeX + avanceeY));
            }
            
            else
            {
                CameraMovements.Instance.targetPosition = new Vector2(transform.position.x, transform.position.y) + bc.offset;
                CameraMovements.Instance.camera.orthographicSize = newZoom;

                //Background.Instance.transform.localScale = new Vector3(zoomActuelFond, zoomActuelFond, 1);
            }
        }
        
        else
        {
            float xMin = (transform.position.x + bc.offset.x - bc.size.x / 2) + largeurTransitionX;
            float xMax = (transform.position.x + bc.offset.x + bc.size.x / 2) - largeurTransitionX2;

            float yMax = (transform.position.y + bc.offset.y + bc.size.y / 2) - largeurTransitionY2;
            float yMin = (transform.position.y + bc.offset.y - bc.size.y / 2) + largeurTransitionY;
            
            Debug.Log(yMin);

            if (xMin > Character.Instance.transform.position.x || xMax < Character.Instance.transform.position.x || 
                yMin > Character.Instance.transform.position.y || yMax < Character.Instance.transform.position.y)
            {
                float avanceeX;
                if (xMin > Character.Instance.transform.position.x)
                {
                    avanceeX = (xMin - Character.Instance.transform.position.x) / largeurTransitionX;
                }
                else if(xMax < Character.Instance.transform.position.x)
                {
                    avanceeX = (xMax - Character.Instance.transform.position.x) / largeurTransitionX2;
                }
                else
                {
                    avanceeX = 0;
                }

                float avanceeY;
                if (yMin > Character.Instance.transform.position.y)
                {
                    avanceeY = (yMin - Character.Instance.transform.position.y) / largeurTransitionY;
                }
                else if (yMax < Character.Instance.transform.position.y)
                {
                    avanceeY = (yMax - Character.Instance.transform.position.y) / largeurTransitionY2;
                }
                else
                {
                    avanceeY = 0;
                }
                
                if (Mathf.Abs(avanceeX) > Mathf.Abs(avanceeY))
                {
                    CameraMovements.Instance.targetPosition = new Vector2(
                        Mathf.Lerp(positionManuelle.x, Character.Instance.transform.position.x + offsetActuelX, Mathf.Abs(avanceeX)),
                        Mathf.Lerp(positionManuelle.y, Character.Instance.transform.position.y, Mathf.Abs(avanceeX)));
                }
                else
                {
                    CameraMovements.Instance.targetPosition = new Vector2(
                        Mathf.Lerp(positionManuelle.x, Character.Instance.transform.position.x + offsetActuelX, Mathf.Abs(avanceeY)),
                        Mathf.Lerp(positionManuelle.y, Character.Instance.transform.position.y, Mathf.Abs(avanceeY)));
                }

                CameraMovements.Instance.camera.orthographicSize = Mathf.Lerp(newZoom, zoomActuel, Mathf.Abs(avanceeX + avanceeY));;
                
                /* Background.Instance.transform.localScale = new Vector3(
                    Mathf.Lerp(zoomActuelFond, 1, Mathf.Abs(avancee)), 
                    Mathf.Lerp(zoomActuelFond, 1, Mathf.Abs(avancee)), 
                    1); */
            }
            
            else
            {
                CameraMovements.Instance.targetPosition = positionManuelle;
                CameraMovements.Instance.camera.orthographicSize = newZoom;

                // Background.Instance.transform.localScale = new Vector3(zoomActuelFond, zoomActuelFond, 1);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            CameraMovements.Instance.followPlayer = false;
            isAtRange = true;
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
