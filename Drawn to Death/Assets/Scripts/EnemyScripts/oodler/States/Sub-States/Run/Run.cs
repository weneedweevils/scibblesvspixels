using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : ChildBaseState
{
    // NEED TO ADD SOME OF THESE TO BOSS SCRIPT

    private float runSpeed;


    public Run(Boss boss, ParentBaseState parentBaseState, float runSpeed) : base(boss, parentBaseState)
    {
    }

    private bool hitObstacle = false;
    private Vector3 oodlerRunDirection;

  
    public override void EnterState()
    {
        hitObstacle = false;
        boss.animator.SetTrigger("Walk");
        boss.GetShadow().SetTrigger("Walk");
        oodlerRunDirection = (boss.glich.transform.position - boss.transform.position).normalized;// Mov
        boss.EnableRunHitbox(true);
        RunHitbox.CollidedWithObstacle += OnHitObstacle;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        OodlerRun();
    }

     public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }


    public void OodlerRun(float speed = 20){

        if(hitObstacle){
            boss.EnableRunHitbox(false);
            parentBaseState.NextSubState();
        }

        var step = speed * Time.deltaTime;
        boss.oodlerRB.MovePosition(boss.transform.position + oodlerRunDirection * step);
        boss.CheckSpriteDirection();

    }

    public void OnHitObstacle(){
        hitObstacle = true;
    }

    public override void ResetState()
    {
        base.ResetState();

    }

}
