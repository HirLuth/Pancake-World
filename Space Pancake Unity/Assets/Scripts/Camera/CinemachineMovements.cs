using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineMovements : MonoBehaviour
{
    public static CinemachineMovements Instance;
    private CinemachineVirtualCamera camera;

    private float stockageSize;


    private void Awake()
    {
        Instance = this;
        camera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        stockageSize = camera.m_Lens.OrthographicSize;
    }


    public void CameraSize(float modificateur)
    {
        camera.m_Lens.OrthographicSize = stockageSize + modificateur;
    }
}
