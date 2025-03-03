using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabWalkSFX : MonoBehaviour {
    
    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;
    private FMOD.Studio.PLAYBACK_STATE playbackState;

    private EnemyAI enemyAI;

    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);  
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
        enemyAI = GetComponent<EnemyAI>();
        instance.start();
    }

    void Update()
    {
        instance.getPlaybackState(out playbackState);

        // Check if the enemy is dead or not, then play/stop the sound effect as required
        if (!enemyAI.isDead() && 
            playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED){
            // start the sfx playback again if it has stopped
            // We have to attach the instance again here too
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, GetComponent<Transform>(), GetComponent<Rigidbody2D>());
            instance.start();
        }
        if (enemyAI.isDead() &&
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