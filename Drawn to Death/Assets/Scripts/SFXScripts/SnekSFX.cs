using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnekSFX : MonoBehaviour
{
    private EnemyAI selfAI;

    [Header("FMOD Events")]
    public FMODUnity.EventReference snekSlitherSFX;
    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.PLAYBACK_STATE playbackState;

    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(snekSlitherSFX);  
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        selfAI = GetComponent<EnemyAI>();
        instance.start();
    }

    void Update()
    {
        instance.getPlaybackState(out playbackState);

        // Check if the enemy is dead or not, then play/stop the sound effect as required
        if (!selfAI.isDead() && selfAI.state != State.idle &&
            playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED){
            // start the sfx playback again if it has stopped
            // We have to attach the instance again here too
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
            instance.start();
        }
        if ((selfAI.isDead() || selfAI.state == State.idle) &&
            playbackState == FMOD.Studio.PLAYBACK_STATE.PLAYING){
            // stop the sfx if the enemy is dead
            instance.stop(0);
        }
    }

    void OnDestroy()
    {
        instance.stop(0);
    }
}
