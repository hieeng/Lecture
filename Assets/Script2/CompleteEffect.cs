using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteEffect : MonoBehaviour
{
    public Transform target;
    float time = 0;
    float startTime = 0;
    bool isStart = true;

    private void Update() 
    {

        PlusTime();
        IsStart();
        Rotate();
        EffectSound();
    }

    private void PlusTime()
    {
        time += Time.deltaTime;
        startTime += Time.deltaTime;
    }

    private void IsStart()
    {
        if(startTime < 1.5f)
            return;
        isStart = false;
    }

    private void Rotate()
    {
        if (isStart)
            return;
        transform.RotateAround(target.position, Vector3.up, Time.deltaTime * 40);
    }

    private void EffectSound()
    {
        if (time <= 5f)
            return;
        GameManager.instance.soundManager.ExplosionSound();
        time = 0;
    }
}
