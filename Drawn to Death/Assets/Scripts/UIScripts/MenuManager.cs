 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene { Menu, Intructions, Level_1}
public class MenuManager : MonoBehaviour
{
    public Scene nextScene;
    public void QuitGame(){
        Application.Quit();
    }

    public void GotoScene()
    {
        SceneManager.LoadScene((int)nextScene);
    }

}
