 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scene { Menu, Intructions, Level_1, Level_2, Level_3, Ded, End}
public class MenuManager : MonoBehaviour, IDataPersistence
{
    public TextMeshProUGUI text;
    public GameObject Button;
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

    public void OnHovered()
    {
        text.color = new Color(255,255,255,0.50f);
    }

    public void OnHoveredExit()
    {
        text.color = new Color(255, 255, 255, 1f);
    }
}
