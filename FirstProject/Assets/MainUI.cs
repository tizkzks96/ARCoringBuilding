using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.Common;
using GoogleARCore;

public class MainUI : MonoBehaviour
{
    public GameObject mainUI;

    public Camera m_camera;

    private GameObject m_anchor;
    private bool CubeWorldViewChangeSwitchBtn = true;

    private void Awake()
    {
        m_anchor = m_camera.transform.Find("Anchor").gameObject;
        print("★★★★★" + m_anchor);
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

            m_anchor.transform.localPosition = new Vector3(0, -0.2f, 1.5f);
            m_anchor.transform.localRotation = Quaternion.Euler(320, 300, 45);
            cubeWorld.SetActive(true);
            cubeWorld.transform.SetParent(m_anchor.transform);
            cubeWorld.transform.localPosition = Vector3.zero;
            cubeWorld.transform.localRotation = Quaternion.identity;

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
        Vector3 rotation = DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles;
        DetectedPlaneVisualizer.cubeWorld.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y + 90, rotation.z);
    }

    public void CubeLeftRotate()
    {
        Vector3 rotation = DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles;
        DetectedPlaneVisualizer.cubeWorld.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y - 90, rotation.z);
    }

    public void CubeUpRotate()
    {
        Vector3 rotation = DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles;
        DetectedPlaneVisualizer.cubeWorld.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z + 90);
    }

    public void CubeDownRotate()
    {
        Vector3 rotation = DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles;
        DetectedPlaneVisualizer.cubeWorld.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z - 90);
        //DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles += DetectedPlaneVisualizer.cubeWorld.transform.forward * -90;
    }
}
