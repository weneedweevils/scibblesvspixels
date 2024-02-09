 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour

{
    public int GameStart = 1 ;
    public void QuitGame(){
        Application.Quit();
    }

    public void StartGame(){
        SceneManager.LoadScene(GameStart);
        
    }

}
