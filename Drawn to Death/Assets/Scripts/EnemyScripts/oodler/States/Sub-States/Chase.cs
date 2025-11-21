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
     private BossTimer bossTimer;
     
   


    public Chase(Boss boss,  ParentBaseState parentBaseState) : base(boss, parentBaseState)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        reachedTarget = false;
        Debug.Log("<color=red> ENTERED CHASE");
        boss.ShowShadow();
        playerOffSet = boss.glich.transform.localPosition;
        bossTimer = new BossTimer(5f);
       

    }

    public override void ExitState()
    {
        base.ExitState();
        Debug.Log("exiting Empty state");
        bossTimer = null;

    }

    public override void FrameUpdate()
    {
        // If the distance between glich and oodler gets shorter, oodler will quickly snap to glichs posiiton
        if (Vector3.Distance(boss.glich.transform.position, boss.transform.position) < 20f){
            reachedTarget = boss.Stalk(reachedTarget, 100f);
            if(reachedTarget){
                if(bossTimer.Update()){
                    if(parentBaseState == boss.oodlerRun){
                        //childStateMachine.ChangeState(boss.goToRunPosition);
                        parentBaseState.NextSubState();
                    }
                    else if(parentBaseState == boss.oodlerSlam){
                        parentBaseState.NextSubState();
                    }
                    else if(parentBaseState == boss.oodlerGrab){
                        //childStateMachine.ChangeState(boss.prepareGrab);
                    }
                }   
            }
        }
        else{
           reachedTarget = boss.Stalk(reachedTarget, 50f);
        }
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
