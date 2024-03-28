using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool skipCutscene;
    public Scene scene;
    public Vector3 playerPosition;
    
    public GameData()
    {
        skipCutscene = false;
        scene = Scene.Level_1;
        playerPosition = Vector3.zero;
    }
}
