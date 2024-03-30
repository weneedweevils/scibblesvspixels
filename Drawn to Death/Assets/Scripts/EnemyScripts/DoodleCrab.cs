using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoodleCrab : EnemyAI
{
    [Header("Doodle Crab Specific References")]
    public float lungeForce = 25000f;

    override protected void Start()
    {
        deathDuration = 25f / 60f;
        reviveDuration = 69f / 60f;
        attackDuration = 12f / 60f;
        invincibilityDuration = 20f / 60f;
        base.Start();
    }

    override protected void FixedUpdate()
    {
        //Continue with the base class implementation of FixedUpdate
        base.FixedUpdate();
    }

    override protected void Attack()
    {
        if (target != null && attackTimer.IsUseable())
        {
            //Play the FMOD event correlating to the attack
            FMODUnity.RuntimeManager.PlayOneShot(attackSfx);
            
            //Lunge at the target
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;
            rb.AddForce(direction * lungeForce * Time.deltaTime);
            
            //Start the Attack Timer
            attackTimer.StartTimer();
            animator.SetBool("attacking", true);
            animator.SetBool("chasing", false);
        }

        if (attackTimer.IsOnCooldown())
        {
            animator.SetBool("attacking", false);
            animator.SetBool("chasing", true);
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (health > 0)
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    {
                        //Get a reference to the player
                        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
                        if (attackTimer.IsActive() && team == Team.oddle && player.invincibilityTimer.IsUseable())
                        {
                            //Damage player
                            player.Damage(damage);
                        }
                        break;
                    }
                case "Enemy":
                    {
                        //Get a reference to the enemy
                        EnemyAI otherAI = collision.gameObject.GetComponent<EnemyAI>();
                        if (attackTimer.IsActive() && otherAI != null && team != otherAI.team && otherAI.team != Team.neutral && otherAI.invincibilityTimer2.IsUseable())
                        {
                            Debug.Log(string.Format("{0} Hut {1} for {2} damage", name, otherAI.name, damage));
                            //Damage enemy
                            otherAI.Damage(damage, false, true);

                            //Start enemies secondary invincibility timer
                            otherAI.invincibilityTimer2.StartTimer();
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
