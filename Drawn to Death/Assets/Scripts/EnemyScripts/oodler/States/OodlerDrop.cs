﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerDrop : OodlerBase
{
    public OodlerDrop(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    float delay = 0f;
    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        delay = 0f;

    }

    public override void ExitState()
    {
        base.ExitState();
        boss.caught = false;

    }

    public override void FrameUpdate()
    {

        base.FrameUpdate();

        // move boss to drop zone 
        if (!boss.ReachedDropZone())
        {
            boss.MoveToDropZone(10f);
            boss.MoveGlichWithOodler();
        }

        // Once we get to drop zone wait 5 seconds to drop glich
        else
        {
            Debug.Log("here");
            if (delay > 2f)
            {
                if (!boss.GlichReachedDropZone())
                {
                    Debug.Log("have not reached drop zone");
                    //boss.MoveOffScreen();
                    boss.DropGlich(20);
                }
                else
                {
                    Debug.Log("have eached drop zone");
                    boss.EnableGlichColliders(true);
                    boss.ControlAllies(boss.Glich, false);
                    boss.PlayerScript.PausePlayerInput(false);
                    oodlerStateMachine.ChangeState(boss.oodlerIdle);
                }
            }
            else
            {
                boss.MoveGlichWithOodler();
                delay += Time.deltaTime;
            }

            

            
            
        }
    }

   


}