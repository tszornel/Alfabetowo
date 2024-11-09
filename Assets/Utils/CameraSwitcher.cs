using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraSwitcher
{
   

    public static List<CinemachineVirtualCamera> Cameras = new List<CinemachineVirtualCamera>();

    public static CinemachineVirtualCamera ActiveCamera = null;
    public static bool IsActiveCamera(CinemachineVirtualCamera camera) {

        return camera == ActiveCamera;
    }


    public static CinemachineVirtualCamera SwitchCamera(CinemachineVirtualCamera camera) {

        GameLog.LogMessage("Switch camera to:" + camera.Name);
        camera.Priority = 10;

      
        ActiveCamera = camera;

        foreach (CinemachineVirtualCamera c in Cameras)
        {
            if (c != camera && c.Priority != 0) 
            { 
                c.Priority = 0; 
            
            }
        }

        return ActiveCamera;
    
    }


    public static void SwitchCameraBack(CinemachineVirtualCamera camera, GameObject Hero)
    {
        camera.m_Follow = Hero.transform;
       
    }

        public static CinemachineVirtualCamera SwitchCamera(CinemachineVirtualCamera camera,GameObject cameraGroup,GameObject lighter)
    {

        GameLog.LogMessage("Switch camera to:" + camera.Name);
        camera.Priority = 10;

       // camera.m_LookAt = cameraGroup.transform;
        camera.m_Follow = cameraGroup.transform;
        ActiveCamera = camera;

        foreach (CinemachineVirtualCamera c in Cameras)
        {
            if (c != camera && c.Priority != 0)
            {
                c.Priority = 0;

            }
        }

        return ActiveCamera;

    }

    public static void Register(CinemachineVirtualCamera camera) { 
    
        Cameras.Add(camera);
        GameLog.LogMessage("Register camera" + camera);
    }

    public static void UnRegister(CinemachineVirtualCamera camera)
    {
        GameLog.LogMessage("Deregister camera" + camera);
        Cameras.Remove(camera);
    }


}
