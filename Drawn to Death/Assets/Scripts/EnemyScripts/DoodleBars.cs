using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleBars : EnemyAI
{
    //[Header("Doodle Bars Specific References")]
    

    override protected void Start()
    {
        deathDuration = 25f / 60f;
        attackDuration = 12f / 60f;
        invincibilityDuration = 20f / 60f;
        base.Start();
    }

    override protected void FixedUpdate()
    {
        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying && state != State.dead)
        {
            //Continue with the base class implementation of FixedUpdate
            base.FixedUpdate();
        }
    }

    override protected void Attack()
    {
        return;
    }

    override protected void MoveEnemy()
    {
        return;
    }

    override public bool Revive(float percentMaxHP = 1f, float percentDamage = 1f, float percentSpeed = 1f)
    {
        return false;
    }
}
