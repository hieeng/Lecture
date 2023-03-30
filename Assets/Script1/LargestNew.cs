using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class LargestNew : MonoBehaviour
{
    private void Start() 
    {
        GameObject obj = new GameObject();

        int size = Marshal.SizeOf(obj);
    }
}

