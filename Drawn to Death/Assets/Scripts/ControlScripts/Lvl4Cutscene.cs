using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl4Cutscene : MonoBehaviour
{
    enum Side { left, right}
    private Side entrySide;

    public EnemyAI triggerEnemy;
    public bool activeTrigger = true;

    public DialogueSequence sequence402;

    private void FixedUpdate()
    {
        if (activeTrigger && triggerEnemy?.state == State.dead)
        {
            activeTrigger = false;
            
            if (PlayerMovement.instance.transform.position.x > transform.position.x)
            {
                entrySide = Side.right;
            }
            else
            {
                entrySide = Side.left;
            }

            DialogueManager.instance.timelines[0 + (int)entrySide].SetActive(true);
            sequence402.activateTimeline = 3 + (int)entrySide;
        }
    }
}
