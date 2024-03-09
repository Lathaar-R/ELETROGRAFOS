using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseScript : MonoBehaviour
{
    #region State Machine Variables
    // State Actions
    private Action _currentState;
    private Action _enterState;
    private Action _exitState;

    private Action _selectedElementFunction;

    #endregion

    #region Fields and Properties
    // Private Serialized
    [SerializeField] private BoardScript _board;
    [SerializeField] private LayerMask plugsMask;

    // Private
    private Vertice vVertice;
    private Vertice wVertice;
    private Edge curEdge;
    private LineRenderer curEdgeLine;

    // placeholder variables
    public GameObject verticePrefab;
    public GameObject edgePrefab;
    public Image funcImage;


    #endregion

    private void Awake()
    {
        ChangeState(GamePlayState, null, null);

        ChangeSelectedElementFunction(VerticeFunction);


    }

    private void OnEnable()
    {
        // InputManeger.Instance.GameInputs.Keyboard.W.performed += OnWPressed;
        CallBackManeger.Instance.SelectVertice += OnVerticeSelect;
        CallBackManeger.Instance.SelectEdge += OnEdgeSelect;
        CallBackManeger.Instance.MoveVertice += OnMoveVertice;
    }

    private void OnDisable()
    {
        // InputManeger.Instance.GameInputs.Keyboard.W.performed -= OnWPressed;
        CallBackManeger.Instance.SelectVertice -= OnVerticeSelect;
        CallBackManeger.Instance.SelectEdge -= OnEdgeSelect;
        CallBackManeger.Instance.MoveVertice -= OnMoveVertice;
    }

    void Start()
    {

    }


    void Update()
    {
        _currentState?.Invoke();
    }

    #region State Machine

    public void ChangeState(Action newState, Action enterState, Action exitState)
    {
        _exitState?.Invoke();
        _currentState = newState;
        _enterState = enterState;
        _exitState = exitState;
        _enterState?.Invoke();
    }

    public void ChangeSelectedElementFunction(Action newFunction)
    {
        _selectedElementFunction = newFunction;

        vVertice?.Deselect();
        vVertice = null;
        wVertice = null;
        curEdge = null;
        curEdgeLine = null;
    }

    public void GamePlayState()
    {
        _selectedElementFunction?.Invoke();
    }

    public void VerticeFunction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), plugsMask);

            if (hit != null)
            {
                int boardId = int.TryParse(hit.name, out int i) ? i : -1;
                if (boardId == -1)
                    Debug.Log("Error: Id not found");
                else if (_board.Vertices.Find(v => v.BoardId == boardId) != null)
                {
                    Debug.Log("Error: Id already exists");
                }
                else
                {
                    // Instantiate Vertice
                    var component = Instantiate(verticePrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                    var vertice = component.GetComponent<Vertice>(); ;
                    component.transform.position = hit.transform.position;

                    // Add Vertice to Graph
                    _board.AddVertice(vertice, boardId);


                    //Debug.Log("Hit " + hit.name + " at " + component.transform.position);

                }


            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //Remove Vertice that was clicked
            var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), plugsMask);

            if (hit != null)
            {
                int boardId = int.TryParse(hit.name, out int i) ? i : -1;
                var vert = _board.Vertices.Find(v => v.BoardId == boardId);

                if (vert == null)
                    Debug.Log("Error: Id not found");

                else
                {
                    _board.RemoveVertice(boardId);
                }
            }
        }
    }

    private void MoveVerticeFunction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), plugsMask);

            if (hit != null)
            {
                if (vVertice == null)
                {
                    int boardId = int.TryParse(hit.name, out int i) ? i : -1;
                    if (boardId == -1)
                        Debug.Log("Error: Id not found");

                    else
                    {
                        vVertice = _board.Vertices.Find(v => v.BoardId == boardId);
                        if (vVertice != null)
                        {
                            Debug.Log("Vertice " + vVertice.Id + " selected" + " BoardId: " + vVertice.BoardId);
                            vVertice.Select();
                        }
                        else
                        {
                            Debug.Log("Error: Vertice not found");
                        }
                    }
                }
                else
                {
                    int boardId = int.TryParse(hit.name, out int i) ? i : -1;
                    if (boardId == -1)
                        Debug.Log("Error: Id not found");

                    else
                    {
                        wVertice = _board.Vertices.Find(v => v.BoardId == boardId);
                        if (wVertice != null)
                        {
                            wVertice.BoardId = vVertice.BoardId;
                            wVertice.transform.position = vVertice.transform.position;
                        }

                        vVertice.BoardId = boardId;
                        vVertice.transform.position = hit.transform.position;
                        vVertice.Deselect();
                        vVertice = null;

                        _board.UpdateGraph();
                    }
                }
            }
        }
    }

    private void EdgeFunction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (vVertice != null)
            {
                var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), plugsMask);

                if (hit != null)
                {
                    int boardId = int.TryParse(hit.name, out int i) ? i : -1;
                    if (boardId == -1)
                        Debug.Log("Error: Id not found");

                    else
                    {
                        wVertice = _board.Vertices.Find(v => v.BoardId == boardId);
                        if (wVertice != null)
                        {
                            if(_board.Edges.Where(e => (e.V.Id == vVertice.Id && e.W.Id == wVertice.Id) || (e.V.Id == wVertice.Id && e.W.Id == vVertice.Id)).Count() > 1)
                            {
                                Debug.Log("Error: More tahn two edges between the same vertices are not allowed");
                                vVertice.Deselect();
                                vVertice = null;
                                wVertice = null;
                                return;
                            }
                            Debug.Log("Vertice " + wVertice.Id + " selected" + " BoardId: " + wVertice.BoardId);
                            Vector2 pos = vVertice.transform.position;

                            curEdge = Instantiate(edgePrefab, Vector3.zero, Quaternion.identity).GetComponent<Edge>();
                            curEdge.V = vVertice;
                            curEdge.W = wVertice;
                            curEdgeLine = curEdge.GetComponentInChildren<LineRenderer>();


                            _board.AddEdge(curEdge, vVertice.Id, wVertice.Id);


                            curEdge = null;
                            vVertice.Deselect();
                            vVertice = null;
                            wVertice = null;
                            curEdgeLine = null;
                        }
                        else
                        {
                            Debug.Log("Error: Vertice not found");
                        }
                    }
                }
                else
                {
                    Destroy(curEdge.gameObject);
                    curEdge = null;
                    vVertice.Deselect();
                    vVertice = null;
                    curEdgeLine = null;
                }
            }
            else
            {
                var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), plugsMask);

                if (hit != null)
                {
                    int boardId = int.TryParse(hit.name, out int i) ? i : -1;
                    if (boardId == -1)
                        Debug.Log("Error: Id not found");

                    else
                    {
                        vVertice = _board.Vertices.Find(v => v.BoardId == boardId);
                        if (vVertice != null)
                        {
                            Debug.Log("Vertice " + vVertice.Id + " selected" + " BoardId: " + vVertice.BoardId);
                            vVertice.Select();
                        }
                        else
                        {
                            Debug.Log("Error: Vertice not found");
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            //Remove Edge that was clicked
            var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), LayerMask.GetMask("Wire"));

            if (hit != null)
            {
                if (hit.TryGetComponent<Edge>(out var edge))
                {
                    _board.RemoveEdge(edge);
                }
                else
                {
                    Debug.Log("Error: Edge not found");
                }
            }
        } 
    }

    #endregion

    #region Input Callbacks
    private void OnWPressed(InputAction.CallbackContext context)
    {
        if (_selectedElementFunction == VerticeFunction)
        {
            ChangeSelectedElementFunction(EdgeFunction);
            funcImage.color = Color.red;
        }
        else
        {
            ChangeSelectedElementFunction(VerticeFunction);
            funcImage.color = Color.green;
        }
    }

    private void OnVerticeSelect()
    {
        if (_selectedElementFunction != VerticeFunction)
        {
            ChangeSelectedElementFunction(VerticeFunction);
        }
    }

    private void OnEdgeSelect()
    {
        if (_selectedElementFunction != EdgeFunction)
        {
            ChangeSelectedElementFunction(EdgeFunction);
        }
    }

    private void OnMoveVertice()
    {
        if (_selectedElementFunction != MoveVerticeFunction)
        {
            ChangeSelectedElementFunction(MoveVerticeFunction);
        }
    }
    #endregion
}
