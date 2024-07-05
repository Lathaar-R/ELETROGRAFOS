using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEditor.SceneManagement;
using System;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private SaveFile saveFile;
    [SerializeField] private Pedido[] pedidos;
    [SerializeField] private GameObject levelSelectButtonPrefab;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup menuCanvasGroup;
    private RectTransform levelsPanelTransform;

    private void Awake()
    {
        foreach (var pedido in pedidos)
        {
            var button = Instantiate(levelSelectButtonPrefab);
            var buttonScript = button.GetComponent<Button>();
            var text = button.GetComponentInChildren<TMP_Text>();
            var level = int.Parse(pedido.name[^3].ToString());
            var stage = int.Parse(pedido.name[^1].ToString());
            text.text = stage.ToString();
            buttonScript.onClick.AddListener(() => OnJogar(stage));
            button.transform.SetParent(GameObject.Find("Level " + level).transform, false);
            button.name = stage.ToString() + " " + level.ToString();

        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        levelsPanelTransform = levelsPanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowLevels();
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            HideLevels();
        }

    }

    public void JogarMenu()
    {
        //        Debug.Log("Jogar");
        ShowLevels();
        HideMenu();
    }

    public void HideMenu()
    {
        menuCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            menuCanvasGroup.interactable = false;
            menuCanvasGroup.blocksRaycasts = false;
        });

        //add a movement tween to the menu
        menuCanvasGroup.transform.DOLocalMoveY(-200, 0.5f).SetEase(Ease.OutCubic);

    }

    public void ShowMenu()
    {
        menuCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            menuCanvasGroup.interactable = true;
            menuCanvasGroup.blocksRaycasts = true;
        });


    }

    public void OnJogar(int level)
    {
        //GameManagerScript.Instance.ChangeToLevel(pedidos[level]);
        HideLevels();
        GameManagerScript.Instance.StartLevel(pedidos[level - 1]);
    }

    public void OnSair()
    {
        Application.Quit();
    }

    public void OnOpcoes()
    {
        Debug.Log("Opcoes");
    }

    public void ShowLevels()
    {
        //Update the unlocked levels
        for (int i = 0; i < pedidos.Length; i++)
        {
            int level = int.Parse(pedidos[i].name[^3].ToString());
            int stage = int.Parse(pedidos[i].name[^1].ToString());

            var button = GameObject.Find("Level " + level).transform.GetChild(stage - 1).GetComponent<Button>();
            button.interactable = saveFile.level > level || (saveFile.level == level && saveFile.stage >= stage);
            Debug.Log("Level " + level + " Stage " + stage + " Interactable " + button.interactable);
        }

        canvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });

        levelsPanelTransform.anchoredPosition = new Vector2(-200, 0);
        levelsPanelTransform.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
    }

    public void HideLevels()
    {
        canvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });

        levelsPanelTransform.DOAnchorPos(new Vector2(-200, 0), 0.5f).SetEase(Ease.OutCubic);
    }
}
