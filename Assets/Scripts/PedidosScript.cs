using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PedidosScript : MonoBehaviour
{
    #region Fields and Properties
    [SerializeField] private TMP_Text _text;

    #endregion
    void Start()
    {
        Tipo [] tipos = {Tipo.Simples, Tipo.Multi, Tipo.Pseudo, Tipo.Dirigido, Tipo.MultiDirigido, Tipo.PseudoDirigido};
        int i = Random.Range(0, tipos.Length);
        _text.text = "Um grafo ";
        i = 0;

        switch (tipos[i])
        {
            case Tipo.Simples:
                _text.text = "Simples";
                break;
            case Tipo.Multi:
                _text.text = "Multi";
                break;
            case Tipo.Pseudo:
                _text.text = "Pseudo";
                break;
            case Tipo.Dirigido:
                _text.text = "Dirigido";
                break;
            case Tipo.MultiDirigido:
                _text.text = "MultiDirigido";
                break;
            case Tipo.PseudoDirigido:
                _text.text = "PseudoDirigido";
                break;
        }

        int vNum = Random.Range(3, 6);
        _text.text += " com " + vNum + " vértices";

        int eNum = Random.Range(3, 6);
        _text.text += " e " + eNum + " arestas";

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
