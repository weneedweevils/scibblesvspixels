using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Rise : ChildBaseState
{

    private Vector3 airPosition;
    BossTimer bossTimer;
    public Rise(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }


    public override void EnterState()
    {
        base.EnterState();
        boss.ShowShadow();
        boss.SetBossVulnerability(false);
        boss.EnableAreaHitbox(false);
        boss.ChangeSpriteSortingOrder(8);
        airPosition = boss.transform.position;
        airPosition.y = airPosition.y + 12f;
        Debug.Log("entered rise state");
        Debug.Log(parentStateMachine.currentOodlerState);
        bossTimer = new BossTimer(4f);

        if(boss.IsCaught()){
            boss.playerScript.animator.SetTrigger("Grabbed");
            boss.playerScript.ChangeSpriteSortingOrder(9);
        }

      
    }

    public override void ExitState()
    {
        base.ExitState();
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        IfCaught();
        if(bossTimer.Update()){
            if(RiseOodler()){
                // // Check if we have caught glich and we are in the grab parent state
                if(parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught())
                     childStateMachine.ChangeState(boss.carryGlich);
                 else{
                     childStateMachine.ChangeState(boss.chase);
                 }
            }

        }
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }


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
         if(parentStateMachine.currentOodlerState == boss.oodlerGrab && boss.IsCaught()){
             boss.MoveGlichWithOodler();
        }

    }
}