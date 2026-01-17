using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerDrop : ParentBaseState
{
    List<ChildBaseState> orderedSubStateList;
    public OodlerDrop(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterParentState()
    {

        orderedSubStateList = new List<ChildBaseState>
        {
            new Chase(boss, this, chaseTime: 3f, chaseSpeed: 50f),
        };
        base.InitializeQueue(orderedSubStateList);
        orderedSubStateList = null;
        base.EnterParentState();
        

    }
    
    public override void ExitParentState()
    {
        base.ExitParentState();
        boss.SetBossCaught(false);

    }

    public override void ParentFrameUpdate()
    {

        base.ParentFrameUpdate();

        //// move boss to drop zone 
        //if (!boss.ReachedDropZone())
        //{
        //    boss.MoveToDropZone(10f);
        //    boss.MoveGlichWithOodler();
        //}

        //// Once we get to drop zone wait 5 seconds to drop glich
        //else
        //{
        //    Debug.Log("here");
        //    if (delay > 2f)
        //    {
        //        if (!boss.GlichReachedDropZone())
        //        {
        //            Debug.Log("have not reached drop zone");
        //            //boss.MoveOffScreen();
        //            boss.DropGlich(20);
        //        }
        //        else
        //        {
        //            Debug.Log("have eached drop zone");
        //            boss.EnableGlichColliders(true);
        //            boss.ControlAllies(boss.glich, false);
        //            boss.playerScript.EnableInput();
                   
        //            oodlerStateMachine.ChangeState(boss.oodlerIdle);
        //        }
        //    }
        //    else
        //    {
        //        boss.MoveGlichWithOodler();
        //        delay += Time.deltaTime;
        //    }

            

            
            
        //}
    }

   


}
