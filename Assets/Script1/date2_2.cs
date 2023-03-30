using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class date2_2 : MonoBehaviour
{
    const float PI = 3.141592f;
    ArrayList array = new ArrayList();

    void Solution()
    {
        float temp = 0;
  
        for (int i = 0; i < 11; i++)
        {
            temp = (float)Math.Round(1 - (0.2f * i), 1);

            //Debug.Log(temp);

            for (int j = 0; j < 20; j++)
            {
                //sin = (float)Math.Round(Mathf.Sin(j * (PI / 180)), 1);  //사인 계산
                var d = (double)j / 19;
                d *= Math.PI;
                d *= 2.0;

                var sin = Math.Sin(d);
                var yy = (sin * 5.0 + 5.0);
                var yyy = 10.0 - Math.Round(yy, 0);

                if (i == 0)
                {
                    Debug.Log(Math.Round(yyy, 0));
                }


                if (i == yyy)
                    array.Add("□");
                else
                    array.Add("■");

            /*
                if (sin == temp || sin == temp - 0.1f)  //사인값과 현재행의 값이 같을때 별찍기
                {
                    array.Add("■");
                }
                else
                {
                    array.Add("□");
                }
            */
            }
            array.Add("\n");
        }

        string str = string.Join("",array.ToArray());

        Debug.Log(str);
    }

    void Start()
    {
        Solution();
    }

}
