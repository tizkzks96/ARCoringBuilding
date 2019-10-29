using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroy : Singleton<ObjectDestroy>
{
    public GameObject target;

    public void DestroyObejct()
    {
        if(target != null)
        Destroy(target);
    }

}
