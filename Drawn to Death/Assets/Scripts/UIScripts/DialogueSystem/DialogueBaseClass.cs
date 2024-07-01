/*  References:
 *      - Youtube: Undertale DIALOGUE|CUTSCENE in Unity
 *          - By: Pandemonium
 *          - Link: https://youtu.be/8eJ_gxkYsyY?si=BGYvHOm4rYW8rUnV
 *          
 */

using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public PlayerControlMap controls;
        private PlayerMovement player;

        public void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();
            controls = player.getControls();
        }

        public bool finished { get; protected set; }

        // Writes the text in a scroll style with controllable input, speed, font, and text colour
        protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont, float delay)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;

            char[] displayedText = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                displayedText[i] = input[i] == '\n' ? '\n' : ' ';
            }

            for (int i = 0; i < input.Length; i++)
            {
                displayedText[i] = input[i];
                textHolder.text = new string(displayedText);
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitUntil(() => controls.Player.SkipText.WasPerformedThisFrame());
            finished = true;
        }

    }
}
