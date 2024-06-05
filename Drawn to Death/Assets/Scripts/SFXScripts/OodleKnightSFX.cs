using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OodleKnightSFX : MonoBehaviour
{

    void PlaySound(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/OodleKnightWalk", this.transform.position);
    }

    void PlayAttack(){
        FMODUnity.RuntimeManager.PlayOneShot("event:/OodleKnightAttack", this.transform.position);
    }
}
