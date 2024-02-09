using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Used this tutorial https://www.youtube.com/watch?v=JivuXdrIHK0
public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenuUI;
    public int Menu = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(gameIsPaused == true){
                Resume();
            }
            else{
                Pause();
            }
        }
        
    }
    public void Resume(){
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        
    }
    void Pause(){
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

    }

    public void LoadMenu(){
        SceneManager.LoadScene(Menu);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
}
