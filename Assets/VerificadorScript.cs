using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class VerificadorScript : MonoBehaviour
{
    [SerializeField] private BoardScript _board;

    public void OnVerificarPressed()
    {
        bool correct = false;

        switch (GameManagerScript.Instance.LevelType)
        {
            case LevelType.PedidosEscritos:
                correct = VerificarGrafo();
                break;
            case LevelType.PedidosRepresentados:
                
                break;
            case LevelType.Validacao:
                
                break;
        }
        

        GameManagerScript.Instance.OnVerificar(correct);
    }

    private bool VerificarGrafo()
    {
        bool result = true;

        result = result && TipoDoGrafo(GameManagerScript.Instance.CurrentPedido);
        Debug.Log(result);

        result = result && (GameManagerScript.Instance.CurrentPedido.vNum == _board.Vertices.Count);
        Debug.Log(result);

        result = result && (GameManagerScript.Instance.CurrentPedido.eNum == _board.Edges.Count);
        Debug.Log(result);

        foreach (var d in GameManagerScript.Instance.CurrentPedido.vDegrees)
        {
            result = result && _board.Vertices.Any(v => VerificarGrau(v, d));
            Debug.Log(result);
        }

        foreach (var d in GameManagerScript.Instance.CurrentPedido.vDegreesIn)
        {
            result = result && _board.Vertices.Any(v => VerificarGrau(v, d, true));
            Debug.Log(result);
        }

        foreach (var d in GameManagerScript.Instance.CurrentPedido.vDegreesOut)
        {
            result = result && _board.Vertices.Any(v => VerificarGrau(v, d, false));
            Debug.Log(result);
        }

        result = result && (GameManagerScript.Instance.CurrentPedido.isDirected == _board.Graph.IsDirected);
        Debug.Log(result);

        result = GameManagerScript.Instance.CurrentPedido.isRegular ? VerificarGrafoRegular() : result;
        Debug.Log(result);

        result = GameManagerScript.Instance.CurrentPedido.isComplete ? VerificarGrafoCompleto() : result;
        Debug.Log(result);

        result = GameManagerScript.Instance.CurrentPedido.isBipartite ? VerificarBiPartido() : result;
        Debug.Log(result);

        result = GameManagerScript.Instance.CurrentPedido.compConex.Length > 0 ? VerificarCompConex() : result;
        Debug.Log(result);

        return result;
    }

    private bool TipoDoGrafo(Pedido pedido)
    {
        Tipo tipo;
        var b = _board;
        var g = b.Graph;
        bool directed = g.IsDirected;
        bool multi = false;
        bool pseudo = false;

        foreach (Edge e in b.Edges)
        {
            foreach (Edge e2 in b.Edges)
            {
                if (e != e2)
                {
                    if ((e.V.Id == e2.V.Id && e.W.Id == e2.W.Id) || (e.V.Id == e2.W.Id && e.W.Id == e2.V.Id))
                    {
                        multi = true;
                        break;
                    }
                }
            }
            if (multi) break;
        }

        foreach (Edge v in b.Edges)
        {
            if (v.V.Id == v.W.Id)
            {
                pseudo = true;
                break;
            }
        }

        if (!directed && !multi && !pseudo)
        {
            tipo = Tipo.Simples;
        }
        else if (directed && !multi && !pseudo)
        {
            tipo = Tipo.Dirigido;
        }
        else if (!directed && multi && !pseudo)
        {
            tipo = Tipo.Multi;
        }
        else if (!directed && !multi && pseudo)
        {
            tipo = Tipo.Pseudo;
        }
        else if (directed && multi && !pseudo)
        {
            tipo = Tipo.MultiDirigido;
        }
        else if (directed && !multi && pseudo)
        {
            tipo = Tipo.PseudoDirigido;
        }
        else
        {
            tipo = Tipo.Simples;
        }


        return tipo == pedido.tipo;
    }

    private bool VerificarGrau(Vertice v, int grau, bool inOut = true)
    {

        if (inOut)
        {
            return v.InDegree == grau;
        }
        else
        {
            return v.OutDegree == grau;
        }

    }

    private bool VerificarCompConex()
    {
        // use the DFS algorithm to check how many connected components the graph has

        var g = _board.Graph;
        var visited = new bool[g.AdjacencyList.Length];
        int count = 0;

        for (int i = 0; i < g.AdjacencyList.Length; i++)
        {
            if (!visited[i])
            {
                DFS(i, visited, g);
                count++;
            }
        }

        return count == GameManagerScript.Instance.CurrentPedido.compConex.Length;
    }

    private bool VerificarGrafoRegular()
    {
        var g = _board.Graph;
        var vertices = _board.Vertices;

        if (g.IsDirected)
        {
            foreach (var v in vertices)
            {
                if (v.InDegree != v.OutDegree)
                {
                    return false;
                }
            }
        }
        else
        {
            foreach (var v in vertices)
            {
                if (v.Degree != vertices[0].Degree)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool VerificarGrafoCompleto()
    {
        var g = _board.Graph;
        var vertices = _board.Vertices;

        if (g.IsDirected)
        {
            return false;
        }
        else
        {
            foreach (var v in vertices)
            {
                if (v.Degree != vertices.Count - 1)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool VerificarBiPartido()
    {
        var g = _board.Graph;
        var vertices = _board.Vertices;
        var visited = new bool[g.AdjacencyList.Length];
        var colors = new int[g.AdjacencyList.Length];

        for (int i = 0; i < g.AdjacencyList.Length; i++)
        {
            if (!visited[i])
            {
                if (!BFS(i, visited, colors, g))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private bool BFS(int v, bool[] visited, int[] colors, Graph g)
    {
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(v);
        visited[v] = true;
        colors[v] = 0;

        while (queue.Count > 0)
        {
            int current = queue.Dequeue();

            foreach (var w in g.AdjacencyList[current])
            {
                if (!visited[w])
                {
                    visited[w] = true;
                    colors[w] = 1 - colors[current];
                    queue.Enqueue(w);
                }
                else if (colors[w] == colors[current])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void DFS(int v, bool[] visited, Graph g)
    {
        visited[v] = true;

        foreach (var w in g.AdjacencyList[v])
        {
            if (!visited[w])
            {
                DFS(w, visited, g);
            }
        }
    }
}
