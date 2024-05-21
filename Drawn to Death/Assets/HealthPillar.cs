using UnityEngine;

public class HealthPillar : EnemyAI
{


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

    public override float PathLength(bool toPlayer = false)
    {
        return Mathf.Infinity;
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
        base.Kill();
    }

    public override void Damage(float damageTaken, bool makeInvincible = true, bool animateHurt = false, Vector2 knockbackDir = default, float knockbackPower = 0, bool lifeSteal = false)
    {
        

        //Inflict damage
        health -= damageTaken;
        healthBar.SetHealth(health, maxHealth);


        //Check death conditions
        if (health <= 0)
        {
            Kill(); // Ded
            return;
        }

        //Flash hurt color
        if (animateHurt)
        {
            selfImage.color = hurtCol;
        }

        //Start invincibility timer
        if (makeInvincible)
        {
            invincibilityTimer.StartTimer();
        }

        return;
    }
}

