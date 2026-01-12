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

    public Rise(Boss boss, ParentBaseState parentBaseState, float riseSpeed, float riseDelay) : base(boss, parentBaseState)
    {
        this.riseSpeed = riseSpeed;
        this.riseDelay = riseDelay;
       
    }


    public override void EnterState()
    {
        base.EnterState();
        boss.ShowShadow();
        boss.EnableAreaHitbox(false);
        boss.EnableSpriteHitbox(true);
        airPosition = boss.transform.position;
        airPosition.y = airPosition.y + 12f;
        riseTimer = new BossTimer(riseDelay);
        boss.animator.SetTrigger("Idle");

        if (boss.IsCaught()){
            boss.EnableGlichColliders(false);
            boss.playerScript.animator.SetTrigger("Grabbed");
            
        }
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
        
        IfCaught();
        if(riseTimer.Update()){
            if(RiseOodler()){
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


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }


    // MOVE TO BOSS FUNCTION
    public bool RiseOodler(float speed = 10f)
    {
        
        var step = speed * Time.deltaTime;
        boss.oodlerRB.MovePosition(Vector3.MoveTowards(boss.transform.position, airPosition, step));

        if (Vector3.Distance(boss.transform.position, airPosition) < 0.3f)
        {
            boss.oodlerRB.MovePosition(airPosition);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void IfCaught(){
         //if(parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught()){
             //boss.MoveGlichWithOodler();
        //}

    }

    public override void ResetState()
    {
        base.ResetState();

    }
}