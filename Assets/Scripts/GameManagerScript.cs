using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }
    [SerializeField] private int level;
    [SerializeField] private LevelType levelType;
    [SerializeField] private bool animateStart;
    [SerializeField] private bool notMenuStart;
    [SerializeField] private Pedido currentPedido;
    [SerializeField] private GameObject continousCanvas;
    //[SerializeField] private GameObject changeLevelAnimation;
    [SerializeField] private Image fadeOutImage;


    public int Level => level;
    public LevelType LevelType => levelType;

    public Pedido CurrentPedido => currentPedido;

    private void Awake()
    {
        if (Instance == null)
        {
            //SceneManager.sceneLoaded += OnSceneLoaded;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(continousCanvas);
        }
        else
        {
            // var c = GameObject.Find("Continuos Canvas");
            // Destroy(c);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (notMenuStart)
        {
            StartLevel(currentPedido);
        }
        else
        {
            StartMenu();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {

        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeToLevel(Pedido pedido)
    {
        currentPedido = pedido;

        if (!fadeOutImage.gameObject.activeSelf)
        {
            fadeOutImage.gameObject.SetActive(true);
            fadeOutImage.DOFade(1, 1f).OnComplete(() => ChangeToLevel(pedido));
        }
        else
        {
            StartCoroutine(LoadLevel());
        }
    }

    private IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(0.25f);
        //Asyncrhonous loading
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level");
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {

            if (asyncLoad.progress >= 0.98f)
            {
                yield return new WaitForSeconds(0.25f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void StartLevel(Pedido pedido)
    {
        currentPedido = pedido;
        levelType = currentPedido.levelType;
        CallBackManeger.Instance.onStartLevel?.Invoke();
        //fadeOutImage.gameObject.SetActive(false);

        if (animateStart)
        {
            StartAnimation();
        }
    }

    void StartMenu()
    {
        CallBackManeger.Instance.onStartMenu?.Invoke();
    }

    public void OnVerificar(bool result)
    {
        if (result)
        {
            StartCoroutine(CorrectGraph());
        }
        else
        {
            StartCoroutine(IncorrectGraph());
        }
    }

    private IEnumerator CorrectGraph()
    {
        CallBackManeger.Instance.grafoCorreto?.Invoke();
        yield return new WaitForSeconds(1f);
        CallBackManeger.Instance.onEndLevelAnimation?.Invoke();
    }

    private IEnumerator IncorrectGraph()
    {
        CallBackManeger.Instance.grafoIncorreto?.Invoke();
        yield return new WaitForSeconds(1f);
        //CallBackManeger.Instance.onEndLevelAnimation?.Invoke();
    }

    #region Callbacks

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        fadeOutImage = GameObject.Find("FadeOutImage").GetComponent<Image>();

        if (scene.name.Contains("Level"))
        {
            fadeOutImage.DOFade(0, 1.5f).OnComplete(() => StartLevel(currentPedido));
        }
        else
        {
            fadeOutImage.DOFade(0, 1.5f).OnComplete(StartMenu);
        }
    }

    void StartAnimation()
    {
        CallBackManeger.Instance.onStartLevelAnimation?.Invoke();
    }

    #endregion


}


public enum LevelType
{
    PedidosEscritos = 1,
    PedidosRepresentados = 2,
    Validacao = 4,
    All = PedidosEscritos | PedidosRepresentados | Validacao
}
