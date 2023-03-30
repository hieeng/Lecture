using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class date10_1 : StudyBase
{
    protected override void OnLog()
    {
		var graph = new Graph<string>();
        graph.Add("경기도");
        graph.Add("강원도");
        graph.Add("충청도");
        graph.Add("경상도");
        graph.Add("전라도");
        graph.Add("제주도");

        graph.SetEdge("경기도", "강원도", 7, 5);
        graph.SetEdge("경기도", "충청도", 5, true);

        graph.SetEdge("강원도", "충청도", 9, false);
        graph.SetEdge("강원도", "경상도", 13, true);

        graph.SetEdge("충청도", "경상도", 8, true);
        graph.SetEdge("충청도", "전라도", 7, true);

        graph.SetEdge("전라도", "경상도", 6, true);

        graph.SetEdge("경상도", "제주도", 14, false);
        graph.SetEdge("제주도", "경기도", 27, false);


		// 경상도 to 충청도 short (8) 경상도 > 충청도(8)
		// 경상도 to 충청도 long (57) 경상도 > 제주도(14) > 경기도(27) > 강원도(7) > 충청도(9)
		_ShortLongLog(graph.SearchAll("경상도", "충청도", Graph<string>.SearchPolicy.Visit));

		// 전라도 to 제주도 short (20) 전라도 > 경상도(6) > 제주도(14)
		// 전라도 to 제주도 long (46) 전라도 > 충청도(7) > 경기도(5) > 강원도(7) > 경상도(13) > 제주도(14)
        _ShortLongLog(graph.SearchAll("전라도", "제주도", Graph<string>.SearchPolicy.Visit));

		// 경기도 to 경상도 short (13) 경기도 > 충청도(5) > 경상도(8)
		// 경기도 to 경상도 long (48) 경기도 > 강원도(7) > 충청도(9) > 경기도(5) > 충청도(5) > 전라도(7) > 충청도(7) > 경상도(8)
        _ShortLongLog(graph.SearchAll("경기도", "경상도", Graph<string>.SearchPolicy.Pass));

		//강원도 to 전라도 short (16) 강원도 > 충청도(9) > 전라도(7)
		//강원도 to 전라도 long (108) 강원도 > 경기도(5) > 강원도(7) > 충청도(9) > 경상도(8) > 강원도(13) > 경상도(13) > 제주도(14) > 경기도(27) > 충청도(5) > 전라도(7)
        _ShortLongLog(graph.SearchAll("강원도", "전라도", Graph<string>.SearchPolicy.Pass));

		// 충청도 to 제주도 short (22) 충청도 > 경상도(8) > 제주도(14)
		// 충청도 to 제주도 long (96) 충청도 > 경기도(5) > 강원도(7) > 경기도(5) > 충청도(5) > 경상도(8) > 강원도(13) > 경상도(13) > 전라도(6) > 충청도(7) > 전라도(7) > 경상도(6) > 제주도(14)
        _ShortLongLog(graph.SearchAll("충청도", "제주도", Graph<string>.SearchPolicy.Pass));


		void _ShortLongLog<T>(List<GraphPath<T>> _paths)
        {
		    if (_paths == null || _paths.Count < 1)
		        return;

            var _sPath = _paths[0];
            var _lPath = _sPath;

            var _sWeight = _sPath.GetTotalWeight();
            var _lWeight = _sWeight;

            for (int index = 1; index < _paths.Count; ++index)
            {
                var _path = _paths[index];
                var _weight = _path.GetTotalWeight();

                if (_weight < _sWeight)
                {
                    _sPath = _path;
                        _sWeight = _weight;
                }
                else if (_lWeight < _weight)
                {
                    _lPath = _path;
                    _lWeight = _weight;
                }
            }

            _PathLog($"{_sPath.Start.Vertex} to {_sPath.End.Vertex} short ", _sPath);
            _PathLog($"{_lPath.Start.Vertex} to {_lPath.End.Vertex} long ", _lPath);
        }

        void _PathLog<T>(string tag, GraphPath<T> _path)
        {
            if (_path.IsNoWay)
            {
                Log($"{tag}(0) {_path.Start.Vertex} // {_path.End.Vertex}");
                return;
            }

            var sb = new StringBuilder();
            sb.Append(_path.Start.Vertex);
            var _curNode = _path.Start;
            int _total = 0;
            for (int index = 1; index < _path.Count; ++index)
            {
                _curNode.TryGetValue(_path.Vertexs[index], out var _edge);
                _curNode = _edge.Node;
                _total += _edge.Weight;

                sb.Append($" > {_curNode.Vertex}({_edge.Weight})");
            }

            Log($"{tag}({_total}) {sb}");
        }
    }
}

public sealed class Graph<T> : IEnumerable<GraphNode<T>>
{
    readonly Dictionary<T, GraphNode<T>> nodes = new Dictionary<T, GraphNode<T>>();
    public int Count => nodes.Count;

    EqualityComparer<T> eComparer = EqualityComparer<T>.Default;


    public GraphNode<T> Add(T vertex)
    {
        if (Contains(vertex))
            throw new Exception("이미 있는 키 값입니다.");

        GraphNode<T> node = new GraphNode<T>();
        node.Vertex = vertex;

		nodes.Add(vertex, node);
        return nodes[vertex];
    }

    public bool Contains(T vertex)
    {
        if (!nodes.ContainsKey(vertex))
            return false;
        return true;
    }

    public bool TryGetValue(T vertex, out GraphNode<T> result)
    {
        if (!nodes.ContainsKey(vertex))
        {
            result = default;
            return false;
        }
        
        result = nodes[vertex];
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<GraphNode<T>> GetEnumerator()
	{
        foreach (var node in nodes)
            yield return node.Value;
	}

    public void SetEdge(T from, T to, int weight, bool isBoth)
    {
        if (!nodes.ContainsKey(from) && !nodes.ContainsKey(to))
            throw new Exception("출발지 또는 목적지가 없습니다.");

		var nodeStart = nodes[from];
        var nodeEnd = nodes[to];

        if (isBoth)
        {
            nodeStart.Edges.Add(nodeStart.Set(to, nodeEnd, weight));
            nodeEnd.Edges.Add(nodeEnd.Set(from, nodeStart, weight));
            nodeStart.Edges.Add(nodeStart.Set(to, nodeEnd, weight));

            nodes[from].Edges.Add(nodeStart.Edges[1]);
            nodes[to].Edges.Add(nodeEnd.Edges[0]);
        }
        else
            nodes[from].Edges.Add(nodeStart.Set(to, nodeEnd, weight));
    }

    public void SetEdge(T a, T b, int weigth_ab, int weigth_ba)
    {
        if (!nodes.ContainsKey(a) && !nodes.ContainsKey(b))
            throw new Exception("출발지 또는 목적지가 없습니다.");

        var nodeA = nodes[a];
        var nodeB = nodes[b];

        nodeA.Edges.Add(nodeA.Set(b, nodeB, weigth_ab));
        nodeB.Edges.Add(nodeB.Set(a, nodeA, weigth_ba));
        nodeA.Edges.Add(nodeA.Set(b, nodeB, weigth_ab));

        nodes[a].Edges.Add(nodeA.Edges[1]);
        nodes[b].Edges.Add(nodeB.Edges[0]);
    }

    public GraphPath<T> CreatePath(T start, T end)
    {
        var path = new GraphPath<T>(nodes[start], nodes[end]);

        return path;
    }

    public List<GraphPath<T>> SearchAll(T start, T end, SearchPolicy policy)
    {
        var path = CreatePath(start, end);
        var paths = new List<GraphPath<T>>();

        if (policy == 0)    //정점 제외
        {

        }
        else                //간선 제외
        {

        }

        return paths;
    }

    private void DFS(T start, T end)
    {
        var path = CreatePath(start, end);
        if(path.IsNoWay)
            return;
        
        List<int> visited = new List<int>();

    }

    public bool Remove(T vertex)
    {
        if (!nodes.ContainsKey(vertex))
            return false;
		nodes[vertex].Release(nodes[vertex]);
        return true;
    }

    public void Clear()
    {
        nodes.Clear();
    }

    public enum SearchPolicy
    {
        Visit = 0,
        Pass,
    }
}

public class GraphNode<T> : IEnumerable<KeyValuePair<T, int>>
{
    public T Vertex;
    public List<Edge> Edges = new List<Edge>();
    EqualityComparer<T> eComparer = EqualityComparer<T>.Default;
    public struct Edge
    {
        public T Vertex;
        public GraphNode<T> Node;
        public int Weight;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
	{
        for (int i = 0; i < Edges.Count; i++)
		    yield return new KeyValuePair<T, int>(Vertex, Edges[i].Weight);
	}

    public void TryGetValue(T vertex, out Edge result)
    {
        for(int i = 0; i < Edges.Count; i++)
        {
            if (eComparer.Equals(vertex, Edges[i].Vertex))
            {
                result = Edges[i];
                return;
            }
        }
        result = default;
    }

    public Edge Set(T to, GraphNode<T> node, int weight)
    {
        Edge newEdge;
        newEdge.Vertex = to;
        newEdge.Node = node;
        newEdge.Weight = weight;

        return newEdge;
    }

    public void Release(GraphNode<T> node)
    {
        node.Vertex = default;
        node.Edges.Clear();
    }
}

public class GraphPath<T> : IEnumerable<T>
{
    public GraphNode<T> Start { private set; get; } = null;
    public GraphNode<T> End { private set; get; } = null;
    EqualityComparer<T> eComparer = EqualityComparer<T>.Default;

    public readonly List<T> Vertexs = new List<T>();
    public int Count => Vertexs.Count;

    public GraphPath(GraphNode<T> start, GraphNode<T> end)
    {
        this.Start = start;
        this.End = end;
    }

    public bool IsNoWay
    {
        get
        {
            return !eComparer.Equals(End.Vertex, Vertexs[Count]);
        }
    }

    public int GetTotalWeight()
    {
        return 0;
    }

    public bool IsVisited(T vertex)
    {
        for (int i = 0; i < Count; i++)
            if(eComparer.Equals(Vertexs[i], vertex))
                return true;
        return false;
    }

    public bool IsPassed(T vertex)
    {
        return false;
    }

    public bool IsPassed(T from, T to)
    {
        bool fromB = false;
        bool toB = false;

        for (int i = 0; i < Count; i++)
        {
            if (eComparer.Equals(Vertexs[i], from))
                fromB = true;
            else if (eComparer.Equals(Vertexs[i], to))
                toB = true;
        }

        return fromB && toB;
    }

    public GraphPath<T> Clone()
    {
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Vertexs.Count; i++)
            yield return Vertexs[i];
    }
}