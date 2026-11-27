using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class QuickDeath : MonoBehaviour
{
      float start;
      float time;


    void Start()
    {
        start = 0f;
        time = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerMovement.instance.timelinePlaying){
            if(start >= time){
                Debug.Log("kill player");
                PlayerMovement.instance.Damage(1000000);
            }
            start = start + Time.deltaTime;
        }
        
    }
}

