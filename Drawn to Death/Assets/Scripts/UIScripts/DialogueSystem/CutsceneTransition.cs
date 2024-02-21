using DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this script in the last line of any dialogue holder that will transition to a new timeline (cutscene)
public class CutsceneTransition : MonoBehaviour
{
    [SerializeField] private GameObject nextCutscene;

    private DialogueLine line;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<DialogueLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (line.finished)
        {
            nextCutscene.SetActive(true);
        }
    }
}
