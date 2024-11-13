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
    private string currentlyTypedText;

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
        if (isTextDisplayed)
        {
			if (sentences.Count == 0 && !isTypingCoroutineOn)
                HideDialogueBox();
            else
            {
                if (isTypingCoroutineOn)
                {
                    StopCoroutine(currentTextCoroutine);
                    DialogueText.text = currentlyTypedText;
                    StoppedTyping();
                }
                else
                {
					currentTextCoroutine = StartCoroutine(TypeSentence());
                }
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
            currentTextCoroutine = StartCoroutine(TypeSentence());

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

    void StoppedTyping()
    {
        currentlyTypedText = null;
        isTypingCoroutineOn = false;
    }
    IEnumerator TypeSentence()
    {
        isTypingCoroutineOn = true;
		currentlyTypedText = sentences.Dequeue();

		DialogueText.text = "";
        foreach(char letter in currentlyTypedText.ToCharArray())
        {
            DialogueText.text += letter;
            for(int i = 0; i < dialogueTypingSpeed; i++)
            {
                yield return null;
            }
        }


        StoppedTyping();
    }
}
