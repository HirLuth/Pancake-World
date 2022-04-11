using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineMovements : MonoBehaviour
{
    public static CinemachineMovements Instance;
    private CinemachineVirtualCamera camera;

    public float stockageSize;

    public bool estFixe;
    

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
        if (!estFixe)
        {
            camera.m_Lens.OrthographicSize = stockageSize + modificateur;
        }
    }


    public void ChangeFollow(Transform objectFollowed, float dezoom)
    {
        camera.m_Lens.OrthographicSize = dezoom;
        camera.m_Follow = objectFollowed;
    }
}
