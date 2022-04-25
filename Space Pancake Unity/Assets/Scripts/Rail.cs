using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

 [ExecuteInEditMode]
public class Rail : MonoBehaviour
{
    private Transform[] nodes;

    public static Rail Instance;

    private void Start()
    {
        nodes = GetComponentsInChildren<Transform>();
        Instance = this;
    }


    public Vector3 LinearPosition(int seg, float ratio)
    {
        Vector3 p1 = nodes[seg].position;
        Vector3 p2 = nodes[seg + 1].position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    public Vector3 CatmullPosition(int seg, float ratio)
    {
        Vector3 p1, p2, p3, p4;

        if (seg == 0)
        {
            p1 = nodes[seg].position;
            p2 = p1;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }
        
        else if (seg == nodes.Length - 2)
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = p3;
        }

        else
        {
            p1 = nodes[seg - 1].position;
            p2 = nodes[seg].position;
            p3 = nodes[seg + 1].position;
            p4 = nodes[seg + 2].position;
        }
    }

    public Quaternion Orientation(int seg, float ratio)
    {
        Quaternion q1 = nodes[seg].rotation;
        Quaternion q2 = nodes[seg + 1].rotation;

        return Quaternion.Lerp(q1, q2, ratio);
    }
    
    
    private void OnDrawGizmos()
    {
        for (int i = 1; i < nodes.Length - 1; i++)
        {
            Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position, 3.0f);
        }
    }
}
