using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void LateUpdate() 
    {
        Follow();
    }
    
    private void Follow()
    {
        transform.position = target.position + offset;
    }
}
