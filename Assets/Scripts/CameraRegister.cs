using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRegister : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        CameraSwitcher.Register(GetComponent<CinemachineVirtualCamera>());
    }


    private void OnDisable()
    {
        CameraSwitcher.UnRegister(GetComponent<CinemachineVirtualCamera>());
    }
}
