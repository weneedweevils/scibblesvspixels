using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubieSFX : MonoBehaviour
{
    // FMOD sound event path
    //public string sfx;

    void PlaySwish(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/CubieSwish", this.transform.position);
    }

    void PlaySwoosh(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/CubieSwoosh", this.transform.position);
    }
}
