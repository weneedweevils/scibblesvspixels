using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Credits : MonoBehaviour
{
    public GameObject credits;
    private float scrollSpeed = 1.5f; // Speed for credits from menu (Speed on credits from end cutscene = 80)
    public UnityEngine.UI.Button menuButton;
    PlayerInput playerInput;

    private void Start()
    {
        Debug.Log("Started");
        playerInput = CustomInput.instance.playerInput;
        
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (credits.gameObject.transform.GetChild(0).transform.position.y >= 0)
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
