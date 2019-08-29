using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore.Examples.HelloAR;

public class BuildingUI : MonoBehaviour
{
    public GameObject MainPanel;

    public GameObject EnvironmentPanel;

    public GameObject buildingSlot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// BuildingSlot object 를 생성한다.
    /// 
    /// </summary>
    public void InstantiateBuildingSlot()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas == null)
            return;

        //slot 생성
        GameObject slot = Instantiate(buildingSlot, canvas.transform.Find("BottomUI/EnvironmentPanel"));

        //slot buildingsystem 변수에 this 연결
        //slot.GetComponent<PreparationBlockSlotDragHandler>().BuildingSystem = transform.GetComponent<BuildingSystem>();

        print("slot : " + slot);
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
