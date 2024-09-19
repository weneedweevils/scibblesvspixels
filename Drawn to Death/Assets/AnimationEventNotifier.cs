using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventNotifier : MonoBehaviour
{

    public event Action SlamNotifier;
    

    public void SlamStarted(){
        SlamNotifier?.Invoke();
    }
}
