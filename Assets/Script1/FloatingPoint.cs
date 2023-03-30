using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class FloatingPoint : MonoBehaviour
{  
    void FP(float f)
    {
        float orign = f;
        int count = 0;
        int num;
        float dec;
        ArrayList numB = new ArrayList();
        ArrayList decB = new ArrayList();
        int pointPostion;
        ArrayList pointPostionB = new ArrayList();
        ArrayList mantissa = new ArrayList();
        ArrayList fp = new ArrayList();

        num = (int)orign;
        dec = orign - (int)orign;
        numB = NtoB(Mathf.Abs(num));    //음수대비 절대값
        decB = DtoB(Mathf.Abs(dec));

        //부호파트
        if (orign >= 0)
        {
            fp.Add(0);
        }
        else
        {
            fp.Add(1);
        }

        //지수파트
        pointPostion = numB.Count - 1 + 127;
        if (pointPostion <= 127)    //128보다 작으면 7자리가 되므로 0을 첫번째 자리에 추가
        {
            fp.Add(0);
        }
        pointPostionB = NtoB(pointPostion);
        fp.AddRange(pointPostionB);

        //가수파트
        mantissa.AddRange(numB);
        mantissa.AddRange(decB);
        while ((int)mantissa[0] == 0 )  //가수 첫번째 부분이 0일때 앞으로 당기기
        {
            mantissa.RemoveAt(0);
            count++;
            if (count == 100)
            {
                Debug.Log("error");
                break;
            }
        }
        mantissa.RemoveAt(0);   //가수의 1부분 삭제
        for (int i = mantissa.Count; i < 23; i++)   //32번째 자리까지 빈곳에 0채우기
        {
            mantissa.Add(0);
        }
        fp.AddRange(mantissa);

        //문자열로 변환
        string str = string.Join("",fp.ToArray());

        Debug.Log(str);
    }

    ArrayList NtoB(int num)
    {
        ArrayList numB = new ArrayList();

        for (int i = 0; num != 0; i++)
        {
            numB.Add(num % 2);
            num /= 2;
        }

        numB.Reverse();
        return numB;
    }

    ArrayList DtoB(float dec)
    {
        ArrayList decB = new ArrayList();

        for (int i = 0; dec != 0; i++)
        {
            dec *= 2;
            int temp = ((int)dec);
            decB.Add(temp);
            if (dec >= 1)
                dec -= 1;
        }

        return decB;
    }

    void Start()
    {
        FP(13.5f);
        FP(-27.125f);
        FP(0.125f);
        FP(-0.75f);
    }

}
