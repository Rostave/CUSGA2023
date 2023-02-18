using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R0.SingaltonBase;
using Cinemachine;

public class CameraControl : SingletonBehaviour<CameraControl>
{
    protected override void OnEnableInit()
    {
        
    }

    public CinemachineVirtualCamera ca;
    private void Awake()
    {
        ca = transform.GetComponentInChildren<CinemachineVirtualCamera>();
    }

}
