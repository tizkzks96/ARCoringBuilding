using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.Common;
using GoogleARCore;

public class MainUI : MonoBehaviour
{
    public GameObject mainUI;

    public Camera camera;

    private bool CubeWorldViewChangeSwitchBtn = true;

    private void Awake()
    {
        mainUI.transform.Find("");
    }

    public void CubeWorldViewChange()
    {
        //GameObject cubeWorld = ;
        MeshRenderer meshRenderer = GoogleARCore.Examples.Common.DetectedPlaneVisualizer.m_MeshRenderer;
        GameObject cubeWorld = DetectedPlaneVisualizer.cubeWorld;
        Anchor cubeWorldAnchor = DetectedPlaneVisualizer.cubeWorldAnchor;

        if (DetectedPlaneVisualizer.cubeWorld == null)
        {
            return;
        }
        if (CubeWorldViewChangeSwitchBtn)
        {
            CubeWorldViewChangeSwitchBtn = false;

            meshRenderer.enabled = false;

            cubeWorld.transform.SetParent(camera.transform);
            cubeWorld.transform.localPosition = new Vector3(0, -0.2f, 1.5f);
            cubeWorld.transform.localRotation = Quaternion.Euler(320, 300, 45);
            cubeWorld.SetActive(true);
        }
        else
        {
            CubeWorldViewChangeSwitchBtn = true;

            meshRenderer.enabled = false;


            cubeWorld.transform.SetParent(cubeWorldAnchor.transform);
            cubeWorld.transform.localPosition = Vector3.zero;
            cubeWorld.transform.localRotation = Quaternion.Euler(0, 0, 0);
            cubeWorld.SetActive(true);
        }
    }

    public void CubeRightRotate()
    {
        DetectedPlaneVisualizer.cubeWorld.transform.eulerAngles += transform.up * 90;
    }

    public void CubeLeftRotate()
    {
        DetectedPlaneVisualizer.cubeWorld.transform.eulerAngles += transform.up * -90;
    }

    public void CubeUpRotate()
    {
        DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles += transform.forward * 90;
    }

    public void CubeDownRotate()
    {
        DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles += transform.forward * -90;
    }
}
