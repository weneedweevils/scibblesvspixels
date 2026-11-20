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
        time = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerMovement.instance.timelinePlaying){
            if(start >= time)
            {
                PlayerMovement.instance.Damage(1000000f);
            }
            start = start + Time.deltaTime;
            Debug.Log(start);
        }
        
    }
}
