using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManeger : MonoBehaviour
{
    // Singleton
    public static InputManeger Instance { get; private set; }

    private GameInputs _gameInputs;
    public GameInputs GameInputs => _gameInputs;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _gameInputs = new GameInputs();
            _gameInputs.Enable();

        }
        else
            Destroy(gameObject);
    }


}
