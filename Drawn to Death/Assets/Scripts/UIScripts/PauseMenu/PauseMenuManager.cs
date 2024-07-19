using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PauseMenuManager : MonoBehaviour
{
    // STATE MACHINE AND CONTROL MAP //
    public MenuStateMachine menuStateMachine;
    //public PlayerControlMap controls;
    
    // MENU STATES //
    public EmptyState emptyState;
    public PauseActiveState pauseState;
    public ControlScreenState controlsState;
    public SettingsState settingsState;
    public QuitState quitState;
    public MenuState menuState;
    public ControllerRebindState controllerRebindState;
    public KeyboardRebindState keyboardRebindState;

    // PlAYER MOVEMENT //
    public PlayerMovement player;
    public Attack attack;
    public GameObject Attack;
    public GameObject Player;
    public PlayerInput playerInput;


    // UI OBJECTS //
    [Header("UI OBJECTS")]
    public GameObject GameMenuObject;
    public GameObject ControlScreen;
    public GameObject PauseMenu;
    public GameObject Settings;
    public GameObject KeyboardRebindUI;
    public GameObject ControllerRebindUI;


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
        keyboardRebindState = new KeyboardRebindState(this, menuStateMachine);
        controllerRebindState = new ControllerRebindState(this, menuStateMachine);
    }


    private void Start()
    {
        Debug.Log("Hello");
        menuStateMachine.Initialize(emptyState);

        Player = GameObject.Find("Player");
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        Attack = GameObject.Find("Eraser");
        attack = player.GetComponentInChildren<Attack>();

        playerInput = player.GetComponent<PlayerInput>();
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

    public void GoToRebindController()
    {
        menuStateMachine.ChangeState(controllerRebindState);
    }

    public void GoToRebindKeyboard()
    {
        menuStateMachine.ChangeState(keyboardRebindState);
    }
}

