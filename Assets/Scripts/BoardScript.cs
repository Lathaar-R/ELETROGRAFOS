using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;

public class BoardScript : MonoBehaviour
{
    #region Fields and Properties
    private Graph _graph;
    private StartAnimationHandler _startAnimationHandler;
    [SerializeField] private int totalVertices;
    [SerializeField] private List<Vertice> _vertices = new List<Vertice>();
    [SerializeField] private List<Edge> _edges = new List<Edge>();
    [SerializeField] private GameObject _verticePrefab;
    [SerializeField] private GameObject _edgePrefab;
    [SerializeField] private Transform _boardTransform;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 finalPos;

    public List<Vertice> Vertices => _vertices;
    public List<Edge> Edges => _edges;
    public Graph Graph => _graph;
    public int TotalVertices => totalVertices;


    #endregion

    private void Awake()
    {
        _graph = new Graph(this);

        _startAnimationHandler = new StartAnimationHandler(_boardTransform, GetComponent<Collider2D>(), Vector2.right, LevelType.All);
        //_startAnimationHandler.MoveToStart();
    }

    private void OnEnable()
    {
        //CallBackManeger.Instance.onUpdateGraph += UpdateGraph;
        //CallBackManeger.Instance.onStartLevelAnimation += _startAnimationHandler.MoveToCenter;
        // CallBackManeger.Instance.onUpdateGraph += CreateWires;
        CallBackManeger.Instance.grafoCorreto += TerminaLevel;
    }

    private void OnDisable()
    {
        //CallBackManeger.Instance.onUpdateGraph -= UpdateGraph;
        //CallBackManeger.Instance.onStartLevelAnimation -= _startAnimationHandler.MoveToCenter;
        // CallBackManeger.Instance.onUpdateGraph -= CreateWires;
        CallBackManeger.Instance.grafoCorreto -= TerminaLevel;
    }

    private void TerminaLevel()
    {
        //destroy all vertices and edges
        foreach (var vertice in _vertices)
        {
            Destroy(vertice.gameObject);
        }

        foreach (var edge in _edges)
        {
            Destroy(edge.gameObject);
        }
    }

    public bool AddVertice(Vector3 pos, int boardId, int id = -1)
    {
        var component = Instantiate(_verticePrefab, gameObject.transform);
        var vertice = component.GetComponent<Vertice>(); ;
        component.transform.position = pos;

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

        // Remove arestas que cont�m o vertice
        var e = _edges.Where(edge => edge.V.Id == vertice.Id || edge.W.Id == vertice.Id).ToList();
        foreach (var edge in e)
        {
            _edges.Remove(edge);
            Destroy(edge.gameObject);
        }
        Destroy(vertice.gameObject);

        // Remove o vertice da matriz
        _graph.RemoveVertice(vertice.Id);

        UpdateGraph();
        return true;
    }

    public bool AddEdge(Vertice v, Vertice w)
    {
        var edge = Instantiate(_edgePrefab, gameObject.transform).GetComponent<Edge>();
        edge.V = v;
        edge.W = w;

        edge.V = _vertices.Find(ve => ve.Id == v.Id);
        edge.W = _vertices.Find(ve => ve.Id == w.Id);

        if (!_graph.IsDirected)
        {
            edge.W.Degree++;
            edge.V.Degree++;
        }
        else
        {
            edge.V.OutDegree++;
            edge.W.InDegree++;
        }


        var ec = Color.HSVToRGB(UnityEngine.Random.value, 1, 1);

        edge.WireColor = ec;

        _edges.Add(edge);
        _graph.AddEdge(v.Id, w.Id);
        UpdateGraph();
        return true;
    }

    public bool RemoveEdge(Edge edge, bool isDirected = false)
    {
        if (edge == null)
            return false;

        Vertice v = _vertices.Find(ve => ve.Id == edge.V.Id);
        Vertice w = _vertices.Find(ve => ve.Id == edge.W.Id);

        if (!_graph.IsDirected)
        {
            w.Degree--;
            v.Degree--;
        }
        else
        {
            v.OutDegree--;
            w.InDegree--;
        }

        _edges.Remove(edge);
        _graph.RemoveEdge(edge);
        Destroy(edge.gameObject);
        UpdateGraph();
        return true;
    }

    public bool RemoveEdge(Edge[] edge, bool isDirected = false)
    {
        if (edge == null || edge.Length == 0)
            return false;

        foreach (var e in edge)
        {
            Vertice v = _vertices.Find(ve => ve.Id == e.V.Id);
            Vertice w = _vertices.Find(ve => ve.Id == e.W.Id);

            if (!_graph.IsDirected)
            {
                w.Degree--;
                v.Degree--;
            }
            else
            {
                v.OutDegree--;
                w.InDegree--;
            }

            _edges.Remove(e);
            _graph.RemoveEdge(e);
            Destroy(e.gameObject);
        }

        UpdateGraph();
        return true;
    }

    public void MudarDirigido()
    {
        bool d = _graph.ChangeDirected();

        for (int i = 0; i < _vertices.Count; i++)
        {
            if (d)
            {
                _vertices[i].InDegree = _edges.Count(e => e.W.Id == _vertices[i].Id);
                _vertices[i].OutDegree = _edges.Count(e => e.V.Id == _vertices[i].Id);
            }
            else
            {
                _vertices[i].Degree = _edges.Count(e => e.W.Id == _vertices[i].Id || e.V.Id == _vertices[i].Id);
            }
        }

        CallBackManeger.Instance.MudarDirigidoButton(d);
        UpdateGraph();
    }

    public void UpdateGraph()
    {
        _vertices = _vertices.OrderBy(v => v.Id).ToList();
        CallBackManeger.Instance.UpdateGraph();
        //StartCoroutine(CreateWires());
        CreateWires();
    }

    public void CreateWires()
    {
        //yield return new WaitForFixedUpdate();

        Vector2[] angles = { Vector2.right, new Vector2(1, 1), Vector2.up, new Vector2(-1, 1), Vector2.left, new Vector2(-1, -1), Vector2.down, new Vector2(1, -1) };

        bool hasWeight = _edges.Any(e => e.Weight > 1);
        //Debug.Log(hasWeight);

        var edges = Edges.OrderBy(ed => Vector2.Distance(ed.V.transform.position, ed.W.transform.position)).ToList();
        //var edges = Edges.ToList();
        foreach (var vertice in Vertices)
        {
            vertice.Directions = new bool[8];
        }

        for (int k = 0; k < edges.Count; k++)
        {
            Vector2 v = edges[k].V.transform.position;
            Vector2 w = edges[k].W.transform.position;
            var lr = edges[k].gameObject.GetComponentInChildren<LineRenderer>();
            lr.useWorldSpace = true;
            lr.positionCount = 1;
            edges[k].LinePoints = new List<Vector2>();

            if (edges[k].V.BoardId != edges[k].W.BoardId)
            {
                Vector2 pos = v;
                int i = 0;

                while (Vector2.Distance(pos, w) > 0.1f)
                {
                    Vector2 dir = w - pos;
                    dir = angles.OrderBy(ang => Vector2.Angle(ang, dir)).First();

                    if (i == 0)
                    {
                        var d = dir;
                        var d2 = -dir;
                        bool[] dire = edges[k].V.Directions;
                        var a = angles.ToList();

                        while (dire[Array.IndexOf(angles, d)])
                        {
                            a.Remove(d);
                            d = a.OrderBy(ang => Vector2.Angle(ang, w - pos)).First();
                        }
                        edges[k].V.Directions[Array.IndexOf(angles, d)] = true;
                        dir = d;


                        dire = edges[k].W.Directions;
                        a = angles.ToList();

                        while (dire[Array.IndexOf(angles, d2)])
                        {
                            a.Remove(d2);
                            d2 = a.OrderBy(ang => Vector2.Angle(ang, pos - w)).First();
                        }
                        edges[k].W.Directions[Array.IndexOf(angles, d2)] = true;

                        w += d2 * 0.75f;

                    }


                    if (/*i == 0*/ false)
                    {



                        // pos += dir * 0.75f;
                        // points.Add(pos);

                    }
                    else
                    {
                        // if (w.x - v.x <= 0.75f && w.y - v.y > 0.75f)
                        //     dir.x = 0;

                        // if (w.y - v.y <= 0.75f && w.x - v.x > 0.75f)
                        //     dir.y = 0;

                        var hit = Physics2D.RaycastAll(pos, dir, 0.75f, LayerMask.GetMask("Component")).ToList();
                        // if (hit != null)
                        //     Debug.Log(hit.collider.gameObject.name);

                        hit.RemoveAll(h => h.collider.gameObject == edges[k].V.gameObject || h.collider.gameObject == edges[k].W.gameObject);
                        //                        Debug.Log(i);
                        Debug.DrawRay(Vector2.zero, pos + dir * 0.75f, Color.red, 10f);
                        if (hit.Count > 0)
                        {
                            //Debug.Log("Hit");
                            //find the closest angle to "angle" that is not dir
                            var a = angles.ToList();
                            a.Remove(dir);

                            var index = a.OrderBy(ang => Vector2.Angle(ang, w - pos)).First();

                            dir = index;
                            pos += dir * 0.75f;
                            edges[k].LinePoints.Add(pos);
                        }
                        else
                        {
                            hit = Physics2D.RaycastAll(pos, dir, 1.5f, LayerMask.GetMask("Component")).ToList();

                            hit.RemoveAll(h => h.collider.gameObject == edges[k].V.gameObject || h.collider.gameObject == edges[k].W.gameObject);
                            if (hit.Count > 0)
                            {
                                //Debug.Log("Hit2");
                                var a = angles.ToList();
                                a.Remove(dir);

                                var index = a.OrderBy(ang => Vector2.Angle(ang, w - pos)).First();

                                dir = index;
                                pos += dir * 0.75f;
                                edges[k].LinePoints.Add(pos);
                            }
                            else
                            {
                                pos += dir * 0.75f;
                                edges[k].LinePoints.Add(pos);
                            }

                        }

                        if (i++ > 100)
                        {
                            Debug.Log("Loop infinito - nao foi possivel criar o fio");
                            break;
                        }
                    }
                }


            }

            edges[k].LinePoints.Add(edges[k].W.transform.position);

            var col = edges[k].gameObject.GetComponent<EdgeCollider2D>();

            edges[k].LinePoints.Insert(0, edges[k].V.transform.position);
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i < edges[k].LinePoints.Count; i++)
            {
                points.Add(edges[k].LinePoints[i]);
            }

            lr.positionCount = points.Count;
            lr.SetPositions(points.ToArray());

            var lr2 = edges[k].gameObject.GetComponentsInChildren<LineRenderer>()[1];
            lr2.useWorldSpace = true;
            lr2.positionCount = points.Count;

            lr2.SetPositions(points.ToArray());

            col.points = new List<Vector2>(points.Select(p => (Vector2)p)).ToArray();
            col.offset = -edges[k].V.transform.position;
            col.gameObject.transform.position = edges[k].V.transform.position;

            if (hasWeight)
            {
                edges[k].WeightImage.SetActive(true);
                edges[k].WeightText.text = edges[k].Weight.ToString();

                var lr3 = edges[k].gameObject.GetComponentsInChildren<LineRenderer>()[2];
                lr3.useWorldSpace = true;
                lr3.positionCount = 3;

                Vector2 middle = edges[k].LinePoints[edges[k].LinePoints.Count / 2];
                lr3.SetPosition(1, middle);
                lr3.SetPosition(0, middle + (edges[k].LinePoints[(edges[k].LinePoints.Count / 2) - 1] - middle) * 0.2f);
                lr3.SetPosition(2, middle + (edges[k].LinePoints[(edges[k].LinePoints.Count / 2) + 1] - middle) * 0.2f);

                edges[k].WeightImage.transform.position = points[points.Count / 2];

            }
            else
            {
                if (edges[k].WeightImage != null)
                {
                    edges[k].WeightImage.SetActive(false);
                    var lr3 = edges[k].gameObject.GetComponentsInChildren<LineRenderer>()[2];
                    lr3.useWorldSpace = true;
                    lr3.positionCount = 0;
                }
            }

            if (Graph.IsDirected)
            {
                edges[k].Direction.SetActive(true);
                //Debug.Log(edges[k].LinePoints.Count);
                edges[k].Direction.transform.position = edges[k].W.transform.position + ((Vector3)edges[k].LinePoints[^2] - edges[k].W.transform.position).normalized * 0.85f;
                edges[k].Direction.transform.up = -((Vector3)edges[k].LinePoints[^2] - edges[k].W.transform.position).normalized;
            }
            else
            {
                edges[k].Direction.SetActive(false);
            }


        }

        // //change the lines to localspace 
        // for (int i = 0; i < edges.Count; i++)
        // {
        //     var lr = edges[i].gameObject.GetComponentsInChildren<LineRenderer>();

        //     for (int j = 0; j < lr.Length; j++)
        //     {
        //         lr[i].useWorldSpace = false;

                
        //     }

        //     edges[i].lines.transform.position -= lr[0].GetPosition(0);
        // }
    }



}


#region Callbacks

// private void OnStartLevelAnimation()
// {
//     transform.position = startPos;
//     Debug.Log("StartLevelAnimation");
//     Invoke(nameof(Move), 0.5f);
// }

// private void Move()
// {
//     transform.DOMove(finalPos, 0.5f + UnityEngine.Random.value * 1f).SetEase(Ease.OutCubic);
// }

#endregion


public class Graph
{
    // Lista de adjac�ncias para todos os v�rtices
    private List<int>[] _adjList;
    // Matriz de adjac�ncias para todos os v�rtices
    private int[,] _adjMatrix;
    // Refer�ncia para o script da placa
    private BoardScript _board;
    // Indica se o grafo � direcionado
    private bool _isDirected;
    // Properties
    public List<int>[] AdjacencyList => _adjList;
    public int[,] AdjacencyMatrix => _adjMatrix;
    public bool IsDirected => _isDirected;
    public Graph(BoardScript board)
    {
        _board = board;
        _adjList = new List<int>[board.TotalVertices];
        _adjMatrix = new int[board.TotalVertices, board.TotalVertices];
    }

    // M�todo para adicionar um v�rtice ao grafo
    public int AddVertice(int index = -1)
    {
        // Debug.Log("AddVertice");
        int newId = -1;
        // Ache um espa�o vazio na lista de adjac�ncias
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

    //Metodo para remover um v�rtice do grafo
    public void RemoveVertice(int v)
    {
        // Remove o v�rtice da lista de adjac�ncias
        _adjList[v] = null;

        // Remove o v�rtice das listas de adjac�ncias dos outros v�rtices
        for (int i = 0; i < _adjList.Length; i++)
        {
            _adjList[i]?.RemoveAll(vert => vert == v);
        }



        // Remova a linha e coluna que contem o v�rtice da matriz de adjac�ncias
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

    // M�todo para adicionar uma aresta ao grafo
    public void AddEdge(int v, int w)
    {
        // Adiciona w � lista de adjac�ncias de v
        _adjList[v].Add(w);

        // Adciona aresta na matriz de adjac�ncias
        _adjMatrix[v, w] = 1;

        // Para grafo n�o direcionado
        if (!_isDirected)
        {
            _adjList[w].Add(v);
            _adjMatrix[w, v] = 1;
        }
    }

    // M�todo para remover uma aresta do grafo
    public void RemoveEdge(Edge edge)
    {
        int v = edge.V.Id;
        int w = edge.W.Id;

        // Remove w da lista de adjac�ncias de v
        _adjList[v].Remove(w);

        // Remove aresta da matriz de adjac�ncias
        _adjMatrix[v, w] = 0;

        // Para grafo n�o direcionado
        if (!_isDirected)
        {
            _adjList[w].Remove(v);
            _adjMatrix[w, v] = 0;
        }
    }

    public bool ChangeDirected()
    {
        _isDirected = !_isDirected;

        for (int i = 0; i < _adjList.Length; i++)
        {
            _adjList[i] = null;
        }

        for (int i = 0; i < _board.Vertices.Count; i++)
        {
            int id = _board.Vertices[i].Id;
            _adjList[id] = new List<int>();

            for (int j = 0; j < _board.Edges.Count; j++)
            {
                if (_board.Edges[j].V.Id == id)
                {
                    _adjList[id].Add(_board.Edges[j].W.Id);
                }
                else if (_board.Edges[j].W.Id == id && !_isDirected)
                {
                    _adjList[id].Add(_board.Edges[j].V.Id);
                }
            }
        }


        for (int i = 0; i < _adjMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _adjMatrix.GetLength(1); j++)
            {
                _adjMatrix[i, j] = 0;
            }
        }

        for (int i = 0; i < _adjList.Length; i++)
        {
            if (_adjList[i] == null) continue;

            for (int j = 0; j < _adjList[i].Count; j++)
            {
                _adjMatrix[i, _adjList[i][j]] = 1;
            }


        }

        for (int i = 0; i < _adjMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _adjMatrix.GetLength(1); j++)
            {
                //Debug.Log(_adjMatrix[i, j]);
            }
        }

        return _isDirected;
    }
}
