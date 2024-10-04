using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI NPCname;
   [SerializeField] private TextMeshProUGUI NPCdialogue;
 [Range(1,10)]  [SerializeField] private int typingSpeed;

    private Queue<string> sentences = new Queue<string>();// FIFO 
    private string s;
    private bool endConversation;
    private bool isTyping;

    private Coroutine typeDialogueCoroutine;

    private const string HTML_Alpha = "<color=#00000000>";
    private const float Max_Type_Time = 0.1f;
    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //activate gameObject
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        //update the name
        NPCname.text = dialogue.NPCName;

        //add dialogue text to queue
        for (int i = 0; i < dialogue.sentences.Length; i++)
        {
            sentences.Enqueue(dialogue.sentences[i]);
        }

    }

    public void DisplayNextSentence(Dialogue dialogue)
    {


        if (sentences.Count == 0)//if there is nothing in the queue
        {
            if (!endConversation)
            {
                StartDialogue(dialogue);
            }
            else if (endConversation && !isTyping)
            {
                EndDialogue();
                return;
            }
        }
        //if there is somenthing in the queue
        if (!isTyping)
        {
            s = sentences.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeSentence(s));
        }
        //dialogue Is being typed out
        else
        {
            FinishParagraphEarly();
        }
     

        //update conversation bool
        if (sentences.Count == 0)
        {
            endConversation = true;
           
        }
    }

    public void EndDialogue()
    {
      //clear queue
      sentences.Clear();

        //return bool to false
        endConversation = false;

        //deactivate gameObject
        if (gameObject.activeSelf)
        {
            gameObject.SetActive (false);
        }
    }

    IEnumerator TypeSentence(string s)//display all text invisable and then go one by one
    {
        isTyping = true;

        NPCdialogue.text = "";

        string originalText = s;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in s.ToCharArray())
        {
            alphaIndex++;
            NPCdialogue.text = originalText;

            displayedText = NPCdialogue.text.Insert(alphaIndex, HTML_Alpha);
            NPCdialogue.text = displayedText;

            yield return new WaitForSeconds(Max_Type_Time / typingSpeed);
        }
        isTyping = false;   
    }

    private void FinishParagraphEarly()
    {
        //stop the coroutine
        StopCoroutine(typeDialogueCoroutine);

        //finish display text
        NPCdialogue.text = s;

        //upadate isTyping bool
        isTyping = false;
    }
  
}

