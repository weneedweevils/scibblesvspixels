using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBaseState
{
    protected PauseMenuManager pauseMenuManager;
    protected MenuStateMachine menuStateMachine;


    public MenuBaseState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine)
    {
        this.pauseMenuManager = pauseMenuManager;
        this.menuStateMachine = menuStateMachine;
    }


    public virtual void EnterState() { }

    public virtual void ExitState() { }

 
    public virtual void FrameUpdate() {
        
        // This will functionally make the escape button a back button 
        if (!pauseMenuManager.player.inFreezeDialogue() && !pauseMenuManager.player.timelinePlaying)
        {
            if (pauseMenuManager.playerInput.actions["Escape"].triggered && !menuStateMachine.EndOfStack())
            {
                pauseMenuManager.PlayBackSound();
                menuStateMachine.GoBackState();

            }
        }
    }

}
