using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFraise : MonoBehaviour
{
    [Header("Gizmo")]
    [SerializeField] private Color gizmoColor;
    [SerializeField] private float hauteurGizmo;

    [Header("Variables modifiables")] 
    public List<SoldatFraise> listFraiseToControl;
    private Character character;

    void Start()
    {
        character = Character.Instance;
        foreach (SoldatFraise fraise in listFraiseToControl)
        {
            if (SpawnPointManagement.spawnPointLocation.x >= transform.position.x && character.activatespawnpoint)
            {
                fraise.iaIsWalking = true;
            }
            else
            {
                fraise.iaIsWalking = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            foreach (SoldatFraise fraise in listFraiseToControl)
            {
                fraise.iaIsWalking = true;
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(transform.position,new Vector2(transform.position.x, transform.position.y + hauteurGizmo));
    }
}
