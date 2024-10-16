using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private GameObject choiceButtonPrefab; // Prefab for the choice button
    [SerializeField] private Transform choicesParent; // Parent transform for choice buttons
    [SerializeField] private ScrollRect choicesScrollRect; // Reference to ScrollRect (optional)
    [SerializeField] private GameObject choicePreviewWindow; // The secondary window for choice previews
    [SerializeField] private TextMeshProUGUI choicePreviewText; // Text component for the choice preview
    [SerializeField] private GameObject hoverIndicator; // Visual indicator for current selection

    private List<Button> choiceButtons = new List<Button>();
    private int currentSelection = 0;

    public void ShowChoices(List<DialogueChoice> choices)
    {
        // Activate the choices parent to make sure it's visible
        choicesParent.gameObject.SetActive(true);

        // Clear existing choices
        foreach (Transform child in choicesParent)
        {
            Destroy(child.gameObject);
        }
        choiceButtons.Clear();

        // Instantiate a button for each choice
        foreach (var choice in choices)
        {
            ChoiceButton choiceButtonComponent = Instantiate(choiceButtonPrefab, choicesParent).GetComponent<ChoiceButton>();
            choiceButtonComponent.Setup(choice.buttonText, choice.playerText, () => OnChoiceSelected(choice), choicePreviewWindow, choicePreviewText);
            choiceButtons.Add(choiceButtonComponent.GetComponent<Button>());
        }

        // Ensure the scroll view starts at the top
        if (choicesScrollRect != null)
        {
            choicesScrollRect.verticalNormalizedPosition = 1;
        }

        UpdateHoverIndicator();
    }

    private void UpdateHoverIndicator()
    {
        if (choiceButtons.Count > 0)
        {
            Vector3 buttonPosition = choiceButtons[currentSelection].transform.position;
            hoverIndicator.transform.position = new Vector3(buttonPosition.x - 150f, buttonPosition.y, buttonPosition.z);

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

    private void OnChoiceSelected(DialogueChoice choice)
    {
        // Call a method in DialogueManager to handle the choice
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.ExecuteAction(choice.actionType, choice);

        // Hide choice buttons and choice preview window
        HideChoices();
        choicePreviewWindow.SetActive(false);

        // Display the next dialogue (NPC's response)
        if (choice.nextDialogue != null)
        {
            dialogueManager.StartDialogue(choice.nextDialogue);
        }
        else
        {
            dialogueManager.DisplayNextSentence(new Dialogue() { sentences = new string[] { "." } }, null); // Display the NPC's response before ending
        }
    }

    public void HideChoices()
    {
        // Clear existing choices
        foreach (Transform child in choicesParent)
        {
            Destroy(child.gameObject);
        }
        choicesParent.gameObject.SetActive(false); // Hide the choices panel
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