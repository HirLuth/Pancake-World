using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixe : MonoBehaviour
{
    public float dezoom;
    private float stockage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        CinemachineMovements.Instance.estFixe = true;
        
        stockage = CinemachineMovements.Instance.stockageSize;
        CinemachineMovements.Instance.ChangeFollow(gameObject.transform, dezoom);
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CinemachineMovements.Instance.estFixe = false;
        
        CinemachineMovements.Instance.ChangeFollow(other.transform, stockage);
        
    }
}
