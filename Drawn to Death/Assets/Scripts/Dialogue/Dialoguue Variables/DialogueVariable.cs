using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueVariable", menuName = "Dialogue/Dialogue Variable")]
public class DialogueVariable : ScriptableObject
{
    public string text;

    public string GetText()
    {
        return text;
    }
}
