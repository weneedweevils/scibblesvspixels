 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scene {Splash_Screen, Menu, Intructions, Level_1, Level_2, Level_3, Ded, End, Credits}
public class MenuManager : MonoBehaviour, IDataPersistence
{
    [Header("Next Scene")]
    public Scene nextScene;
    public bool newGame = false;
    public bool loadGame = false;

    [Header("References")]
    public TextMeshProUGUI text;

    public void QuitGame()
    {
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

    public void ShowCredits()
    {
        nextScene = Scene.Credits;
        GotoScene(nextScene);
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
