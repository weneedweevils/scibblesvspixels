using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //Singleton instance
    public static DialogueManager Instance { get; private set; }

    public Transform dialogueParentContainer;
    public DialogueSequence dialogue;
    public int dialogueID = 0;
    public bool dialogueActive = false;

    private DialogueBox currentDialogue = null;
    private DialogueEntry currentEntry;
    private List<GameObject> objectsToActivate;

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

        objectsToActivate = new List<GameObject>();
        StartDialogue(dialogue);
    }

    public void StartDialogue(DialogueSequence dialogueSequence)
    {
        dialogue = dialogueSequence;
        dialogueActive = true;
        dialogueID = 0;
        NextDialogue();
    }

    public void AddObjectToActivate(GameObject objectToActivate)
    {
        objectsToActivate.Add(objectToActivate);
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
            //Activate objects
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }

            //Clear Variables
            currentDialogue = null;
            dialogueActive = false;
            objectsToActivate.Clear();
        }
    }
}
