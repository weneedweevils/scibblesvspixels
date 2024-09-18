using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.UI;

// This is the reworked PauseMenu //
// You can create new parts to the menu by creating new states following the same structure as the MENU States listed below //

public class PauseMenuManager : MonoBehaviour

{
    public enum Scene { Splash_Screen, Menu, Intructions, Level_1, Level_2, Level_3, Ded, End, Credits }


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

    public GameObject inputHandler;

    [HideInInspector]
    public PlayerInput playerInput;


    // UI OBJECTS //
    [Header("UI OBJECTS")]
    public GameObject GameMenuObject;
    public GameObject ControlScreen;
    public GameObject PauseMenu;
    public GameObject Settings;
    public GameObject KeyboardRebindUI;
    public GameObject ControllerRebindUI;
  

    // UI First Selected Buttons //
    public GameObject SettingsFirstButton;
    public GameObject PauseFirstButton;
    public GameObject ControlsFirstButton;
    private GameObject currentButton;

    //public int Menu = 1;
    public bool paused = false;

    // ENEMY OBJECTS //
    [Header("ENEMY OBJECTS")]
    public GameObject EnemiesObject;
    public GameObject BlockersObject;



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
        playerInput = inputHandler.GetComponent<PlayerInput>();
    }

    // Initialize to the Empty State //
    private void Start()
    {
        menuStateMachine.Initialize(emptyState);
    }

    private void Update()
    {
        menuStateMachine.currentMenu.FrameUpdate();
    }


    // function to go to the control screen state//
    public void GoToControls()
    {
        menuStateMachine.ChangeState(controlsState);
        EnemiesObject.SetActive(false);
        BlockersObject.SetActive(false);
    }


    // function to go back to last state in the stack //
    public void Escape()
    {
        menuStateMachine.GoBackState();
        PlayBackSound();
        if (menuStateMachine.currentMenu == pauseState)
        {
            EnemiesObject.SetActive(true);
            BlockersObject.SetActive(true);
        }
    }


    // Button Methods //
    // These methods below should only be called via buttons on the UI //

    // Goes to the settings screen where you can adjust volume
    public void GoToSettings()
    {
        menuStateMachine.ChangeState(settingsState);
       
    }

    // Quits the game //
    public void GoToQuit()
    {
        menuStateMachine.ChangeState(quitState);
    }

    // Goes to the Main Menu //
    public void GoToMenu()
    {
        menuStateMachine.ChangeState(menuState);
    }

    // Goes to the Rebind Controller Screen //
    public void GoToRebindController()
    {
        menuStateMachine.ChangeState(controllerRebindState);
    }

    // Goes to the Rebind Keyboard Screen //
    public void GoToRebindKeyboard()
    {
        menuStateMachine.ChangeState(keyboardRebindState);
    }

    // Opens the Pause Menu //
    public void GoToPauseMenu()
    {
       
        menuStateMachine.ChangeState(pauseState);
       
    }

    public void PlayAcceptSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIAccept");
    }

    public void PlayLoadSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UILoad");
    }
    public void PlayBackSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIBack");
    }

    public void OnHovered()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UIHover");
    }

    public void SetCurrentButton(GameObject button)
    {
        currentButton = button;
        EventSystem.current.SetSelectedGameObject(currentButton);
    }

    public void OnDeviceChanged()
    {
        EventSystem.current.SetSelectedGameObject(currentButton);
    }

    public void DisableControls()
    {

    }

    public void EnableControls()
    {

    }
}

