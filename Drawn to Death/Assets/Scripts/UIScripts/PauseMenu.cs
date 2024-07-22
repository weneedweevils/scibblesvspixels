using UnityEngine;
using UnityEngine.InputSystem;
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
    private PlayerMovement player;
    private PlayerInput playerInput;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInput = player.getInputSystem();
    }

    // Update is called once per frame
    void Update()
    {
        //If the player is not in a dialogue or timeline, then the pause menu can be accessed via escape
        if (!player.inFreezeDialogue() && !player.timelinePlaying)
        {
            if (playerInput.actions["Escape"].triggered)
            {

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
                if (ControlsUI.activeSelf)
                {
                    ControlsBack();
                }

                if (SettingsUI.activeSelf)
                {
                    SettingsBack();
                }
            }
        }


    }

    // resumes the game
    public void Resume(){
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    // pauses the game
    void Pause(){
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        

    }

    // load menu
    public void LoadMenu(){
        StartCoroutine(MenuManager.LoadScene(Scene.Menu));
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    // load controls/instructions screen
    public void InstructionScreen()
    {
        MenuUI.SetActive(false);
        ControlsUI.SetActive(true);
        Debug.Log("Here");
        
    }

    // go back from controls/instructions screen to pause screen
    public void ControlsBack()
    {
        ControlsUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    // go back to from settings screen to pause ui
    public void SettingsBack(){
        SettingsUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    // go to the settings menu
    public void SettingsMenu()
    {
        SettingsUI.SetActive(true);
        MenuUI.SetActive(false);
    }

    public void OnHovered()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIHover");
    }

    // quit the game
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayAcceptSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIAccept");
    }

    public void PlayLoadSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UILoad");
    }
    public void PlayBackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIBack");
    }
}
