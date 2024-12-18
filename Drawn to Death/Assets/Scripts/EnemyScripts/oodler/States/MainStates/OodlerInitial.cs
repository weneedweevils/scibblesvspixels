using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerInitial : BaseState
{
   
    public OodlerInitial(Boss boss, StateMachine oodlerStateMachine, ChildStateMachine childStateMachine) : base(boss, oodlerStateMachine, childStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        if (boss.blockers.Length > 0)
        {
            foreach (EnemyAI blocker in boss.blockers)
            {
                if (blocker.isDead())
                {
                    boss.healthBarParent.SetActive(true);
                    oodlerStateMachine.ChangeState(boss.oodlerIdle);
                    boss.HealthCrystal1.SetActive(true);
                    boss.HealthCrystal2.SetActive(true);
                    boss.HealthCrystal3.SetActive(true);
                    boss.HealthCrystal4.SetActive(true);
                }
            }
        }
    }


}
