using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    [SerializeField] public bool stopMovement;

    // Starts Dialogue
    public void ActivateDialogue()
    {
        dialogue.SetActive(true);
    }

    // Checks if Dialogue is running
    public bool DialogueActive()
    {
        return dialogue.activeInHierarchy;
    }

}
