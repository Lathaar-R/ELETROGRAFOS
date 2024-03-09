using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vertice : MonoBehaviour, IGraphElement
{
    [SerializeField] private int _id = -1;
    [SerializeField] private int _boradId = -1;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private SpriteRenderer _selectedSprite;

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

    public void Select()
    {
        _selectedSprite.color = Color.green;
    }

    public void Deselect()
    {
        _selectedSprite.color = Color.white;
    }
}
