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

    [Button]
    public void OpenDoor()
    {
        animator?.SetTrigger("Open");
        timer.Start(() => doorCollider.enabled = false);
        FMODUnity.RuntimeManager.PlayOneShot(openSFX);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenDoor();
        }
    }
}
