using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class EmptyState : MenuBaseState
{



    public EmptyState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }



    public override void FrameUpdate()
    {
        if (!pauseMenuManager.player.inFreezeDialogue() && !pauseMenuManager.player.timelinePlaying)
        {
            if (pauseMenuManager.controls.Player.Escape.WasPerformedThisFrame())
            {
                Debug.Log("we got here");
                menuStateMachine.ChangeState(pauseMenuManager.pauseState);
              
            }
        }
    }

   

   

    public override void EnterState()
    {
        base.EnterState();
        Resume();
        Debug.Log("Entered Empty State");
    }

    public override void ExitState()
    {
        base.ExitState();
        Pause();

    }

    public void Resume()
    {
   
 
    
        pauseMenuManager.PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        pauseMenuManager.paused = false;

    }
    
    void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuManager.paused = true;
  
    
 
    }


}

