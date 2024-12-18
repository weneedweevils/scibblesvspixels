using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleBars2 : EnemyAI
{
    [Header("Doodle Bars Specific References")]
    public DoodleBars[] friends;
    public Color inactiveCol;

    override protected void Start()
    {
        deathDuration = 25f / 60f;
        attackDuration = 12f / 60f;
        invincibilityDuration = 20f / 60f;
        type = Type.bars;
        Debug.Log("Doodle bars start called");
        base.Start();

    }

    override protected void FixedUpdate()
    {
        if (!playerMovement.inFreezeDialogue() && !playerMovement.timelinePlaying && state != State.dead)
        {
            //Continue with the base class implementation of FixedUpdate
            base.FixedUpdate();
            if (isolated)
            {
                selfImage.color = inactiveCol;
            }
        }
    }

    override protected void BlockerActivation()
    {
        base.BlockerActivation();
        selfImage.color = Color.white;
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
