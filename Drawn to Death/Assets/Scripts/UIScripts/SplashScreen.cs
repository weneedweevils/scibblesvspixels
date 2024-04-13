using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer videoPlayer;
    private bool videoStarted = false;

    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!videoStarted && videoPlayer.isPlaying)
        {
            videoStarted = true;
        }
        else if (videoStarted && !videoPlayer.isPlaying)
        {
            MenuManager.GotoScene(Scene.Menu);
        }
    }
}
