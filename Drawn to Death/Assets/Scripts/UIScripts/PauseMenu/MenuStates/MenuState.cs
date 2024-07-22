using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState :MenuBaseState
{
    public MenuState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entered Main Menu");
        base.EnterState();
        LoadMainMenu();
        

    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }


    public void LoadMainMenu()
    {
        SceneManager.LoadScene((int)Scene.Menu);
        Time.timeScale = 1f;
        //pauseMenuManager.paused = false;
        
    }
}
