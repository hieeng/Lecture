using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.UI;

public class date2_1 : MonoBehaviour
{
    
    const int num = 9;
    //ArrayList array1 = new ArrayList();
    //ArrayList array2 = new ArrayList();
    ArrayList array3 = new ArrayList();
    
    void Solution()
    {
        int count = 0;

        for (int i = 0; i < num; i++)
        {
            //좌측
            for (int j = 0; j <= i; j++)
            {
                array3.Add(j + 1);
            }
            array3.Add("\n");

            if (i == num - 1)   //마지막 한줄 추가 안하기 위해
            {
                break;
            }

            //우측
            count = 0;

            for (int k = 0; k < (num - i - 1); k++)
            {
                array3.Add("  ");
                count++;
            }
            for (int l = 0; l <= i; l++)
            {
                array3.Add(count + 1);
                count++;
            }
            array3.Add("\n");
        }

        string str3 = string.Join("",array3.ToArray());

        Debug.Log(str3);

        /*
        //좌측
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                array1.Add("*");
            }
            array1.Add("\n");
        }

        //우측
        for (int k = 0; k < num; k++)
        {
            for (int l = 0; l < (num-k-1); l++)
            {
                array2.Add("  ");
            }
            for (int m = 0; m <= k; m++)
            {
                array2.Add("*");
            }
            array2.Add("\n");
        }

        string str1 = string.Join("",array1.ToArray());
        string str2 = string.Join("",array2.ToArray());

        Debug.Log(str1);
        Debug.Log(str2);
        */

    }

    private void Start() 
    {
        Solution();
    }
}
