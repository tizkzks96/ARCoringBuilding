using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.HelloAR;

public class BuildingUI : MonoBehaviour
{
    public GameObject MainPanel;

    public GameObject EnvironmentPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBuildingInfo(BuildingInfo buildingInfo)
    {
    }

    public void temp(GameObject buildingInfo)
    {
        print("temp : " + HelloARController.instance.HorizontalPlanePrefab);

        HelloARController.instance.HorizontalPlanePrefab = buildingInfo;
        print("temp1 : " + HelloARController.instance.HorizontalPlanePrefab);
    }
}
