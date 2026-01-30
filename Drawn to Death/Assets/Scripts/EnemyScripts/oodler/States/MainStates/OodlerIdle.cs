using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerIdle : ParentBaseState
{

    private float timer = 0f;
    List<ChildBaseState> orderedSubStateList;

    public OodlerIdle(Oodler boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    public override void EnterParentState()
    {
        orderedSubStateList = new List<ChildBaseState>
        {
            //new Chase(boss, this, chaseTime: 0f, chaseSpeed: 50f),
             new EmptyChildState(boss, this)
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


    // This function will determine which state we go to next
    private ParentBaseState DecideState()
    {

        if(boss.phase == Oodler.Phase.P1)
        {
            return DecidePhaseOne();
        }

        if (boss.phase == Oodler.Phase.P2)
        {
            //return DecidePhaseTwo();
        }

        if (boss.phase == Oodler.Phase.P3)
        {
            //return DecidePhaseThree();
        }



        return boss.oodlerSlam;
    }


    private ParentBaseState DecidePhaseOne()
    {
        if (boss.GetGlichHealth() / boss.GetGlichMaxHealth() > 0.5f)
        {
            return boss.oodlerIntimidate;
        }
        else
        {
            return boss.oodlerSlam;
        }


    }

    //private ParentBaseState DecidePhaseTwo()
    //{


    //}

    //private ParentBaseState DecidePhaseThree()
    //{


    //}


}
