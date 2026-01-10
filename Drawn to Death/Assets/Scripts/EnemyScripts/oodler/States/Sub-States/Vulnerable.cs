

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Vulnerable : ChildBaseState
{
    BossTimer vulnerableTimer;
    public Vulnerable(Boss boss, ParentBaseState parentBaseState, float vulnerabilityTime = 5f) : base(boss, parentBaseState)
    {
        vulnerableTimer = new BossTimer(vulnerabilityTime);
    }

    public override void EnterState()
    {
        base.EnterState();
        Debug.Log("<color=red> ENTERED vulnerable");
        boss.EnableAreaHitbox(true);
        boss.EnableAttackHitbox(false);
        //boss.SetSlamCooldown(true); // set to true so that the oodler does not hurt anyone on the ground
        boss.HideShadow();
        boss.SetBossVulnerability(true);
        boss.animator.SetTrigger("Stunned");
        vulnerableTimer = new BossTimer(boss.bossVulnerabilityTime);
       
    }

    public override void ExitState()
    {
        base.ExitState();
        boss.SetBossVulnerability(false);
        Debug.Log("exiting Empty state");

        boss.animator.SetTrigger("Idle");
    }

    public override void FrameUpdate()
    {
        if(vulnerableTimer.Update()){

            parentBaseState.NextSubState();
        }
        base.FrameUpdate();
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void ResetState()
    {
        base.ResetState();

    }
}

