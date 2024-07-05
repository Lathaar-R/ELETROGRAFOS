using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ElementMenuScript : MonoBehaviour
{
    #region Fields and Properties
    private StartAnimationHandler _startAnimationHandler;
    [SerializeField] private Button _verticeButton;
    [SerializeField] private Button _edgeButton;
    [SerializeField] private Button _moveVerticeButton;
    [SerializeField] private Button _weightButton;
    [SerializeField] private Image _verticeImage;
    [SerializeField] private Image _edgeImage;
    [SerializeField] private Image _moveVerticeImage;
    [SerializeField] private Image _weightImage;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;


    #endregion

    private void Awake() {
        _verticeImage.color = Color.green;
        _moveVerticeImage.color = Color.white;
        _edgeImage.color = Color.white;
        _weightImage.color = Color.white;

        _startAnimationHandler = new StartAnimationHandler(_transform, Vector2.left, LevelType.PedidosEscritos | LevelType.PedidosRepresentados);
        //_startAnimationHandler.MoveToStartCanvas();
    }

    #region Enable and Disable Callbacks
    void OnEnable()
    {
        CallBackManeger.Instance.selectVertice += OnVerticeSelected;
        CallBackManeger.Instance.selectEdge += OnEdgeSelected;
        CallBackManeger.Instance.moveVertice += OnMoveVerticeSelected;
        CallBackManeger.Instance.weightEdge += OnWeightSelected;
        //CallBackManeger.Instance.onStartLevelAnimation += _startAnimationHandler.MoveToCenterCanvas;
    }

    void OnDisable()
    {
        CallBackManeger.Instance.selectVertice -= OnVerticeSelected;
        CallBackManeger.Instance.selectEdge -= OnEdgeSelected;
        CallBackManeger.Instance.moveVertice -= OnMoveVerticeSelected;
        CallBackManeger.Instance.weightEdge -= OnWeightSelected;
        //CallBackManeger.Instance.onStartLevelAnimation -= _startAnimationHandler.MoveToCenterCanvas;
    }
    #endregion

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

    public void OnWeightClick()
    {
        CallBackManeger.Instance.WeightEdgeButton();
    }

    #region Callbacks

    private void OnEdgeSelected()
    {
        _verticeImage.color = Color.white;
        _moveVerticeImage.color = Color.white;
        _edgeImage.color = Color.green;
        _weightImage.color = Color.white;
        
    }

    private void OnVerticeSelected()
    {
        _verticeImage.color = Color.green;
        _moveVerticeImage.color = Color.white;
        _edgeImage.color = Color.white;
        _weightImage.color = Color.white;
    }

    private void OnMoveVerticeSelected()
    {
        _verticeImage.color = Color.white;
        _moveVerticeImage.color = Color.green;
        _edgeImage.color = Color.white;
        _weightImage.color = Color.white;
    }

    private void OnWeightSelected()
    {
        _verticeImage.color = Color.white;
        _moveVerticeImage.color = Color.white;
        _edgeImage.color = Color.white;
        _weightImage.color = Color.green;
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

    #endregion
}
