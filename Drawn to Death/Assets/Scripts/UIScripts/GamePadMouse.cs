using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;
using MouseButton = UnityEngine.InputSystem.LowLevel.MouseButton;


// Followed this tutorial for this script https://www.youtube.com/watch?v=Y3WNwl1ObC8

public class GamePadMouse : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private RectTransform cursorTransform;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RectTransform canvasRectTransform;
    [SerializeField]
    private float cursorSpeed = 100f;

    private Camera m_Camera;
    private Mouse virtualMouse;
    private bool previousMouseState;


    private void Awake()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnEnable() {

        // This will add a virtual mouse component on starting this script if it already exists but not added we add the existing one
        if (virtualMouse == null)
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        else if (!virtualMouse.added)
            InputSystem.AddDevice(virtualMouse);

        // Pairs device to user playerinput
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);
      
        // When starting the script we will retrive the position of the cursor
        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.position;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;

        

    }

    private void OnDisable()
    {
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
    }

    private void UpdateMotion()
    {
        if(virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
        stickValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPos = currentPosition + stickValue;

        Debug.Log(stickValue);

        // Makes sure the cursor does not go outside the screen
        newPos.x = Mathf.Clamp(newPos.x, 0f,Screen.width);
        newPos.y = Mathf.Clamp(newPos.x, 0f, Screen.height);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, stickValue);

        

        bool leftClickPressed = Gamepad.current.buttonSouth.isPressed;

        if (previousMouseState != leftClickPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, leftClickPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = leftClickPressed;
        }

        AnchorCursor(newPos);
    }

    private void AnchorCursor(Vector2 position)
    {

        
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, null, out anchoredPosition); //canvas.renderMode != RenderMode.ScreenSpaceOverlay ? null : m_Camera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
        
    }



}
