using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RepresentadorCompScript : MonoBehaviour
{
    private StartAnimationHandler _startAnimationHandler;
    private BoxCollider2D _boxCollider;
    private bool _isDragging = false;
    [SerializeField] private int rep = 0;
    [SerializeField] private bool editable;
    [SerializeField] private BoardScript _board;
    //[SerializeField] private RepresentadorScript _representador;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _adjMatrixPrefab;
    [SerializeField] private List<GameObject[]> _adjMatrixCells;
    [SerializeField] private Transform _adjMatrixGridMain;
    [SerializeField] private Transform _adjMatrixGridRows;
    [SerializeField] private Transform _adjMatrixGridCols;
    [SerializeField] private GridLayoutGroup _adjMatrixGridGroup;
    [SerializeField] private GameObject _textAL;
    [SerializeField] private GameObject _textAM;
    [SerializeField] private GameObject _textIM;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private RectTransform _matrixMainTransform;
    [SerializeField] private RectTransform _matrixRowsTransform;
    [SerializeField] private RectTransform _matrixColsTransform;
    [SerializeField] private RectTransform _matrixBoxTransform;

    private void Awake()
    {
        _inputField.text = "";
        _adjMatrixCells = new List<GameObject[]>();
        _boxCollider = GetComponent<BoxCollider2D>();

        _startAnimationHandler = new StartAnimationHandler(_transform, Vector2.left, LevelType.PedidosRepresentados);
        //_startAnimationHandler.MoveToStartCanvas();
        OnChangeRepresentation(rep);
    }

    private void OnEnable()
    {
        //CallBackManeger.Instance.onStartLevelAnimation += _startAnimationHandler.MoveToCenterCanvas;
        CallBackManeger.Instance.onEndLevel += OnEndLevel;
        CallBackManeger.Instance.onUpdateGraph += OnUpdateGraph;
    }



    private void OnDisable()
    {
        //CallBackManeger.Instance.onStartLevelAnimation -= _startAnimationHandler.MoveToCenterCanvas;
        CallBackManeger.Instance.onEndLevel -= OnEndLevel;
        CallBackManeger.Instance.onUpdateGraph -= OnUpdateGraph;
    }



    void Update()
    {
        if (rep == 1)
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                var mousePos = Mouse.current.position.ReadValue();
                bool isOver = RectTransformUtility.RectangleContainsScreenPoint(_matrixBoxTransform, mousePos);
                if (isOver)
                {
                    _isDragging = true;
                }
            }

        if (_isDragging)
        {
            var movement = Mouse.current.delta.ReadValue();

            _matrixMainTransform.anchoredPosition += movement;

            float minX = 310 - (_board.Vertices.Count) * 50;
            float minY = -360 + (_board.Vertices.Count) * 50;
            minX = minX > 0 ? 0 : minX;
            minY = minY < 0 ? 0 : minY;
            _matrixMainTransform.anchoredPosition = new Vector2(Mathf.Clamp(_matrixMainTransform.anchoredPosition.x, minX, 0),
                Mathf.Clamp(_matrixMainTransform.anchoredPosition.y, 0, minY));

            _matrixRowsTransform.anchoredPosition = new Vector2(_matrixRowsTransform.anchoredPosition.x, _matrixMainTransform.anchoredPosition.y);
            _matrixColsTransform.anchoredPosition = new Vector2(_matrixMainTransform.anchoredPosition.x, _matrixColsTransform.anchoredPosition.y);

            if (!Mouse.current.rightButton.isPressed)
            {
                _isDragging = false;
            }
        }
    }

    private void OnStartLevelAnimation()
    {

    }
    private void OnEndLevel()
    {

    }

    public void OnChangeRepresentation(int rep)
    {
        this.rep = rep;
        _textAL.SetActive(rep == 0);
        _textAM.SetActive(rep == 1);
        _textIM.SetActive(rep == 2);

        switch (rep)
        {
            case 0: //Adjacency List

                break;

            case 1: //Adjacency Matrix

                break;

            case 2: //Incidence Matrix

                break;

            default:
                break;
        }

        OnUpdateGraph();
    }

    // private void OnEndEdit(string value)
    // {
    //     string input = value;
    //     switch (rep)
    //     {
    //         case 0: //Adjacency List 
    //             // Check if input is in the format "0: 1, 2,3, ...\n 1: 0, 2, 3, ... \n ..."
    //             string pattern = @"^(\d+:\s?(\d+,\s?)*\d*\n)*";
    //             bool match = Regex.IsMatch(input, pattern);
    //             if (match)
    //             {
    //                 // Split input by lines
    //                 string[] lines = input.Split('\n');
    //                 foreach (string line in lines)
    //                 {
    //                     // Split each line by ":"
    //                     string[] vertices = line.Split(':');
    //                     // Get the out vertice
    //                     int outVertice = int.Parse(vertices[0]);
    //                     // Get the in vertices
    //                     string[] inVertices = vertices[1].Split(',');

    //                     // check if the out vertice is in the graph
    //                     if (!_board.Vertices.Any(v => v.Id == outVertice))
    //                     {
    //                         var t = _representador.PlaceSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
    //                         _board.AddVertice(t.position, int.Parse(t.name), outVertice);
    //                     }

    //                     // check if the in vertices are in the graph
    //                     foreach (string inVertice in inVertices)
    //                     {
    //                         int inVerticeInt = int.Parse(inVertice);
    //                         if (!_board.Vertices.Any(v => v.Id == inVerticeInt))
    //                         {
    //                             var t = _representador.PlaceSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
    //                             _board.AddVertice(t.position, int.Parse(t.name), inVerticeInt);
    //                         }
    //                     }

    //                     // Add the edges
    //                     foreach (string inVertice in inVertices)
    //                     {
    //                         int inVerticeInt = int.Parse(inVertice);
    //                         if (!_board.Edges.Any(e => e.V.Id == outVertice && e.W.Id == inVerticeInt))
    //                         {
    //                             _board.AddEdge(_board.Vertices.Find(v => v.Id == outVertice), _board.Vertices.Find(v => v.Id == inVerticeInt));
    //                         }
    //                     }

    //                     // Remove the vertices that are not in the input
    //                     if (_board.Vertices.Any(v => v.Id != outVertice))
    //                     {
    //                         _board.RemoveVertice(_board.Vertices.Find(v => v.Id != outVertice).BoardId);
    //                     }

    //                     // Remove the edges that are not in the input
    //                     for (int i = 0; i < _board.Edges.Count; i++)
    //                     {
    //                         if (!inVertices.Contains(_board.Edges[i].W.Id.ToString()) && _board.Edges[i].V.Id == outVertice)
    //                         {
    //                             _board.RemoveEdge(_board.Edges[i].V.BoardId, _board.Edges[i].W.BoardId);
    //                             i--;
    //                         }
    //                     }

    //                 }
    //             }
    //             else
    //             {

    //             }

    //             break;

    //         case 1: //Adjacency Matrix

    //             break;

    //         default:
    //             break;
    //     }

    //     CallBackManeger.Instance.UpdateGraph();
    //     OnUpdateGraph();
    // }

    void OnUpdateGraph()
    {
        string result = "";
        switch (rep)
        {
            case 0: //Adjacency List 
                var adjList = _board.Graph.AdjacencyList;

                for (int i = 0; i < adjList.Length; i++)
                {
                    if (adjList[i] == null) continue;

                    result += i + ": ";
                    bool hasVert = false;
                    foreach (var vert in adjList[i])
                    {
                        result += vert + ", ";
                        hasVert = true;
                    }
                    result = hasVert ? result.Remove(result.Length - 2) : result;
                    result += "\n";
                }

                _inputField.text = result;

                break;

            case 1: //Adjacency Matrix
                //delete all cells
                foreach (var cell in _adjMatrixCells)
                {
                    foreach (var c in cell)
                    {
                        Destroy(c);
                    }
                }

                int gridNum = _board.Vertices.Count + 1;
                _adjMatrixGridGroup.constraintCount = gridNum - 1;
                _adjMatrixCells = new List<GameObject[]>();
                for (int i = 0; i < gridNum; i++)
                {
                    _adjMatrixCells.Add(new GameObject[gridNum]);

                    for (int j = 0; j < gridNum; j++)
                    {
                        if (i == 0 && j == 0)
                            continue;

                        else if (i == 0)
                        {
                            _adjMatrixCells[i][j] = Instantiate(_adjMatrixPrefab, _adjMatrixGridCols);
                            //Debug.Log("i == 0");
                            _adjMatrixCells[i][j].GetComponentInChildren<TMP_InputField>().text = _board.Vertices[j - 1].Id.ToString();
                        }

                        else if (j == 0)
                        {
                            _adjMatrixCells[i][j] = Instantiate(_adjMatrixPrefab, _adjMatrixGridRows);
                            // Debug.Log("j == 0");
                            _adjMatrixCells[i][j].GetComponentInChildren<TMP_InputField>().text = _board.Vertices[i - 1].Id.ToString();
                        }

                        else
                        {
                            _adjMatrixCells[i][j] = Instantiate(_adjMatrixPrefab, _adjMatrixGridMain);
                            //Debug.Log("i != 0 && j != 0");
                            _adjMatrixCells[i][j].GetComponentInChildren<TMP_InputField>().text = _board.Graph.AdjacencyMatrix[i - 1, j - 1].ToString();
                        }

                    }
                }


                break;

            default:
                break;
        }


        float minX = 310 - (_board.Vertices.Count) * 50;
        float minY = -360 + (_board.Vertices.Count) * 50;
        minX = minX > 0 ? 0 : minX;
        minY = minY < 0 ? 0 : minY;
        _matrixMainTransform.anchoredPosition = new Vector2(Mathf.Clamp(_matrixMainTransform.anchoredPosition.x, minX, 0),
            Mathf.Clamp(_matrixMainTransform.anchoredPosition.y, 0, minY));

        _matrixRowsTransform.anchoredPosition = new Vector2(_matrixRowsTransform.anchoredPosition.x, _matrixMainTransform.anchoredPosition.y);
        _matrixColsTransform.anchoredPosition = new Vector2(_matrixMainTransform.anchoredPosition.x, _matrixColsTransform.anchoredPosition.y);
    }
}
