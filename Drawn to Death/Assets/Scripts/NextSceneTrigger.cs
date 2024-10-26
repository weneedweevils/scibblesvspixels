using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NextSceneTrigger : MonoBehaviour
{
    public GameObject shop;
    public PlayerInput playerInput;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().timelinePlaying = true; // Stop Movement once trigger activated
            shop.SetActive(true);
            CustomInput.instance.playerInput.SwitchCurrentActionMap("UI");
        }
    }
}
