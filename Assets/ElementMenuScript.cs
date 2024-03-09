using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementMenuScript : MonoBehaviour
{
    #region Fields and Properties
    [SerializeField] private Button _verticeButton;
    [SerializeField] private Button _edgeButton;
    [SerializeField] private Button _moveVerticeButton;
    [SerializeField] private Image _verticeImage;
    [SerializeField] private Image _edgeImage;
    [SerializeField] private Image _moveVerticeImage;


    #endregion

    #region Enable and Disable Callbacks
    void OnEnable()
    {
        CallBackManeger.Instance.SelectVertice += OnVerticeSelected;
        CallBackManeger.Instance.SelectEdge += OnEdgeSelected;
        CallBackManeger.Instance.MoveVertice += OnMoveVertice;
    }

    void OnDisable()
    {
        CallBackManeger.Instance.SelectVertice -= OnVerticeSelected;
        CallBackManeger.Instance.SelectEdge -= OnEdgeSelected;
        CallBackManeger.Instance.MoveVertice -= OnMoveVertice;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {

    }

    public void OnVerticeClick()
    {
        CallBackManeger.Instance.SelectVerticeButton();
    }

    public void OnEdgeClick()
    {
        CallBackManeger.Instance.SelectEdgeButton();
    }

    public void OnMoveVerticeClick()
    {
        CallBackManeger.Instance.MoveVerticeButton();
    }

    #region Callbacks

    private void OnEdgeSelected()
    {
        _verticeImage.color = Color.white;
        _moveVerticeImage.color = Color.white;
        _edgeImage.color = Color.green;
    }

    private void OnVerticeSelected()
    {
        _verticeImage.color = Color.green;
        _moveVerticeImage.color = Color.white;
        _edgeImage.color = Color.white;
    }

    private void OnMoveVertice()
    {
        _verticeImage.color = Color.white;
        _moveVerticeImage.color = Color.green;
        _edgeImage.color = Color.white;
    }

    #endregion
}
