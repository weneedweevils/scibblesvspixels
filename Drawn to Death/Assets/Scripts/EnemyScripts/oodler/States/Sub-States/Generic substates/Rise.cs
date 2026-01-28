using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Rise : ChildBaseState
{

    private Vector3 airPosition;
    BossTimer riseTimer;
    private float riseSpeed;
    private float riseDelay;
    private bool riseStarted;

    public Rise(Oodler boss, ParentBaseState parentBaseState, float riseSpeed, float riseDelay) : base(boss, parentBaseState)
    {
        this.riseSpeed = riseSpeed;
        this.riseDelay = riseDelay;
       
    }


    public override void EnterState()
    {
        base.EnterState();
        boss.EnableAreaHitbox(false);
        boss.EnableSpriteHitbox(true);
        boss.SetAirPosition();
        boss.ResetShadow();
        riseTimer = new BossTimer(riseDelay);
        riseStarted = false;
        boss.animator.SetTrigger("Idle");

       
        // boss.playerScript.ChangeSpriteSortingOrder(8);

        Debug.Log("Entered Rise State");

    }

    public override void ExitState()
    {
        boss.EnableSpriteHitbox(false);
        Debug.Log("Changed sprite order back to 8");
        boss.BringSpriteToForeground();
        base.ExitState();
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        if(riseTimer.Update()){

            if (!riseStarted)
            {
                boss.ShowShadow();
                riseStarted=true;
            }

            if (boss.RiseOodler()){
                // // Check if we have caught glich and we are in the grab parent state
                //if (parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught())
                //{
                //boss.ControlAllies(boss.dropZoneObject, true);
                //childStateMachine.ChangeState(boss.carryGlich);
                //}
                //else
                //{
                parentBaseState.NextSubState();
                //}
            }

        }
    }


    public override void ResetState()
    {
        base.ResetState();

    }
}