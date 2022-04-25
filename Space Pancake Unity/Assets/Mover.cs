using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rail rail;

    private int currentSeg;
    private float transition;
    private bool isCompleted;

    public bool go;
    public float vitesseRail;


    void Update()
    {
        if (!isCompleted && go)
            Play();

        else
            CameraMovements.Instance.isOnRail = false;
    }

    private void Play()
    {
        CameraMovements.Instance.isOnRail = true;
        
        transition += Time.deltaTime / vitesseRail;

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
}
