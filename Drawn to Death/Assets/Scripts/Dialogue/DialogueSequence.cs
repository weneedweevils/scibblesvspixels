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
    public DialogueVariable[] entryVariables;

    public string GetText()
    {
        if (entryVariables == null || entryVariables.Length == 0)
        {
            return dialogueText;
        }

        List<string> variables = new List<string>();
        foreach(DialogueVariable var in entryVariables)
        {
            variables.Add(var.GetText());
        }
        string[] parameters = variables.ToArray();
        return string.Format(dialogueText, parameters);
    }

    public string SFXEventPath()
    {
        return DialogueManager.instance.DialogueSFXEventPath(dialogueSFX);
    }
}