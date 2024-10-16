﻿using UnityEngine;


public class EmptyState : MenuBaseState
{



    public EmptyState(PauseMenuManager pauseMenuManager, MenuStateMachine menuStateMachine) : base(pauseMenuManager, menuStateMachine)
    {
    }



    public override void FrameUpdate()
    {
        if (!pauseMenuManager.player.inFreezeDialogue() && !pauseMenuManager.player.timelinePlaying)
        {

            if (pauseMenuManager.playerInput.actions["Escape"].triggered)
            {
               
                pauseMenuManager.GoToPauseMenu();

            }
        }
    }

   
    public override void EnterState()
    {
        base.EnterState();
        Resume();
        pauseMenuManager.playerInput.SwitchCurrentActionMap("Player");
       

       
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

