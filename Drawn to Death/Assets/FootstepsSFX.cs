using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsSFX : MonoBehaviour
{
    // FMOD sound event path
    public string sfx;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlaySound(){
        FMODUnity.RuntimeManager.PlayOneShot(sfx);
    }
}
