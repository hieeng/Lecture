using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class date9_1 : StudyBase
{
    protected override void OnLog()
    {
		// Hash는 알고리즘 마다 저장 순서가 다를수 있습니다
		var map = new MyDictionary<int, string>();

        //map.Add(101, "김민준");
        //map.Add(201, "윤서준");
        //map.Add(101, "박민준");
		map[101] = "김민준";
		map[201] = "윤서준";
		map[101] = "박민준";
		// [101, 박민준], [201, 윤서준]
        map.LogValues();

		map.Add(302, "김도윤");
		map.Remove(101);
		map.Add(102, "서예준");
		// [102, 서예준], [201, 윤서준], [302, 김도윤]
		map.LogValues();
    }
}

public sealed class MyDictionary<K, T> : IEnumerable<KeyValuePair<K, T>>
{
    int Max = 1000;
    private List<int> buckets = new List<int>();
    private Entry[] entries = new Entry[1000];
    public int Count { private set; get; } = 0;
    private int entrieSize = 0;
    private int maxHash = 0;

    Comparer<T> comparer = Comparer<T>.Default;
    EqualityComparer<K> eComparerK = EqualityComparer<K>.Default;
    EqualityComparer<T> eComparerT = EqualityComparer<T>.Default;

    public T this[K key] 
    {
        set
        {
            Add(key, value);
        }
        get
        {
            return Find(key);
        }
    }

    public bool ContainsKey(K key)
    {
        if (FindEntry(key) >= 0)
            return true;
        return false;
    }

    public bool ContainsValue(T value)
    {
        if (FindEntry(value) >= 0)
            return true;
        return false;
    }

    public bool TryGetValue(K key, out T result)
    {
        if (FindEntry(key) < 0)
        {
            result = default;
            return false;
        }

        int hash = Math.Abs(key.GetHashCode()) % Max;

        result = entries[buckets[hash]].value;

        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
    {
        for(int i = 0; i < entries.Length; i++)
        {
            if (eComparerK.Equals(entries[i].key, default))
                continue;
            yield return new KeyValuePair<K, T> (entries[i].key, entries[i].value);
        }
    }

    public bool Add(K key, T value)
    {
        int hash = Math.Abs(key.GetHashCode()) % Max;

        if (Count == 0)
        {
            for (int i = 1; i <= hash; i++)
            {
                buckets.Add(-1);
                if (i == hash)
                {
                    buckets.Add(entrieSize);
                    AddEntry(entrieSize, key, value, hash);
                    maxHash = hash;
                }
            }   
        }
        else
        {
            if (hash > maxHash)
            {
                for (int i = maxHash; i <= hash; i++)
                {
                    buckets.Add(-1);
                    if (i == hash)
                    {
                        if (buckets[i] != -1)
                            AddEntry(entries[buckets[i]].next, key, value, hash);
                        else
                        {
                            buckets.Add(entrieSize);
                            AddEntry(entrieSize, key, value, hash);
                            maxHash = hash;
                        }
                    }
                }
            }
            else
            {
                if (buckets[hash] != -1)
                    AddEntry(entries[buckets[hash]].next, key, value, hash);
                else
                {
                    buckets.Insert(hash, entrieSize);
                    AddEntry(entrieSize, key, value, hash);
                }
            }
        }
        entrieSize++;
        Count++;

        return true;
    }

    public bool Remove(K key)
    {
        if (FindEntry(key) < 0)
            return false;

        int hash = Math.Abs(key.GetHashCode()) % Max;

        Release(ref entries[buckets[hash]]);
        Count--;

        return true;
    }

    public void Clear()
    {
        for (int i = 0; i < entries.Length; i++)
            Release(ref entries[i]);
        Count = 0;
    }

    private void AddEntry(int i, K key, T value, int hash)
    {
        entries[i].value = value;
        entries[i].key = key;
        entries[i].hashCode = hash;
        entries[i].next = i;
    }

    private T Find(K key)
    {
        if (key == null)
            throw new Exception("찾는 키가 없습니다.");

        int hash = Math.Abs(key.GetHashCode()) % Max;

        return entries[buckets[hash]].value;
    }

    private int FindEntry(K key)
    {
        for(int i = 0; i < entries.Length; i++)
            if (eComparerK.Equals(entries[i].key, key))
                return 1;

        return -1;
    }

    private int FindEntry(T value)
    {
        for(int i = 0; i < entries.Length; i++)
            if (eComparerT.Equals(entries[i].value, value))
                return 1;

        return -1;
    }

    private void Release(ref Entry entry)
    {
        entry.hashCode = default;
        entry.key = default;
        entry.value = default;
        entry.next = default;
    }

    struct Entry
    {
        public int hashCode;
        public int next;
        public K key;
        public T value;
	}
}