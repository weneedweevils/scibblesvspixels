using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : ChildBaseState
{
    // NEED TO ADD SOME OF THESE TO BOSS SCRIPT

    private float runSpeed;
    private float runAcceleration;


    public Run(Oodler boss, ParentBaseState parentBaseState, float runSpeed, float runAcceleration = 0) : base(boss, parentBaseState)
    {
        this.runSpeed = runSpeed;
        this.runAcceleration = runAcceleration;
    }

    private bool hitObstacle = false;
    private Vector3 oodlerRunDirection;

  
    public override void EnterState()
    {
        hitObstacle = false;
        boss.animator.SetTrigger("Walk");
        oodlerRunDirection = (boss.glich.transform.position - boss.transform.position).normalized;// Mov
        boss.EnableRunHitbox(true);
        RunHitbox.CollidedWithObstacle += OnHitObstacle;
        boss.CheckSpriteDirection();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

   

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        var acceleration = runAcceleration * Time.deltaTime;
        runSpeed = runSpeed + acceleration;
        boss.OodlerRun(runSpeed , oodlerRunDirection);
        if (hitObstacle)
        {
            boss.EnableRunHitbox(false);
            parentBaseState.NextSubState();
        }

    }


   

    public void OnHitObstacle(){
        hitObstacle = true;
    }

    public override void ResetState()
    {
        base.ResetState();

    }

}
