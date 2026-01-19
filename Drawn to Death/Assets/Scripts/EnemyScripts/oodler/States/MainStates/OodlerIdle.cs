using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerIdle : ParentBaseState
{

    private float timer = 0f;
    List<ChildBaseState> orderedSubStateList;

    public OodlerIdle(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    public override void EnterParentState()
    {
        orderedSubStateList = new List<ChildBaseState>
        {
            new Chase(boss, this, chaseTime: 0f, chaseSpeed: 50f),
        };
        base.InitializeQueue(orderedSubStateList);
        orderedSubStateList = null;
        base.EnterParentState();
    }

    public override void ExitParentState()
    {
        base.ExitParentState();
    }

    public override void ParentFrameUpdate()
    {

        base.ParentFrameUpdate();
       
    }

    // this is where we decide 
    public override void NextSubState()
    {
        if (subStateQueue.Count > 0)
        {
            nextChildState = subStateQueue.Dequeue();
            ChangeChildState(nextChildState);
        }
        else
        {
            oodlerStateMachine.ChangeState(DecideState());
        }

    }


    // FIX THIS ujifasuifhui9ashfh8ashuifhyu8iasfh
    private ParentBaseState DecideState()
    {
        return boss.oodlerGrab;
    }



}
