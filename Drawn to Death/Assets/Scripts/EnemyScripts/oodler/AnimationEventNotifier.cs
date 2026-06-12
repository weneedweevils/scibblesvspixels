using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventNotifier : MonoBehaviour
{

    public event Action AttackNotifier;
    public event Action HitBoxActive;
    public event Action AttackEndNotifier;


    public void AttackStarted(){
        AttackNotifier?.Invoke();
    }

    public void SetHitBoxTrue(){
        HitBoxActive?.Invoke();
    }

    public void AttackEnded()
    {
        AttackEndNotifier?.Invoke();  
    }
}
