
// Given parameters this class will determine what state the boss will transition to when playing against the player
using UnityEngine;

public class StateDecider
{
    
    // Have conditions where it will decide based on player health, boss health, proximity to player etc if not select random
    public void pickState(float playerHealth, float oodlerHealth, float playerProximity, int columnsLeft, int phase)
    {
        if(phase == 1)
        {

        }

        Debug.Log(playerHealth);

    }
}