using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : ChildBaseState
{
    public Run(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }

    private bool hitObstacle = false;
    private Vector3 oodlerRunDirection;
   

    public override void EnterState()
    {
        hitObstacle = false;
        boss.animator.SetTrigger("Walk");
        boss.shadowAnimator.SetTrigger("Walk");
        oodlerRunDirection = (boss.glich.transform.position - boss.transform.position).normalized;// Mov
        boss.EnableRunHitbox(true);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(OodlerRun()){
            
        }
    }

     public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }


    public bool OodlerRun(float speed = 20){

        if(hitObstacle){
            boss.EnableRunHitbox(false);
             boss.animator.SetTrigger("idle");
            return true;
        }

        var step = speed * Time.deltaTime;
        boss.oodlerRB.MovePosition(boss.transform.position + oodlerRunDirection * step);
        boss.CheckSpriteDirection();
        return false;

    }

    public void OnHitObstacle(){
        hitObstacle = true;
    }

}
