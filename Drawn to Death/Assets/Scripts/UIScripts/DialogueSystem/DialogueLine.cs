using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private Text textHolder;

        [Header ("Text Customizers")]
        [SerializeField] [TextArea] private string input;
        [SerializeField] private Color textColor;
        [SerializeField] private Font textFont;

        [Header("Time Variables")]
        [SerializeField] private float delay;

        [Header("Character Image")]
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;

        private IEnumerator lineAppear;

        // Runs when line starts
        private void Awake()
        {
            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }

        // Ensures line runs and gets reset properly if text gets repeated
        private void OnEnable()
        {
            ResetLine();
            lineAppear = WriteText(input, textHolder, textColor, textFont, delay);
            StartCoroutine(lineAppear);
        }

        // Used to let the player skip the dialogue scrolling
        private void Update()
        {
            if (playerInput.actions["SkipText"].triggered)
            {
                if (textHolder.text != input)
                {
                    StopCoroutine(lineAppear);
                    textHolder.text = input;
                }
                else
                {
                    finished = true;
                }
            }
        }

        // Resets line so it can be repeated if nessecary
        private void ResetLine()
        {
            textHolder = GetComponent<Text>();
            textHolder.text = "";
            finished = false;
        }
    }
}
