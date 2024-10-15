using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static PublicEnums;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCname;
    [SerializeField] private TextMeshProUGUI NPCdialogue;
    [SerializeField] private ChoiceManager choiceManager; // Reference to ChoiceManager
    [Range(1, 10)][SerializeField] private int typingSpeed;

    private Queue<string> sentences = new Queue<string>();
    private string currentSentence;
    private bool endConversation;
    private bool isTyping;
    private bool waitingForPlayerInput = false;

    private const string HTML_Alpha = "<color=#00000000>"; // Used for typing effect
    private const float Max_Type_Time = 0.1f; // Controls typing speed
    private Coroutine typeDialogueCoroutine;

    public void StartDialogue(Dialogue dialogue)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        NPCname.text = dialogue.NPCName;

        // Add dialogue text to queue
        sentences.Clear();
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(dialogue);
    }

    public void DisplayNextSentence(Dialogue dialogue)
    {
        // If there are no sentences left
        if (sentences.Count == 0)
        {
            if (!endConversation)
            {
                EndDialogue();
            }
            return; // Exit if there are no sentences
        }

        // If there are sentences, continue displaying
        if (!isTyping && sentences.Count > 0)
        {
            currentSentence = sentences.Dequeue(); // Get the next sentence
            typeDialogueCoroutine = StartCoroutine(TypeSentence(currentSentence)); // Type out the next sentence
        }
        else if (isTyping)
        {
            FinishParagraphEarly(); // Allow finishing the current sentence early if requested
        }

        // Update the end conversation state
        if (sentences.Count == 0)
        {
            endConversation = true;
            // Show choices if available
            if (dialogue.choices.Count > 0)
            {
                choiceManager.ShowChoices(dialogue.choices);
            }
            else
            {
                // Wait for player input to end the dialogue
                waitingForPlayerInput = true;
            }
        }
    }

    private void Update()
    {
        // Check for player input to end the dialogue if it's the last sentence
        if (waitingForPlayerInput && Input.GetKeyDown(KeyCode.E))
        {
            EndDialogue();
            waitingForPlayerInput = false;
        }
    }

    public void EndDialogue()
    {
        sentences.Clear();
        endConversation = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void ExecuteAction(ActionType actionType, DialogueChoice choice)
    {
        switch (actionType)
        {
            case ActionType.UnlockQuest:
              //  if (!string.IsNullOrEmpty(choice.questID))
                {
              //      UnlockQuest(choice.questID);
                }
                break;
            case ActionType.ChangeNPCState:
             //   if (choice.changesNPCState)
                {
                    ChangeNPCState();
                }
                break;
            case ActionType.UpdateInventory:
               // if (choice.rewardItem != null)
                {
              //      UpdateInventory(choice.rewardItem);
                }
                break;
            case ActionType.None:
                break;
        }
    }

    private void UnlockQuest(string questId)
    {
        GameState.Instance.UnlockQuest(questId);
        Debug.Log($"Quest {questId} unlocked!");
    }

    private void ChangeNPCState()
    {
        Debug.Log("NPC state changed!");
    }

    private void UpdateInventory(Item item)
    {
        GameState.Instance.UpdateInventory(item);
        Debug.Log($"{item.name} added to inventory!");
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        NPCdialogue.text = "";

        string originalText = sentence;
        int alphaIndex = 0;

        foreach (char c in originalText)
        {
            alphaIndex++;
            NPCdialogue.text = originalText; // Keep the full text visible
            string displayedText = NPCdialogue.text.Insert(alphaIndex, HTML_Alpha);
            NPCdialogue.text = displayedText; // Apply typing effect

            yield return new WaitForSeconds(Max_Type_Time / typingSpeed);
        }

        isTyping = false;
    }

    private void FinishParagraphEarly()
    {
        StopCoroutine(typeDialogueCoroutine);
        NPCdialogue.text = currentSentence; // Show full sentence
        isTyping = false;
    }
}