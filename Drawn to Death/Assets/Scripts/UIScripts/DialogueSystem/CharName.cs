using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharName : MonoBehaviour
{
    private Text textHolder;

    [Header("Text Customizers")]
    [SerializeField] private string input;
    [SerializeField] private Color textColor;
    [SerializeField] private Font textFont;

    // Ensures line runs and gets reset properly if text gets repeated
    private void OnEnable()
    {
        ResetLine();
        textHolder.color = textColor;
        textHolder.font = textFont;
        textHolder.text = input;
    }

    // Resets line so it can be repeated if nessecary
    private void ResetLine()
    {
        textHolder = transform.GetChild(0).gameObject.GetComponent<Text>();
        textHolder.text = "";
    }
}
