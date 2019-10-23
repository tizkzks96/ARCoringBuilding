using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMoneyCtrl : EconomySystem
{
    public GameObject placeObject;
    public float time;
    public float increseMoney;

    private void OnEnable()
    {
        if(placeObject == null)
        {
            return;
        }
        print("placeObject  : " + placeObject);
        print("time  : " + time);
        print("increseMoney  : " + increseMoney);

        StartCoroutine(EconomySystem.Instance.IncreseGage(placeObject, time, increseMoney));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
