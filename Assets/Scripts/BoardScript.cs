using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class BoardScript : MonoBehaviour
{
    #region Fields and Properties
    private Graph _graph;
    [SerializeField] private int totalVertices;
    [SerializeField] private List<Vertice> _vertices = new List<Vertice>();
    [SerializeField] private List<Edge> _edges = new List<Edge>();

    public List<Vertice> Vertices => _vertices;
    public List<Edge> Edges => _edges;
    public Graph Graph => _graph;
    public int TotalVertices => totalVertices;


    #endregion

    private void Awake()
    {
        _graph = new Graph(this);
    }

    public bool AddVertice(Vertice vertice, int boardId, int id = -1)
    {
        if (vertice == null)
            return false;

        _vertices.Add(vertice);
        vertice.BoardId = boardId;
        id = _graph.AddVertice(id);
        vertice.Id = id;

        if (id == -1)
        {
            _vertices.Remove(vertice);
            Destroy(vertice.gameObject);
            return false;
        }

        UpdateGraph();
        return true;
    }

    public bool RemoveVertice(int boardId)
    {
        Vertice vertice = _vertices.Find(v => v.BoardId == boardId);
        if (vertice == null)
            return false;

        // Remove o vertice
        var v = _vertices.Remove(vertice);

        // Remove arestas que contém o vertice
        var e = _edges.Where(edge => edge.V.Id == vertice.Id || edge.W.Id == vertice.Id).ToList();
        foreach (var edge in e)
        {
            _edges.Remove(edge);
            Destroy(edge.gameObject);
        }
        Destroy(vertice.gameObject);

        _graph.RemoveVertice(vertice.Id);

        UpdateGraph();
        return true;
    }

    public bool AddEdge(Edge edge, int v, int w, bool isDirected = false)
    {
        if (edge == null)
            return false;

        edge.V = _vertices.Find(ve => ve.Id == v);
        edge.W = _vertices.Find(ve => ve.Id == w);

        _edges.Add(edge);
        _graph.AddEdge(v, w, isDirected);
        UpdateGraph();
        return true;
    }

    public bool RemoveEdge(Edge edge, bool isDirected = false)
    {
        if (edge == null)
            return false;

        _edges.Remove(edge);
        _graph.RemoveEdge(edge, isDirected);
        Destroy(edge.gameObject);
        UpdateGraph();
        return true;
    }

    public void UpdateGraph()
    {
        for (int i = 0; i < Edges.Count; i++)
        {
            Edges[i].Drawn = false;
        }
        CallBackManeger.Instance.UpdateGraph();
    }
}

public class Graph
{
    // Lista de adjacências para todos os vértices
    private List<int>[] _adjList;
    // Matriz de adjacências para todos os vértices
    private int[,] _adjMatrix;
    // Referência para o script da placa
    private BoardScript _board;
    // Indica se o grafo é direcionado
    private bool _isDirected;
    // Properties
    public List<int>[] AdjacencyList => _adjList;
    public int[,] AdjacencyMatrix => _adjMatrix;

    public Graph(BoardScript board)
    {
        _board = board;
        _adjList = new List<int>[board.TotalVertices];
        _adjMatrix = new int[board.TotalVertices, board.TotalVertices];
    }

    // Método para adicionar um vértice ao grafo
    public int AddVertice(int index = -1)
    {
        Debug.Log("AddVertice");
        int newId = -1;
        // Ache um espaço vazio na lista de adjacências
        if (index == -1)
            for (int i = 0; i < _adjList.Length; i++)
            {
                //Debug.Log(_adjList[i].Count);   
                if (_adjList[i] == null)
                {
                    _adjList[i] = new List<int>();
                    newId = i;
                    break;
                }
            }
        else
        {
            if (_adjList[index] != null)
                return -1;

            _adjList[index] = new List<int>();
            newId = index;
        }

        return newId;
    }

    //Metodo para remover um vértice do grafo
    public void RemoveVertice(int v)
    {
        // Remove o vértice da lista de adjacências
        _adjList[v] = null;

        // Remove o vértice das listas de adjacências dos outros vértices
        for (int i = 0; i < _adjList.Length; i++)
        {
            _adjList[i]?.RemoveAll(vert => vert == v);
        }

        #region Old Code
        // // Refaz a lista de adjacências
        // Array.Resize(ref _adj, _vertices);
        // for (int i = 0; i < _vertices; i++)
        // {
        //     _adj[i].Clear();
        // }

        // for (int i = 0; i < _vertices; i++)
        // {
        //     var vert = _board.Vertices.Find(ve => ve.Id == i);

        //     for (int j = 0; j < _board.Edges.Count; j++)
        //     {
        //         if (_board.Edges[j].V.Id == vert.Id)
        //         {
        //             _adj[i].Add(_board.Edges[j].W.Id);
        //         }
        //         else if (_board.Edges[j].W.Id == vert.Id && !_isDirected)
        //         {
        //             _adj[i].Add(_board.Edges[j].V.Id);
        //         }
        //     }
        // }

        // // Refaz a matriz de adjacências usando a lista de adjacências
        // Array.Resize(ref _adjMatrix, _vertices * _vertices);
        // for (int i = 0; i < _vertices; i++)
        // {
        //     for (int j = 0; j < _vertices; j++)
        //     {
        //         _adjMatrix[i * _vertices + j] = 0;
        //     }
        // }

        // for (int i = 0; i < _vertices; i++)
        // {
        //     foreach (var item in _adjList[i])
        //     {
        //         _adjMatrix[i * _vertices + item] = 1;
        //     }
        // }

        #endregion


        // Remova a linha e coluna que contem o vértice da matriz de adjacências
        for (int i = 0; i < _adjMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _adjMatrix.GetLength(1); j++)
            {
                if (i == v || j == v)
                {
                    _adjMatrix[i, j] = 0;
                }
            }
        }

    }

    // Método para adicionar uma aresta ao grafo
    public void AddEdge(int v, int w, bool isDirected = false)
    {
        // Adiciona w à lista de adjacências de v
        _adjList[v].Add(w);

        // Adciona aresta na matriz de adjacências
        _adjMatrix[v, w] = 1;

        // Para grafo não direcionado
        if (!isDirected)
        {
            _adjList[w].Add(v);
            _adjMatrix[w, v] = 1;
        }
    }

    // Método para remover uma aresta do grafo
    public void RemoveEdge(Edge edge, bool isDirected = false)
    {
        int v = edge.V.Id;
        int w = edge.W.Id;

        // Remove w da lista de adjacências de v
        _adjList[v].Remove(w);

        // Remove aresta da matriz de adjacências
        _adjMatrix[v, w] = 0;

        // Para grafo não direcionado
        if (!isDirected)
        {
            _adjList[w].Remove(v);
            _adjMatrix[w, v] = 0;
        }
    }


}
