using UnityEngine;


public class OodlerRecover : OodlerBase
{
    private float timer = 0f;
    public OodlerRecover(Boss boss, OodlerStateMachine oodlerStateMachine) : base(boss, oodlerStateMachine)
    {
        
    }

    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        boss.ShowShadow();
        boss.SetBossVulnerability(false);
        timer = 0f;
        boss.EnableAreaHitbox(false);
        boss.ChangeSpriteSortingOrder(8);

    
        if (boss.IsCaught())
        {
            boss.PlayerScript.PausePlayerInput(true);
            boss.EnableGlichColliders(false);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        
        base.FrameUpdate();
        boss.MoveUp();

        // if we caught glich move with boss
        if (boss.IsCaught())
        {
            
            boss.MoveGlichWithOodler();
        }
        
        // Once we reached the max air height wait 3 seconds before going to drop state or idle state
        if(boss.ReachedAirPosition())
        {
            timer = timer + Time.deltaTime;
            if (timer > boss.airTime)
            {

                Debug.Log("caught is set to" + boss.IsCaught());

                if (boss.IsCaught()) {
                    Debug.Log("CHanging states to drop");
                    oodlerStateMachine.ChangeState(boss.oodlerDrop);
                    
                }
                else if(boss.grabbing)
                {
                    oodlerStateMachine.ChangeState(boss.oodlerIdle);
                }

                else if(boss.SlamNum < boss.allowedSlams)
                {
                        oodlerStateMachine.ChangeState(boss.oodlerChase);
                }
                else
                {
                        boss.SlamNum = 0;
                        oodlerStateMachine.ChangeState(boss.oodlerIdle);
                }

                
            }
        }
    }


}
