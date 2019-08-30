using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine.UI;

public class ARCoreTest : MonoBehaviour
{
    public Material material;

    public Button btn1;
    public Button btn2;
    public Button btn3;

    public bool flag1 = false;
    public bool flag2 = false;
    public bool flag3 = false;

    public 
    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(() => {
            if (flag1 == true)
            {
                flag1 = false;
            }
            else
            {
                flag1 = true;
            }
            btn1.transform.GetChild(0).GetComponent<Text>().text = "현재 : " + flag1;
        });

        btn2.onClick.AddListener(() => {
            if (flag2 == true)
            {
                flag2 = false;
            }
            else
            {
                flag2 = true;
            }
            btn2.transform.GetChild(0).GetComponent<Text>().text = "현재 : " + flag2;
        });

        btn3.onClick.AddListener(() => {
            if (flag3 == true)
            {
                flag3 = false;
            }
            else
            {
                flag3 = true;
            }
            btn3.transform.GetChild(0).GetComponent<Text>().text = "현재 : " + flag3;
        });

        //Test();
    }

    public void Test()
    {

        //TARCoreBackgroundRenderer();
        

    }


    public void ARCoreCameraConfigFilterTest()
    {
        ARCoreCameraConfigFilter aRCoreCameraConfigFilter = new ARCoreCameraConfigFilter();
        if (flag1)
            aRCoreCameraConfigFilter.DepthSensorUsage.DoNotUse = true;
        if (flag2)
            aRCoreCameraConfigFilter.DepthSensorUsage.RequireAndUse = true;
    }

    public void TARCoreBackgroundRenderer()
    {
        ARCoreBackgroundRenderer aRCoreBackgroundRenderer = new ARCoreBackgroundRenderer
        {
            BackgroundMaterial = material
        };
        print("aRCoreBackgroundRenderer : " + aRCoreBackgroundRenderer);
        print("aRCoreBackgroundRenderer.BackgroundMaterial : " + aRCoreBackgroundRenderer.BackgroundMaterial);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
