using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        // These variables are only needed if using this in timeline
        [SerializeField] private GameObject nextCutscene;
        [SerializeField] private GameObject dialogueObject;

        // Starts dialogue in a coroutine
        private void OnEnable()
        {
            StartCoroutine(DialogueSequence());
        }

        // Ensures sequence of lines runs properly
        private IEnumerator DialogueSequence()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(()=> transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
            gameObject.SetActive(false);
            if (nextCutscene != null)
            {
                nextCutscene.SetActive(true);
                dialogueObject.SetActive(false);
            }
        }

        // Deactivates any lines that shouldn't be on screen
        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
