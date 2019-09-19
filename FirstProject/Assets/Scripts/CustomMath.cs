using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMath : MonoBehaviour
{
    public static float DistanceToPoint(Vector3 a, Vector3 b)
    {
        return (float)Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.z - b.z, 2));
    }

}
