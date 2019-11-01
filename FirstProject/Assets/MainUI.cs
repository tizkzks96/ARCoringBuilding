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
public class MainUI : Singleton<MainUI>
{
    public GameObject mainUI;

    public Camera m_camera;

    public GameObject destroyParticle;
    public GameObject createParticle;
    public GameObject goodParticle;
    public GameObject SandParticle;
    public GameObject GrassParticle;
    public GameObject WarterParticle;
    public GameObject RoadParticle;

    private GameObject m_anchor;
    public bool CubeWorldViewChangeSwitchBtn = true;

    private Button leftRotationBtn;
    private Button rightRotationBtn;
    private Button upRotationBtn;
    private Button downRotationBtn;


    bool isPlaying = false;

    private void Awake()
    {
        m_anchor = m_camera.transform.Find("Anchor").gameObject;
        print("★★★★★" + m_anchor);

        mainUI.SetActive(true);

        leftRotationBtn = mainUI.transform.Find("LeftRotateBtn").transform.GetComponent<Button>();
        rightRotationBtn = mainUI.transform.Find("RightRotateBtn").transform.GetComponent<Button>();
        upRotationBtn = mainUI.transform.Find("UpRotateBtn").transform.GetComponent<Button>();
        downRotationBtn = mainUI.transform.Find("DownotateBtn").transform.GetComponent<Button>();

        leftRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.LEFT));
        rightRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.RIGHT));
        upRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.UP));
        downRotationBtn.onClick.AddListener(() => RotateCube(ROTATE_DIRECTION.DOWN));

        mainUI.SetActive(false);
    }

    public void OpenUI()
    {
        mainUI.SetActive(true);
    }

    public void CubeWorldViewChange()
    {
        //GameObject cubeWorld = ;
        //MeshRenderer meshRenderer = DetectedPlaneVisualizer.m_MeshRenderer;
        GameObject cubeWorld = DetectedPlaneVisualizer.cubeWorld;
        Anchor cubeWorldAnchor = DetectedPlaneVisualizer.cubeWorldAnchor;

        if (DetectedPlaneVisualizer.cubeWorld == null)
        {
            return;
        }
        if (CubeWorldViewChangeSwitchBtn)
        {
            CubeWorldViewChangeSwitchBtn = false;

            //meshRenderer.enabled = false;

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

            //meshRenderer.enabled = false;

            cubeWorld.transform.SetParent(cubeWorldAnchor.transform);
            cubeWorld.transform.localPosition = Vector3.zero;
            cubeWorld.transform.localRotation = Quaternion.Euler(0, 0, 0);
            cubeWorld.SetActive(true);
        }
    }


    public void RotateCube(ROTATE_DIRECTION rotate)
    {
        float speed = 180;
        if (isPlaying == true)
            return;

        isPlaying = true;

        switch (rotate)
        {
            case ROTATE_DIRECTION.RIGHT:
                print("right");
                StartCoroutine(CubeRotate(90, 0, speed));

                break;
            case ROTATE_DIRECTION.LEFT:
                print("LEFT");
                StartCoroutine(CubeRotate(-90, 0, -speed));

                break;
            case ROTATE_DIRECTION.UP:
                print("UP");
                StartCoroutine(CubeRotate(0, 90, speed));

                break;
            case ROTATE_DIRECTION.DOWN:
                print("DOWN");
                StartCoroutine(CubeRotate(0, -90, -speed));

                break;
            default:
                break;
        }
    }
    public IEnumerator CubeRotate(float right, float up, float speed)
    {
        Vector3 rotation = DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles;
        float rv;
        float duration = 0;

        while (duration <= 89.9999f)
        {
            yield return null;

            rv = Time.deltaTime * speed;
            duration += Mathf.Abs(rv);

            Vector3 localEulerAngles = DetectedPlaneVisualizer.cubeWorld.transform.localEulerAngles;
            if (up == 0)
            {
                DetectedPlaneVisualizer.cubeWorld.transform.localRotation = 
                    Quaternion.Euler(rotation.x, localEulerAngles.y + rv, localEulerAngles.z);
            }
            if (right == 0)
            {
                DetectedPlaneVisualizer.cubeWorld.transform.localRotation = 
                    Quaternion.Euler(localEulerAngles.x , localEulerAngles.y, localEulerAngles.z + rv);
            }
        }
        DetectedPlaneVisualizer.cubeWorld.transform.localRotation = Quaternion.Euler(rotation.x, rotation.y + right, rotation.z + up);
        isPlaying = false;
    }
}
