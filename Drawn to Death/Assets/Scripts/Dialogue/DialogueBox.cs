using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [Header("Image References")]
    public Image background;
    public Image border;
    public Image speakerImage;

    [Header("Text References")]
    public TMPro.TextMeshProUGUI speakerName;
    public TMPro.TextMeshProUGUI dialogueText;

    private DialogueEntry dialogueEntry;
    private bool finished = false;
    private float delay = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (finished)
            {
                DialogueManager.Instance.NextDialogue();
            }
            else
            {
                finished = true;
                StopCoroutine("WriteText");
                dialogueText.text = dialogueEntry.dialogueText;
            }
        }
    }

    public void SetDialogueEntry(DialogueEntry _dialogueEntry)
    {
        //Set the dialogueEntry
        dialogueEntry = _dialogueEntry;
        DialogueStyle style = _dialogueEntry.dialogueStyle;

        //Set the speaker Image
        speakerImage.sprite = dialogueEntry.speakerImage;

        //Set the speaker name & style
        speakerName.text = style.speakerName;
        speakerName.color = style.nameColor;
        speakerName.font = style.nameFont;

        //Set the dialogue text & style
        dialogueText.color = style.textColor;
        dialogueText.font = style.textFont;
        delay = style.writeDelay;

        if (delay > 0) StartCoroutine("WriteText");
        else dialogueText.text = dialogueEntry.dialogueText;
    }

    protected IEnumerator WriteText()
    {
        dialogueText.text = "";
        string input = dialogueEntry.dialogueText;
        bool writingTag = false;
        string subString = "";

        for (int i = 0; i < input.Length; i++)
        {
            subString += input[i];
            if (input[i].Equals('<') || input[i].Equals('>'))
            {
                writingTag = !writingTag;
                continue;
            }
            if (writingTag) continue;

            dialogueText.text += subString;
            subString = "";
            yield return new WaitForSeconds(delay);
        }

        finished = true;
    }
}
