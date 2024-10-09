using UnityEngine;

public class PrepareAttack : ChildBaseState
{
    private BossTimer bossTimer;
    private bool reachedTarget = false;
    private bool attackCharged = false;
    private bool stopOodler = false;

    public PrepareAttack(Boss boss, ChildStateMachine childStateMachine, StateMachine parentStateMachine) : base(boss, childStateMachine, parentStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.animator.SetTrigger("SlamWindUp");
        boss.GetShadow().SetTrigger("SlamWindUp");
        reachedTarget = false;
        attackCharged = false;
        bossTimer = new BossTimer(0f);
        stopOodler = false;
    }

    public override void ExitState()
    {
        base.ExitState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Following if statement will stalk glich, once the redoutline is fully revealed we will stop the oodler for sometime to give the player time to react
        if(!stopOodler){ // This logic here seems flawed
            reachedTarget = boss.Stalk(reachedTarget, 100f);
            if(reachedTarget){
                attackCharged = boss.RevealAttack();
                if(attackCharged){
                    if(bossTimer.Update()){
                        boss.ShowAttack();
                        stopOodler = true;
                        
                        childStateMachine.ChangeState(boss.swingHand);
                        // change our state to the actual attack state
                    }
                }
            }
        }
    }


    public override void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}
