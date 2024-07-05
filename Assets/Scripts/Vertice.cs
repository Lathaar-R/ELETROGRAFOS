using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vertice : MonoBehaviour, IGraphElement
{
    [SerializeField] private int _id = -1;
    [SerializeField] private int _boradId = -1;
    [SerializeField] private int degree = -1;
    [SerializeField] private int outDegree = 0;
    [SerializeField] private int inDegree = 0;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private SpriteRenderer _selectedSprite;
    [SerializeField] private SpriteRenderer sevenGegRenderer;
    [SerializeField] private bool[] directions = new bool[8];
    [SerializeField] private Sprite[] sevenSeg;

    public int Id
    {
        get { return _id; }
        set
        {
            _id = value;
            idText.text = value.ToString();
        }
    }

    public int BoardId
    {
        get { return _boradId; }
        set { _boradId = value; }
    }

    public int Degree
    {
        get { return degree; }
        set { degree = value; }
    }

    public int OutDegree
    {
        get { return outDegree; }
        set { outDegree = value; }
    }

    public int InDegree
    {
        get { return inDegree; }
        set { inDegree = value; }
    }

    public bool[] Directions
    {
        get { return directions; }
        set { directions = value; }
    }

    private void Awake() {
        CallBackManeger.Instance.onUpdateGraph += ChangeNumber;
    }

    private void OnDisable() {
        CallBackManeger.Instance.onUpdateGraph -= ChangeNumber;
    }

    private void ChangeNumber()
    {
        sevenGegRenderer.sprite = sevenSeg[Id];
    }

    public void Select()
    {
        _selectedSprite.color = Color.green;
    }

    public void Deselect()
    {
        _selectedSprite.color = Color.white;
    }


}
