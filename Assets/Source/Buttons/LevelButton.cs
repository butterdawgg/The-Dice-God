using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] int levelSceneID;
    [SerializeField] string defaultText = "button";
    [SerializeField] string highlightedText = "> button <";
    [SerializeField] int defaultFontSize = 100;
    [SerializeField] int highlightedFontSize = 120;
    [SerializeField] Color defaultTextColor = Color.gray;
    [SerializeField] Color highlightedTextColor = Color.white;

    private TextMeshProUGUI text;
    private Button button;
    private bool isMouseOver = false;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);

        if (levelSceneID == 1)
        {
            SerializeManager.Instance.SetLevelLockedState(levelSceneID, false);
        }
    }

    private void Update()
    {
        if (isMouseOver & !SerializeManager.Instance.GetLevelLockedState(levelSceneID))
        {
            text.text = highlightedText;
            text.fontSize = highlightedFontSize;
            text.color = highlightedTextColor;
        }
        else
        {
            text.text = defaultText;
            text.fontSize = defaultFontSize;
            text.color = defaultTextColor;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isMouseOver = true;

        if (!SerializeManager.Instance.GetLevelLockedState(levelSceneID))
            AudioManager.Instance.PlaySound("ButtonHover");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isMouseOver = false;
    }

    private void OnButtonClick()
    {
        if (!SerializeManager.Instance.GetLevelLockedState(levelSceneID))
        {
            isMouseOver = false;

            text.text = "level " + levelSceneID;
            text.fontSize = defaultFontSize;
            text.color = defaultTextColor;

            AudioManager.Instance.PlaySound("ButtonClick");

            FindObjectOfType<MenuManager>().ChangeScene(levelSceneID);
        }
    }
}