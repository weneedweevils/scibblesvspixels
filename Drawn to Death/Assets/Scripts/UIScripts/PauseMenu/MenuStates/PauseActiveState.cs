using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseActiveState : MenuBaseState
{
    public PauseActiveState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entered Pause Menu");
        base.EnterState();
        pauseMenuManager.PauseMenu.SetActive(true);
        pauseMenuManager.playerInput.SwitchCurrentActionMap("UI");
        EventSystem.current.SetSelectedGameObject(pauseMenuManager.PauseFirstButton);
    }

    public override void ExitState()
    {
        base.ExitState();
        pauseMenuManager.PauseMenu.SetActive(false);
        

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

}
