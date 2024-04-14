using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndCutscene : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer videoPlayer;
    private bool videoStarted = false;
    
    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;
    
    public GameObject menuButton;
    public GameObject credits;
    public UnityEngine.UI.Button skipButton;
    public float creditStartTime;
    public float scrollSpeed;
    private bool skipped = false;
    
    


    // Start is called before the first frame update
    void Start()
    {
        skipButton.gameObject.SetActive(false);
        menuButton.SetActive(false);
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
        skipButton.onClick.AddListener(() => { SkipVideo(); });
    }


    public void SkipVideo()
    {
        videoPlayer.time = videoPlayer.length-10;
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        skipButton.gameObject.SetActive(false);
        skipped = true;
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
            skipButton.gameObject.SetActive(false);
        }

        if (videoPlayer.isPlaying && !skipped)
        {
            if(Input.GetMouseButtonDown(0))
            {
                skipButton.gameObject.SetActive(true);
                
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                skipButton.gameObject.SetActive(false);
            }
        }

        if (videoPlayer.time > videoPlayer.length - 20)
        {
            skipped = true;
            skipButton.gameObject.SetActive(false);
        }

        if (videoPlayer.time > creditStartTime)
        {
            credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;
            if (Input.GetMouseButton(0))
            {
                credits.transform.position += scrollSpeed*3 * Vector3.up * Time.deltaTime;
            }
        }
    }
}
