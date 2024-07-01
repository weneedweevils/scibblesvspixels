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
    public EmptyState emptyState;
    public PauseActiveState pauseState;
    public ControlScreenState controlsState;
    public SettingsState settingsState;
    public QuitState quitState;
    public MenuState menuState;

    // PlAYER MOVEMENT //
    public PlayerMovement player;


    // UI OBJECTS //
    [Header("UI OBJECTS")]
    public GameObject GameMenuObject;
    public GameObject ControlScreen;
    public GameObject PauseMenu;
    public GameObject Settings;


    public int Menu = 0;
    public bool paused = false;



    private void Awake()
    {
        menuStateMachine = new MenuStateMachine();
        emptyState = new EmptyState(this, menuStateMachine);
        pauseState = new PauseActiveState(this, menuStateMachine);
        controlsState = new ControlScreenState(this, menuStateMachine);
        settingsState = new SettingsState(this, menuStateMachine);
        quitState = new QuitState(this, menuStateMachine);
        menuState = new MenuState(this, menuStateMachine);
    }


    private void Start()
    {
        Debug.Log("Hello");
        menuStateMachine.Initialize(emptyState);
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        controls = player.getControls();
    }

    private void Update()
    {
        menuStateMachine.currentMenu.FrameUpdate();
    }


    // function to go to the control screen state//
    public void GoToControls()
    {
        menuStateMachine.ChangeState(controlsState);
    }


    // function to go back to last state in the stack //
    public void Escape()
    {
        menuStateMachine.GoBackState();
    }

    // goes to the settings screen
    public void GoToSettings()
    {
        menuStateMachine.ChangeState(settingsState);
    }

    public void GoToQuit()
    {
        menuStateMachine.ChangeState(quitState);
    }

    public void GoToMenu()
    {
        menuStateMachine.ChangeState(menuState);
    }
}

