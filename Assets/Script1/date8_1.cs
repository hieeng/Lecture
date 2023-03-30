using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class date8_1 : StudyBase
{
    protected override void OnLog()
    {
        var bTree = new BST<int>();

        bTree.Insert(10);
        bTree.Insert(5);
        bTree.Insert(9);
        bTree.Insert(15);

        // 5 9 10 15
        bTree.LogValues();

        bTree.Insert(2);
        bTree.Remove(9);
        bTree.Insert(7);
        // 2 5 7 10 15
        bTree.LogValues();

        // 5 7 10
        bTree.GetOverlaps(5, 10).LogValues();
    }
}

public class BSTNode<T>
{
    public T data { get; set; }
    public BSTNode<T> Left { get; set; } = null;
    public BSTNode<T> Right { get; set; } = null;

    public BSTNode(T value)
    {
        this.data = value;
    }

    internal void Release()
    {
        Left = null;
        Right = null;
        data = default;
    }
}

public sealed class BST<T> : IEnumerable<T>
{
    private int size = 0;
    public int Count { get => size; }
    public BSTNode<T> Root { private set; get; } = null;
    public IComparer<T> Comparer { private set; get; } = null;
    Comparer<T> comparer = Comparer<T>.Default;
    EqualityComparer<T> eComparer = EqualityComparer<T>.Default;

    List<T> tree = new List<T>();
    List<T> overLap = new List<T>();


    public bool Contains(T value)
    {
        InOrder(Root);

        for (int i = 0; i < tree.Count; i++)
        {
            if (eComparer.Equals(tree[i], value))
                return true;
        }

        return false;
    }

    public BSTNode<T> Find(T value)
    {
        BSTNode<T> temp = Root;

        while (true)
        {
            int comp = comparer.Compare(temp.data, value);

            if (comp == 0)
                return temp;
            else if (comp > 0)
            {
                if (temp.Left != null)
                    temp = temp.Left;
                else
                    break;
            }
            else
            {
                if (temp.Right != null)
                    temp = temp.Right;
                else
                    break;
            }
        }

        return null;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator()
    {
        tree.Clear();
        InOrder(Root);

        for (int i = 0; i < tree.Count; i++)
            yield return tree[i];
    }

    public IEnumerator<T> GetOverlaps(T min, T max)
    {
        overLap.Clear();
        Range(Root, min, max);

        for (int i = 0; i < overLap.Count; i++)
            yield return overLap[i];
    }


    public BSTNode<T> Insert(T value)
    {
        BSTNode<T> node = Root;

        if (node == null)
        {
            Root = new BSTNode<T>(value);
            size++;
            return Root;
        }

        while (node != null)
        {
            int comp = comparer.Compare(node.data, value);

            if (comp == 0)
                throw new Exception("중복된 값입니다.");
            else if (comp > 0)
            {
                if (node.Left == null)
                {
                    node.Left = new BSTNode<T>(value);
                    size++;
                    return node.Left;
                }

                node = node.Left;
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new BSTNode<T>(value);
                    size++;
                    return node.Right;
                }
                node = node.Right;
            }
        }
        return node;
    }

    public void Remove(T value)
    {
        Remove(Root, value);
    }

    private BSTNode<T> Remove(BSTNode<T> node, T value)
    {
        if (node == null)
            throw new Exception("값이 없습니다.");

        int comp = comparer.Compare(node.data, value);
        
        if (comp > 0)
            node.Left = Remove(node.Left, value);
        else if (comp < 0)
            node.Right = Remove(node.Right, value);
        else
        {
            //자식 X
            if (node.Left == null && node.Right == null)
            {
                node = null;
                size--;
            }
            //자식 1개
            else
            {
                var child = node.Left != null ? node.Left : node.Right;
                node = child;
                size--;
            }
        }
        return node;
    }

    public void Clear()
    {
        AllClear(Root);
        size = 0;
    }

    private void InOrder(BSTNode<T> node)
    {
        if (node == null)
            return;
        
        InOrder(node.Left);
        tree.Add(node.data);
        InOrder(node.Right);
    }

    private void Range(BSTNode<T> node, T min, T max)
    {
        if (node == null)
            return;

        Range(node.Left, min, max);
        if (comparer.Compare(node.data, min) >= 0 && comparer.Compare(node.data, max) <= 0)
            overLap.Add(node.data);
        Range(node.Right, min, max);
    }

    private void AllClear(BSTNode<T> node)
    {
        if (node == null)
            return;
        
        AllClear(node.Left);
        AllClear(node.Right);
        node.Release();
    }
}