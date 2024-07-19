using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public int activateTimeline = -1;
    [Space(15)]public DialogueEntry[] dialogueEntries;
}

[System.Serializable]
public class DialogueEntry
{
    public string Identifier;
    [Space(5)] [TextArea] public string dialogueText;
    [Space(10)] public float writeDelay = 0.05f;
    public DialogueStyle dialogueStyle;
    public DialogueSFX dialogueSFX;

    public string SFXEventPath()
    {
        return DialogueManager.Instance.DialogueSFXEventPath(dialogueSFX);
    }
}