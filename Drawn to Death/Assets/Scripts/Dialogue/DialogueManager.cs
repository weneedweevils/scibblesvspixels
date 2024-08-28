using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.InputSystem;

public enum DialogueSFX
{
    None,
    OodleGeneral,
    OodleQuestion,
    OodleConfused,
    OodleMad_NOTIMPLEMENTED,
    GlichGeneral,
    GlichQuestion_NOTIMPLEMENTED,
    GlichConfused,
    GlichMad
}

public class DialogueManager : MonoBehaviour
{
    //Singleton instance
    public static DialogueManager Instance { get; private set; }

    public Transform dialogueParentContainer;
    public int dialogueID = 0;
    public bool dialogueActive = false;
    public GameObject[] timelines;

    private DialogueSequence dialogue;
    private DialogueBox currentDialogue = null;
    private DialogueEntry currentEntry;

    public GameObject inputHandler;
    [HideInInspector]
    public PlayerInput playerInput;

    private Dictionary<DialogueSFX, string> sfx = new Dictionary<DialogueSFX, string>()
    {
        { DialogueSFX.None, null},

        //Oodler
        { DialogueSFX.OodleGeneral, "event:/OodleDialogueGeneral"},
        { DialogueSFX.OodleQuestion, "event:/OodlerDialogueQuestion"},
        { DialogueSFX.OodleConfused, "event:/OodlerDialogueConfused"},
        { DialogueSFX.OodleMad_NOTIMPLEMENTED, null},

        //Glich
        { DialogueSFX.GlichGeneral, "event:/GlichDialogueGeneral"},
        { DialogueSFX.GlichQuestion_NOTIMPLEMENTED, null},
        { DialogueSFX.GlichConfused, "event:/GlichDialogueConfused"},
        { DialogueSFX.GlichMad, "event:/GlichDialogueMad"}
    };

    private void Awake()
    {
        // Ensure only one instance of InputManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (dialogueParentContainer == null)
            Debug.LogError("Error in DialogueManager - dialogueParentContainer is null");

        playerInput = inputHandler.GetComponent<PlayerInput>();
        
    }

    public void StartDialogue(DialogueSequence dialogueSequence)
    {
        dialogue = dialogueSequence;
        dialogueActive = true;
        dialogueID = 0;
        NextDialogue();
    }

    public void PrevDialogue()
    {
        if (dialogueActive)
        {
            dialogueID = Mathf.Max(0, dialogueID - 2);
            NextDialogue();
        }
    }

    public void ResetDialogue()
    {
        if (dialogueActive)
        {
            dialogueID -= 1;
            NextDialogue();
        }
    }

    public void NextDialogue()
    {
        //Destroy the previous Dialogue Box
        if (currentDialogue != null)
        {
            Destroy(currentDialogue.gameObject);
        }

        //Instanciate the next Dialogue Box if it exists
        if (dialogueID < dialogue.dialogueEntries.Length)
        {
            //Next Dialogue Entry
            currentEntry = dialogue.dialogueEntries[dialogueID];
            ++dialogueID;
            
            //Create the Dialogue Box
            currentDialogue = Instantiate(currentEntry.dialogueStyle.dialogueBlueprint, dialogueParentContainer);
            currentDialogue.SetDialogueEntry(currentEntry);
        }
        //There is no next Dialogue Box -> End of Dialogue Sequence
        else
        {
            //Activate Timeline
            if (0 <= dialogue.activateTimeline && dialogue.activateTimeline < timelines.Length)
            {
                timelines[dialogue.activateTimeline].SetActive(true);
            }

            //Clear Variables
            currentDialogue = null;
            dialogueActive = false;
        }
    }

    public string DialogueSFXEventPath(DialogueSFX dialogueSFX)
    {
        return sfx[dialogueSFX];
    }

    public DialogueBox GetCurrentDialogue()
    {
        return currentDialogue;
    }

    public void SetCurrentDialogueNull()
    {
        currentDialogue = null;
    }
}
