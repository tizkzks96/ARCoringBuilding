using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.HelloAR;

public class SlotInfo : MonoBehaviour
{
    private BuildingInfo slotinfo;

    public BuildingInfo Slotinfo { get => slotinfo; set => slotinfo = value; }

    public void SetBuildingInfo()
    {
        HelloARController.instance.HorizontalPlanePrefab = Slotinfo.BuildingPrefab;
    }
}
