using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public Rail rail;

    private int currentSeg;
    private float transition;
    private bool isCompleted;

    void Update()
    {
        if (!isCompleted)
            Play();

        else
            CameraMovements.Instance.isOnRail = false;
    }

    private void Play()
    {
        CameraMovements.Instance.isOnRail = true;
        
        transition += Time.deltaTime;

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

        CameraMovements.Instance.transform.position = rail.LinearPosition(currentSeg, transition);
        CameraMovements.Instance.transform.transform.rotation = rail.Orientation(currentSeg, transition);
    }
}
