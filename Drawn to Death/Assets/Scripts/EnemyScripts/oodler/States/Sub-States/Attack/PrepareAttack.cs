using UnityEngine;

public class PrepareAttack : ChildBaseState
{
    private BossTimer slamWarningTimer;
    private bool reachedTarget = false;
    private bool attackCharged = false;
    private bool stopOodler = false;
    private float slamWarningTime;
    private float chaseSpeed;
    
   /// <summary>
   /// This sub-state is the state where the boss shakes its fist above the player before a slam attack
   /// </summary>
   /// <param name="boss"></param>
   /// <param name="parentBaseState"></param>
   /// <param name="slamWarningTime"></param>
   /// <param name="chaseSpeed"></param>
    public PrepareAttack(Oodler boss, ParentBaseState parentBaseState, float slamWarningTime, float chaseSpeed) : base(boss, parentBaseState)
    {
        this.chaseSpeed = chaseSpeed;
        this.slamWarningTime = slamWarningTime;
    }

    public override void EnterState()
    {
        base.EnterState();
        boss.animator.SetTrigger("SlamWindUp");
        //boss.GetShadow().SetTrigger("SlamWindUp");
        reachedTarget = false;
        attackCharged = false;
        slamWarningTimer = new BossTimer(slamWarningTime);
        stopOodler = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        ResetState();

    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        // Following if statement will stalk glich, once the redoutline is fully revealed we will stop the oodler for sometime to give the player time to react
        if(!stopOodler){
            reachedTarget = boss.MoveToGlich(chaseSpeed);
            if(reachedTarget){
                attackCharged = boss.RevealAttack();
                if(attackCharged){
                    if(slamWarningTimer.Update()){
                        boss.ShowAttack();
                        stopOodler = true;
                        parentBaseState.NextSubState();
                        // change our state to the actual attack state
                    }
                }
            }
        }
    }

    public override void ResetState()
    {
        reachedTarget = false;
        attackCharged = false;
        slamWarningTimer = null;
        stopOodler = false;
    }


}
