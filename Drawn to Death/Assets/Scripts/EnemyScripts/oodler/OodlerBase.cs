using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class OodlerBase
{

    protected Boss boss;
    protected OodlerStateMachine oodlerStateMachine;

    public OodlerBase(Boss boss, OodlerStateMachine oodlerStateMachine) {
        this.boss = boss;
        this.oodlerStateMachine = oodlerStateMachine;
    }

  
    public virtual void EnterState() { }

    public virtual void ExitState() { } 

    public virtual void FrameUpdate() { }

    public virtual void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType) {
    
    }


}
