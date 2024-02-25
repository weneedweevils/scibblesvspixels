using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserSFX : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;

    // Make sure to set this to whether the eraser attack actually hit or not
    // 0 for no, 1 for yes
    [SerializeField]
    int isHit;

    // Start initializes the FMOD instance, but doesn't play it
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        //instance.start();
    }

    // Update the IsHit Parameter each frame
    void Update()
    {
        instance.setParameterByName("IsHit",  isHit);
    }
    
    // Play the sound 
    void PlaySound(){
        instance.start();
        instance.release();
    }
}
