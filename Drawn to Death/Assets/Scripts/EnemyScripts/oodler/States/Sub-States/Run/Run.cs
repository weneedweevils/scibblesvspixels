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
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(boss.runSFXInstance, boss.transform);
        boss.runSFXInstance.start(); // Bugged?
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
        //boss.runSFXInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(boss.transform)); // This fixes the problem. it seems like attachinstance to gameobject might need to be run right before a sound is started otherwise it is not called. I will try attaching the instance to game object above instead of start in boss.
        if (hitObstacle)
        {
            boss.EnableRunHitbox(false);
            parentBaseState.NextSubState();
            boss.runSFXInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
