using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class date7_2 : StudyBase
{
    protected override void OnLog()
    {
        var lList = new LinkedListTest<string>();

        lList.AddFirst("My name is");
        lList.AddLast("AlphaGo");
        lList.AddLast("Hi");
        // My name is, AlphaGo, Hi
        lList.LogValues();

        lList.Remove("Hi");
        lList.AddFirst("Hello");
        // Hello, My name is, AlphaGo
        lList.LogValues();

        lList.RemoveFirst();
        lList.AddLast("I'm glad to meet you");
        // My name is, AlphaGo, I'm glad to meet you
        lList.LogValues();
    }
}

public sealed class LinkedListTest<T> : IEnumerable<T>
{
    private int size = 0;
    public int Count { get => size; }
    LinkedListNodeTest<T> head = null;
    
    public LinkedListNodeTest<T> First { get => head; }

    public LinkedListNodeTest<T> Last{ get => head == null ? null : head.prev; }

    public bool Contains(T value)
    {
        if (Find(value) != null)
            return true;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator()
    {
        var node = head;

        while (node != null)
        {
            yield return node.item;
            node = node.next;
            if (node == head)
                break;
        }
    }

    public LinkedListNodeTest<T> AddFirst(T value)
    {
        LinkedListNodeTest<T> result = new LinkedListNodeTest<T>(this, value);

        if (head == null)
            InsertNodeToEmpty(result);
        else
        {
            InsertNodeBefore(head, result);
            head = result;
        }

        return result;
    }

    public LinkedListNodeTest<T> AddLast(T value)
    {
        LinkedListNodeTest<T> result = new LinkedListNodeTest<T>(this, value);

        if (head == null)
            InsertNodeToEmpty(result);
        else
            InsertNodeBefore(head, result);
            
        return result;
    }

    public LinkedListNodeTest<T> AddBefore(LinkedListNodeTest<T> node, T value)
    {
        LinkedListNodeTest<T> result = new LinkedListNodeTest<T>(node.list, value);

        InsertNodeBefore(node, result);
        if (node == head)
            head = result;

        return result;
    }

    public LinkedListNodeTest<T> AddAfter(LinkedListNodeTest<T> node, T value)
    {
        LinkedListNodeTest<T> result = new LinkedListNodeTest<T>(node.list, value);

        InsertNodeBefore(node.next, result);

        return result;
    }

    public bool Remove(T value)
    {
        LinkedListNodeTest<T> node = Find(value);
        
        if (node != null)
        {
            RemoveNode(node);
            return true;
        }
        return false;
    }

    public void RemoveNode(LinkedListNodeTest<T> node)
    {
        if (node == null)
            throw new Exception($"{nameof(node)}가 null 입니다.");

        node.next.prev = node.prev;
        node.prev.next = node.next;

        size--;
    }

    public void RemoveFirst()
    {
        if (head == null)
            throw new Exception ("헤드가 없습니다.");
        RemoveNode(head);
    }

    public void RemoveLast()
    {
        if (head == null)
            throw new Exception ("헤드가 없습니다.");
        RemoveNode(head.prev);
    }

    public void Clear()
    {
        LinkedListNodeTest<T> node = head;

        while (node != null)
        {
            RemoveNode(node);
            node = node.next;
        }

        size = 0;
    }

    public LinkedListNodeTest<T> Find(T value)
    {
        LinkedListNodeTest<T> node = head;
        EqualityComparer<T> comparer = EqualityComparer<T>.Default;

        if (node != null)
        {
            while (node != head)
            {
                if (comparer.Equals(node.item, value))
                    return node;
                node = node.next;
            }
        }

        return null;
    }

    private void InsertNodeToEmpty(LinkedListNodeTest<T> newNode)
    {
        newNode.next = newNode;
        newNode.prev = newNode;
        head = newNode;
        size++;
    }

    private void InsertNodeBefore(LinkedListNodeTest<T> node, LinkedListNodeTest<T> newNode)
    {
        newNode.next = node;
        newNode.prev = node.prev;
        node.prev.next = newNode;
        size++;
    }
}

public class LinkedListNodeTest<T>
{
    public LinkedListTest<T> list;
    public LinkedListNodeTest<T> next;
    public LinkedListNodeTest<T> prev;
    public T item;

    public LinkedListNodeTest(LinkedListTest<T> list, T value)
    {
        this.list = list;
        this.item = value;
    }
}