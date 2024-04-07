using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Used this tutorial https://www.youtube.com/watch?v=JivuXdrIHK0
public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject ControlsUI;
    public GameObject MenuUI;
    public GameObject SettingsUI;
    public int Menu = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (!ControlsUI.activeSelf && !SettingsUI.activeSelf)
            {

                if (gameIsPaused == true)
                {
                    Resume();
                }
                else
                {

                    Pause();
                }
            }
            if(ControlsUI.activeSelf)
            {
                ControlsBack();
            }

            if (SettingsUI.activeSelf)
            {
                SettingsBack();
            }
        }
      
        
    }
    public void Resume(){
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Debug.Log("WORKING");
        
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

    public void InstructionScreen()
    {
        MenuUI.SetActive(false);
        ControlsUI.SetActive(true);
        Debug.Log("Here");
        
    }

    public void ControlsBack()
    {
        ControlsUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    public void SettingsBack(){
        SettingsUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    public void SettingsMenu()
    {
        SettingsUI.SetActive(true);
        MenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
