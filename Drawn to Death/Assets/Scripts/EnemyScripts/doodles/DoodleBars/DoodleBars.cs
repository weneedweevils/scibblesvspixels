using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleBars : EnemyAI
{
    [Header("Doodle Bars Specific References")]
    public DoodleBars[] friends;

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

    override public bool Revive(float percentMaxHP = 1f, float percentDamage = 1f, float percentSpeed = 1f, float percentAttkSpeed = 1f)
    {
        return false;
    }

    public override void Kill()
    {
        Debug.Log(invincibilityTimer);
        base.Kill();
        foreach (DoodleBars friend in friends)
        {
            if (!friend.isDead())
            {
                friend.Damage(friend.health);
            }
        }
    }
}
