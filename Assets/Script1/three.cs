using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class three : MonoBehaviour
{
    void test()
    {
        string str1 = "Hello World!";
        string str2 = str1;
        string str3 = str1;
        
        //Debug.Log(Object.ReferenceEquals(str1, str2));

    }

    void Start()
    {
        test();
    }
}
