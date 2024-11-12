using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{

    public string title;
    public string[] dialogue;
    
    private SpriteRenderer keyboardPrompt;
    private bool isTriggered;
    
    private DialogueManager dialogueManager;

    public bool isSaveable;
    public bool isSaved;

    public UnityEvent npcEvent;

    // Start is called before the first frame update
    void Start()
    {
        keyboardPrompt = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        keyboardPrompt.enabled = false;
        isTriggered = false;

        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isTriggered)
        {
            dialogueManager.DisplayNextLine(title, dialogue);

            if(isSaveable)
                isSaved = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            keyboardPrompt.enabled = true;
            isTriggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            keyboardPrompt.enabled = false;
            isTriggered = false;

            dialogueManager.HideDialogueBox();
        }
    }
}
