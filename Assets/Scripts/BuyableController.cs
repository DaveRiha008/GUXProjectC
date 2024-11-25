using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuyableController : MonoBehaviour
{

    public string title;
    public string[] itemDescription;

    private string[] itemBoughtDialogue;
    private string[] notEnoughMoneyDialogue;

    private string[] skinOwnedEDialogue;
	private string[] skinOwnedOnBuyDialogue;
	private string[] confirmationDialogue;

	public int price;
    
    private SpriteRenderer keyboardPromptE;
    private SpriteRenderer keyboardPromptF;
    private bool isTriggered;
    private bool wasBought = false;
    private bool isSkinBought = false;
    private bool isConfirmationShown = false;
    
    private DialogueManager dialogueManager;

    private PlayerController player;

    public UnityEvent ExecuteOnBuy;

    // Start is called before the first frame update
    void Start()
    {
        keyboardPromptE = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        keyboardPromptE.enabled = false;
        keyboardPromptF = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        keyboardPromptF.enabled = false;
        isTriggered = false;
        isConfirmationShown = false;


	    dialogueManager = FindObjectOfType<DialogueManager>();
        player = FindObjectOfType<PlayerController>();

        itemBoughtDialogue = new string[] {$"{title} bought. Thank you for your purchase!"};
        notEnoughMoneyDialogue = new string[] {"Sorry, you don't have enough money for this purchase"};
        skinOwnedEDialogue = new string[] {"Change skin for 0 Coins"};
		skinOwnedOnBuyDialogue = new string[] { "Skin equipped!" };
		confirmationDialogue = new string[] { $"Are you sure you want to buy {title}?" };

	}

	// Update is called once per frame
	void Update()
    {
        if(!wasBought)
            HandleInput();
    }

    void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.E) && isTriggered)
        {
            isConfirmationShown = false;
            if(isSkinBought)
                dialogueManager.DisplayNextLine(title, skinOwnedEDialogue);
            else
                dialogueManager.DisplayNextLine(title, itemDescription);
        }

        if(Input.GetKeyDown(KeyCode.F) && isTriggered)
        {
            if (!isConfirmationShown)
            {
                dialogueManager.DisplayNextLine(title, confirmationDialogue);
                isConfirmationShown = true;
                return;
            }
            if (player.coinsInInventory < price)
            {
                dialogueManager.StartNewDialogueOnTopOfPrevious(title, notEnoughMoneyDialogue);
            }
            else
            {
                if (!(wasBought || isSkinBought))
                    GameLoggingScript.WriteLineToLog($"Player bought {title}", GameLoggingScript.outputFile);

                player.RemoveCoinsFromInventory(price);
                ExecuteOnBuy.Invoke();

				if (!isSkinBought)
                    dialogueManager.StartNewDialogueOnTopOfPrevious(title, itemBoughtDialogue);
                else
                    dialogueManager.StartNewDialogueOnTopOfPrevious(title, skinOwnedOnBuyDialogue);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!wasBought)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                keyboardPromptE.enabled = true;
                keyboardPromptF.enabled = true;
                isTriggered = true;
				isConfirmationShown = false;

			}
		}
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            keyboardPromptE.enabled = false;
            keyboardPromptF.enabled = false;
            isTriggered = false;
            isConfirmationShown = false;

            dialogueManager.HideDialogueBox();
        }
    }

    // add to "ExecuteOnBuy" event of shop items that can't be repurchased/ interacted with again
    public void HideShopItem()
    {
        keyboardPromptE.enabled = false;
        keyboardPromptF.enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        wasBought = true;
    }

    // reduce price to zero - for items in shop that can be interacted with again (e.g. skins)
    public void ReducePriceToZero()
    {
        price = 0;
        isSkinBought = true;
    }
}
