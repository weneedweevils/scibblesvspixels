using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodlerSlam : OodlerBase
{

    bool reachedTarget = false;
    bool delay = true;
    private float timer = 0f;
    private float delayTimer = 0f;


    public OodlerSlam(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {

    }
    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        timer = 0f;
        delayTimer = 0f;    
        reachedTarget = false;
        boss.SetLastPosition();
        delay = true;
        boss.AttackSprite.color = new Color(243, 255, 104, 1);


    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        if (!delay)
        {
            if (!reachedTarget && boss.ReachedPlayerReal())
            {
                reachedTarget = true;
            }

            if (!reachedTarget)
            {
                boss.Slam();
            }
            else
            {
                timer += Time.deltaTime;
            }



            if (timer > 5f)
            {
                oodlerStateMachine.ChangeState(boss.oodlerIdle);
            }
        }
        else
        {

            delayTimer += Time.deltaTime;
            if (delayTimer > 0.2f)
            {
                boss.AttackSprite.color = new Color(243, 104, 104, 0);
                delay = false;
            }
        }

    }
}
