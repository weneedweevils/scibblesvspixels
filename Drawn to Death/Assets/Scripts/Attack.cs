﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //Player game object
    public GameObject player;

    //Use key
    public KeyCode attackButton = KeyCode.Mouse0;

    //Timer
    public float attackDuration = 0f;
    public float attackCooldown = 0f;
    private float attackTimer = 0f;

    private bool attacking = false;
    private bool onCooldown = false;

    //Stats
    public float damage = 100;
    public float piercing = 3;

    //Animation
    private Animator animator;

    //Hitbox
    private BoxCollider2D hitbox;
    private Vector2 initialHitboxOffset;
    private Vector2 initialHitboxSize;

    // FMOD sound event path
    public string sfx;

    // Start is called before the first frame update
    void Start()
    {
        //Collect components
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();

        //Save initial attack hitbox information
        initialHitboxOffset = hitbox.offset;
        initialHitboxSize = hitbox.size;

        // Get a reference to the script that controls the FMOD event
        //eraserSFX = GetComponent<eraserSFX>;
    }

    // Update is called once per frame
    void Update()
    {
        //Attack Timer
        if (attacking || onCooldown)
        {
            attackTimer += Time.deltaTime;
        }
        //Check if attack is over -> start the cooldown
        if (attacking && attackTimer >= attackDuration)
        {
            onCooldown = true;
            attacking = false;
            animator.SetBool("attacking", false);
            attackTimer -= attackDuration;
        }
        //Check if cooldown is over
        if (onCooldown && attackTimer >= attackCooldown)
        {
            onCooldown = false;
            attackTimer = 0f;
        }
        //Attack
        if (Input.GetKey(attackButton) && !player.GetComponent<PlayerMovement>().inFreezeDialogue() && !player.GetComponent<PlayerMovement>().timelinePlaying)
        {
            animator.SetBool("attacking", true);
            attacking = true;

            // Play the FMOD event correlating to the attack
            FMODUnity.RuntimeManager.PlayOneShot(sfx);
        }
    }

    //Used to flip the attack hitbox as needed when rotating
    public void FlipHitbox(bool flip)
    {
        if (flip)
        {
            hitbox.offset = -initialHitboxOffset;
        } else
        {
            hitbox.offset = initialHitboxOffset;
        }
    }
}