using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class direcaoScript : MonoBehaviour
{
    private StartAnimationHandler _startAnimationHandler;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    private void Awake() {
        _startAnimationHandler = new StartAnimationHandler(_transform, Vector2.left, LevelType.PedidosEscritos | LevelType.PedidosRepresentados);
       // _startAnimationHandler.MoveToStartCanvas();
    }

    private void OnEnable()
    {
        //CallBackManeger.Instance.onStartLevelAnimation += _startAnimationHandler.MoveToCenterCanvas;
    }

    private void OnDisable()
    {
        //CallBackManeger.Instance.onStartLevelAnimation -= _startAnimationHandler.MoveToCenterCanvas;
    }

    void Start()
    {

    }

    void Update()
    {

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
}
