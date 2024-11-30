using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlScreenState : MenuBaseState
{
    public ControlScreenState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entered Control Screen");
        base.EnterState();
        pauseMenuManager.ControlScreen.SetActive(true);
        pauseMenuManager.SetCurrentButton(pauseMenuManager.ControlsFirstButton);
    }

    public override void ExitState()
    {
        base.ExitState();
        pauseMenuManager.ControlScreen.SetActive(false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }


   
    
}
