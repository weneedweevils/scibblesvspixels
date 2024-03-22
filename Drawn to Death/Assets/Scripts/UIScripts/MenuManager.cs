 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene { Menu, Intructions, Level_1, Level_2, Level_3, Ded, End}
public class MenuManager : MonoBehaviour, IDataPersistence
{
    public Scene nextScene;
    public bool newGame = false;
    public bool loadGame = false;

    public void QuitGame(){
        Application.Quit();
    }

    public void GotoScene()
    {
        if (newGame)
        {
            DataPersistenceManager.instance.NewGame();
            nextScene = Scene.Level_1;
        }
        GotoScene(nextScene);
    }

    public static void GotoScene(Scene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public void LoadData(GameData data)
    {
        if (loadGame)
        {
            nextScene = data.scene;
        }
        return;
    }

    public void SaveData(ref GameData data)
    {
        return;
    }
}
