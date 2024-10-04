using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Scripting;

public class BaseState
{

    protected Boss boss;
    protected StateMachine oodlerStateMachine;
    protected ChildStateMachine childStateMachine;

    public BaseState(Boss boss, StateMachine oodlerStateMachine) {
        this.boss = boss;
        this.oodlerStateMachine = oodlerStateMachine;
        childStateMachine = new ChildStateMachine();
    }
  
    public virtual void EnterState() { }

    public virtual void ExitState() { } 

    public virtual void FrameUpdate() { }

    public virtual void AnimationTriggerEvent(Boss.AnimationTriggerType triggerType) {
    
    }

    public virtual void getChildStateMachine() { }


}
