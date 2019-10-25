using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.Common;
using GoogleARCore;
using UnityEngine.UI;

public enum ROTATE_DIRECTION
{
    RIGHT, LEFT, UP, DOWN
}
public class MainUI : MonoBehaviour
{
    public GameObject mainUI;

    public Camera m_camera;

    private GameObject m_anchor;
    private bool CubeWorldViewChangeSwitchBtn = true;

    private Button leftRotationBtn;
    private Button rightRotationBtn;
    private Button upRotationBtn;
    private Button downRotationBtn;


    bool isPlaying = false;

    private void Awake()
    {
        m_anchor = m_camera.transform.Find("Anchor").gameObject;
        print("★★★★★" + m_anchor);

        leftRotationBtn = mainUI.transform.Find("LeftRotateBtn").transform.GetComponent<Button>();
        rightRotationBtn = mainUI.transform.Find("LeftRotateBtn").transform.GetComponent<Button>();
        upRotationBtn = mainUI.transform.Find("LeftRotateBtn").transform.GetComponent<Button>();
        downRotationBtn = mainUI.transform.Find("LeftRotateBtn").transform.GetComponent<Button>();

        leftRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.RIGHT));
        rightRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.LEFT));
        upRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.UP));
        downRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.DOWN));
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


    public void RotateCube(ROTATE_DIRECTION rotate)
    {
        float speed = 90;
        if (isPlaying == true)
            return;

        isPlaying = true;

        switch (rotate)
        {
            case ROTATE_DIRECTION.RIGHT:
                StartCoroutine(CubeRotate(90, 0, speed));

                break;
            case ROTATE_DIRECTION.LEFT:
                StartCoroutine(CubeRotate(-90, 0, speed));

                break;
            case ROTATE_DIRECTION.UP:
                StartCoroutine(CubeRotate(0, 90, speed));

                break;
            case ROTATE_DIRECTION.DOWN:
                StartCoroutine(CubeRotate(0, -90, speed));

                break;
            default:
                break;
        }
    }
    public IEnumerator CubeRotate(float right, float up, float speed)
    {
        Vector3 rotation = DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles;
        float rv = 0;
        float duration = 0;

        while (duration <= 89.9999f)
        {
            yield return null;

            rv = Time.deltaTime * speed;
            duration += rv;

            //right left 분기점 만들어야댐
            DetectedPlaneVisualizer.cubeWorld.transform.localRotation = Quaternion.Euler(rotation.x, DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles.y + rv, DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles.z);
        }
        DetectedPlaneVisualizer.cubeWorld.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y + right, rotation.z + up);
        isPlaying = false;
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
