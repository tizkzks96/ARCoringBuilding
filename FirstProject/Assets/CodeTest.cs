using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTest : MonoBehaviour
{

    List<int> num = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        num.Add(1);
        num.Add(2);
        num.Clear();
        //for (int i =0; num.Length>i; i++ )
        //{
        //    num[i] = i;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btn1()
    {
        foreach (int a in num)
        {
            print("a : " + a);
        }
    }
}
