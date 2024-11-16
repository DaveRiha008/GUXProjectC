using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{
    public INTERACTABLE_TYPE myType = INTERACTABLE_TYPE.SIGN;

    public string title;
    public string[] dialogue;
    
    private SpriteRenderer keyboardPrompt;
    private bool isTriggered;

    private bool firstInteract = true;
    
    private DialogueManager dialogueManager;

    public bool isSaveable;
    public bool isSaved;

    public UnityEvent npcEvent;
    public UnityEvent interactedWith;

    // Start is called before the first frame update
    void Start()
    {
        keyboardPrompt = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        keyboardPrompt.enabled = false;
        isTriggered = false;

        dialogueManager = FindObjectOfType<DialogueManager>();
        interactedWith.AddListener(delegate { GameLoggingScript.InteractedWithInteractable(myType); });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isTriggered)
        {
            firstInteract = false;
            interactedWith.Invoke();
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

public enum INTERACTABLE_TYPE { COW, GEORGE, SIGN, NPC, SHOPKEEPER, SAVEABLE_NPC }