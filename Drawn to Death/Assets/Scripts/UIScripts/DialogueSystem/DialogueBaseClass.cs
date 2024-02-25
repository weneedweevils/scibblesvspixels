/*  References:
 *      - Youtube: Undertale DIALOGUE|CUTSCENE in Unity
 *          - By: Pandemonium
 *          - Link: https://youtu.be/8eJ_gxkYsyY?si=BGYvHOm4rYW8rUnV
 *          
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; protected set; }

        // Writes the text in a scroll style with controllable input, speed, font, and text colour
        protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont, float delay)
        {
            textHolder.color = textColor;
            textHolder.font = textFont;
            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }

    }
}
