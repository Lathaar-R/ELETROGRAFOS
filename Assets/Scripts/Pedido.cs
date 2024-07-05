using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pedido", menuName = "Pedido")]
public class Pedido : ScriptableObject
{
    public string descricao;
    public Tipo tipo;
    public LevelType levelType;
    public int vNum;
    public int eNum;
    public int[] vDegrees;
    public int[] vDegreesOut;
    public int[] vDegreesIn;
    public bool isDirected;
    public bool isRegular;
    public int[] compConex;
    public bool isBipartite;
    public bool isComplete;
}
