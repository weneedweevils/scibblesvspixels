using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public int activateTimeline = -1;
    [Space(15)]public DialogueEntry[] dialogueEntries;
}

[System.Serializable]
public struct DialogueEntry
{
    public string Identifier;
    [Space(5)] [TextArea] public string dialogueText;
    [Space(10)] public Sprite speakerImage;
    public DialogueStyle dialogueStyle;
}