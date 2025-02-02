using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsChanged : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField]
    private GameObject gamePadMouse;
    private GamePadMouse gamePadMouseScript;
    


    public void OnDeviceChanged(PlayerInput pi)
    {
        if (pi.currentControlScheme.Equals("KBM"))
        {
            gamePadMouse.SetActive(false);

        }
        else if (pi.currentControlScheme.Equals("Gamepad"))
        {
            if (gamePadMouse != null)
            {
                gamePadMouse.SetActive(true);
            }
        }
    }

    public void Awake()
    {
        
        playerInput = CustomInput.instance.playerInput;
       
    }

    private void OnEnable() { 
    
        if (playerInput.currentControlScheme.Equals("Gamepad")) {
            gamePadMouse.SetActive(true);
        }
        playerInput.onControlsChanged += OnDeviceChanged;
        
    }

    private void OnDisable()
    {
        playerInput.onControlsChanged -= OnDeviceChanged;
        gamePadMouse.SetActive(false);
    }
}
