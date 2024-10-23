using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static PublicEnums;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private GameObject choiceButtonPrefab; // Prefab for the choice button
    [SerializeField] public  Transform choicesParent; // Parent transform for choice buttons
    [SerializeField] private ScrollRect choicesScrollRect; // Reference to ScrollRect (optional)
    [SerializeField] private GameObject choicePreviewWindow; // The secondary window for choice previews
    [SerializeField] private TextMeshProUGUI choicePreviewText; // Text component for the choice preview
    [SerializeField] private GameObject hoverIndicator; // Visual indicator for current selection
    [SerializeField] private ChoiceManager choiceManager;
    private List<Button> choiceButtons = new List<Button>();
    private int currentSelection = 0;

    public void ShowChoices(List<DialogueChoice> choices)
    {
        Debug.Log("Clearing existing choices.");
        foreach (Transform child in choicesParent)
        {
            Destroy(child.gameObject);
        }
        choiceButtons.Clear();

        choicesParent.gameObject.SetActive(true);

        foreach (var choice in choices)
        {
            ChoiceButton choiceButtonComponent = Instantiate(choiceButtonPrefab, choicesParent).GetComponent<ChoiceButton>();
            choiceButtonComponent.Setup(choice.buttonText, choice.playerText, () => OnChoiceSelected(choice), choicePreviewWindow, choicePreviewText);
            choiceButtons.Add(choiceButtonComponent.GetComponent<Button>());
        }

        if (choicesScrollRect != null)
        {
            choicesScrollRect.verticalNormalizedPosition = 1;
        }
        UpdateHoverIndicator();
        Debug.Log("Choices displayed: " + choices.Count);
    }

    private void UpdateHoverIndicator()
    {
        if (choiceButtons.Count > 0)
        {
            Vector3 buttonPosition = choiceButtons[currentSelection].transform.position;
            hoverIndicator.transform.position = new Vector3(buttonPosition.x, buttonPosition.y, buttonPosition.z); // Removed fixed offset

            // Optionally change button color or add visual effect to highlight selected button
            for (int i = 0; i < choiceButtons.Count; i++)
            {
                ColorBlock colors = choiceButtons[i].colors;
                if (i == currentSelection)
                {
                    colors.normalColor = Color.yellow; // Highlight color
                }
                else
                {
                    colors.normalColor = Color.white; // Default color
                }
                choiceButtons[i].colors = colors;
            }

            // Update the choice preview
            ShowPreview(choiceButtons[currentSelection]);
        }
    }

    public void Navigate(int direction)
    {
        currentSelection += direction;
        if (currentSelection < 0) currentSelection = choiceButtons.Count - 1;
        if (currentSelection >= choiceButtons.Count) currentSelection = 0;
        UpdateHoverIndicator();
    }

    public void OnChoiceSelected(DialogueChoice choice)
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.ExecuteAction(choice.actionType, choice);
        HideChoices();
        choicePreviewWindow.SetActive(false);
    }



    public void HideChoices()
    {
        Debug.Log("Hiding choices.");
        foreach (Transform child in choicesParent)
        {
            Destroy(child.gameObject);
        }
        choiceButtons.Clear();
        choicesParent.gameObject.SetActive(false);
    }


        void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Navigate(-1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Navigate(1);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            choiceButtons[currentSelection].onClick.Invoke();
        }
    }

    private void ShowPreview(Button button)
    {
        ChoiceButton choiceButton = button.GetComponent<ChoiceButton>();
        if (choiceButton != null)
        {
            choicePreviewText.text = choiceButton.playerText; // Show the playerText
            choicePreviewWindow.SetActive(true);
        }
    }
}