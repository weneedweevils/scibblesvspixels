using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class SkipCutscene : MonoBehaviour
{
    public PlayerMovement player;

    [HideInInspector]
    public PlayerInput playerInput;

    private Slider progressSlider;
    public GameObject childObject;
    public GameObject cutsceneObjects;
    public DataPersistenceManager dataPersistenceManager;
    public GameObject hud;
    public DialogueManager dialogueManager;
    public TMPro.TextMeshProUGUI label;
    private InputAction action;

    private bool isPressed;
    float skipTimer = 0f;

    private bool skippedCutscene = false;
    float skipTime = 2f;

    private void Awake()
    {
        playerInput = CustomInput.instance.playerInput;
        progressSlider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        // Get display string from action.
        action = playerInput.actions["SkipCutscene"];

        if (action != null)
        {
            string bindingText = action.GetBindingDisplayString().ToUpper();
            label.text = string.Format("SKIP CUTSCENE\n[{0}]", bindingText);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!skippedCutscene)
        {
            isPressed = playerInput.actions["SkipCutscene"].ReadValue<float>() > 0;

            // Increase the progress bar to skip cutscene
            if (isPressed)
            {
                //childObject.SetActive(true);
                skipTimer += Time.deltaTime;
                progressSlider.value = skipTimer / skipTime;

                //Once we pass the time required to press the skip button actually skip the cutscene
                if (skipTimer >= skipTime)
                {
                    HandleSkipCutscene();
                    skippedCutscene = true;
                }

            }

            else
            {

                // Decrease the progress bar to skip cutscene
                if(skipTimer > 0f)
                {
                    skipTimer -= Time.deltaTime;
                    progressSlider.value = skipTimer / skipTime;
                    if (skipTimer <= 0f)
                    {
                        skipTimer = 0f;
                    }
                }
                
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HandleSkipCutscene()
    {
        Destroy(dialogueManager.GetCurrentDialogue().gameObject);
        dialogueManager.SetCurrentDialogueNull();
        dialogueManager.dialogueActive = false;
        player.SetTimelineActive(false);
        cutsceneObjects.SetActive(false);
        hud.SetActive(true);
        player.GetPencil().enabled = true;
    }

    public void OnDisable()
    {
        Debug.Log("Destroyed object");
        Destroy(gameObject); 
    }
}