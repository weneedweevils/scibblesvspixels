using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardRebindState : MenuBaseState
{
    public KeyboardRebindState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        pauseMenuManager.KeyboardRebindUI.SetActive(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        pauseMenuManager.KeyboardRebindUI.SetActive(false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
}
