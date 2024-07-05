using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class Edge : MonoBehaviour, IGraphElement
{
    private int _id;
    private BoardScript _board;
    private List<Vector2> _linePoints;
    private Color wireColor;
    [SerializeField] private int pointNumber;
    [SerializeField] private Vertice _v;
    [SerializeField] private Vertice _w;
    [SerializeField] private int _weight = 1;
    private LineRenderer _lineRenderer;
    [SerializeField] private GameObject connectionV;
    [SerializeField] private GameObject connectionW;
    [SerializeField] private GameObject direction;
    [SerializeField] private EdgeCollider2D _lineCollider;
    [SerializeField] private GameObject _weightImage;
    [SerializeField] private TMP_Text   _weightText;
    public GameObject lines;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public Vertice V
    {
        get { return _v; }
        set
        {
            _v = value;
            transform.position = _v.transform.position;
        }
    }
    public Vertice W
    {
        get { return _w; }
        set { _w = value; }
    }

    public int Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public GameObject WeightImage
    {
        get { return _weightImage; }
        set { _weightImage = value; }
    }
    
    public TMP_Text WeightText
    {
        get { return _weightText; }
        set { _weightText = value; }
    }

    public List<Vector2> LinePoints
    {
        get { return _linePoints; }
        set { _linePoints = value; }
    }

    public GameObject Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public Color WireColor 
    {
        get {return wireColor; }
        set {
            wireColor = value;

            _lineRenderer.endColor = value;
            _lineRenderer.startColor = value;
        }
    }

    private void Awake()
    {
        _linePoints = new List<Vector2>();
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _board = FindObjectOfType<BoardScript>();
        _weightImage.SetActive(false);
        CallBackManeger.Instance.onUpdateGraph += OnUpdateGraph;
    }



    private void OnUpdateGraph()
    {

    }

    // private void OnEnable()
    // {
    //     CallBackManeger.Instance.onUpdateGraph += OnUpdateGraph;
    //     CallBackManeger.Instance.mudarDirigido += OnMudarDirigido;
    // }

    // private void OnDisable()
    // {
    //     CallBackManeger.Instance.onUpdateGraph -= OnUpdateGraph;
    //     CallBackManeger.Instance.mudarDirigido -= OnMudarDirigido;
    // }
    // public void CreateLineOld(Vector2 v, Vector2 w)
    // {
    //     Vector2 pos = v;
    //     _lineRenderer.positionCount = 1;
    //     _lineRenderer.SetPosition(0, v);
    //     int n = 0;
    //     var dir = (w - v).normalized;

    //     var hit = Physics2D.RaycastAll(v + dir, (w - v).normalized, Vector2.Distance(v, w) - 2,
    //                                 LayerMask.GetMask("Plug"));



    //     bool hitV = hit.Any(h => _board.Vertices.Any(v => v.BoardId == int.Parse(h.collider.name)));
    //     var dist = Vector2.Distance(v, w) > 4 ? (Vector2.Distance(v, w) < 6 ? 0.85f : 0.78f) : 1;
    //     float r = 1;
    //     float angle = (_board.Edges.Where(e => (e.V.Id == _v.Id && e.W.Id == _w.Id) || (e.V.Id == _w.Id && e.W.Id == _v.Id)).Any(ed => ed.Drawn) ? -90 : 90) * (v.y > w.y ? -1 : 1);
    //     Vector2 arcDir = Quaternion.AngleAxis(angle, Vector3.forward) * (w - v).normalized;

    //     if (V.BoardId == W.BoardId)
    //     {
    //         _lineRenderer.positionCount = 30;
    //         Vector2 center = V.transform.position + V.transform.localPosition.normalized * 0.8f;
    //         float angleStep = 360 / _lineRenderer.positionCount;
    //         float startAngle = Vector2.SignedAngle(Vector2.left, V.transform.localPosition.normalized);
    //         for (int k = 0; k < _lineRenderer.positionCount; k++)
    //         {
    //             float ang = k * angleStep + startAngle;
    //             Vector2 position = center + new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad)) * 0.5f;
    //             _lineRenderer.SetPosition(k, position);
    //         }

    //     }
    //     else
    //     {
    //         while (Vector2.Distance(pos, _w.transform.position) > 0.1f)
    //         {
    //             if (n++ > 100)
    //             {
    //                 Debug.LogError("Error: Max iterations reached");
    //                 break;
    //             }

    //             if (!hitV)
    //             {

    //                 pos += ((Vector2)_w.transform.position - pos).normalized * 0.1f;
    //                 //Debug.Log(_board.Edges.Where(e => (e.V.Id == _v.Id && e.W.Id == _w.Id) || (e.V.Id == _w.Id && e.W.Id == _v.Id)).Count() > 1);
    //                 if (_board.Edges.Where(e => (e.V.Id == _v.Id && e.W.Id == _w.Id) || (e.V.Id == _w.Id && e.W.Id == _v.Id)).Count() > 1)
    //                 {
    //                     dist = Vector2.Distance(v, w) > 4 ? (Vector2.Distance(v, w) < 6 ? 0.15f : 0.1f) : 0.2f;
    //                     r = 0.3f;
    //                     Debug.Log((_board.Edges.Where(e => (e.V.Id == _v.Id && e.W.Id == _w.Id) || (e.V.Id == _w.Id && e.W.Id == _v.Id)).Any(ed => ed.Drawn)));
    //                     angle = (_board.Edges.Where(e => (e.V.Id == _v.Id && e.W.Id == _w.Id) || (e.V.Id == _w.Id && e.W.Id == _v.Id)).Any(ed => ed.Drawn) ? -90 : 90) * (v.y > w.y ? -1 : 1);
    //                     arcDir = Quaternion.AngleAxis(angle, Vector3.forward) * (w - v).normalized;

    //                     var p = -1 + 1 * InverseLerp(v, w, pos) * 2;
    //                     var arc = Mathf.Sqrt(r - Mathf.Pow(p * r, 2));

    //                     var newPos = pos + arcDir * arc - arcDir * (dist == 1 ? 0 : (dist == 0.9f ? 0.05f : 0.09f));
    //                     _lineRenderer.positionCount++;
    //                     _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, newPos);
    //                 }
    //                 else
    //                 {
    //                     pos += ((Vector2)_w.transform.position - pos).normalized * 0.1f;
    //                     _lineRenderer.positionCount++;
    //                     _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pos);
    //                 }

    //             }
    //             else
    //             {
    //                 pos += ((Vector2)_w.transform.position - pos).normalized * 0.1f;
    //                 var p = -1 + 1 * InverseLerp(v, w, pos) * 2;
    //                 var arc = Mathf.Sqrt(r - Mathf.Pow(p * r, 2));

    //                 var newPos = pos + arcDir * arc - arcDir * (dist == 1 ? 0 : (dist == 0.9f ? 0.19f : 0.22f));
    //                 _lineRenderer.positionCount++;
    //                 _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, newPos);
    //             }
    //         }
    //     }
    //     Vector2 vAngle = Vector2.zero, wAngle = Vector2.zero;
    //     int i = 0;
    //     int j = _lineRenderer.positionCount - 1;
    //     // if (!hitV && _board.Edges.Where(e => (e.V.Id == _v.Id && e.W.Id == _w.Id) || (e.V.Id == _w.Id && e.W.Id == _v.Id)).Count() <= 1)
    //     // {
    //     //     vAngle = (W.transform.position - V.transform.position).normalized;
    //     //     wAngle = (V.transform.position - W.transform.position).normalized;
    //     // }
    //     // else
    //     // {
    //     while (Physics2D.OverlapPoint(_lineRenderer.GetPosition(i), LayerMask.GetMask("Component")))
    //     {
    //         i++;
    //         if (i == _lineRenderer.positionCount)
    //             break;
    //     }

    //     Vector3 pointV = _lineRenderer.GetPosition(i);
    //     vAngle = (pointV - V.transform.position).normalized;

    //     while (Physics2D.OverlapPoint(_lineRenderer.GetPosition(j), LayerMask.GetMask("Component")))
    //     {
    //         j--;
    //         if (j == 0)
    //             break;
    //     }

    //     Vector3 pointW = _lineRenderer.GetPosition(j);
    //     wAngle = (pointW - W.transform.position).normalized;

    //     //}

    //     connectionW.SetActive(true);
    //     direction.SetActive(true);
    //     connectionV.transform.position = V.transform.position;
    //     connectionV.transform.up = vAngle;

    //     if (_board.Graph.IsDirected)
    //     {
    //         direction.transform.up = -wAngle;
    //         direction.transform.position = _lineRenderer.GetPosition(j);
    //         connectionW.SetActive(false);
    //     }
    //     else
    //     {
    //         connectionW.transform.position = W.transform.position;
    //         connectionW.transform.up = wAngle;
    //         direction.SetActive(false);
    //     }

    //     // Create the collider for the line renderer
    //     Vector2[] points = new Vector2[j - i - 1];

    //     for (int k = i, l = 0; k < j - 1; k++, l++)
    //     {
    //         points[l] = -transform.position;
    //         points[l] += (Vector2)_lineRenderer.GetPosition(k);
    //     }

    //     _lineCollider.points = points;
    //     Drawn = true;

    // }


    public float InverseLerp(Vector2 a, Vector2 b, Vector2 value)
    {
        Vector2 AB = b - a;
        Vector2 AV = value - a;
        return Vector2.Dot(AV, AB) / Vector2.Dot(AB, AB);
    }

}
