using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.Common;
using UnityEngine.UI;

public class SliderCtrl : MonoBehaviour
{
    private GameObject target = null;
    private Slider slider;
    public void ObjectHeightCtrl(GameObject target)
    {
        target.transform.localPosition = new Vector3(target.transform.localPosition.x, slider.value - 0.5f, target.transform.localPosition.z);
    }

    IEnumerator AttemptLinkObject()
    {
        while (target == null)
        {
            if(DetectedPlaneVisualizer.cubeWorld != null)
            {
                print("AA");
                target = DetectedPlaneVisualizer.cubeWorld;
                slider.onValueChanged.AddListener(
                    delegate { ObjectHeightCtrl(target); }
                );
            }
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        StartCoroutine(AttemptLinkObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
