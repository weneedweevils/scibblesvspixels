﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventNotifier : MonoBehaviour
{

    public event Action SlamNotifier;
    public event Action HitBoxActive;
    

    public void SlamStarted(){
        SlamNotifier?.Invoke();
    }

    public void SetHitBoxTrue(){
        HitBoxActive?.Invoke();
    }
}