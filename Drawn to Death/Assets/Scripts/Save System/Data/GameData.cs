using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool skipCutscene;
    public bool skipTutorial;
    public Scene scene;
    public Vector3 playerPosition;
    
    public GameData(bool tutorial)
    {
        skipCutscene = false;
        skipTutorial = tutorial;
        scene = Scene.Level_1;
        playerPosition = Vector3.zero;
    }
}
