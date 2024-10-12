using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController _instance;
    public static CameraController Instance{get => _instance;}

    void Awake()
    {
        if(_instance != null)
        {
            Destroy(_instance);
        }
        _instance = this;
    }

    private CinemachineVirtualCamera setUp_Full;
    private CinemachineVirtualCamera setUp_Local;

    void Start()
    {
        setUp_Full = GetComponentsInChildren<CinemachineVirtualCamera>()[0];
        setUp_Local = GetComponentsInChildren<CinemachineVirtualCamera>()[1];
        SwitchToFull();
    }

    public void SwitchToFull()
    {
        setUp_Full.Priority = 100;
        setUp_Local.Priority = 10;
    }

    public void SwitchToLocal(Transform target)
    {
        setUp_Full.Priority = 10;
        setUp_Local.Priority = 100;
        setUp_Local.Follow = target;
    }
}
