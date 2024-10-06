using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Rise : ChildBaseState
{

    private Vector3 airPosition;
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
      
    }

    public override void ExitState()
    {
        base.ExitState();
        
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(RiseOodler()){
            childStateMachine.ChangeState(boss.prepareAttack);
        }
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }


    public bool RiseOodler(float speed = 15)
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
}