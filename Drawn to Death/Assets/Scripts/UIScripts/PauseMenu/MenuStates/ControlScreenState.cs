using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
