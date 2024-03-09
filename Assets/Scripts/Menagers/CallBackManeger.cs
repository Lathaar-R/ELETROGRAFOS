using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBackManeger : MonoBehaviour
{
    // Singleton
    public static CallBackManeger Instance { get; private set; }

    #region Fields and Properties
    // Public
    
    // Callbacks
    public Action onUpdateGraph;
    public Action SelectVertice;
    public Action SelectEdge;
    public Action MoveVertice;
    public Action VerificarGrafo;
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateGraph()
    {
        onUpdateGraph?.Invoke();
    }

    public void SelectVerticeButton()
    {
        SelectVertice?.Invoke();
    }

    public void SelectEdgeButton()
    {
        SelectEdge?.Invoke();
    }

    public void MoveVerticeButton()
    {
        MoveVertice?.Invoke();
    }

    public void VerificarGrafoButton()
    {
        VerificarGrafo?.Invoke();
    }
}
