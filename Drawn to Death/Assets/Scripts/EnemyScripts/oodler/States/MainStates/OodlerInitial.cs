using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerInitial : ParentBaseState
{
    List<ChildBaseState> orderedSubStateList;
    public OodlerInitial(Boss boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
    }

    public override void EnterParentState()
    {
        orderedSubStateList = new List<ChildBaseState>
        {
            new EmptyChildState(boss, this),
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
        //if (boss.blockers.Length > 0)
        //{
        //    foreach (EnemyAI blocker in boss.blockers)
        //    {
        //        if (blocker.isDead())
        //        {
        //            boss.healthBarParent.SetActive(true);
        //            boss.HealthCrystal1.SetActive(true);
        //            boss.HealthCrystal2.SetActive(true);
        //            boss.HealthCrystal3.SetActive(true);
        //            boss.HealthCrystal4.SetActive(true);


        //            oodlerStateMachine.ChangeState(boss.oodlerIdle);
        //        }
        //    }
        //}
    }


}
