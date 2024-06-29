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
        if (currentDialogue != null)
        {
            Destroy(currentDialogue.gameObject);
        }

        if (dialogueID < dialogue.dialogueEntries.Length)
        {
            currentEntry = dialogue.dialogueEntries[dialogueID];
            currentDialogue = Instantiate(currentEntry.dialogueStyle.dialogueBlueprint, dialogueParentContainer);
            currentDialogue.SetDialogueEntry(currentEntry);
            ++dialogueID;
        }
        else
        {
            currentDialogue = null;
            dialogueActive = false;
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
            objectsToActivate.Clear();
        }
    }
}
