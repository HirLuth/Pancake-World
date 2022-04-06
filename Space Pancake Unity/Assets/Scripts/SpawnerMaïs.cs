using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnerMaïs : MonoBehaviour
{
    [SerializeField] private Color gizmoColor;
    [SerializeField] private GameObject maïsAReactionReference;
    [SerializeField] private GameObject maïsAReactionActuel;
    [SerializeField] private float timer;
    [SerializeField] private float timerToRespawn;
    [SerializeField] private float gizmoSize;
    
    void Start()
    {
        maïsAReactionActuel = Instantiate(maïsAReactionReference, transform.position, quaternion.Euler(0, 0, 0));
    }
    
    void Update()
    {
        if (maïsAReactionActuel == null)
        {
            timer += Time.deltaTime;
            if (timer>= timerToRespawn)
            {
                timer = 0;
                maïsAReactionActuel = Instantiate(maïsAReactionReference, transform.position, quaternion.Euler(0, 0, 0));
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position,new Vector2(gizmoSize,gizmoSize));
    }
}
