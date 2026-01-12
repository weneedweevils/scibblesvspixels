using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// This sub-state will make the oodler go to glichs position and close in on him
/// </summary>
public class Chase : ChildBaseState
{
     private bool reachedTarget = false;
     private Vector3 playerOffSet;
     private BossTimer chaseTimer;
     private float chaseSpeed;
     private float chaseTime;
     
   


    public Chase(Boss boss,  ParentBaseState parentBaseState, float chaseTime, float chaseSpeed = 100f) : base(boss, parentBaseState)
    {
        this.chaseSpeed = chaseSpeed;
        this.chaseTime = chaseTime;
    }

    public override void EnterState()
    {
        base.EnterState();
        reachedTarget = false;
        boss.ShowShadow();
        playerOffSet = boss.glich.transform.localPosition;
        chaseTimer = new BossTimer(chaseTime);
        //time we continue following glich


    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("exiting Empty state");
        ResetState();

    }

    public override void FrameUpdate()
    {
        // If the distance between glich and oodler gets shorter oodler speeds up to glich's position
        if (Vector3.Distance(boss.glich.transform.position, boss.transform.position) < 20f){
            reachedTarget = boss.MoveToGlich(chaseSpeed);
            if(reachedTarget){
                if(chaseTimer.Update()){
                    parentBaseState.NextSubState();
                }   
            }
        }
        else{
           reachedTarget = boss.MoveToGlich(chaseSpeed/2f);
        }
    }

    public override void ResetState()
    {
        base.ResetState();
        chaseTimer = null;
        reachedTarget = false;
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }


}
