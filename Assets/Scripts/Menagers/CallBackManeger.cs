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
    public Action onStartLevel;
    public Action onEndLevel;
    public Action onStartLevelAnimation;
    public Action onEndLevelAnimation;

    public Action onUpdateGraph;
    public Action selectVertice;
    public Action selectEdge;
    public Action moveVertice;
    public Action weightEdge;
    public Action<bool> verificarGrafo;
    public Action grafoCorreto;
    public Action grafoIncorreto;
    public Action<bool> mudarDirigido;
    public Action onStartMenu;
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
        selectVertice?.Invoke();
    }

    public void SelectEdgeButton()
    {
        selectEdge?.Invoke();
    }

    public void MoveVerticeButton()
    {
        moveVertice?.Invoke();
    }

    public void WeightEdgeButton()
    {
        weightEdge?.Invoke();
    }

    public void VerificarGrafoButton(bool correct)
    {
        verificarGrafo?.Invoke(correct);
    }

    public void MudarDirigidoButton(bool directed)
    {
        mudarDirigido?.Invoke(directed);
    }

    public void OnStartLevel()
    {
        onStartLevel?.Invoke();
    }
}
