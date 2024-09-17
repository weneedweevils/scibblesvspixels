using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        skipButton.gameObject.SetActive(true);
        menuButton.SetActive(false);
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
        //skipButton.onClick.AddListener(() => { SkipVideo(); });
        playerInput = GetComponent<PlayerInput>();
}

    public void SkipVideo()
    {
        videoPlayer.time = videoPlayer.length-10;
        instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        skipButton.gameObject.SetActive(false);
        skipped = true;
    }

    private void Update()
    {
        if (videoPlayer.isPlaying && !skipped)
        {
            if (playerInput.actions["Escape"].triggered)
            {
                if (skipButton.IsActive())
                {
                    skipButton.gameObject.SetActive(false);
                }
                else
                {
                    skipButton.gameObject.SetActive(true);
                }
            }
        }
    }
 

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!videoStarted && videoPlayer.isPlaying)
        {
            videoStarted = true;
        }
        else if (videoStarted && !videoPlayer.isPlaying)
        {
            
            EventSystem.current.SetSelectedGameObject(menuButton);
            skipButton.gameObject.SetActive(false);
        }

      

        if (videoPlayer.time > videoPlayer.length - 20)
        {
            skipped = true;
            skipButton.gameObject.SetActive(false);
        }

        if (videoPlayer.time > creditStartTime)
        {
            menuButton.SetActive(true);
            if (credits.gameObject.transform.GetChild(0).transform.position.y >= 270)
            {
                scrollSpeed = 0;
                return;
            }
            credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;

            if (playerInput.actions["ScrollFaster"].IsPressed())
            {
                credits.transform.position += scrollSpeed * 3 * Vector3.up * Time.deltaTime;
            }
        }
    }
}
