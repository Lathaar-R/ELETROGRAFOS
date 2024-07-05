using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PedidosScript : MonoBehaviour
{
    #region Fields and Properties
    private StartAnimationHandler _startAnimationHandlerVer;
    private StartAnimationHandler _startAnimationHandlerChe;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private BoardScript _board;
    [SerializeField] private Pedido _curPedido;
    [SerializeField] private Image correctImage;
    [SerializeField] private RectTransform _transformChegada;
    [SerializeField] private Vector3 _startPosChegada;
    [SerializeField] private Vector3 _endPosChegada;
    [SerializeField] private RectTransform _transformVerificacao;
    [SerializeField] private Vector3 _startPosVerificacao;
    [SerializeField] private Vector3 _endPosVerificacao;

    public Pedido CurPedido
    {
        get { return _curPedido; }
        set { _curPedido = value; }
    }

    #endregion

    private void Awake() {
        _startAnimationHandlerChe = new StartAnimationHandler(_transformChegada, Vector2.up, LevelType.PedidosEscritos | LevelType.PedidosRepresentados);
        //_startAnimationHandlerChe.MoveToStartCanvas();
        _startAnimationHandlerVer = new StartAnimationHandler(_transformVerificacao, Vector2.down, LevelType.PedidosEscritos | LevelType.PedidosRepresentados);
        //_startAnimationHandlerVer.MoveToStartCanvas();
    }

    void OnEnable()
    {
        CallBackManeger.Instance.verificarGrafo += VerificarGrafo;
        CallBackManeger.Instance.onStartLevelAnimation += _startAnimationHandlerVer.MoveToCenterCanvas;
        CallBackManeger.Instance.onStartLevelAnimation += _startAnimationHandlerChe.MoveToCenterCanvas;
        CallBackManeger.Instance.onStartLevel += OnStartLevel;
    }

    void OnDisable()
    {
        CallBackManeger.Instance.verificarGrafo -= VerificarGrafo;
        CallBackManeger.Instance.onStartLevelAnimation -= _startAnimationHandlerVer.MoveToCenterCanvas;
        CallBackManeger.Instance.onStartLevelAnimation -= _startAnimationHandlerChe.MoveToCenterCanvas;
        CallBackManeger.Instance.onStartLevel -= OnStartLevel;
    }

    void Update()
    {

    }

    private void OnStartLevel()
    {
        _text.text = GameManagerScript.Instance.CurrentPedido.descricao;
    }



    private void VerificarGrafo(bool result)
    {
        // bool result = true;

        // result = result && TipoDoGrafo(_curPedido);
        // //Debug.Log(result);

        // result = result && (_curPedido.vNum == _board.Vertices.Count);
        // //Debug.Log(result);

        // result = result && (_curPedido.eNum == _board.Edges.Count);
        // //Debug.Log(result);

        // foreach (var d in _curPedido.vDegrees)
        // {
        //     result = result && _board.Vertices.Any(v => VerificarGrau(v, d));
        //     //Debug.Log(result);
        // }

        // foreach (var d in _curPedido.vDegreesIn)
        // {
        //     result = result && _board.Vertices.Any(v => VerificarGrau(v, d, true));
        //     //Debug.Log(result);
        // }

        // foreach (var d in _curPedido.vDegreesOut)
        // {
        //     result = result && _board.Vertices.Any(v => VerificarGrau(v, d, false));
        //     //Debug.Log(result);
        // }

        // result = result && (_curPedido.isDirected == _board.Graph.IsDirected);
        // //Debug.Log(result);

        // result = _curPedido.isRegular ? VerificarGrafoRegular() : result;
        // //Debug.Log(result);

        // result = _curPedido.isComplete ? VerificarGrafoCompleto() : result;
        // //Debug.Log(result);

        // result = _curPedido.isBipartite ? VerificarBiPartido() : result;
        // //Debug.Log(result);

        // result = _curPedido.compConex.Length > 0 ? VerificarCompConex() : result;
        // //Debug.Log(result);

        // if (result)
        // {
        //     correctImage.color = Color.green;
        //     CallBackManeger.Instance.grafoCorreto?.Invoke();
        // }
        // else
        // {
        //     correctImage.color = Color.red;
        //     CallBackManeger.Instance.grafoIncorreto?.Invoke();
        // }
    }

    

    private void OnStartLevelAnimation()
    {
        //_transform = gameObject.GetComponent<RectTransform>();
        _transformChegada.anchoredPosition = _startPosChegada;
        _transformVerificacao.anchoredPosition = _startPosVerificacao;

        Invoke(nameof(Move), 0.5f);

    }

    private void Move()
    {
        _transformChegada.DOAnchorPos(_endPosChegada, 0.5f + UnityEngine.Random.value * 1f).SetEase(Ease.OutCubic);
        _transformVerificacao.DOAnchorPos(_endPosVerificacao, 0.5f + UnityEngine.Random.value * 1f).SetEase(Ease.OutCubic);
    }
}

public enum Tipo
{
    Simples,
    Multi,
    Pseudo,
    Dirigido,
    MultiDirigido,
    PseudoDirigido
}
