using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectOnDeath : MonoBehaviour
{
    // Used to enable a game object when an enemy with this script has died
    public GameObject dialogue;
    private static bool hasDied;
    private EnemyAI enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        hasDied = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasDied && enemyAI.state == State.dead)
        {
            hasDied = true;
            dialogue.SetActive(true);
        }
        else if (hasDied)
        {
            enabled = false;
        }
    }
}
