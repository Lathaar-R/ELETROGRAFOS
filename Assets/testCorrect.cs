using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class testCorrect : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    // Start is called before the first frame update
    void OnEnable()
    {
        CallBackManeger.Instance.grafoCorreto += GrafoCorreto;
        CallBackManeger.Instance.grafoIncorreto += GrafoIncorreto;
    }

    void OnDisable()
    {
        CallBackManeger.Instance.grafoCorreto -= GrafoCorreto;
        CallBackManeger.Instance.grafoIncorreto -= GrafoIncorreto;
    }

    void GrafoCorreto()
    {
        Color green = new Color(0, 1, 0, 0.2f);
        Color transparent = new Color(0, 0, 0, 0);
        _sprite.DOColor(green, 0.3f).OnComplete(() => _sprite.DOColor(transparent, 0.3f));

        Debug.Log("Grafo Correto");
    }

    void GrafoIncorreto()
    {
        Color red = new Color(1, 0, 0, 0.2f);
        Color transparent = new Color(0, 0, 0, 0);
        _sprite.DOColor(red, 0.3f).OnComplete(() => _sprite.DOColor(transparent, 0.3f));

        Debug.Log("Grafo Incorreto");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
