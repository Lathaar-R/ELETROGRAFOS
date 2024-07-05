using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Text.RegularExpressions;

public class RepresentadorScript : MonoBehaviour
{
    private StartAnimationHandler _startAnimationHandler;
    [SerializeField] private TMP_InputField[] _representadorInputField;
    [SerializeField] private BoardScript _board;
    [SerializeField] private GameObject verticePrefab;
    [SerializeField] private GameObject edgePrefab;
    [SerializeField] private Transform[] placeSequence;
    private EventSystem _eventSystem;
    [SerializeField] private int _index = 0;
    [SerializeField] private int _maxIndex = 0;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    public Transform[] PlaceSequence { get => placeSequence; set => placeSequence = value; }

    private void OnEnable()
    {
        CallBackManeger.Instance.onUpdateGraph += OnUpdateGraph;
        //CallBackManeger.Instance.onStartLevelAnimation += _startAnimationHandler.MoveToCenterCanvas;
    }

    private void OnDisable()
    {
        CallBackManeger.Instance.onUpdateGraph -= OnUpdateGraph;
        //CallBackManeger.Instance.onStartLevelAnimation -= _startAnimationHandler.MoveToCenterCanvas;
    }

    void Awake()
    {
        // Debug.Log(_representadorInputField.text.Split('\n').Length);
        _representadorInputField[0].text = "V = {}";
        _representadorInputField[1].text = "E = {}";
        _eventSystem = EventSystem.current;

        _startAnimationHandler = new StartAnimationHandler(_transform, Vector2.down, LevelType.PedidosEscritos | LevelType.PedidosRepresentados);
       // _startAnimationHandler.MoveToStartCanvas();
    }

    private void Update()
    {
        for (int i = 0; i < _representadorInputField.Length; i++)
        {
            if (_eventSystem.currentSelectedGameObject == _representadorInputField[i].gameObject)
            {
                if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
                {

                }
            }
        }


        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
        {

            #region Old Code
            // var lines = new List<string>(_representadorInputField.text.Split('\n'));
            // var vertices = new List<string>();
            // var edges = new List<string>();

            // for (int i = 0; i < lines.Count; i++)
            // {
            //     var input = lines[i].Split(' ');
            //     if (input[0].Length == 0)
            //         continue;

            //     if (input.Length == 1)
            //     {
            //         if (!int.TryParse(input[0], out int n))
            //             continue;

            //         if (n > 12)
            //             continue;

            //         if (vertices.Contains(input[0]))
            //             continue;

            //         vertices.Add(input[0]);

            //         var v = _board.Vertices.Find(v => v.Id == n);
            //         if (v == null)
            //         {
            //             // Instantiate Vertice
            //             // var component = Instantiate(verticePrefab, Vector3.zero, Quaternion.identity);
            //             // var vertice = component.GetComponent<Vertice>();
            //             var t = placeSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
            //             // component.transform.position = t.position;

            //             // Add Vertice to Graph
            //             _board.AddVertice(t.position, int.Parse(t.name), n);
            //         }
            //     }
            //     else if (input.Length == 2)
            //     {
            //         if (!(int.TryParse(input[0], out int v) && int.TryParse(input[1], out int w)))
            //         {
            //             continue;
            //         }

            //         if (edges.Contains(input[0] + " " + input[1]) || edges.Contains(input[1] + " " + input[0]))
            //             continue;

            //         edges.Add(input[0] + " " + input[1]);

            //         var e = _board.Vertices.FindAll(vert => vert.Id == v || vert.Id == w);
            //         if (e.Count == 2 && !_board.Edges.Any(edge => (edge.V.Id == v && edge.W.Id == w) || (edge.V.Id == w && edge.W.Id == v)))
            //         {
            //             var edge = Instantiate(edgePrefab, Vector3.zero, Quaternion.identity);
            //             Vertice ve = _board.Vertices.Find(vert => vert.Id == v);
            //             Vertice we = _board.Vertices.Find(vert => vert.Id == w);
            //             _board.AddEdge(ve, we);
            //         }
            //     }
            // }

            // for (int i = 0; i < _board.Vertices.Count; i++)
            // {
            //     if (!vertices.Contains(_board.Vertices[i].Id.ToString()))
            //     {
            //         _board.RemoveVertice(_board.Vertices[i].BoardId);
            //     }
            // }

            // for (int i = 0; i < _board.Edges.Count; i++)
            // {
            //     if (!edges.Contains(_board.Edges[i].V.Id + " " + _board.Edges[i].W.Id) && !edges.Contains(_board.Edges[i].W.Id + " " + _board.Edges[i].V.Id))
            //     {
            //         Edge edge = _board.Edges[i];
            //         _board.RemoveEdge(edge);
            //     }
            // }

            // // List<string> newLines = new List<string>();
            // // List<string> vertices = new List<string>();
            // // List<string> edges = new List<string>();

            // // foreach (var line in lines)
            // // {
            // //     var input = line.Split(' ');

            // //     if (input[0].Length == 0)
            // //         continue;

            // //     if (input.Length == 1)
            // //     {
            // //         if (!int.TryParse(input[0], out int n))
            // //             continue;

            // //         if (n > 12)
            // //             continue;

            // //         if(vertices.Contains(input[0]))
            // //             continue;

            // //         vertices.Add(input[0]);

            // //         var v = _board.Vertices.Find(v => v.Id == n);
            // //         if (v == null)
            // //         {
            // //             // Instantiate Vertice
            // //             var component = Instantiate(verticePrefab, Vector3.zero, Quaternion.identity);
            // //             var vertice = component.GetComponent<Vertice>();
            // //             var t = placeSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
            // //             component.transform.position = t.position;

            // //             // Add Vertice to Graph
            // //             _board.AddVertice(vertice, int.Parse(t.name), n);
            // //         }
            // //     }
            // //     else if (input.Length == 2)
            // //     {
            // //         if (!(int.TryParse(input[0], out int v) && int.TryParse(input[1], out int w)))
            // //         {
            // //             continue;
            // //         }

            // //         if (edges.Contains(input[0] + " " + input[1]) || edges.Contains(input[1] + " " + input[0]))
            // //             continue;

            // //         edges.Add(input[0] + " " + input[1]);

            // //         var e = _board.Vertices.FindAll(vert => vert.Id == v || vert.Id == w);
            // //         if (e.Count == 2 && !_board.Edges.Any(edge => (edge.V.Id == v && edge.W.Id == w) || (edge.V.Id == w && edge.W.Id == v)))
            // //         {
            // //             var edge = Instantiate(edgePrefab, Vector3.zero, Quaternion.identity);
            // //             _board.AddEdge(edge.GetComponent<Edge>(), v, w);
            // //         }
            // //     }
            // // }


            // // var newText = string.Join("\n", vertices.ToArray()) + "\n" + string.Join("\n", edges.ToArray()) + "\n";
            // // Debug.Log(newText);

            // // _representadorInputField.text = newText;
            #endregion

        }

    }

    public void OnSelectRepresentador()
    {
        for (int i = 0; i < _representadorInputField.Length; i++)
        {
            if (_eventSystem.currentSelectedGameObject == _representadorInputField[i].gameObject)
            {
                StartCoroutine(MoveToNextInputField(i));
                Debug.Log(_representadorInputField[i].stringPosition);
                _representadorInputField[i].MoveTextEnd(false);
                _representadorInputField[i].stringPosition = 3;
                _representadorInputField[i].caretPosition = 3;
                Debug.Log(_representadorInputField[i].stringPosition);

            }
        }
    }

    private IEnumerator MoveToNextInputField(int i)
    {
        yield return new WaitForEndOfFrame();

    }

    public void OnEndEditVertice(string value)
    {
        // Check if string is in the for V = {1, 2, 3, 4, 5, ...}
        string pattern = @"V\s*=\s*\{\s*(?:\d+\s*,?\s*)*\}";
        var rightFormat = Regex.IsMatch(value, pattern);
        if (rightFormat)
        {
            List<int> vertices = new();
            List<int> verticesList = new();

            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsNumber(value[i]))
                {
                    vertices.Add(int.Parse(value[i].ToString()));

                    if (value.Length >= i + 1 && char.IsNumber(value[i + 1]))
                    {
                        int n = int.Parse(vertices[^1].ToString() + value[i + 1]);
                        vertices[^1] = n;
                        i++;
                    }
                }
            }

            foreach (var vertice in vertices)
            {
                Debug.Log(vertice);
                var v = _board.Vertices.Find(v => v.Id == vertice);
                if (v == null)
                {
                    verticesList.Add(vertice);
                }

            }

            foreach (var n in verticesList)
            {
                // add vertice to graph
                var t = placeSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
                var b = _board.AddVertice(t.position, int.Parse(t.name), n);
                Debug.Log(b);
            }

            for (int i = _board.Vertices.Count - 1; i >= 0; i--)
            {
                var vertice = _board.Vertices[i];
                if (!vertices.Contains(vertice.Id))
                {
                    _board.RemoveVertice(vertice.BoardId);
                }
            }
        }
        else
        {
            WrongFormat(0);

        }
    }
    public void OnEndEditAresta(string value)
    {
        // Check if string is in the for E = {(1, 2), (2, 3), (3, 4), ...}
        string pattern = @"E\s*=\s*\{(\s*(\(\s*\d+\s*,\s*\d+\s*\)\s*,?\s*)*)?\}";
        var rightFormat = Regex.IsMatch(value, pattern);

        if (rightFormat)
        {
            List<string> edges = new();
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsNumber(value[i]))
                {
                    edges.Add(value[i].ToString());

                    if (value.Length >= i + 1 && char.IsNumber(value[i + 1]))
                    {
                        var n = edges[^1] + value[i + 1];
                        edges[^1] = n;
                        i++;
                    }

                }
            }

            foreach (var edge in edges)
            {
                Debug.Log(edge);
            }

            List<(Vertice, Vertice)> vertices = new();

            for (int j = 0; j < edges.Count; j += 2)
            {
                if (int.TryParse(edges[j].ToString(), out int v) && int.TryParse(edges[j + 1].ToString(), out int w))
                {
                    var ve = _board.Vertices.Find(vert => vert.Id == v);
                    var we = _board.Vertices.Find(vert => vert.Id == w);
                    if (ve != null && we != null)
                    {
                        //if (!_board.Edges.Any(edge => (edge.V.Id == v && edge.W.Id == w) || (edge.V.Id == w && edge.W.Id == v)))
                        //{
                        vertices.Add((ve, we));
                        //}
                    }
                    else
                    {
                        WrongFormat(1);
                        vertices.Clear();
                        break;
                    }
                }
                else
                {
                    WrongFormat(1);
                    vertices.Clear();
                    break;
                }
            }

            foreach (var (v, w) in vertices)
            {
                if (!_board.Edges.Any(edge => (edge.V.Id == v.Id && edge.W.Id == w.Id) || (edge.V.Id == w.Id && edge.W.Id == v.Id)))
                {
                    _board.AddEdge(v, w);
                }
            }

            List<Edge> edgesToRemove = new();
            for (int i = 0; i < _board.Edges.Count; i++)
            {
                var edge = _board.Edges[i];
                if (!vertices.Any(v => (v.Item1.Id == edge.V.Id && v.Item2.Id == edge.W.Id) || (v.Item1.Id == edge.W.Id && v.Item2.Id == edge.V.Id)))
                {
                    edgesToRemove.Add(edge);
                }
            }

            _board.RemoveEdge(edgesToRemove.ToArray());
        }
        else
        {
            WrongFormat(1);
        }
    }

    private void WrongFormat(int i)
    {
        //tween the color of the input field to red than back to white
        _representadorInputField[i].image.DOColor(Color.red, 0.5f).OnComplete(() => _representadorInputField[i].image.DOColor(Color.white, 0.5f));
    }

    public bool IsStringInCorrectFormat(string input)
    {
        // The pattern matches "E = {(a, b), (c, d), ..., (y, z)}"
        // \d+ matches one or more digits
        // \s* matches zero or more spaces
        // \(, \), \{, and \} escape the special characters (, ), {, and }
        // (?:...) is a non-capturing group
        // ,? matches zero or one comma
        // The * after the non-capturing group allows for zero or more repetitions of the group
        string pattern = @"E\s*=\s*\{\s*(?:\(\s*\d+\s*,\s*\d+\s*\)\s*,?\s*)*\}";

        // Use the Regex.IsMatch method to check if the input matches the pattern
        return Regex.IsMatch(input, pattern);
    }

    public void OnValueChanged(string value)
    {
        var lines = new List<string>(value.Split('\n'));

        // if (lines.Any(line => line.Length > 5))
        // {
        //     int i = 0;
        //     List<string> newLines = new List<string>();
        //     newLines = new List<string>(lines);

        //     foreach (var line in lines)
        //     {
        //         if (line.Length > 5)
        //         {
        //             newLines[i] = line[..5];
        //         }
        //         i++;
        //     }
        //     lines = new List<string>(newLines);

        //     _representadorInputField.text = string.Join("\n", lines.ToArray());
        // }

        // if (lines.Count >= 2 && lines[^1].Length == 0 && lines[^2].Length == 0)
        // {
        //     lines.RemoveAt(lines.Count - 1);
        //     _representadorInputField.text = string.Join("\n", lines.ToArray());
        // }

    }

    private void OnUpdateGraph()
    {
        for (int i = 0; i < _representadorInputField.Length; i++)
        {
            if (_representadorInputField[i].gameObject.name.Equals("Vertices"))
            {
                var vertices = _board.Vertices.Select(v => v.Id.ToString()).ToArray();
                _representadorInputField[i].text = "V = {" + string.Join(", ", vertices) + "}";
            }
            else if (_representadorInputField[i].gameObject.name.Equals("Arestas"))
            {
                var edges = _board.Edges.Select(e => "(" + e.V.Id + ", " + e.W.Id + ")").ToArray();
                _representadorInputField[i].text = "E = {" + string.Join(", ", edges) + "}";
            }
        }
    }

    // private void OnStartLevelAnimation()
    // {
    //     //_transform = gameObject.GetComponent<RectTransform>();
    //     _transform.anchoredPosition = startPos;

    //     Invoke(nameof(Move), 0.5f);
    // }

    // private void Move()
    // {
    //     _transform.DOAnchorPos(endPos, 0.5f + UnityEngine.Random.value * 1f).SetEase(Ease.OutCubic);
    // }

    #region Old Code
    // // Update is called once per frame
    // void Update()
    // {
    //     if (Keyboard.current.upArrowKey.wasPressedThisFrame)
    //     {
    //         var i = (_index - 1);
    //         if (i >= 0)
    //         {
    //             _eventSystem = EventSystem.current;
    //             _eventSystem.SetSelectedGameObject(_representadorInputField[i].gameObject);
    //         }
    //     }
    //     else if (Keyboard.current.downArrowKey.wasPressedThisFrame)
    //     {
    //         var i = (_index + 1);
    //         if (i < _representadorInputField.Length)
    //         {
    //             _eventSystem = EventSystem.current;
    //             _eventSystem.SetSelectedGameObject(_representadorInputField[i].gameObject);
    //         }
    //     }
    //     else if (Keyboard.current.enterKey.wasPressedThisFrame)
    //     {
    //         var i = (_index + 1);
    //         if (i < _representadorInputField.Length)
    //         {
    //             _eventSystem = EventSystem.current;
    //             _eventSystem.SetSelectedGameObject(_representadorInputField[i].gameObject);
    //         }
    //     }
    // }

    // #region Callbacks
    // public void OnEndEdit(string value)
    // {
    //     _emptyIndex = 0;
    //     for (int i = 0; i < _representadorInputField.Length; i++)
    //     {
    //         var input = _representadorInputField[i].text.Split(' ');
    //         // Check if its empty
    //         if (input[0].Length == 0)
    //             continue;

    //         else if (input.Length == 1)
    //         {
    //             Debug.Log("One");
    //             if (!int.TryParse(input[0], out int n))
    //             {
    //                 _representadorInputField[i].text = "";
    //                 continue;
    //             }
    //             var v = _board.Vertices.Find(v => v.Id == n);
    //             if (v == null)
    //             {
    //                 Debug.Log("Vertice");
    //                 // Instantiate Vertice
    //                 var component = Instantiate(verticePrefab, Vector3.zero, Quaternion.identity);
    //                 var vertice = component.GetComponent<Vertice>();
    //                 var t = placeSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
    //                 component.transform.position = t.position;

    //                 // Add Vertice to Graph
    //                 _board.AddVertice(vertice, int.Parse(t.name), n);


    //             }
    //             _emptyIndex = i;
    //         }
    //         else if (input.Length == 2)
    //         {
    //             Debug.Log("Two");
    //             if (!(int.TryParse(input[0], out int v) && int.TryParse(input[1], out int w)))
    //             {
    //                 _representadorInputField[i].text = "";
    //                 continue;
    //             }

    //             var e = _board.Vertices.FindAll(vert => vert.Id == v || vert.Id == w);
    //             if (e.Count == 2)
    //             {
    //                 var edge = Instantiate(edgePrefab, Vector3.zero, Quaternion.identity);
    //                 _board.AddEdge(edge.GetComponent<Edge>(), v, w);
    //             }
    //             _emptyIndex = i;
    //         }
    //     }
    // }

    // public void OnSelectRepresentador()
    // {
    //     _eventSystem = EventSystem.current;
    //     if(_eventSystem.currentSelectedGameObject.TryGetComponent(out TMP_InputField curInputField))
    //     {
    //         if(curInputField.text.Length == 0)
    //         {
    //             int index = 0;
    //             while(index < _representadorInputField.Length && _representadorInputField[index].text.Length != 0)
    //             {
    //                 index++;
    //             }

    //             var curIndex = _representadorInputField.ToList().IndexOf(curInputField);
    //             _representadorInputField[curIndex].gameObject.transform.SetSiblingIndex(index);
    //             _representadorInputField[index].gameObject.transform.SetSiblingIndex(curIndex);

    //             var aux = _representadorInputField[index];
    //             _representadorInputField[index] = curInputField;
    //             _representadorInputField[curIndex] = aux;


    //         }            

    //         _index = _representadorInputField.ToList().IndexOf(curInputField);
    //     }






    //     // if (index <= _emptyIndex)
    //     // {
    //     //     _index = index;
    //     // }
    //     // else
    //     // {
    //     //     _index = 0;

    //     //     _representadorInputField[index].gameObject.transform.SetSiblingIndex(_index);
    //     //     _representadorInputField[index].gameObject.name = _index.ToString();
    //     //     _representadorInputField[_index].gameObject.transform.SetSiblingIndex(index);
    //     //     _representadorInputField[_index].gameObject.name = index.ToString();



    //     //     var aux = _representadorInputField[index];
    //     //     _representadorInputField[index] = _representadorInputField[_index];
    //     //     _representadorInputField[_index] = aux;
    //     // }
    // }
    // #endregion



    #endregion
}
