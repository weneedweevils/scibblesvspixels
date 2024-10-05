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
     
   


    public Chase(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }



    public override void EnterState()
    {
        base.EnterState();
        reachedTarget = false;
        Debug.Log("Entering Chase State");
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
            Stalk(100f);
            if(reachedTarget){
                if(bossTimer.Update()){
                    childStateMachine.ChangeState(boss.goToRunPosition);
                }   
            }
        }
        else{
           Stalk(20f);
        }
        
        
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }



    

    // This function will follow the players position with an offset of 10 units above them
    public void Stalk(float speed = 20f)
    {
        var step = speed * Time.deltaTime;
        playerOffSet = boss.glich.transform.localPosition;
        playerOffSet.y = playerOffSet.y + 10f;
        boss.oodlerRB.MovePosition(Vector3.MoveTowards(boss.transform.position, playerOffSet, step));
        boss.MoveShadowSprite();
        if(Vector3.Distance(boss.transform.position, playerOffSet)<1f){
            boss.oodlerRB.MovePosition(playerOffSet);
            reachedTarget = true;
        }
    }

}
