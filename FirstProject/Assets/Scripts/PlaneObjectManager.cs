using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.Common;

public class PlaneObjectManager : Singleton<PlaneObjectManager>
{
    public GameObject buildingSite;

    public void CRE()
    {
        
        //for (int i = 0; i < DetectedPlaneGenerator.Instance.NewPlanes.Count; i++)
        //{
        //    DetectedPlaneVisualizer newPlane = DetectedPlaneGenerator.Instance.NewPlanes[i];
        //    // Instantiate a plane visualization prefab and set it to track the new plane. The
        //    // transform is set to the origin with an identity rotation since the mesh for our
        //    // prefab is updated in Unity World coordinates.
        //    GameObject planeObject =
        //        Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
        //    planeObject.GetComponent<DetectedPlaneVisualizer>().Initialize(NewPlanes[i]);
        //}
        
    }

    public void CreateBuildingSite(Vector3 position, Anchor anchor)
    {
        Instantiate(buildingSite, position,Quaternion.identity, anchor.transform);
    }

}
