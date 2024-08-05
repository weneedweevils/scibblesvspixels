using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueStyle", menuName = "Dialogue/Dialogue Style")]
public class DialogueStyle : ScriptableObject
{
    [Header("Style")]
    public DialogueBox dialogueBlueprint;
    public Sprite speakerImage;

    [Header("Name")]
    public string speakerName;
    public TMPro.TMP_FontAsset nameFont;
    public Color nameColor;

    [Header("Content")]
    public TMPro.TMP_FontAsset textFont;
    public Color textColor;
}
