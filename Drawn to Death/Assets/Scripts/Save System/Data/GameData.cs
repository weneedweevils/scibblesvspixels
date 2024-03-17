using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool newGame;
    public Scene scene;
    public Vector3 playerPosition;
    
    public GameData()
    {
        newGame = true;
        playerPosition = Vector3.zero;
    }
}
