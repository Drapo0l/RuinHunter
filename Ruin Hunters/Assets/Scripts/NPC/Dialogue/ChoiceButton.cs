using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI choiceText; // Reference to the text component
    private GameObject choicePreviewWindow;
    private TextMeshProUGUI choicePreviewText;
    public string playerText; 

    public void Setup(string buttonText, string playerText, System.Action onClickAction, GameObject previewWindow, TextMeshProUGUI previewText)
    {
        if (choiceText == null) // Ensure the choiceText is assigned
        {
            choiceText = GetComponentInChildren<TextMeshProUGUI>();
        }

        choiceText.text = buttonText; // Set the button text
        GetComponent<Button>().onClick.AddListener(() => onClickAction()); // Add the click listener

        choicePreviewWindow = previewWindow;
        choicePreviewText = previewText;
        this.playerText = playerText; // Assign playerText
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowPreview();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HidePreview();
    }

    private void ShowPreview()
    {
        choicePreviewText.text = playerText;
        choicePreviewWindow.SetActive(true);
    }

    private void HidePreview()
    {
        choicePreviewWindow.SetActive(false);
    }
}