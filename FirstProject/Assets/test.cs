using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    public AugmentedImage Image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            
            return;
        }

        float halfWidth = Image.ExtentX / 2;
        float halfHeight = Image.ExtentZ / 2;

        transform.localPosition =
                (halfWidth * Vector3.left) + (halfHeight * Vector3.back);//Image.CenterPose.position;
    }
}
