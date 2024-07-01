using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitState : MenuBaseState
{
    public QuitState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Quitting the game");
        base.EnterState();
        Application.Quit();
        pauseMenuManager.paused = false;
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        menuStateMachine.ChangeState(pauseMenuManager.emptyState);
    }
}
