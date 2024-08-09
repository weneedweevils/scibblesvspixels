using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public enum TriggerMode
    {
        OnStart,
        OnEnable,
        OnDisable,
        OnTriggerEnter,
        OnTriggerExit,
    }

    public TriggerMode activationMethod;
    public DialogueSequence dialogueSequence;
    public bool activeTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        if (activationMethod == TriggerMode.OnStart && activeTrigger)
        {
            DialogueManager.Instance.StartDialogue(dialogueSequence);
            activeTrigger = false;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (activationMethod == TriggerMode.OnEnable && activeTrigger)
        {
            DialogueManager.Instance.StartDialogue(dialogueSequence);
            activeTrigger = false;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (activationMethod == TriggerMode.OnDisable && activeTrigger)
        {
            DialogueManager.Instance.StartDialogue(dialogueSequence);
            activeTrigger = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && activationMethod == TriggerMode.OnTriggerEnter && activeTrigger)
        {
            DialogueManager.Instance.StartDialogue(dialogueSequence);
            activeTrigger = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && activationMethod == TriggerMode.OnTriggerExit && activeTrigger)
        {
            DialogueManager.Instance.StartDialogue(dialogueSequence);
            activeTrigger = false;
            gameObject.SetActive(false);
        }
    }
}
