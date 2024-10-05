using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.TextCore;
using Vector3 = UnityEngine.Vector3;

public class Land : ChildBaseState
{
    public Land(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }
    private bool reachedPosition = false;
    private Vector3 runGroundPosition;

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        reachedPosition = false;
        runGroundPosition = boss.transform.position + new Vector3(0, -12f, 0);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        reachedPosition = LandOodler();
        if(reachedPosition){
            childStateMachine.ChangeState(boss.run);
        }
        
    }

    // This method will "Land" the oodler on the ground
    public bool LandOodler(float speed = 15)
    {

        boss.SetAirPosition(); 
        var step = speed * Time.deltaTime;
        boss.oodlerRB.MovePosition(Vector3.MoveTowards(boss.transform.position, runGroundPosition, step));
        Debug.Log(boss.transform.position);
        Debug.Log(runGroundPosition);
        Debug.Log(Vector3.Distance(boss.transform.position, runGroundPosition));
        if (Vector3.Distance(boss.transform.position, runGroundPosition) < 0.3f)
        {
            //boss.transform.position = runGroundPosition;
            boss.HideShadow();
            return true;
        }
        else
        {
            return false;
        }
    }
}
