using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] Collider2D doorCollider;
    [SerializeField] private float openDelay = 0.5f;
    [SerializeField] private FMODUnity.EventReference openSFX;
    private StateTimer timer;

    public void Start()
    {
        timer = new StateTimer(new float[] { openDelay });
    }

    /// <summary>
    /// Begins the door opening sequence
    /// </summary>
    [Button]
    public void OpenDoor()
    {
        animator?.SetTrigger("Open");   // Play the door opening animation
        timer.Start(() => doorCollider.enabled = false);    // disable door collider when timer ends
        FMODUnity.RuntimeManager.PlayOneShot(openSFX);      // Play door sfx
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenDoor();
        }
    }
}
