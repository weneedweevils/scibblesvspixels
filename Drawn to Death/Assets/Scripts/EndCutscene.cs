using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutscene : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer videoPlayer;
    private bool videoStarted = false;
    
    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;
    
    public GameObject menuButton;
    public GameObject credits;
    public float creditStartTime;
    public float scrollSpeed;
    
    


    // Start is called before the first frame update
    void Start()
    {
        menuButton.SetActive(false);
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
            menuButton.SetActive(true);
        }
        
        if (videoPlayer.time > creditStartTime)
        {
            credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;
        }
    }
}
