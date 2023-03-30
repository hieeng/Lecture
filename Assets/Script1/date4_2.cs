using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class date4_2 : MonoBehaviour
{
    private int SomeCount<T> (T number)
    {
        dynamic dnumber = number;
        string tempStr = "";
        int tempInt = 0;
        
        if (dnumber.GetType() == tempStr.GetType())
        {
            if (!int.TryParse(dnumber, out tempInt))
            {
                throw new Exception("지원하지 않는 형식입니다.");
            }
            dnumber = int.Parse(dnumber);
        }

        if (dnumber < 0)
            dnumber *= -1.0;
        int count = 0;

        while (1 < dnumber)
        {
            ++count;
            dnumber /= 2.0;
        }

        return count;
    }
    
    private void Start()
    {
        try
        {
            Debug.Log(SomeCount<double>(10.0));
            Debug.Log(SomeCount<float>(-20.0f));
            Debug.Log(SomeCount<int>(13));
            Debug.Log(SomeCount<string>("-123"));
            Debug.Log(SomeCount<string>("asdf"));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}

public class date4_2_answer
{
    public void PrintAnswer()
    {
            Debug.Log($"double : {CalcSomeCount<double>(10.0)}");
            Debug.Log($"float  : {CalcSomeCount<float>(-20.0f)}");
            Debug.Log($"int    : {CalcSomeCount<int>(13)}");
            Debug.Log($"string : {CalcSomeCount<string>("-123")}");
            Debug.Log($"string : {CalcSomeCount<string>("asdf")}");
    }

    private int CalcSomeCount<T>(T number)
    {
        if (!double.TryParse(number.ToString(), out var d))
            throw new System.Exception($"지원하지 않는 형식입니다. {typeof(T).Name}");

        if (d < 0.0)
            d *= -1.0;

        var count = 0;

        while (1.0 < d)
        {
            ++count;
            d /= 2.0;
        }

        return count;
    }
}