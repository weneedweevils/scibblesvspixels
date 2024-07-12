using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneTrigger : MonoBehaviour
{
    public Scene nextScene = Scene.End;
    public Animator transition;
    public float transitionTime = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().timelinePlaying = true; // Stop Movement once trigger activated
            if (nextScene != Scene.End)
            {
                GameData data = DataPersistenceManager.instance.GetGameData();
                data.skipCutscene = false;
                DataPersistenceManager.instance.UpdateGame();
            }
            StartCoroutine(MenuManager.LoadScene(nextScene, transition, transitionTime));
        }
    }
}
