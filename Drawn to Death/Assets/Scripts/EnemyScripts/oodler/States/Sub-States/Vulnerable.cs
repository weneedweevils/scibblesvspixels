

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Vulnerable : ChildBaseState
{
    BossTimer vulnerableTimer;
    public Vulnerable(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Entered vulnerable state");
        base.EnterState();
        boss.EnableAreaHitbox(true);
        boss.EnableAttackHitbox(false);
        //boss.SetSlamCooldown(true); // set to true so that the oodler does not hurt anyone on the ground
        boss.HideShadow();
        boss.SetBossVulnerability(true);
        boss.animator.SetTrigger("Idle");
        boss.GetShadow().SetTrigger("Idle");
        vulnerableTimer = new BossTimer(5f);
       
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.SetBossVulnerability(false);
        Debug.Log("exiting Empty state");
    }

    public override void FrameUpdate()
    {
        if(vulnerableTimer.Update()){
            childStateMachine.ChangeState(boss.rise);
        }
        base.FrameUpdate();
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}

