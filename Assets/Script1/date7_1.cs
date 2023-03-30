using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class date7_1 : StudyBase
{
    protected override void OnLog()
    {
        var aList = new ListTest<int>();

        aList.Add(2);
        // 2
        aList.LogValues();

        aList.Insert(0, 1);
        // 1, 2
        aList.LogValues();

        aList.Add(4);
        aList.LogValues();

        aList.Insert(aList.Count - 1, 3);
        // 1, 2, 3, 4
        aList.LogValues();

        aList.Remove(2);
        aList.RemoveAt(0);
        // 4
        Log(aList[aList.Count - 1]);
    }
}

public sealed class ListTest<T> : IEnumerable<T>
{

    private const int defaultCapacity = 4;
    private T[] array;
    private int size;
    public int Count { get => size; }

    public int Capcity
    {
        get => array.Length;
        set
        {
            if (value != array.Length)
            {
                if (value > 0)
                {
                    T[] newArray = new T[value];
                    if (size > 0)
                        Array.Copy(array, 0, newArray, 0, size);
                    array = newArray;
                }
            }
            else
                array = new T[0];
        }
    }

    public ListTest()
    {
        array = new T[0];
        size = 0;
    }

    public T this[int index]
    {
        set => array[index] = value;
        get => array[index];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return array[i];
    }

    public bool Contains(T value)
    {
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;

        for (int i = 0; i < array.Length; i++)
        {
            if (comparer.Equals(array[i], value))
                return true;
        }
        return false;
    }

    public void Add(T value)
    {
        if (size == array.Length)
            EnsureCapacity();
        array[size++] = value;
    }

    public void Insert(int index, T value)
    {
        if (size == array.Length)
            EnsureCapacity();

        size++;

        T[] temp = new T[array.Length - index];

        for (int i = 0; i < temp.Length; i++)
            temp[i] = array[index + i];
        array[index] = value;

        for (int i = index + 1, j = 0; i < array.Length; i++)
            array[i] = temp[j++];
    }

    public bool Remove(T value)
    {
        int index = IndexOf(value);

        if (index >= 0)
        {
            RemoveAt(index);
            return true;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        size--;
        if (index < size)
            Array.Copy(array, index + 1, array, index, size - index);
        array[size] = default(T);
    }

    public void Clear()
    {
        for (int i = 0; i < array.Length; i++)
            RemoveAt(i);
    }

    private void EnsureCapacity()
    {
        int newCapacity = array.Length == 0 ? defaultCapacity : array.Length * 2;
        Capcity = newCapacity;
    }

    private int IndexOf(T value)
    {
        return Array.IndexOf(array, value, 0, size);
    }
}