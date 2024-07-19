using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRebindState : MenuBaseState
{
    public ControllerRebindState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }


    public override void EnterState()
    {
        base.EnterState();
        pauseMenuManager.ControllerRebindUI.SetActive(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        pauseMenuManager.ControllerRebindUI.SetActive(false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

}
