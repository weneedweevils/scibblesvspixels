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
    public float nameFontSize = 40;
    public Color nameColor = Color.white;

    [Header("Content")]
    public TMPro.TMP_FontAsset textFont;
    public float textFontSize = 68;
    public Color textColor = Color.white;
}
