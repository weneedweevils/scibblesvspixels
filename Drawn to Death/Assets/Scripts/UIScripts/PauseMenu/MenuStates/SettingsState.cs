using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SettingsState : MenuBaseState
{
    public SettingsState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }

    public override void EnterState()
    {
        
        base.EnterState();
        pauseMenuManager.Settings.SetActive(true);
        pauseMenuManager.SetCurrentButton(pauseMenuManager.SettingsFirstButton);
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
