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
    [SerializeField] private AudioSource typingSound; // Add this line for typing sound
    [Range(1, 10)][SerializeField] private int typingSpeed;

    private Queue<string> sentences = new Queue<string>();
    private string currentSentence;
    private bool endConversation;
    private bool isTyping;
    private bool waitingForPlayerInput = false;
    private const string HTML_Alpha = "<color=#00000000>"; // Used for typing effect
    private const float Max_Type_Time = 0.1f; // Controls typing speed
    private Coroutine typeDialogueCoroutine;
    public NPC currentNPC;

    public void StartDialogue(Dialogue dialogue)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        // Ensure previous choices are hidden and cleared
        Debug.Log("Hiding choices before starting new dialogue.");
        choiceManager.HideChoices();

        NPCname.text = dialogue.NPCName;
        sentences.Clear();

        // Add new sentences to the queue
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // Display the first sentence
        DisplayNextSentence(dialogue, currentNPC);
    }

    public void DisplayNextSentence(Dialogue dialogue, NPC npc)
    {
        currentNPC = npc;

        if (sentences.Count == 0)
        {
            if (!endConversation)
            {
                EndDialogue();
            }
            return;
        }

        if (!isTyping && sentences.Count > 0)
        {
            currentSentence = sentences.Dequeue();
            Debug.Log("Displaying sentence: " + currentSentence);
            typeDialogueCoroutine = StartCoroutine(TypeSentence(currentSentence));
        }
        else if (isTyping)
        {
            FinishParagraphEarly();
        }

        if (sentences.Count == 0 && dialogue.choices.Count == 0)
        {
            endConversation = true;
            choiceManager.HideChoices();
            waitingForPlayerInput = true;
        }
        else if (sentences.Count == 0 && dialogue.choices.Count > 0)
        {
            Debug.Log("Showing choices for new dialogue.");
            choiceManager.ShowChoices(dialogue.choices);
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

        // Resume the game (if needed)
        // Time.timeScale = 1;

        if (!choiceManager.choicesParent.gameObject.activeSelf) // Ensure choices are not displayed
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }

            if (currentNPC != null && currentNPC.isPartyMemeber && currentNPC.questForPlayer != null)
            {
                currentNPC.gameObject.SetActive(false); // Turn off the NPC after dialogue ends if not a party member and no quest for player
            }
        }

        // Hide choices and choice preview
        choiceManager.HideChoices();
        if (currentNPC != null)
        {
            currentNPC.HideInteractElements(); // Call method to hide interact elements
        }
    }

    public void ExecuteAction(ActionType actionType, DialogueChoice choice)
    {
        switch (actionType)
        {
            case ActionType.UnlockQuest:
                currentNPC.GiveQuest();
                break;
            case ActionType.ChangeNPCState:
                ChangeNPCState(choice);
                break;
            case ActionType.UpdateInventory:
                // Existing logic...
                break;
            case ActionType.None:
                break;
        }
    }

    private void UnlockQuest(string questId)
    {
        GameState.Instance.UnlockQuest(questId);
        //Debug.Log($"Quest {questId} unlocked!");
    }
    private void ChangeNPCState(DialogueChoice choice)
    {
        Debug.Log("Changing NPC state.");
        currentNPC.dialogue = choice.nextDialogue;
        Debug.Log("Starting new dialogue.");
        StartDialogue(choice.nextDialogue);
        choiceManager.HideChoices();
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

        // Reset AudioSource settings
        typingSound.volume = 1.0f;
        typingSound.spatialBlend = 0.0f;
        typingSound.pitch = 1.0f;

        foreach (char c in originalText)
        {
            alphaIndex++;
            NPCdialogue.text = originalText; // Keep the full text visible
            string displayedText = NPCdialogue.text.Insert(alphaIndex, HTML_Alpha);
            NPCdialogue.text = displayedText; // Apply typing effect

            // Play typing sound for each letter
            if (typingSound != null)
            {
                typingSound.PlayOneShot(typingSound.clip, 0.1f); // Adjust the volume as needed
            }

            // Add a slight delay to avoid overlap and weirdness
            yield return new WaitForSeconds((Max_Type_Time / typingSpeed) + 0.01f); // Slight delay added here
        }

        isTyping = false;

        // Ensure typing sound stops when done
        if (typingSound != null)
        {
            typingSound.Stop();
        }
    }
    private void FinishParagraphEarly()
    {
        StopCoroutine(typeDialogueCoroutine);
        NPCdialogue.text = currentSentence; // Show full sentence
        isTyping = false;
        typingSound.Stop(); // Stop typing sound
    }
}