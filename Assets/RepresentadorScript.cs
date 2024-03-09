using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RepresentadorScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField _representadorInputField;
    [SerializeField] private BoardScript _board;
    [SerializeField] private GameObject verticePrefab;
    [SerializeField] private GameObject edgePrefab;
    [SerializeField] private Transform[] placeSequence;
    private EventSystem _eventSystem;
    [SerializeField] private int _index = 0;
    [SerializeField] private int _maxIndex = 0;


    private void OnEnable()
    {
        CallBackManeger.Instance.onUpdateGraph += OnUpdateGraph;
    }

    private void OnDisable()
    {
        CallBackManeger.Instance.onUpdateGraph -= OnUpdateGraph;
    }

    void Awake()
    {
        Debug.Log(_representadorInputField.text.Split('\n').Length);
        _representadorInputField.text = "";
        _eventSystem = EventSystem.current;
    }

    private void Update()
    {
        if (_eventSystem.currentSelectedGameObject != _representadorInputField.gameObject)
            return;


        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame)
        {
            var lines = new List<string>(_representadorInputField.text.Split('\n'));
            var vertices = new List<string>();
            var edges = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                var input = lines[i].Split(' ');
                if (input[0].Length == 0)
                    continue;

                if (input.Length == 1)
                {
                    if (!int.TryParse(input[0], out int n))
                        continue;

                    if (n > 12)
                        continue;

                    if (vertices.Contains(input[0]))
                        continue;

                    vertices.Add(input[0]);

                    var v = _board.Vertices.Find(v => v.Id == n);
                    if (v == null)
                    {
                        // Instantiate Vertice
                        var component = Instantiate(verticePrefab, Vector3.zero, Quaternion.identity);
                        var vertice = component.GetComponent<Vertice>();
                        var t = placeSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
                        component.transform.position = t.position;

                        // Add Vertice to Graph
                        _board.AddVertice(vertice, int.Parse(t.name), n);
                    }
                }
                else if (input.Length == 2)
                {
                    if (!(int.TryParse(input[0], out int v) && int.TryParse(input[1], out int w)))
                    {
                        continue;
                    }

                    if (edges.Contains(input[0] + " " + input[1]) || edges.Contains(input[1] + " " + input[0]))
                        continue;

                    edges.Add(input[0] + " " + input[1]);

                    var e = _board.Vertices.FindAll(vert => vert.Id == v || vert.Id == w);
                    if (e.Count == 2 && !_board.Edges.Any(edge => (edge.V.Id == v && edge.W.Id == w) || (edge.V.Id == w && edge.W.Id == v)))
                    {
                        var edge = Instantiate(edgePrefab, Vector3.zero, Quaternion.identity);
                        _board.AddEdge(edge.GetComponent<Edge>(), v, w);
                    }
                }
            }

            for (int i = 0; i < _board.Vertices.Count; i++)
            {
                if (!vertices.Contains(_board.Vertices[i].Id.ToString()))
                {
                    _board.RemoveVertice(_board.Vertices[i].BoardId);
                }
            }

            for (int i = 0; i < _board.Edges.Count; i++)
            {
                if (!edges.Contains(_board.Edges[i].V.Id + " " + _board.Edges[i].W.Id) && !edges.Contains(_board.Edges[i].W.Id + " " + _board.Edges[i].V.Id))
                {
                    Edge edge = _board.Edges[i];
                    _board.RemoveEdge(edge);
                }
            }

            // List<string> newLines = new List<string>();
            // List<string> vertices = new List<string>();
            // List<string> edges = new List<string>();

            // foreach (var line in lines)
            // {
            //     var input = line.Split(' ');

            //     if (input[0].Length == 0)
            //         continue;

            //     if (input.Length == 1)
            //     {
            //         if (!int.TryParse(input[0], out int n))
            //             continue;

            //         if (n > 12)
            //             continue;

            //         if(vertices.Contains(input[0]))
            //             continue;

            //         vertices.Add(input[0]);

            //         var v = _board.Vertices.Find(v => v.Id == n);
            //         if (v == null)
            //         {
            //             // Instantiate Vertice
            //             var component = Instantiate(verticePrefab, Vector3.zero, Quaternion.identity);
            //             var vertice = component.GetComponent<Vertice>();
            //             var t = placeSequence.First<Transform>(t => !_board.Vertices.Find(vert => vert.BoardId == int.Parse(t.name)));
            //             component.transform.position = t.position;

            //             // Add Vertice to Graph
            //             _board.AddVertice(vertice, int.Parse(t.name), n);
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
            //             _board.AddEdge(edge.GetComponent<Edge>(), v, w);
            //         }
            //     }
            // }


            // var newText = string.Join("\n", vertices.ToArray()) + "\n" + string.Join("\n", edges.ToArray()) + "\n";
            // Debug.Log(newText);

            // _representadorInputField.text = newText;

        }

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
        var vertices = _board.Vertices.Select(v => v.Id.ToString()).ToArray();
        var edges = _board.Edges.Select(e => e.V.Id + " " + e.W.Id).ToArray();
        _representadorInputField.text = string.Join("\n", vertices) + "\n" + string.Join("\n", edges) + "\n";
    }

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
