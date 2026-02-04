using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerIdle : ParentBaseState
{

    private float timer = 0f;
    private int p1SlamNum;
    private int p2SlamNum;
    List<ChildBaseState> orderedSubStateList;

    public OodlerIdle(Oodler boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
        p1SlamNum = 0;
        p2SlamNum = 0;
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
            var nextChildState = subStateQueue.Dequeue();
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
            return DecidePhaseTwo();
        }

        if (boss.phase == Oodler.Phase.P3)
        {
            //return DecidePhaseThree();
        }



        return boss.oodlerSlam;
    }


    private ParentBaseState DecidePhaseOne()
    {
        if (p1SlamNum < 1)
        {
            p1SlamNum++;
            return boss.oodlerQuickSlam;
        }
        else
        {
            p1SlamNum = 0;
            return boss.oodlerSlam;
        }
           


    }

    private ParentBaseState DecidePhaseTwo()
    {

        if (p2SlamNum < 3)
        {
            p2SlamNum++;
            return boss.oodlerQuickSlam;
        }
        else
        {

            p2SlamNum = 0;
            if (boss.GlichInOpen())
            {
                return boss.oodlerRun;
            }
            else
            {
                return boss.oodlerGrab;
            }
        }
    }
}
