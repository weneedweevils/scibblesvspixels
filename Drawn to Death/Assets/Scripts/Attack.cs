﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();
        initialHitboxOffset = hitbox.offset;
        initialHitboxSize = hitbox.size;
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
        if (Input.GetKey(attackButton) && !attacking && !onCooldown)
        {
            animator.SetBool("attacking", true);
            attacking = true;
        }
    }

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