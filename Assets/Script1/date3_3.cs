using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class date3_3 : MonoBehaviour
{
    List<List<string>> list = new List<List<string>>();
    List<string> printf = new List<string>();

    private void AddData()
    {
        list.Add(new List<string> {"1", "스타워커", "순두부", "피자", "햄버거", "스파게티"});
        list.Add(new List<string> {"2", "루루", "openGL", "c#", "math"});
        list.Add(new List<string> {"3", "카로", "CoinHUD", "IStackable"});
        list.Add(new List<string> {"4", "쿠퍼", "파워디그"});
        list.Add(new List<string> {"5", "리아", "UI", "2D", "Sprite", "Texture"});
    }

    private void Find(string name)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Count; j++)
            {
                if (j == 1)     //이름은 출력하지 않기 위해
                {
                    continue;
                }

                if (list[i][1] == name)
                {
                    printf.Add(list[i][j]);
                    printf.Add("  ");
                }
            }
        }

        if (!printf.Any())
        {
            printf.Add("일치하는 데이터 없음");
        }

        string str = string.Join("",printf.ToArray());
        printf.Clear();

        Debug.Log(str);
    }

    private void Start() 
    {
        AddData();
        Find("스타워커");
        Find("지오");
        Find("쿠퍼");
    }
}

public class date3_3_answer
{
    private Dictionary<string, int> _numberSet = new Dictionary<string, int>()
    {
        {"스타워커", 1},
        {"루루", 2},
        {"카로", 3},
        {"쿠퍼", 4},
        {"리아", 5},
    };

    private Dictionary<string, List<string>> _listSet = new Dictionary<string, List<string>>()
    {
        { "스타워커", new List<string>() { "순두부", "피자", "햄버거", "스파게티" } },
        { "루루",     new List<string>() { "openGL", "c#", "math" } },
        { "카로",     new List<string>() { "CoinHUD", "IStackable" } },
        { "쿠퍼",     new List<string>() { "파워디그" } },
        { "리아",     new List<string>() { "UI", "2D", "Sprite", "Texture" } },
    };

    public void PrintAnswer()
    {
        Find("스타워커");
    }

    private void Find(string name)
    {
        if (!_numberSet.TryGetValue(name, out var number))
            number = int.MinValue;
        if (!_listSet.TryGetValue(name, out var list))
            list = null;

        var sb = new StringBuilder();
        sb.AppendLine($"number : {(int.MinValue == number ? "unknown" : number.ToString())}");

        if (null == list)
            sb.Append("list : empty");
        else
        {
            sb.Append("list : ");

            for (int i = 0, size = list.Count; i < size; ++i)
            {
                if (0 < i)
                    sb.Append(", ");
                sb.Append(list[i]);
            }
        }
        Debug.Log(sb.ToString());
    }
}