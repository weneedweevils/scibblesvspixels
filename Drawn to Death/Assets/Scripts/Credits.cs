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
    private float scrollSpeed = 100;
    public UnityEngine.UI.Button menuButton;
    PlayerInput playerInput;

    private void Start()
    {
        Debug.Log("Started");
        playerInput = GetComponent<PlayerInput>();
        
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        credits.transform.position += scrollSpeed * Vector3.up * Time.deltaTime;
        if (playerInput.actions["ScrollFaster"].IsPressed())
        {
            credits.transform.position += scrollSpeed * 3 * Vector3.up * Time.deltaTime;
        }
    }
}
