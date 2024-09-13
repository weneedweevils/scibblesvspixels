using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class DialogueBox : MonoBehaviour
{
    [Header("Image References")]
    public Image background;
    public Image border;
    public Image speakerImage;

    [Header("Text References")]
    public TMPro.TextMeshProUGUI speakerName;
    public TMPro.TextMeshProUGUI dialogueText;

    public FMODUnity.StudioEventEmitter fmodEmitter;

    private DialogueEntry dialogueEntry;
    private bool finished = false;
    private float delay = 0f;

    private void Update()
    {
        if (DialogueManager.Instance.playerInput.actions["SkipText"].triggered)
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

        if (DialogueManager.Instance.playerInput.actions["ReloadDialogue"].triggered)
        {
            DialogueManager.Instance.ResetDialogue();
        }

        if (DialogueManager.Instance.playerInput.actions["PreviousDialogue"].triggered)
        {
            DialogueManager.Instance.PrevDialogue();
        }
    }

    public void SetDialogueEntry(DialogueEntry _dialogueEntry)
    {
        //Set the dialogueEntry
        dialogueEntry = _dialogueEntry;
        DialogueStyle style = _dialogueEntry.dialogueStyle;

        //Set the speaker Image
        if (style.speakerImage != null)
        {
            speakerImage.sprite = style.speakerImage;
        }

        //Set the speaker name & style
        speakerName.text = style.speakerName;
        speakerName.color = style.nameColor;
        if (DialogueManager.fancyFont)
        {
            speakerName.font = style.nameFont;
            speakerName.fontSize = style.nameFontSize;
        }
        else
        {
            speakerName.fontSize = style.nameFontSize * 0.8f;
        }

        //Set the dialogue text & style
        dialogueText.color = style.textColor;
        if (DialogueManager.fancyFont)
        {
            dialogueText.font = style.textFont;
            dialogueText.fontSize = style.textFontSize;
        }
        else
        {
            dialogueText.fontSize = style.textFontSize * 0.8f;
        }
        delay = dialogueEntry.writeDelay;

        //Set Dialogue Text
        if (delay > 0) StartCoroutine("WriteText");
        else dialogueText.text = dialogueEntry.dialogueText;

        //Play the correct SFX
        string path = dialogueEntry.SFXEventPath();
        if (path != null)
        {
            fmodEmitter.EventReference = RuntimeManager.PathToEventReference(path);
            fmodEmitter.Play();
        }
    }

    protected IEnumerator WriteText()
    {
        //Initialize starting conditions
        dialogueText.text = "";
        string input = dialogueEntry.dialogueText;
        bool writingTag = false;
        string subString = "";

        //Iterate through input text
        for (int i = 0; i < input.Length; i++)
        {
            //Add the next character from input to a substring
            subString += input[i];

            //Check if this character indicates the start or end of a Rich Text Tag
            if (input[i].Equals('<') || input[i].Equals('>'))
            {
                writingTag = !writingTag;
                continue;
            }

            //Check if we are currently writing a Rich Text tag
            if (writingTag) continue;

            //We are not writing a tag or finished writing a tag so add the substring to the dialogue text
            dialogueText.text += subString;

            //Clear the substring
            subString = "";

            //Wait for delay seconds
            yield return new WaitForSeconds(delay);
        }

        finished = true;
    }
}
