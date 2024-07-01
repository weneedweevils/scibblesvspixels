using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PauseMenuManager : MonoBehaviour
{
    // STATE MACHINE AND CONTROL MAP //
    public MenuStateMachine menuStateMachine;
    public PlayerControlMap controls;
    
    // MENU STATES //
    public EmptyState empty;
    public PauseActiveState pauseActive;
    public ControlScreenState controlScreenState;

    // PlAYER MOVEMENT //
    public PlayerMovement player;


    // UI OBJECTS //
    public GameObject GameMenuObject;
    public GameObject ControlScreen;
    public GameObject PauseMenu;
    public GameObject SoundSettings;
    public bool paused = false;



    private void Awake()
    {
        menuStateMachine = new MenuStateMachine();
        empty = new EmptyState(this, menuStateMachine);
        pauseActive = new PauseActiveState(this, menuStateMachine);
        controlScreenState = new ControlScreenState(this, menuStateMachine);
    }


    private void Start()
    {
        Debug.Log("Hello");
        menuStateMachine.Initialize(empty);
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        controls = player.getControls();
    }

    private void Update()
    {
        menuStateMachine.currentMenu.FrameUpdate();
    }



    public void EnterControls()
    {
        menuStateMachine.ChangeState(controlScreenState);
    }
}

