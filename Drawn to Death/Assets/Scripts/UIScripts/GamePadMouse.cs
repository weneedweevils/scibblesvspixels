using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using MouseButton = UnityEngine.InputSystem.LowLevel.MouseButton;


// Followed this tutorial for most of this script https://www.youtube.com/watch?v=Y3WNwl1ObC8

public class GamePadMouse : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField]
    private RectTransform cursorTransform;
    [SerializeField]
    private GameObject cursorObject;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private RectTransform canvasRectTransform;
    [SerializeField]
    private float cursorSpeed = 100f;
    [SerializeField]
    private float horizontalPadding = 30f;
    private float verticalPadding = 1.0f;

    private Camera m_Camera;
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private bool previousMouseState;
    private Vector2 lastVirtualMousePosition;


    private void Awake()
    {
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerInput = CustomInput.instance.playerInput;
        lastVirtualMousePosition = cursorTransform.position;
        
    }


    
    private void OnEnable() {

        


        currentMouse = Mouse.current;
        
        // This will add a virtual mouse component on starting this script if it already exists but not added we add the existing one
        if (virtualMouse == null)
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");

        else if (!virtualMouse.added)
            InputSystem.AddDevice(virtualMouse);

        // Pairs device to user playerinput
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        //When starting the script we will retrive the position of the cursor
        if (cursorTransform != null)
        {

            // Function will save spawn the cursor where the cursor was last depending on if a gamepad or mouse was used
       
            if (lastVirtualMousePosition != null)
            {
                InputState.Change(virtualMouse.position, lastVirtualMousePosition);
            }
        }

        InputSystem.onAfterUpdate += UpdateMotion;
  

        // May need to put under another function to enable when event is called
        Cursor.visible = false;
        AnchorCursor(currentMouse.position.ReadValue());

    }

  

    private void OnDisable()
    {

       
        lastVirtualMousePosition = virtualMouse.position.ReadValue();
        
       
        currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
        Cursor.visible = true;
       

        if (virtualMouse!=null && virtualMouse.added)
        {
            InputSystem.RemoveDevice(virtualMouse);
        }
        InputSystem.onAfterUpdate -= UpdateMotion;

    }

    private void UpdateMotion()
    {
        if(virtualMouse == null || Gamepad.current == null)
        {
            return;
        }

        Vector2 stickValue = Gamepad.current.leftStick.ReadValue();
     


        stickValue *= cursorSpeed * Time.unscaledDeltaTime * Screen.width; // unscaled deltatime since we pause timescale

        

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPos = currentPosition + stickValue;


        horizontalPadding = Screen.width * 0.02f;
        verticalPadding = Screen.height * 0.05f;

        // Makes sure the cursor does not go outside the screen
        newPos.x = Mathf.Clamp(newPos.x, horizontalPadding, Screen.width-horizontalPadding);
        newPos.y = Mathf.Clamp(newPos.y, verticalPadding, Screen.height-verticalPadding);
        Debug.Log(newPos.x + ", " + newPos.y);
        Debug.Log("Screen Width: "+Screen.width +" ,Screen Height: "+ Screen.height);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, stickValue);

        
        // Used to detect if we clicked a UI button
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
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_Camera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

  
}
