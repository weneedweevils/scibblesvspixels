

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Vulnerable : ChildBaseState
{
    BossTimer vulnerabilityTimer;
    private float vulnerabilityTime;
    private float hazardVulnerabilityTime;
    public Vulnerable(Oodler boss, ParentBaseState parentBaseState, float vulnerabilityTime = 3f, float hazardVulnerabilityTime = 5f) : base(boss, parentBaseState)
    {
        this.vulnerabilityTime = vulnerabilityTime;
        this.hazardVulnerabilityTime=hazardVulnerabilityTime;
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.EnableAreaHitbox(true);
        boss.EnableAttackHitbox(false);
        //boss.SetSlamCooldown(true); // set to true so that the oodler does not hurt anyone on the ground
        boss.HideShadow();
        boss.animator.SetTrigger("Stunned");

        if (boss.checkHazard())
        {
            vulnerabilityTimer = new BossTimer(hazardVulnerabilityTime);
            boss.setHazard(false);
        }
        else
        {
            vulnerabilityTimer = new BossTimer(vulnerabilityTime);
        }
           
       
    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("exiting Empty state");
        boss.animator.SetTrigger("Idle");
    }

    public override void FrameUpdate()
    {
        
        if(vulnerabilityTimer.Update()){

            parentBaseState.NextSubState();
        }
        base.FrameUpdate();
    }

    public override void ResetState()
    {
        base.ResetState();
        vulnerabilityTimer = null;

    }

 
}

