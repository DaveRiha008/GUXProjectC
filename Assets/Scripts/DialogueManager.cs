using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;

    private const int dialogueTypingSpeed = 3;

    public GameObject dialogueBox;
    private Text Header;
    private Text DialogueText;

    public bool isTextDisplayed;
    private Coroutine currentTextCoroutine;
    private bool isTypingCoroutineOn;

    void Start()
    {
        sentences = new Queue<string>();

        isTypingCoroutineOn = false;
        isTextDisplayed = false;
        dialogueBox.SetActive(false);
        Header = dialogueBox.GetComponent<RectTransform>().Find("Header").GetComponent<Text>();
        DialogueText = dialogueBox.GetComponent<RectTransform>().Find("DialogueText").GetComponent<Text>();
    }

    public void DisplayNextLine(string interactableName, string[] dialogueSentences)
    {
        if(isTextDisplayed)
        {
            if(sentences.Count == 0)
                HideDialogueBox();
            else
            {
                if(isTypingCoroutineOn)
                    StopCoroutine(currentTextCoroutine);
                currentTextCoroutine = StartCoroutine(TypeSentence(sentences.Dequeue()));
            }
        }
        else
        {
            Header.text = interactableName;

            sentences.Clear();

            foreach(string sentence in dialogueSentences)
            {
                sentences.Enqueue(sentence);
            }

            if(isTypingCoroutineOn)
                StopCoroutine(currentTextCoroutine);
            currentTextCoroutine = StartCoroutine(TypeSentence(sentences.Dequeue()));

            dialogueBox.SetActive(true);
            isTextDisplayed = true;
        }
    }

    public void HideDialogueBox()
    {
        isTextDisplayed = false;
        dialogueBox.SetActive(false);
    }

    public void StartNewDialogueOnTopOfPrevious(string interactableName, string[] dialogueSentences)
    {
        isTextDisplayed = false;
        DisplayNextLine(interactableName, dialogueSentences);
    }

    IEnumerator TypeSentence(string sentenceToType)
    {
        isTypingCoroutineOn = true;

        DialogueText.text = "";
        foreach(char letter in sentenceToType.ToCharArray())
        {
            DialogueText.text += letter;
            for(int i = 0; i < dialogueTypingSpeed; i++)
            {
                yield return null;
            }
        }

        isTypingCoroutineOn = false;
    }
}
