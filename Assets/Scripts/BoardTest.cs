using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BoardTest : MonoBehaviour
{
    public TMP_InputField inputFieldV;
    public TMP_InputField inputFieldE;
    public TMP_Text text;
    public TMP_Text text2;
    public BoardScript board;

    public GameObject verticePrefab;
    public GameObject edgePrefab;

    // Start is called before the first frame update
    void Start()
    {
        CallBackManeger.Instance.onUpdateGraph += OnUpdateGraph;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnUpdateGraph()
    {

        var graph = board.Graph;
        //Debug.Log(graph.AdjacencyList.Length);
        //print the adjacency list
        string result = "";

        for (int i = 0; i < graph.AdjacencyList.Length; i++)
        {

            if (graph.AdjacencyList[i] != null)
            {
                result += i + ": ";
                foreach (var edge in graph.AdjacencyList[i])
                {
                    result += edge + " ";
                }
                result += "\n";
            }

        }

        text.text = result;

        //print the adj matrix[,]
        result = "";

        for (int i = 0; i < graph.AdjacencyList.Length; i++)
        {
            if (graph.AdjacencyList[i] != null)
            {
                result += i + "| ";
                for (int j = 0; j < graph.AdjacencyList.Length; j++)
                {
                    if (graph.AdjacencyList[j] == null) continue;
                    result += graph.AdjacencyList[i].Contains(j) ? "1 " : "0 ";
                }
                result += "\n";
            }

        }

        text2.text = result;
    }
}
