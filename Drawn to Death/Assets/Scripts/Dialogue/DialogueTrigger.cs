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
        OnEnemyDeath,
    }

    public TriggerMode activationMethod;
    public DialogueSequence dialogueSequence;
    public EnemyAI enemy;

    public bool activeTrigger = true;

    // Start is called before the first frame update
    private void Start()
    {
        if (activeTrigger && activationMethod == TriggerMode.OnStart)
        {
            TriggerDialogue();
        }
    }

    private void FixedUpdate()
    {
        if (activeTrigger && activationMethod == TriggerMode.OnEnemyDeath && enemy?.state == State.dead)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogueSequence);
        activeTrigger = false;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (activeTrigger && activationMethod == TriggerMode.OnEnable)
        {
            TriggerDialogue();
        }
    }

    private void OnDisable()
    {
        if (activeTrigger && activationMethod == TriggerMode.OnDisable)
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activeTrigger && activationMethod == TriggerMode.OnTriggerEnter && collision.gameObject.tag == "Player")
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (activeTrigger && activationMethod == TriggerMode.OnTriggerExit && collision.gameObject.tag == "Player")
        {
            TriggerDialogue();
        }
    }
}