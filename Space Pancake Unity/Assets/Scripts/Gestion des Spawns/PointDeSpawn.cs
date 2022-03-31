using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDeSpawn : MonoBehaviour
{
    [Header("Gestion du Gizmo")]
    [SerializeField] private Color gizmoColor;
    [SerializeField] private float squareScaleX;
    [SerializeField] private float squareScaleY;

    private void OnTriggerEnter2D(Collider2D other)
    {
        SpawnPointManagement.instance.SetSpawnPoint(transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(new Vector3(transform.position.x,transform.position.y + squareScaleY/2),new Vector3(squareScaleX,squareScaleY));
    }
}
