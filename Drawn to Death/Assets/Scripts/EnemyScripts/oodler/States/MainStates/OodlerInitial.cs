using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerInitial : ParentBaseState
{
    List<ChildBaseState> orderedSubStateList;
    public OodlerInitial(Oodler boss, StateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
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
        if (boss.blockers.Length > 0)
        {
            foreach (EnemyAI blocker in boss.blockers)
            {
                if (blocker.isDead())
                {
                    boss.healthBarParent.SetActive(true);
                  


                    oodlerStateMachine.ChangeState(boss.oodlerIdle);
                    boss.musicScript.setIntensity(30f);
                }
            }
        }
    }


}
