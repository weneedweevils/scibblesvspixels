using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsState : MenuBaseState
{
    public SettingsState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entered Settings Screen");
        base.EnterState();
        pauseMenuManager.Settings.SetActive(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        pauseMenuManager.Settings.SetActive(false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
}
