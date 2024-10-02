using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : NPC
{
   [SerializeField] public Text nameText;
   [SerializeField] public Text dialogueText;

    private Queue<string> sentences;// FIFO 

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {

        nameText.text = dialogue.NPCName;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

       string sentence =  sentences.Dequeue();
        StopAllCoroutines();
       StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("End of Conversation");
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }
}
