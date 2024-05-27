using System.Collections;
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
    }

    public override void FrameUpdate()
    {

        base.FrameUpdate();

        if (!boss.ReachedDropZone())
        {
            boss.MoveToDropZone(10f);
            boss.MoveGlichWithOodler();
        }
        else
        {

            Debug.Log("here");
            if (delay > 5f)
            {
                if (!boss.GlichReachedDropZone())
                {
                    Debug.Log("have not reached drop zone");
                    //boss.MoveOffScreen();
                    boss.DropGlich(30);
                }
                else
                {
                    Debug.Log("have eached drop zone");
                    //boss.ControlAllies(boss.Glich);
                    boss.EnableGlichColliders(true);
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
