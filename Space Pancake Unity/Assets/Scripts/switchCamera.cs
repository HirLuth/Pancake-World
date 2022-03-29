using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCamera : MonoBehaviour
{
    [SerializeField] Transform newCamera;
    [SerializeField] Transform originalCamera;


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        CinemachineMovements.Instance.ChangeFollow(newCamera);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        CinemachineMovements.Instance.ChangeFollow(originalCamera);
    }*/
}
